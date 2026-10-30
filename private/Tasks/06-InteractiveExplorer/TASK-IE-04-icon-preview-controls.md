# TASK-IE-04 — Live Preview Controls

**Status:** DONE
**Priority:** high
**Model:** STRONG

## Goal
Expose all library features via live controls affecting selected icon.

## Controls (must)
- Style: Solid/Regular/Light/Thin/Duotone/Brands
- Typeface: Classic/Sharp/Duotone/SharpDuotone
- Size: IconSize/explicit; IconWidth
- Color: foreground/background (CSS vars)
- Rotation/Flip: Rotate, RotateBy, FlipHorizontal/Vertical
- Animations: Beat/Bounce/Shake etc., AnimationDuration/IterationCount
- Pull/Border/Inverse/SwapOpacity, FixedWidth
- Transformations, CanvasPadding
- Layers/Mask: explore composite cases (notes re webfont SVG-only)
- Duotone: primary/secondary colors/opacity (UI toggles)

## Deliverables
- Control components + binding to Icon props
- Live preview updates

## Acceptance
- All relevant props editable; preview matches rendered output

## Outcome
Implemented in `samples\Jenin.FontAwesome.Blazor.Explorer`:
- `ExplorerPreviewState.cs` — plain mutable state model covering every `Icon` parameter the controls drive
  (style, typeface, size/width/fixed-width, foreground/background, rotate/rotate-angle/combining-rotate,
  pull, bordered/inverse/swap-opacity, all 14 animation families plus delay/duration/iteration/direction/timing,
  power transform builder inputs, duotone primary/secondary color+opacity, SVG mask example), with a nested
  `PreviewAnimation` enum (14 members + `None`).
- `Components\PreviewControls.razor(.cs)` — grouped inputs bound with `@bind` + `@bind:after="NotifyAsync"`;
  `OnChanged` callback drives `PreviewPanel.StateHasChanged` so the icon re-renders live.
- `Components\PreviewPanel.razor(.cs)` — renders the selected icon through the real `Icon` component,
  mapping state to parameters (`BuildAnimation()`/`BuildTransform()`, layer/mask gating).

## Evidence
- `dotnet build "Jenin.FontAwesome.Blazor.slnx" -c Release` → 0 warnings, 0 errors.
- `dotnet test tests\Jenin.FontAwesome.Blazor.Tests` → 292 passed (net8.0/net9.0/net10.0).
- `pwsh ./build.ps1` → Build succeeded.
- `pwsh ./eng/verify-encoding.ps1` → passed (314 governed files).
- e2e (Release server, `localhost`): `/explorer?icon=house` prerenders
  `<i class="fa-solid fa-house">`, the full control panel, and facet counts `1992 | 1422 | 169 | 572`;
  `/explorer?q=home&style=solid` search input reflects `home` and reports `29 of 1992 icons`.

## Notes
- Fixed a latent Razor binding bug found during e2e: component-attribute values for member-access
  expressions (e.g. `Query="_state.Query"`, `Items="_rows"`, `State="Preview"`) were being passed as
  literal strings; corrected to `@`-prefixed expressions across `Explorer.razor`, `IconGrid.razor`,
  `PreviewPanel.razor`, and `SearchBar.razor`.
- Interactive grid rows are rendered by `Virtualize` after the circuit connects; server prerender emits
  only the measured spacers (expected, matches IE-03 design).
- `Program.cs` (server host) now registers the Explorer assembly with
  `MapRazorComponents<App>().AddAdditionalAssemblies(...)` so `/explorer` routes resolve in the sample host.