# TASK-IE-05 — Snippet Generator

**Status:** DONE
**Priority:** high
**Model:** STRONG

## Goal
Generate copyable Razor/markup for current icon+props.

## Features
- Razor snippet (<Icon ... />)
- HTML equivalent if relevant
- Copy-to-clipboard with feedback
- Respect defaults (omit unset)
- Include Typeface/Style/IconName/IconSize/animations etc.

## Deliverables
- Snippet service + UI

## Acceptance
- Valid compilable Razor for typical cases

## Outcome
Implemented in `samples\Jenin.FontAwesome.Blazor.Explorer`:
- `Snippets\SnippetGenerator.cs` — pure generator producing three snippet kinds from the selected entry
  and the live `ExplorerPreviewState`: **Razor** (`<Icon … />`), **Preset component** (`<HouseIcon … />`
  with a Razor fallback when no preset exists), and **HTML** (web-font `<i class="fa-…">` with inline
  styles and `data-fa-transform`). Defaults are omitted so the output is minimal and compiles as-is.
- `Snippets\IconNamesReference.cs` — resolves `IconNames.<Group>.<Const>` (for example
  `IconNames.Solid.House`, `IconNames.Brands.Github`) using the generator's identifier algorithm.
- `Snippets\FontAwesomeClassTokens.cs` — reproduces the internal enum→class-token mapping for the
  Free styles the explorer exposes (core surface is frozen and `ValueAttribute` is internal).
- `PreviewIconFactory.cs` — shared animation/transform builder used by both the preview and the
  generator so code and preview always agree; `PreviewPanel` now delegates to it.
- `Components\SnippetPanel.razor(.cs)` — tabbed snippet viewer (Razor / Preset component / HTML) with a
  copy-to-clipboard button and transient "Copied!" feedback; wired into `PreviewPanel`.
- `wwwroot\js\explorer.js` — added `copyToClipboard` (async Clipboard API with a hidden-textarea
  fallback for non-secure origins).
- `Explorer` seeds `Preview.Style` to the selected icon's primary Free style, keeping the preview and
  snippet meaningful across selections (`?icon=github` → `IconNames.Brands.Github` / `IconStyle.Brands`).

## Evidence
- `dotnet build "Jenin.FontAwesome.Blazor.slnx" -c Release` → 0 warnings, 0 errors.
- `dotnet test tests\Jenin.FontAwesome.Blazor.Tests` → 292 passed (net8.0/net9.0/net10.0).
- `pwsh ./build.ps1` → Build succeeded; `pwsh ./eng/verify-encoding.ps1` → passed (320 governed files).
- **Compilability proof:** the generator's exact output for a fully-configured state (all styles,
  typeface, size/width, colors, rotate, combining-rotate, pull, border/inverse/swap, duotone colours +
  opacities, mask, `Animate.Spin(new SpinOptions { … TimingFunction = "ease-in-out" })`, and
  `new Transform().Grow(6).Rotate(-15)`) was compiled as a temporary Razor component and built with
  0 errors, then removed.
- e2e (Release server): `/explorer?icon=house&style=solid` → `<Icon IconName="@(IconNames.Solid.House)"
  IconStyle="IconStyle.Solid" />`; `/explorer?icon=github` → `IconNames.Brands.Github` /
  `IconStyle.Brands`.

## Notes
- Razor attribute values containing string literals (animation `TimingFunction`) are emitted with
  single quotes so the inner double quotes stay valid; float parameter values are emitted with an `f`
  suffix (for example `PrimaryOpacity="0.4f"`) so they bind to `float`/`float?` parameters.
- HTML inline styles use CSS `color`/`background-color` for `Icon.Color`/`Icon.BackgroundColor` and the
  `--fa-*` custom properties for duotone layers, matching the library's rendering rules.