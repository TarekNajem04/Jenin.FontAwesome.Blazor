#!/usr/bin/env python3
"""Self-hosted SVG sprite generator.

Builds a single SVG sprite (`<symbol>` per icon) from the vendored Font Awesome webfonts,
so icons render with zero Font Awesome JavaScript: `<svg><use href="...#fa-solid-house"/></svg>`.

Unlike eng/fa_subset.py (which shrinks fonts but keeps the JS/CSS pipeline), the sprite path
removes the runtime entirely: nothing watches the DOM, nothing is replaced, and the markup is
identical before and after the Blazor circuit connects. Glyph outlines are extracted from the
WOFF2 files themselves with fontTools, so the artwork matches the webfont pixel-for-pixel.

Inputs: eng/used-icons.json (from eng/Find-FontAwesomeUsage.ps1), vendored webfonts,
the --fa codepoint map (parsed from the vendored all.css), and the Free metadata
(styles + aliases).

Requires: pip install fonttools brotli

Usage: python eng/fa_sprite.py --usage eng/used-icons.json
           --css samples/.../wwwroot/fontawesome/css/all.css
           --fonts samples/.../wwwroot/fontawesome/webfonts
           --metadata eng/metadata/fontawesome-7.3.1-free-icon-metadata.json
           --out-dir dist/fa-sprite
"""

from __future__ import annotations

import argparse
import json
import re
import sys
from pathlib import Path

ICON_BLOCK_RE = re.compile(
    r"(?m)^(?P<selectors>(?:\.fa-[a-z0-9-]+(?:\s*,\s*)?)+)\s*\{\s*\n"
    r'\s*--fa:\s*"(?P<escape>\\?[^"]+)";\s*\n\}',
)

STYLE_FONTS = {
    "solid": "fa-solid-900.woff2",
    "regular": "fa-regular-400.woff2",
    "brands": "fa-brands-400.woff2",
}

STYLE_PREFIX = {"solid": "fas", "regular": "far", "brands": "fab"}


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Generate a self-hosted SVG sprite.")
    parser.add_argument("--usage", required=True, type=Path)
    parser.add_argument("--css", required=True, type=Path)
    parser.add_argument("--fonts", required=True, type=Path)
    parser.add_argument("--metadata", required=True, type=Path)
    parser.add_argument("--out-dir", required=True, type=Path)
    return parser.parse_args()


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


def parse_css_icons(css: str) -> dict[str, int]:
    icons: dict[str, int] = {}
    for match in ICON_BLOCK_RE.finditer(css):
        selectors = [s.strip() for s in match.group("selectors").split(",")]
        codepoint = decode_escape(match.group("escape"))
        for selector in selectors:
            if selector.startswith(".fa-"):
                icons[selector[4:]] = codepoint
    return icons


def glyph_svg_path(font_path: Path, codepoint: int) -> tuple[str, float, float] | None:
    """Return (path-d, width, height) normalized into a 512x512 box, y-down."""
    from fontTools.misc.transform import Transform
    from fontTools.pens.boundsPen import BoundsPen
    from fontTools.pens.svgPathPen import SVGPathPen
    from fontTools.pens.transformPen import TransformPen
    from fontTools.ttLib import TTFont

    font = TTFont(str(font_path))
    cmap = font.getBestCmap()
    if codepoint not in cmap:
        return None
    glyph_set = font.getGlyphSet()
    glyph = glyph_set[cmap[codepoint]]
    bounds_pen = BoundsPen(glyph_set)
    glyph.draw(bounds_pen)
    if bounds_pen.bounds is None:
        return None
    x_min, y_min, x_max, y_max = bounds_pen.bounds
    width = x_max - x_min
    height = y_max - y_min
    if width <= 0 or height <= 0:
        return None
    scale = min(512.0 / width, 512.0 / height)
    tx = (512.0 - scale * width) / 2.0 - scale * x_min
    ty = (512.0 - scale * height) / 2.0 + scale * y_max
    pen = SVGPathPen(glyph_set)
    transform_pen = TransformPen(pen, Transform(scale, 0, 0, -scale, tx, ty))
    glyph.draw(transform_pen)
    path = pen.getCommands()
    return path, round(width * scale, 2), round(height * scale, 2)


