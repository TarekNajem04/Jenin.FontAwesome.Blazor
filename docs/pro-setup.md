# Pro setup (licensed Font Awesome)

This library works with **both** Free and licensed Pro assets with no code changes: the
components emit the same FA 7 markup either way, and the Font Awesome engine resolves what
the loaded assets provide. No license keys, kit tokens, or Pro files are ever bundled,
committed, or required to build this repo.

## Point the one-line setup at Pro assets

```csharp
app.MapJeninFontAwesomeAssets(options => {
    options.AssetMode = FontAwesomeAssetMode.SvgNest; // or Webfont
    options.StyleSheetUrl = "/fontawesome-pro/css/all.css"; // your hosted Pro CSS (or kit CSS)
    options.JavaScriptUrl = "https://kit.fontawesome.com/<your-kit-code>.js"; // or hosted Pro JS
});
```

Self-hosted Pro works the same way — point both URLs at your hosted `css/all.css`
(including the Pro weights) and `webfonts/`. The generated config endpoint, the head tags,
and the trimming/code-fix behavior are identical for both editions.

## What changes with Pro assets

| Surface | Free hosts | Licensed Pro hosts |
|---|---|---|
| `IconStyle.Light` / `Thin` | Renders nothing (verified: `content: none`, page font) | Renders the light/thin glyph |
| `IconStyle.Duotone` (+ `Typeface.Duotone`/`SharpDuotone`) | Falls back to the classic solid glyph, single unlayered path | Renders layered `fa-primary`/`fa-secondary` glyphs |
| `Typeface.Sharp` | Falls back to the classic glyph | Renders the sharp family |
| `IconStyle.Solid`/`Regular`/`Brands` | Full rendering | Full rendering |

The [bundle-size dashboard](https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor#bundle-size-dashboard) and the JFA1003 analyzer diagnostic (`Pro-only
style without licensed assets`) both assume this contract: Pro-only styles warn at build
time unless you suppress them per project once licensed
(`dotnet_diagnostic.JFA1003.severity = none`).

## Verify your Pro setup

Run the repeatable render proof against your own assets (never committed):

```powershell
$env:FA_PRO_ASSETS = 'C:\fa-pro-kit'   # a folder with css/ + webfonts/ (+ js/)
pwsh ./eng/verify-pro-render.ps1
```

Without `FA_PRO_ASSETS` the same script verifies the Free-only subset against the vendored
Free 7.3.1 assets (run it locally — neither leg is a CI gate). The script auto-detects the edition from the
present webfonts, serves them locally, boots the `tests/e2e/fixtures/pro-matrix` fixture in
nest and webfont modes, and asserts the family × style × mode matrix in a real browser
(see `tests/e2e/promatrix.spec.ts`). Record licensed-Pro runs for future reference.
