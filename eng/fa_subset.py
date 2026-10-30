#!/usr/bin/env python3
"""Self-hosted Font Awesome subsetter.

Reads the usage manifest produced by eng/Find-FontAwesomeUsage.ps1 and emits a drop-in
replacement for the vendored Web Fonts assets: subset WOFF2 files plus a rewritten CSS file
that keeps every utility class but only the used icon definitions.

Inputs (vendored 7.3.1 assets) are never modified; all outputs go to --out-dir:

    fontawesome-subset.css        rewritten all.css (unused icon blocks removed,
                                @font-face srcs rewritten to the subset files)
    fa-solid-900.subset.woff2    subset fonts (only styles with used glyphs)
    fa-regular-400.subset.woff2
    fa-brands-400.subset.woff2
    fa-v4compatibility.woff2     copied verbatim (v4/v5 shim, tiny)
    report.json / report.md       counts, byte sizes, savings, unresolved dynamics

Icon -> codepoint mapping is parsed from the vendored all.css itself (the `--fa` values),
so the subset always matches the exact CSS it ships with. Aliases resolve through the
checked-in metadata; dynamic (non-literal) references are reported, never guessed.

Requires: pip install fonttools brotli

Usage: python eng/fa_subset.py --usage eng/used-icons.json
           --css samples/.../wwwroot/fontawesome/css/all.css
           --fonts samples/.../wwwroot/fontawesome/webfonts
           --metadata eng/metadata/fontawesome-7.3.1-free-icon-metadata.json
           --out-dir dist/fa-subset
"""

from __future__ import annotations

import argparse
import json
import re
import shutil
import sys
from pathlib import Path

ICON_BLOCK_RE = re.compile(
    r"(?m)^(?P<selectors>(?:\.fa-[a-z0-9-]+(?:\s*,\s*)?)+)\s*\{\s*\n"
    r'\s*--fa:\s*"(?P<escape>\\?[^"]+)";\s*\n\}'
)
FONT_FACE_SRC_RE = re.compile(r"(url\(\")(\.\./webfonts/fa-(solid-900|regular-400|brands-400)\.woff2)(\"\))")

STYLE_FONTS = {
    "solid": "fa-solid-900.woff2",
    "regular": "fa-regular-400.woff2",
    "brands": "fa-brands-400.woff2",
}


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Subset Font Awesome self-hosted assets.")
    parser.add_argument("--usage", required=True, type=Path)
    parser.add_argument("--css", required=True, type=Path)
    parser.add_argument("--fonts", required=True, type=Path)
    parser.add_argument("--metadata", required=True, type=Path)
    parser.add_argument("--out-dir", required=True, type=Path)
    return parser.parse_args()


def load_metadata(path: Path) -> tuple[dict[str, list[str]], dict[str, str]]:
    """Return (name -> free styles, alias -> canonical name)."""
    data = json.loads(path.read_text(encoding="utf-8"))
    styles: dict[str, list[str]] = {}
    aliases: dict[str, str] = {}
    for icon in data["icons"]:
        name = icon["name"]
        styles[name] = [str(s) for s in icon.get("styles", [])]
        for alias in icon.get("aliases", []):
            if isinstance(alias, str):
                aliases[alias] = name
            elif isinstance(alias, dict) and "name" in alias:
                aliases[str(alias["name"])] = name
    return styles, aliases


def parse_css_icons(css: str) -> dict[str, int]:
    """Map icon class suffix (fa-house) to codepoint from --fa definitions."""
    icons: dict[str, int] = {}
    for match in ICON_BLOCK_RE.finditer(css):
        selectors = [s.strip() for s in match.group("selectors").split(",")]
        codepoint = decode_escape(match.group("escape"))
        for selector in selectors:
            if selector.startswith(".fa-"):
                icons[selector[1:]] = codepoint
    return icons


def decode_escape(escape: str) -> int:
    """Decode a CSS string: a backslash hex escape (trailing space tolerated) or a literal char."""
    body = escape.strip()
    if body.startswith("\\"):
        body = body[1:].rstrip()
        if body and all(ch in "0123456789abcdefABCDEF" for ch in body):
            return int(body, 16)
    if body:
        return ord(body[0])
    raise ValueError(f"Cannot decode CSS escape: {escape!r}")


def subset_font(src: Path, unicodes: set[int], dst: Path) -> int:
    from fontTools import subset

    options = subset.Options()
    options.layout_features = []
    options.name_IDs = []
    options.notdef_outline = True
    options.recalc_bounds = True
    font = subset.load_font(str(src), options)
    subsetter = subset.Subsetter(options)
    subsetter.populate(unicodes=sorted(unicodes))
    subsetter.subset(font)
    font.flavor = "woff2"
    font.save(str(dst))
    return dst.stat().st_size