def main() -> int:
    args = parse_args()
    usage = json.loads(args.usage.read_text(encoding="utf-8"))
    css_icons = parse_css_icons(args.css.read_text(encoding="utf-8"))
    data = json.loads(args.metadata.read_text(encoding="utf-8"))
    free_styles: dict[str, list[str]] = {}
    aliases: dict[str, str] = {}
    for icon in data["icons"]:
        free_styles[icon["name"]] = [str(s) for s in icon.get("styles", [])]
        for alias in icon.get("aliases", []):
            name = alias if isinstance(alias, str) else alias.get("name")
            if name:
                aliases[str(name)] = icon["name"]

    symbols: list[str] = []
    missing: list[str] = []
    per_font: dict[str, int] = {f: 0 for f in STYLE_FONTS.values()}

    for entry in usage.get("names", []):
        raw = str(entry["name"])
        name = raw[3:] if raw.startswith("fa-") else raw
        canonical = aliases.get(name, name)
        styles = [str(s) for s in entry.get("styles", [])] or free_styles.get(canonical, ["solid", "regular", "brands"])
        codepoint = css_icons.get(name, css_icons.get(canonical))
        if codepoint is None:
            missing.append(name)
            continue
        for style in styles:
            font_file = STYLE_FONTS.get(style)
            if font_file is None:
                continue
            # Brand glyphs live in the brands font; solid/regular share names but differ in artwork.
            result = glyph_svg_path(args.fonts / font_file, codepoint)
            if result is None:
                missing.append(f"{name} ({style})")
                continue
            path, _, _ = result
            symbol_id = f"fa-{style}-{name}"
            symbols.append(
                f'<symbol id="{symbol_id}" viewBox="0 0 512 512">'
                f'<path d="{path}" fill="currentColor"/></symbol>'
            )
            per_font[font_file] += 1

    out_dir: Path = args.out_dir
    out_dir.mkdir(parents=True, exist_ok=True)
    sprite = (
        '<svg xmlns="http://www.w3.org/2000/svg" style="display:none">\n'
        + "\n".join(symbols)
        + "\n</svg>\n"
    )
    sprite_path = out_dir / "fa-sprite.svg"
    sprite_path.write_text(sprite, encoding="utf-8")

    report = {
        "used_names": sorted({str(e["name"]) for e in usage.get("names", [])}),
        "used_count": len(usage.get("names", [])),
        "symbols": len(symbols),
        "per_font_glyphs": per_font,
        "missing_names": sorted(set(missing)),
        "dynamic_warnings": usage.get("dynamic", []),
        "sprite_file": sprite_path.name,
        "sprite_bytes": sprite_path.stat().st_size,
    }
    (out_dir / "report.json").write_text(json.dumps(report, indent=2) + "\n", encoding="utf-8")
    total = report["sprite_bytes"]
    (out_dir / "report.md").write_text(
        "# Font Awesome sprite report\n\n"
        f"- Used names: **{report['used_count']}**\n"
        f"- Symbols emitted: **{len(symbols)}**\n"
        f"- Missing (no glyph): **{len(report['missing_names'])}**"
        f"{(' (' + ', '.join(sorted(set(missing))) + ')') if missing else ''}\n"
        f"- Dynamic references needing review: **{len(report['dynamic_warnings'])}**\n"
        f"- Sprite: `{sprite_path.name}` ({total} B)\n",
        encoding="utf-8",
    )

    print(f"Sprite: {len(symbols)} symbols, {total} B -> {out_dir}")
    if missing:
        print(f"WARNING: {len(missing)} used names have no glyph: {', '.join(sorted(set(missing)))}", file=sys.stderr)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
