# Switching from Blazicons.FontAwesome

Migrate from [`Blazicons.FontAwesome`](https://github.com/kyleherzog/Blazicons.FontAwesome)
(`<Blazicon Svg="FontAwesomeSolidIcon.Baseball" />`, verified 2026-10-26) to this library's
`Svg` components (same zero-asset inline-SVG story, FA 7.3.1 catalogue) or `Icons` presets.

## 1. Swap the packages

```diff
- dotnet remove package Blazicons.FontAwesome
- dotnet remove package Blazicons.FontAwesome.Solid   # if referenced directly
+ dotnet add package Jenin.FontAwesome.Blazor.Svg    # same trimming story (optional)
```

```razor
@* _Imports.razor *@
- @using Blazicons
+ @using Jenin.FontAwesome.Blazor.Svg
```

No CSS, JS, webfonts, or config scripts are needed in either world — delete nothing else.

## 2. Mapping table

| Blazicons.FontAwesome | Jenin.FontAwesome.Blazor.Svg | Notes |
|---|---|---|
| `<Blazicon Svg="FontAwesomeSolidIcon.Baseball" />` | `<BaseballSolidIcon />` | Style suffix moves into the component name (`Solid`/`Regular`/`Brands`). |
| `<Blazicon Svg="FontAwesomeRegularIcon.Calendar" />` | `<CalendarRegularIcon />` | Same rule. |
| `<Blazicon Svg="Ionicon.ShapesSharp.WithColor("#f00").WithSize("200%")" />` | `<... style="color: #f00; font-size: 200%;" />`, or wrapper CSS | Blazicon has exactly one parameter (`Svg`) by design; ours take `Title`/`CssClass` plus `style`/`width`/`height` passthrough, and the same wrapper-CSS escape hatch. |
| Sizing via surrounding `font-size` | Same: surrounding `font-size` inherits | Both render `1em`-scaled SVG; sizing behavior is identical. |
| Trimming (unused icons dropped) | Same: unreferenced components trim away | Same promise, verified by `eng/verify-optional-trim.ps1` (retention/removal contract) and the bundle-size dashboard. |

## 3. Before / after

```razor
@* Before (Blazicons.FontAwesome) *@
<Blazicon Svg="FontAwesomeRegularIcon.Calendar" />
<Blazicon Svg="FontAwesomeSolidIcon.Baseball" />
```

```razor
@* After (verified by BlaziconsMigrationTests) *@
<CalendarRegularIcon />
<BaseballSolidIcon />
```

```razor
@* Before: fluent styling helpers *@
<Blazicon Svg='Ionicon.ShapesSharp.WithColor("#f00").WithSize("200%")' />
```

```razor
@* After: style passthrough (non-FA families stay on Blazicons) *@
<BaseballSolidIcon style="color: #f00; font-size: 200%;" />
```

## 4. Deliberate differences

- **One shape each.** Blazicons has one component with an `Svg` value; ours has one component
  per (icon, style) with `Title`/`CssClass` and `style`/`width`/`height` passthrough. The
  `Svg` value-object style has no direct equivalent — prefer the named component.
- **Catalogue version.** Their FA set tracks FA 6; ours is FA 7.3.1. Confirm each migrated name
  in the generated set (the compiler catches a missing component, unlike a missing string).
- **Beyond inline SVG.** If you later need layers, masks, duotone, or power transforms, the same
  names exist as `Icons` presets / core `Icon` over hosted FA assets — one catalogue, two
  runtimes. Non-FA Blazicon families (Bootstrap, Lucide, …) have no equivalent here.

## 5. Checklist

- [ ] Every `FontAwesome*Icon.*` value mapped to a `*SolidIcon`/`*RegularIcon`/`*BrandsIcon` component.
- [ ] `.WithColor()`/`.WithSize()` calls converted to `Color`/`IconSize` (or wrapper CSS).
- [ ] `@using Blazicons` replaced; old packages removed.
- [ ] Non-FA `Blazicon` usages intentionally left alone.
