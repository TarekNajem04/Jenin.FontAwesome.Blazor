# Switching from raw Font Awesome class strings

Migrate hand-written `<i class="fa-solid fa-house fa-2x">` anchors (FA 6 or 7) to this
library's typed API. Font Awesome's own [upgrade guide](https://fontawesome.com/docs/web/setup/upgrade/)
confirms v6 class names render unchanged in v7, so this migration is mechanical: same tokens,
now compiler-checked.

## 1. What changes, what stays

The FA assets (CSS/JS/webfonts) stay exactly where they are. Only the anchors move into
components — optionally via the one-line setup in the README instead of hand-written tags.

## 2. Mapping table

| Raw FA classes | Jenin.FontAwesome.Blazor | Notes |
|---|---|---|
| `<i class="fa-solid fa-house">` | `<Icon IconName="fa-house" IconStyle="IconStyle.Solid" />` | Style prefix → `IconStyle`; name → `IconName`. Prefer `IconNames.Solid.House`. |
| `<i class="fa-regular fa-star">` | `<Icon IconName="fa-star" IconStyle="IconStyle.Regular" />` | Same rule. |
| `<i class="fa-brands fa-github">` | `<Icon IconName="fa-github" IconStyle="IconStyle.Brands" />` | Same rule. |
| `fa-2x … fa-10x` | `IconSize="IconSize.X02 … IconSize.X10"` | Exact token mapping, verified. |
| `fa-lg`, `fa-sm`, `fa-xs`, `fa-xl`, `fa-2xl` | `IconSize.Large/Small/ExtraSmall/ExtraLarge/ExtraLarge_2` | Same tokens. |
| `fa-spin`, `fa-beat`, … | `Animation="@Animate.Spin()"` … | Typed animation factories replace class names; duration/delay become `TimeSpan`s. |
| `fa-rotate-90`, `fa-flip-horizontal`, `data-fa-transform="…"` | `RotateAngle`, `Flip…`, or the immutable `Transform` model | Power transforms become composable values instead of strings. |
| `fa-duotone fa-house` (+ `fa-primary`/`fa-secondary`) | `Typeface="Typeface.Duotone"` + `PrimaryColor`/`SecondaryColor` … | Duotone becomes parameters. |
| `fa-stack`, `fa-layers`, `fa-ul`/`fa-li` | `IconStackContainer`, `IconLayersContainer` / `TextLayer` / `CounterLayer`, `IconList` | Stacks, layers, and lists become components. |
| `title="Home"` | `Title="Home"` | Same attribute; also flips the icon from decorative to meaningful (`role="img"`). |
| `aria-label="..."` / `aria-hidden` | `AriaLabel` / `Decorative` | First-class parameters instead of hand-written ARIA. |
| Per-icon reuse (`house` in 12 places) | `<HouseIcon />` preset or `IconNames.Solid.House` | One component / one constant; renames become compiler errors. |

## 3. Before / after

```html
<!-- Before (raw FA6 classes, renders identically under FA7 assets) -->
<i class="fa-solid fa-house fa-2x" title="Home"></i>
```

```razor
@* After (typed, verified by RawClassMigrationTests) *@
<Icon IconName="fa-house"
      IconStyle="IconStyle.Solid"
      IconSize="IconSize.X02"
      Title="Home" />
```

```html
<!-- Before -->
<ul class="fa-ul">
    <li><span class="fa-li"><i class="fa-solid fa-star"></i></span>First</li>
</ul>
```

```razor
@* After *@
<IconList>
    <IconListItem>
        <ItemIcon><Icon IconName="fa-star" IconStyle="IconStyle.Solid" /></ItemIcon>
        <ItemContent>First</ItemContent>
    </IconListItem>
</IconList>
```

## 4. Deliberate differences

- **FA7 baseline.** The catalogue is FA Free 7.3.1. v6 names work unchanged in v7 (Font Awesome's
  words, not ours), but v7-only names obviously need v7 assets, and Pro-only glyphs still need
  licensed assets (never bundled).
- **Decorative by default, explicitly.** FA7 made icons decorative by default and dropped
  `sr-only`/`autoA11y`; our bare `Icon` matches that (`aria-hidden`, `role="presentation"`).
  Reach for `Title`/`AriaLabel` for meaningful icons — the same semantic FA now documents.
- **Strings become values.** Animations, transforms, and sizes stop being strings, so typos
  become compile errors and values compose (`Transform` chaining, `Animate` factories).

## 5. Checklist

- [ ] Every `fa-*` anchor converted (search for `class="fa-` and `fa-stack`/`fa-ul`).
- [ ] Sizes, animations, and transforms converted to typed values (no leftover class strings).
- [ ] `title`/`aria-*` carried into `Title`/`AriaLabel`/`Decorative`.
- [ ] Repeated icons consolidated on presets or `IconNames` constants.
- [ ] Assets confirmed at v7 (v6 works, v7 unlocks new names).
