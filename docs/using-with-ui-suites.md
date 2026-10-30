# Using this library with UI suites

Most Blazor apps already live inside MudBlazor or Blazorise. Instead of fighting those
components, map typed Font Awesome names into their icon slots with the adapter packages —
verified end to end by `eng/verify-suites.ps1` (real suite components, vendored FA CSS,
no live dependencies).

| Suite | Package | Slot | Mapping |
|---|---|---|---|
| MudBlazor | `Jenin.FontAwesome.Blazor.MudBlazor` | `MudIcon` / `MudIconButton.Icon` / `MudNavLink.Icon` (`string`) | `MudIconMapper.ToFontClass(name, style)` → `fa-solid fa-house` |
| Blazorise | `Jenin.FontAwesome.Blazor.Blazorise` | `Icon.Name` (plus `IconStyle`/`IconSize`) | `JeninBlazoriseIcon` maps `IconStyle`/`IconSize` to the Blazorise enums |

```razor
@using Jenin.FontAwesome.Blazor.MudBlazor
@using Jenin.FontAwesome.Blazor.Blazorise

<MudIcon Icon="@MudIconMapper.ToFontClass(IconNames.Solid.House)" Title="Home" />
<JeninMudIcon IconName="fa-house" IconStyle="IconStyle.Solid" Title="Home" />

<JeninBlazoriseIcon IconName="fa-phone" IconStyle="Jenin.FontAwesome.Blazor.IconStyle.Solid" IconSize="Jenin.FontAwesome.Blazor.IconSize.Large" />
```

Blazorise needs its Font Awesome icon provider registered once (`AddFontAwesomeIcons()`
plus a UI provider such as `AddBootstrap5Providers()`); MudBlazor needs nothing beyond the
FA stylesheet your app already serves.

## What maps cleanly, what stays native

- **Names**: every `fa-*` name (including custom kit glyphs and legacy aliases) passes
  through untouched; the adapter never validates names, so nothing new can warn.
- **Styles**: MudBlazor gets the FA class prefix (`fa-solid`, `fa-brands`, …);
  Blazorise gets its own `IconStyle` where one exists (`Solid`, `Regular`, `Light`,
  `DuoTone`). `Thin`/`Brands` have no Blazorise counterpart and fall back to the provider
  default — documented, not silent.
- **Sizes**: Blazorise maps the shared steps (`ExtraSmall`, `Small`, `Large`,
  `X02`–`X10`); everything else (including MudBlazor sizes, which are palette-like)
  stays on the suite default — size with CSS instead.
- **Native-only**: animations, triggers, duotone layering, transforms, and masks have no
  suite equivalent. Compose them with this library's own `Icon` (or the
  `Blazorise.IconStyle`/`IconSize` you already pass) — the adapters deliberately cover
  only the mapping both sides understand.

The fixture behind the gate (`tests/e2e/fixtures/suite-adapters`, pages `/mud` and
`/blazorise`) doubles as copy-paste sample code.
