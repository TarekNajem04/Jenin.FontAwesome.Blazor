# Engineering scripts (`eng/`)

Deterministic code generators, verification gates, and Font Awesome asset tooling.
Two rules apply to everything here:

- **Generators are deterministic.** Regenerating with no input changes must produce
  byte-identical output. Every generator has a verify mode (`-Verify` for PowerShell,
  `--check` for Python) that CI runs to fail when a committed file drifts.
- **Never hand-edit a generated file.** Change the generator or its input, rerun the
  generator, and commit the regenerated output.

## Generators

| Script | Generates | From |
|----|----|----|
| `generate-icon-catalogue.ps1` | `src/Jenin.FontAwesome.Blazor/IconNames.cs` (typed per-style name constants) | `metadata/fontawesome-7.3.1-free-icons.json` (via `update-fontawesome-metadata.ps1`) |
| `generate-icon-presets.ps1` | `src/Jenin.FontAwesome.Blazor.Icons/GeneratedIconComponents.cs` (one preset component per icon) | `metadata/fontawesome-7.3.1-free-icon-metadata.json` |
| `generate-icon-metadata.ps1` | Compact embedded JSON resource for `Jenin.FontAwesome.Blazor.IconPicker` | `metadata/fontawesome-7.3.1-free-icon-metadata.json` |
| `generate-analyzer-icon-names.ps1` | `KnownIconNames.txt` / `KnownIconStyles.txt` / `KnownIconAliases.txt` embedded by the analyzers package | rich + per-style metadata |
| `generate-llms-txt.ps1` | `llms.txt` AI-consumer entry point (every link is verified to exist) | `docs/` pages |
| `fa_svg_components.py` | `src/Jenin.FontAwesome.Blazor.Svg/GeneratedIconComponents.cs` (inline-SVG components, glyph paths extracted from WOFF2 with fontTools) | `metadata/`, vendored `all.css` + `webfonts/` |

Python generators need `pip install fonttools` (`fa_sprite.py` additionally needs `brotli`).

Upgrading the vendored Font Awesome version is a two-step flow:
`update-fontawesome-metadata.ps1` (downloads upstream `metadata/icons.json` once and derives
the compact + rich files) followed by rerunning every generator above.

## Verification gates

| Script | Guards |
|----|----|
| `verify-encoding.ps1` | Every `src`/`samples`/`tests` source file is UTF-8 BOM + CRLF + single final newline (`-Fix` rewrites offenders) |
| `verify-coverage.ps1` | Merged line coverage from the `net10.0` test legs stays above `coverage-threshold.txt` |
| `verify-optional-trim.ps1` | Publish-level IL-trimming contracts for the IconPicker/Icons/Svg probes |
| `verify-templates.ps1` | Template-pack smoke test: pack, install, scaffold, build, run, assert a first icon renders |
| `verify-suites.ps1` | Suite-adapters fixture (real MudBlazor + Blazorise components) renders in a real browser |
| `verify-pro-render.ps1` | Licensed-Pro render matrix (local/manual: Pro assets come from the environment, never committed) |

## Asset and size tooling

| Script | Purpose |
|----|----|
| `Find-FontAwesomeUsage.ps1` | Scans `.razor`/`.cs` for statically known icon references; emits `used-icons.json` (dynamics are reported, never guessed) |
| `fa_subset.py` | Emits subset WOFF2 + rewritten CSS plus `report.json`/`report.md` from the usage manifest |
| `fa_sprite.py` | Builds a self-hosted zero-JS SVG sprite from the vendored webfonts |
| `measure-bundle-sizes.ps1` | Measures trimmed publish output per delivery mode and rewrites the README dashboard (`-Check` asserts freshness) |

## Data files

| Path | Purpose |
|----|----|
| `metadata/` | Vendored Font Awesome metadata (rich + compact + per-style JSON) pinned to the supported release |
| `used-icons.json` | Icon usage manifest consumed by the subset/sprite tooling |
| `bundle-sizes.json` | Baseline for the bundle-size dashboard |
| `coverage-threshold.txt` | Coverage ratchet enforced by `verify-coverage.ps1` |
| `docfx-filter.yml` | DocFX metadata filter for the API reference build |

## Related documentation

- [Repository README](../README.md) (bundle-size dashboard, test commands)
- [TESTING.md](../TESTING.md) (local test and coverage workflow)
- [Repository scripts](../scripts/README.md) (maintenance tooling)
- [Documentation diagnostics](../docs/diagnostics.md) (analyzer rules)
