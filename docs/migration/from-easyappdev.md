# Switching from EasyAppDev FA5 / FA6

Migrate from [`EasyAppDev.Blazor.Icons.FontAwesome5/6`](https://www.nuget.org/packages/EasyAppDev.Blazor.Icons.FontAwesome6)
(v2.0.4, FA6 based on Free 6.7.2, verified 2026-10-26) per-icon components to this library's
`Icons` presets (same shape) or `Svg` components (same trimming story, newer catalogue).

## 1. Swap the packages

```diff
- dotnet remove package EasyAppDev.Blazor.Icons.FontAwesome6
+ dotnet add package Jenin.FontAwesome.Blazor
+ dotnet add package Jenin.FontAwesome.Blazor.Icons   # same per-icon shape (optional)
+ dotnet add package Jenin.FontAwesome.Blazor.Svg     # trimmed inline SVG (optional)
```

```razor
@* _Imports.razor *@
- @using EasyAppDev.Blazor.Icons.FontAwesome6
+ @using Jenin.FontAwesome.Blazor.Icons   @* or .Svg *@
```

## 2. Mapping table

| EasyAppDev | Jenin.FontAwesome.Blazor | Notes |
|---|---|---|
| `<Fa6SolidHome />` | `<HouseIcon />` (Icons) or `<HouseSolidIcon />` (Svg) | Prefix (`Fa6Solid`) becomes style: `IconStyle.Solid` / `*SolidIcon`. |
| `<Fa6RegularStar />` | `<StarIcon IconStyle="IconStyle.Regular" />` or `<StarRegularIcon />` | Icons presets default to solid; pass the style explicitly. |
| `<Fa6BrandsDiscord />` | `<DiscordIcon IconStyle="IconStyle.Brands" />` or `<DiscordBrandsIcon />` | Brands live in the same preset set. |
| `<Fa5SolidHome />` (FA5 set) | `<HouseIcon />` | FA5→FA7 renames are rare; confirm the name in `IconNames` (v6 classes work unchanged in v7 per Font Awesome). |
| No configuration | `AddJeninFontAwesome()` (optional) + one-line asset setup | EasyAppDev inlines SVG (zero assets); our `Svg` package matches that. The core `Icon` path needs hosted FA assets instead (one-line setup in the README). |
| Trimming via physical files | Trimming via linker (`Icons`) or inlined paths (`Svg`) | Same promise, verified by `eng/verify-optional-trim.ps1` and the bundle-size dashboard. |
| Size/color via CSS | Icons presets: `IconSize`/`Color` params; `Svg`: `style` passthrough or wrapper CSS | Same CSS story on both sides; presets additionally offer typed animations, transforms, and composition. |

## 3. Before / after

```razor
@* Before (EasyAppDev FA6) *@
<Fa6SolidHome />
<Fa6RegularStar />
<Fa6BrandsDiscord />
```

```razor
@* After, same shape (verified by EasyAppDevMigrationTests) *@
<HouseIcon />
<StarIcon IconStyle="IconStyle.Regular" />
<DiscordIcon IconStyle="IconStyle.Brands" />
```

```razor
@* After, trimmed inline SVG instead (no FA assets at all) *@
<HouseSolidIcon />
<StarRegularIcon />
<DiscordBrandsIcon />
```

## 4. Deliberate differences

- **Catalogue version.** EasyAppDev FA6 is Free 6.7.2; ours is Free 7.3.1 (newer icons, square
  canvases). A handful of names changed between majors — the compiler will not catch a
  renamed string, so search `IconNames` for each migrated name once.
- **Two per-icon shapes.** EasyAppDev has one (inline SVG). Ours has two: `Icons` presets
  (typed names over the FA engine) and `Svg` components (self-contained paths). Prefer `Svg`
  when you want zero FA assets; prefer `Icons` when you want engine features (layers, masks,
  duotone) with typed names.
- **Composition.** Animations, transforms, layers, stacks, and duotone parameters have no
  EasyAppDev equivalent — they are new capabilities after migration.

## 5. Checklist

- [ ] Every `Fa5*/Fa6*` component mapped to a preset or Svg component (names confirmed in `IconNames`).
- [ ] `@using` directives swapped; old packages removed.
- [ ] Chosen shape deliberately: `Icons` (engine features) vs `Svg` (zero assets).
- [ ] If core `Icon` is used anywhere, the one-line asset setup is wired.