def main() -> int:
    args = parse_args()
    usage = json.loads(args.usage.read_text(encoding="utf-8"))
    css = args.css.read_text(encoding="utf-8")
    css_icons = parse_css_icons(css)
    free_styles, aliases = load_metadata(args.metadata)

    used: set[str] = set()
    for entry in usage.get("names", []):
        # Entries look like "fa-house"; metadata keys omit the prefix.
        used.add(str(entry["name"])[3:] if str(entry["name"]).startswith("fa-") else str(entry["name"]))

    # Resolve aliases to canonical names, but keep requested alias blocks too.
    canonical: set[str] = set()
    alias_hits: dict[str, str] = {}
    known_styles: dict[str, set[str]] = {}
    for entry in usage.get("names", []):
        raw = str(entry["name"])
        short = raw[3:] if raw.startswith("fa-") else raw
        entry_styles = {str(s) for s in entry.get("styles", [])}
        if short in aliases:
            alias_hits[short] = aliases[short]
            canonical.add(aliases[short])
            known_styles.setdefault(aliases[short], set()).update(entry_styles)
        else:
            canonical.add(short)
            known_styles.setdefault(short, set()).update(entry_styles)

    # Codepoints per font, driven by known styles (unknown -> every font; the
    # subsetter ignores codepoints absent from a font).
    needed: dict[str, set[int]] = {font: set() for font in STYLE_FONTS.values()}
    missing: list[str] = []
    for name in sorted(canonical):
        css_key = f"fa-{name}"
        if css_key not in css_icons:
            missing.append(name)
            continue
        codepoint = css_icons[css_key]
        styles = known_styles.get(name) or set(free_styles.get(name, [])) or {"solid", "regular", "brands"}
        for style in styles:
            font = STYLE_FONTS.get(style)
            if font:
                needed[font].add(codepoint)
        if not (styles & set(STYLE_FONTS)):
            for font in needed:
                needed[font].add(codepoint)

    out_dir: Path = args.out_dir
    out_dir.mkdir(parents=True, exist_ok=True)

    # Clear previous subset products so shrunk reruns cannot leave stale fonts behind.
    for stale in out_dir.glob("*.subset.woff2"):
        stale.unlink()
    fonts_dir: Path = args.fonts

    font_reports = []
    total_before = 0
    total_after = 0
    for font_file, unicodes in needed.items():
        src = fonts_dir / font_file
        if not src.exists() or not unicodes:
            continue
        before = src.stat().st_size
        dst = out_dir / font_file.replace(".woff2", ".subset.woff2")
        after = subset_font(src, unicodes, dst)
        total_before += before
        total_after += after
        font_reports.append(
            {
                "font": font_file,
                "subset": dst.name,
                "glyphs": len(unicodes),
                "bytes_before": before,
                "bytes_after": after,
            }
        )

    # Copy the tiny v4-compat shim untouched.
    for extra in ("fa-v4compatibility.woff2",):
        src = fonts_dir / extra
        if src.exists():
            shutil.copy2(src, out_dir / extra)

    # Rewrite the CSS: drop unused icon blocks, repoint @font-face at subsets.
    def keep_block(match: re.Match[str]) -> str:
        selectors = [s.strip() for s in match.group("selectors").split(",")]
        names = [s[4:] if s.startswith(".fa-") else s for s in selectors]
        if any(n in canonical or n in alias_hits for n in names):
            return match.group(0)
        return ""

    subset_css = ICON_BLOCK_RE.sub(keep_block, css)
    # Drop @font-face blocks whose font file has no used glyphs (matched on the original
    # file names, before srcs are rewritten to the subset files below).
    emitted = {font[:-len(".woff2")] for font in needed if (out_dir / font.replace(".woff2", ".subset.woff2")).exists()}
    for base in ("fa-solid-900", "fa-regular-400", "fa-brands-400"):
        if base in emitted:
            continue
        subset_css = re.sub(
            r"@font-face\s*\{(?:[^{}]*" + re.escape(f"{base}.woff2") + r"[^{}]*)\}",
            "",
            subset_css,
        )

    subset_css, face_count = FONT_FACE_SRC_RE.subn(
        lambda m: f'url("../webfonts/{m.group(2).split("/")[-1].replace(".woff2", ".subset.woff2")}")',
        subset_css,
    )
    css_path = out_dir / "fontawesome-subset.css"
    css_path.write_text(subset_css, encoding="utf-8")

    css_before = len(css.encode("utf-8"))
    css_after = len(subset_css.encode("utf-8"))

    report = {
        "used_names": sorted(canonical),
        "used_count": len(canonical),
        "alias_hits": alias_hits,
        "missing_names": sorted(missing),
        "dynamic_warnings": usage.get("dynamic", []),
        "fonts": font_reports,
        "font_bytes_before": total_before,
        "font_bytes_after": total_after,
        "css_bytes_before": css_before,
        "css_bytes_after": css_after,
        "total_bytes_before": total_before + css_before,
        "total_bytes_after": total_after + css_after,
    }
    (out_dir / "report.json").write_text(json.dumps(report, indent=2) + "\n", encoding="utf-8")

    saved = total_before + css_before - total_after - css_after
    pct = 100.0 * saved / (total_before + css_before) if total_before + css_before else 0.0
    (out_dir / "report.md").write_text(
        "# Font Awesome subset report\n\n"
        f"- Used icon names: **{len(canonical)}**\n"
        f"- Alias hits: **{len(alias_hits)}**\n"
        f"- Missing from vendored CSS: **{len(missing)}**{f' ({", ".join(sorted(missing))})' if missing else ''}\n"
        f"- Dynamic references needing review: **{len(report['dynamic_warnings'])}**\n"
        f"- Fonts: {total_before} B -> {total_after} B\n"
        f"- CSS: {css_before} B -> {css_after} B\n"
        f"- **Total: {total_before + css_before} B -> {total_after + css_after} B "
        f"({saved} B saved, {pct:.1f}%)**\n",
        encoding="utf-8",
    )

    print(f"Subset: {len(canonical)} icons, {total_before + css_before} B -> {total_after + css_after} B "
          f"({pct:.1f}% smaller) -> {out_dir}")
    if missing:
        print(f"WARNING: {len(missing)} used names missing from vendored CSS: {', '.join(sorted(missing))}",
              file=sys.stderr)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
