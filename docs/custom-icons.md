# Custom icons

Brand and product teams can register their own glyphs once and use them through the same
typed API, search, and snippets as Font Awesome icons. Consumers own their glyphs
(artwork and licensing); the library never bundles, redistributes, or phones home with
custom path data.

## Register once

```csharp
builder.Services.AddJeninFontAwesome(options => options.RegisterCustomIcon(
    "fa-acme-widget",                    // IconName form (fa- prefix required)
    "M256 8L8 488H504Z",                 // SVG path data (d attribute)
    viewBox: "0 0 512 496",              // optional, defaults to "0 0 512 512"
    label: "Acme Widget",                // optional, defaults to the name
    searchTerms: ["acme", "widget"]));   // optional search keywords
```

## Use everywhere

```razor
<Icon IconName="fa-acme-widget" />
```

A registered name renders as self-contained inline SVG (`<svg viewBox …><path …/></svg>`,
`1em` box, `currentColor` fill, `data-icon` set like the engine's own output) in **every**
hosting mode and **regardless of which Font Awesome assets are loaded** — no engine, no
webfonts, and no kits are involved. Standard composition still applies (`CssClass`,
`Title`/`AriaLabel` (+`<title>`), `AdditionalAttributes` including `width`/`height`, and
the FA CSS utilities such as `fa-2x` when the stylesheet is present). FA-specific styling
(`IconStyle`, `Typeface`, masks, duotone) is intentionally not applied: there is no FA
glyph to style.

Unregistered unknown names keep flowing to Font Awesome untouched (a kit may still
resolve them at runtime) and keep warning via JFA1001 at compile time — register the
glyph, or suppress that single usage with a justification comment.

## Search, picker, and snippets

Registered glyphs appear in `IconMetadataProvider` search results and in the
`IconPicker` component, distinctly marked (`IconMetadata.IsCustom`, `is-custom` CSS class
on the picker item). They are style-agnostic: style filters never exclude them.

```csharp
// Discoverability is the same API as FA icons:
var provider = IconMetadataProvider.Default.WithCustomIcons(options.CustomIcons.Values);
var hit = provider.Find("fa-acme-widget"); // IsCustom == true
var snippet = options.CustomIcons["fa-acme-widget"].ToRazorSnippet();
// => <Icon IconName="fa-acme-widget" />
```

`AddJeninFontAwesomeIconPicker` wires this automatically: when `AddJeninFontAwesome` ran
first, the injected provider already includes the registered customs.

## Sprite hosts

If you serve icons as a single SVG sprite, add your glyph as a `<symbol id="fa-acme-widget">`
to that file: `<SpriteIcon IconName="fa-acme-widget" Sprite="/custom-sprite.svg" />`
resolves it with no registration needed. Sprite generation from FA fonts
(`eng/fa_sprite.py`) is unchanged.

## Custom kit icons

Kit-uploaded glyphs are the hosted counterpart of this feature: same `IconName` usage,
same JFA1001 behavior. See [hosting-with-kits.md](hosting-with-kits.md).
