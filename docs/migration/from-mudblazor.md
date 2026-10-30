# Switching from MudBlazor icons

Migrate Font Awesome usage from [MudBlazor](https://mudblazor.com/components/icons) (`MudIcon` /
`MudIconButton` with FA class strings) to this library's typed API. Verified against the
MudBlazor docs and `MudIcon.razor` source on 2026-10-26.

> Scope: only migrate your **Font Awesome** usage. MudBlazor's built-in Material icons
> (`Icons.Material.Filled.*`) are a different icon family with no FA equivalent — keep
> MudBlazor for those. The two libraries coexist: FA glyphs move here, Material glyphs stay.

## 1. Remove the MudBlazor FA wiring

MudBlazor renders FA class strings inside its own `<span>`/`<svg>` shell, so nothing from it is
reused. Keep the MudBlazor package for the rest of the suite; only the FA parts move.

```diff
- <link href="https://use.fontawesome.com/releases/v5.14.0/css/all.css" rel="stylesheet" />
+ <!-- Host FA 7 assets once (or use the one-line setup in the README) -->
+ <link href="/fontawesome/css/all.css" rel="stylesheet" />
```

```csharp
// Program.cs — replace nothing: MudBlazor registration stays. Add:
builder.Services.AddJeninFontAwesome();
```

## 2. Mapping table

| MudBlazor | Jenin.FontAwesome.Blazor | Notes |
|---|---|---|
| `<MudIcon Icon="fas fa-dragon" />` / `FontClass="fas fa-dragon"` | `<Icon IconName="fa-dragon" IconStyle="IconStyle.Solid" />` | `fas` → `IconStyle.Solid`, `far` → `Regular`, `fab` → `Brands`. Prefer `IconNames.Solid.Dragon` over the string. |
| `<MudIcon Icon="@Icons.Material.Filled.Home" />` | *(no equivalent)* | Different family; keep MudBlazor. |
| `Size="Size.Small"` / default / `Size="Size.Large"` | `IconSize.Small` / default / `IconSize.Large` (`fa-sm`, `fa-lg`) or `IconSize.X02…X10` (`fa-2x…`) | MudBlazor sizes are relative steps; ours emit exact FA size tokens. |
| `Color="Color.Success"` (theme palette) | `Color="green"` / `Color="var(--mud-palette-success)"` | Ours takes any CSS color; reference MudBlazor CSS variables to stay on-palette. |
| `Title="Dragon"` | `Title="Dragon"` | Same behavior: `title` attribute plus tooltip. Ours additionally makes the icon meaningful (`role="img"`) instead of decorative. |
| `<MudIconButton Icon="fas fa-bug" ... />` | `<IconButton IconName="fa-bug" ... />` (+ `Label` for a visible caption) | Ours requires `Title`/`AriaLabel` when icon-only, enforced by design. |
| `Class="..."` / `Style="..."` | `CssClass` / `Style` passthrough, or `AdditionalAttributes` | Same escape hatches, same precedence (explicit parameter wins). |

## 3. Before / after

```razor
@* Before (MudBlazor) *@
<MudIcon Icon="fas fa-dragon" Size="Size.Large" Color="Color.Success" Title="Dragon" />
```

```razor
@* After (typed, verified by MudBlazorMigrationTests) *@
<Icon IconName="fa-dragon"
      IconStyle="IconStyle.Solid"
      IconSize="IconSize.Large"
      Color="green"
      Title="Dragon" />
```

```razor
@* Before *@
<MudIconButton Icon="fas fa-bug" Color="Color.Error" aria-label="Report a bug" />
```

```razor
@* After *@
<IconButton IconName="fa-bug" IconStyle="IconStyle.Solid" Color="red" AriaLabel="Report a bug" />
```

## 4. Deliberate differences

- **No theme-palette coupling.** Our `Color` is a CSS color, not a palette enum. Point it at
  MudBlazor's CSS variables (`var(--mud-palette-*)`) when both libraries share a page.
- **Accessibility default.** A bare MudBlazor FA icon is a plain span; a bare ours is explicitly
  decorative (`aria-hidden`, `role="presentation"`). Add `Title`/`AriaLabel` for meaningful icons.
- **FA version.** The docs examples above pin FA 5. Our catalogue is FA 7.3.1; v6 class names
  work unchanged in v7 (per Font Awesome's upgrade guide), and Pro-only glyphs stay
  host-licensed as before.

## 5. Checklist

- [ ] Every `fas|far|fab fa-*` string has an `IconName` + `IconStyle` pair (or an `IconNames.*` constant).
- [ ] Material-glyph usages (`Icons.Material.*`) intentionally left on MudBlazor.
- [ ] `Size` steps converted to exact `IconSize` tokens and eyeballed once.
- [ ] `Color="Color.*"` converted to CSS colors (palette variables where shared).
- [ ] Icon-only buttons carry `Title` or `AriaLabel`.
- [ ] FA stylesheet points at v7 assets; the v5 link is gone.
