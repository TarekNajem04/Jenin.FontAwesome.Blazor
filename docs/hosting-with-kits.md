# Hosting with Font Awesome Kits

Kits are Font Awesome's recommended hosting: version pinning, managed delivery, and Pro
assets without self-hosted files. This library treats a kit like any other hosted asset
source — no special component is needed — with one setup call.

## Setup

```csharp
builder.Services.AddJeninFontAwesome(options => options.UseFontAwesomeKit("your-kit-code"));

var app = builder.Build();
// ...
app.MapJeninFontAwesomeAssets(); // serves the generated mode config; no asset URLs needed
```

`UseFontAwesomeKit` points the runtime tag at `https://kit.fontawesome.com/<code>.js` and
drops the stylesheet link (kits serve their own CSS). The generated mode-config script
still applies on top — it only sets `autoReplaceSvg`, which kits respect. Pin the kit
version in the [kit settings](https://fontawesome.com/kits); the tag itself carries no
version.

Without the one-line setup, the equivalent manual tags are:

```html
<script src="https://kit.fontawesome.com/<your-kit-code>.js" crossorigin="anonymous"></script>
```

## What a kit changes (and what it does not)

- **Pro assets through the same markup.** Light, Thin, Duotone, and Sharp requests resolve
  when the kit includes Pro; on Free hosts the same markup falls back exactly as described
  in [pro-setup.md](pro-setup.md). See `eng/verify-pro-render.ps1` for the repeatable render verification
  (`eng/verify-pro-render.ps1`).
- **Custom kit icons.** Glyphs uploaded to a kit are plain `fa-*` names at runtime.
  Because custom names are not in the Free catalogue, JFA1001 warns by design. Either write
  the anchor as raw HTML (which the analyzer deliberately leaves alone — see the Kit demo
  page in the samples), or keep the typed component and suppress with a justification:

  ```csharp
  // Kit-custom glyph: suppressed with reason — the kit, not the Free catalogue, owns it.
  #pragma warning disable JFA1001
  var custom = new Icon { IconName = "fa-acme-widget", IconStyle = IconStyle.Solid };
  #pragma warning restore JFA1001
  ```

  Suppress per usage (or per project when the whole app is kit-hosted), never blanket-wide:
  unknown names anywhere else are real findings (typos render nothing).

## Auto-subsetting and the typed API

Kits decide what to deliver based on the kit configuration, not on this library: our
components emit standard FA anchors (`<i class="fa-…">`), so a kit treats them exactly
like hand-written markup. No library-specific step is required to stay subset-friendly —
referencing fewer distinct icons keeps any payload small, whichever host serves it.

## Live-check recipe (owner-run, no committed secrets)

The committed suite never touches a live kit (CI stays offline-safe: the Kit demo page in
the samples uses a non-functional `demo-kit-code-0000` and `kits.spec.ts` asserts only the
emitted tag). To verify against a real kit:

1. Temporarily point a local run at the kit: `options.UseFontAwesomeKit("<real-code>")`
   (uncommitted edit) or set the demo page's `JavaScriptUrl` to the kit URL.
2. `dotnet run` the WebApp sample, open `/kit-demo`, and confirm: the kit script loads
   (network panel), the config endpoint still serves the mode config, custom kit glyphs
   render, and `JFA1003` stays silent for kit-covered Pro styles.
3. Revert the edit. Never commit kit codes or licensed bytes.
