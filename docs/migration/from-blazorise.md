# Switching from Blazorise icons

Migrate from the [Blazorise `Icon` component with the Font Awesome provider](https://blazorise.com/docs/extensions/icons)
(docs v2.4.x, verified 2026-10-26) to this library's typed API.

## 1. Remove the Blazorise FA provider wiring

```diff
- dotnet remove package Blazorise.Icons.FontAwesome
- <link href="_content/Blazorise.Icons.FontAwesome/v6/css/all.min.css?v=2.4.1.0" rel="stylesheet" />
+ <!-- Host FA 7 assets once (or use the one-line setup in the README) -->
+ <link href="/fontawesome/css/all.css" rel="stylesheet" />
```

```csharp
// Program.cs
- builder.Services.AddFontAwesomeIcons();
+ builder.Services.AddJeninFontAwesome();
```

Keep `AddBlazorise(...)` and every other provider: only the FA icon provider moves.

## 2. Mapping table

| Blazorise (FA provider) | Jenin.FontAwesome.Blazor | Notes |
|---|---|---|
| `<Icon Name="IconName.Mail" />` | `<Icon IconName="fa-envelope" />` | Shared-enum names resolve per provider on their side; convert to the concrete FA name. Prefer `IconNames.Solid.Envelope`. |
| `<Icon Name="Blazorise.Icons.FontAwesome.FontAwesomeIcons.Phone" />` | `<Icon IconName="fa-phone" IconStyle="IconStyle.Solid" />` | Provider constants map 1:1 to our `IconNames.*` constants (verified for the fixture set). |
| `<Icon Name="@("fa-phone")" />` (custom string) | `<Icon IconName="fa-phone" />` | Same string, now with style/size/type checking around it. |
| `IconStyle="IconStyle.Regular"` (+ `Solid`, `Light`, `DuoTone`) | `IconStyle="IconStyle.Regular"` (same names) | Blazorise styles fall back per provider; ours always emit the requested FA token (`fa-light`/`fa-duotone` need matching host assets). |
| `IconSize="IconSize.Small"` / `Large` / `x2…x10` | `IconSize.Small` / `Large` / `X02…X10` | Same granularity (`fa-sm`, `fa-lg`, `fa-2x…fa-10x`), verified token-for-token. |
| `options.IconStyle` / `options.IconSize` in `AddBlazorise` | `AddJeninFontAwesome(o => { o.IconStyle = …; o.IconSize = …; })` or nested `<FontAwesomeDefaults>` | Same global-default idea; ours additionally cascades per subtree with documented precedence. |
| Other providers (Bootstrap, Material, …) | *(no equivalent)* | Different families; keep Blazorise for those. |

## 3. Before / after

```razor
@* Before (Blazorise FA provider) *@
<Icon Name="IconName.Mail" IconStyle="IconStyle.Regular" IconSize="IconSize.Large" />
```

```razor
@* After (typed, verified by BlazoriseMigrationTests) *@
<Icon IconName="fa-envelope"
      IconStyle="IconStyle.Regular"
      IconSize="IconSize.Large" />
```

```razor
@* Before *@
<Icon Name="Blazorise.Icons.FontAwesome.FontAwesomeIcons.Phone" />
```

```razor
@* After *@
<Icon IconName="@IconNames.Solid.Phone" IconStyle="IconStyle.Solid" />
```

## 4. Deliberate differences

- **Provider abstraction vs FA-specific API.** Blazorise trades FA depth for provider breadth
  (layers, masks, power transforms, duotone parameters have no Blazorise equivalent to map
  from — they are new capabilities after migration, see the README catalogue).
- **Assets.** The Blazorise FA provider ships v6 assets; ours is FA 7.3.1 and host-owned.
  v6 class names work unchanged in v7 (per Font Awesome's upgrade guide).
- **Global defaults.** Blazorise sets them inside `AddBlazorise`; ours live in
  `AddJeninFontAwesome` and can be narrowed per subtree with `<FontAwesomeDefaults>`.

## 5. Checklist

- [ ] `Blazorise.Icons.FontAwesome` package removed; v6 provider CSS link gone.
- [ ] Every `Icon Name=` converted to a concrete `fa-*` name (`IconNames.*` preferred).
- [ ] `IconStyle`/`IconSize` values carried over (verified token-for-token in tests).
- [ ] Global options moved to `AddJeninFontAwesome` (or subtree defaults).
- [ ] Non-FA providers untouched.
