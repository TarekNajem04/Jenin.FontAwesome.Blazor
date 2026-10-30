# TASK-IE-07 — Samples Integration

**Status:** DONE
**Priority:** high
**Model:** STRONG

## Goal
Make the Explorer a first-class part of the sample application: reachable from the navigation and the
getting-started page, with honest rendering caveats for unsupported (Pro / SVG-only) combinations and
the required Font Awesome attribution — without disturbing the existing sample pages.

The Explorer is a **separate Razor Class Library** (`samples\Jenin.FontAwesome.Blazor.Explorer`) that
consumes the frozen core API. Its project/solution/host wiring landed in IE-03; this task covers the
host-facing integration surface.

## Deliverables
- Explorer in the sample solution and navigation (IE-03 wiring, verified here).
- Getting-started entry point pointing at `/explorer`.
- `UnsupportedNotice` for Pro-only style/typeface and SVG-only effects (M10).
- Font Awesome attribution footer with a deep link to fontawesome.com (C4).

## Acceptance
- `/explorer` is reachable from the sample nav and the Index call-to-action.
- Selecting a Pro-only style/typeface shows an inline caveat instead of a blank/misleading preview,
  while the snippet still emits the selected values.
- The CC BY 4.0 attribution is visible wherever an icon detail is shown.
- No other sample page or route is affected.

## Dependencies
- IE-03 (project + host wiring), IE-04 (preview controls), IE-05 (snippet panel), IE-06 (toolbar/a11y).

## Outcome
- **Host wiring (verified, from IE-03):** `Jenin.FontAwesome.Blazor.slnx` includes the Explorer project;
  `Jenin.FontAwesome.Blazor.Sample.Shared.csproj` references it; `App.razor` links
  `_content/Jenin.FontAwesome.Blazor.Explorer/css/explorer.css`; `Routes.razor` adds the Explorer
  assembly to `AdditionalAssemblies`; `Program.cs` calls `.AddAdditionalAssemblies(…)` on
  `MapRazorComponents`; `NavMenu.razor` links `/explorer` under a "Reference" group; `_Imports.razor`
  imports the Explorer namespaces.
- **Getting-started entry point.** `Pages\Index.razor` gained a primary call-to-action banner
  ("Interactive icon explorer" + "Open the explorer" button) linking to `explorer`.
- **M10 — `Components\UnsupportedNotice.razor(.cs)`.** Renders an inline, `aria-live` caveat banner for
  the current preview selection:
  - Pro style (`Light`/`Thin`/`Duotone`) → "falls back to the equivalent classic solid style".
  - Pro typeface (`Sharp`/`Duotone`/`SharpDuotone`) → "falls back to the classic (non-sharp) style".
  - Optional webfont-mode warning for SVG-only effects (`WebFontMode` + `UsesSvgOnlyFeature`), matching
    the README feature matrix; the sample host runs in SVG mode so only the Pro caveat appears in
    practice. The snippet is always generated for the selected values regardless of the caveat.
  - Wired into `PreviewPanel` above the preview canvas; `UsesSvgOnlyFeature` derives from the preview
    transform/mask/duotone flags and `WebFontMode` is a `PreviewPanel` parameter (default `false`).
- **C4 — attribution footer.** `PreviewPanel` now shows "Icon metadata © Font Awesome, licensed CC BY
  4.0" with a "View {name} on fontawesome.com" deep link built from the selected icon name.
- **Styling.** `explorer.css` gained `.explorer-notice*` (banner + badge, themed via the existing CSS
  custom properties) and `.explorer-preview__attribution` styles.

## Evidence
- `dotnet build "Jenin.FontAwesome.Blazor.slnx" --no-restore -c Release` → **0 warnings, 0 errors**.
- `dotnet test tests\Jenin.FontAwesome.Blazor.Tests` → **292 passed** (net8.0 / net9.0 / net10.0).
- `pwsh ./build.ps1` → Build succeeded (0/0).
- `pwsh ./eng/verify-encoding.ps1 -Fix` → governed files normalised (new `.razor`/`.razor.cs` repaired).
- e2e (Release server):
  - `/explorer?icon=house&style=solid` → **200**; rendered HTML contains
    `explorer-preview__attribution`, `fontawesome.com/search`, and `CC BY 4.0`.
  - `/` → **200**; contains the new call-to-action (`Interactive icon explorer.`, `href="explorer"`,
    `Open the explorer`).
  - `/_content/…/css/explorer.css` → **200** (Development env) and contains `explorer-notice__badge`
    and `explorer-preview__attribution`.

## Notes
- The preview style/typeface are **not URL-addressable** yet (extended route keys were deferred in
  IE-02/IE-03), so the Pro caveat cannot be triggered from a URL in the current build; it is driven by
  the live control panel. This is consistent with the documented routing subset.
- `WebFontMode` defaults to `false` because this host configures Font Awesome SVG mode
  (`fontawesome-config-nest.js` sets `autoReplaceSvg: "nest"`), where transforms/masks/layers are
  active; the webfont warning is dormant but wired for hosts that opt into webfonts.
- Serving the RCL's `_content` static assets returns 500 in **Production** when running a
  `--no-build` Release binary (a pre-existing static-web-assets runtime-manifest quirk, unrelated to
  this task); assets serve correctly under the Development environment.
