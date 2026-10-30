# Migrating from 1.x to 2.0

`Jenin.FontAwesome.Blazor` 2.0 is a **generation reset**. It is intentionally **not backward
compatible** with 1.x: removed members are not shimmed, and there are no `[Obsolete]` bridges.
This guide lists everything you must change when moving a host application from 1.x to 2.0.

> Scope: this is an application-migration guide. For the internal architecture rationale see the
> project governance records; for everyday usage see [README.md](README.md) and for release notes see
> [CHANGELOG.md](CHANGELOG.md).

## 1. Prerequisites and packaging

| Item | 1.x | 2.0 |
|---|---|---|
| Target frameworks | .NET 8 (and earlier) | **.NET 8.0, 9.0, and 10.0** |
| Package version | `1.x` | `2.0.0` |
| Hosting models | various | Blazor WebAssembly, Blazor Server, and Blazor Web App (all four render modes) |
| Font Awesome | FA 6.x typically | markup targets **FA 7.x**; samples ship FA Free 7.3.1 |

Steps:

1. Update `<TargetFramework>` to `net8.0`, `net9.0`, or `net10.0`.
2. Update the package reference to `2.0.0`.
3. Re-apply the CSS/JS setup from the [README](README.md#2-wire-the-assets-one-line--one-component) —
   the shipped JavaScript asset names changed (see §5).

## 2. Namespaces

The public namespaces are the following; use the canonical imports:

```razor
@using Jenin.FontAwesome.Blazor
@using Jenin.FontAwesome.Blazor.Animations
@using Jenin.FontAwesome.Blazor.Components
@using Jenin.FontAwesome.Blazor.Transformations
```

The 1.x helper namespace `Jenin.FontAwesome.Blazor.Extensions` is **no longer public** — its types
were internalized (see §6). Remove that `@using` when upgrading; nothing you can consume lived there.

## 3. Animation API

1.x exposed multiple animation interfaces with fluent-unit operations. 2.0 consolidates everything
onto immutable value objects plus a single factory (`Animate`); each animation has its own options
type deriving from the shared `AnimationOptions`.

| 1.x | 2.0 |
|---|---|
| `Animation` interface family (`IAnimation`, per-type interfaces) | single abstract `Animation` base record over `Animation<TSelf, TOptions>` |
| one shared options bag for every animation | per-animation options types deriving from `AnimationOptions` (e.g. `BeatOptions`, `BounceOptions`) |
| `DurationUnit` and unit-based delay/duration | `TimeSpan` (`Animate.*().SetDelay(...)`, `.SetDuration(...)`) |
| `Animation.*` static helpers / per-type `Animate` statics | `Animate.Beat/Fade/BeatFade/Bounce/Flip/Flip360/Shake/Spin/SpinSnap/Buzz/Float/Jello/Swing/Wag` |
| iteration count required positive | non-negative; omission means Font Awesome's default `infinite` |

Example:

```razor
<Icon IconStyle="IconStyle.Solid"
      IconName="fa-heart"
      Animation="@Animate.Beat(new BeatOptions { Scale = 1.2f, Duration = TimeSpan.FromSeconds(2) })" />

<Icon IconStyle="IconStyle.Solid"
      IconName="fa-sync"
      Animation="@Animate.Spin().SetDelay(TimeSpan.FromSeconds(1)).SetDuration(TimeSpan.FromSeconds(2)).SetIterationCount(3)" />
```

`Icon.Animation` is typed `Animation?`.

## 4. Transformation API

| 1.x | 2.0 |
|---|---|
| `TransformExtensions` / static `Transform.*` factories / `Transform.Apply` | immutable `Transform` with `Add(...)` plus instance operations |
| mutable transform builders | immutable value objects (`new Transform().Grow(6).Rotate(-30)`) |

```razor
<Icon IconName="fa-seedling" Transform="@(new Transform().Shrink(8))" />
```

## 5. JavaScript and static assets

The shipped assets were renamed and reduced to exactly two:

| 1.x asset | 2.0 replacement |
|---|---|
| `fontawesome-Svg[nest]-config.js` | `fontawesome-config-nest.js` (recommended) |
| `fontawesome-Svg[false]-config.js` | `fontawesome-config-webfont.js` |
| `fontawesome-config-utilities.js`, `fontawesome-utilities.js` | removed (no library interop) |

Load the config **before** your Font Awesome runtime script:

```html
<script src="_content/Jenin.FontAwesome.Blazor/js/fontawesome-config-nest.js" type="text/javascript"></script>
<script src="/your-path-to-fontawesome/js/all.min.js" type="text/javascript"></script>
```

The library ships **no** Font Awesome CSS/JS/fonts/SVGs and performs **no** .NET→JS interop. Host
your own Font Awesome files (or use a CDN).

## 6. Removed and renamed members

2.0 removed or renamed the following 1.x surface. There are no compatibility shims.

- **Removed types/members:** `ElementInverseable`, `AutoReplaceSvg`, `ProcessingSvgMethod`,
  `ForceRebuildRenderTree`, `Clicked`, `DoubleClicked`, the event-argument wrappers
  (`ElementEventArgs<T>`, `ElementMouseEventArgs`), `IconId`/DOM ids, `HtmlElementReference`,
  `OldClass`/`OldStyle`, `IsRendered`, `JSRuntime` injection, `BuildAdditionalAttributesAsync`,
  `OwningComponentBase` usage, `DurationUnit`, the animation interface family (`IAnimation`,
  `IFluentAnimation`, `IAnimationDelay`/`IAnimationDuration` and their `…Operations`), `Animation.*`
  statics, per-type `Animate` statics, `TransformExtensions`, and the static `Transform.*` factories
  (use the `Transform` instance surface instead).
- **Renamed:** `ElemenWithTransformations` → `ElementWithTransformations`;
  `Typefaces` → `Typeface`; `DuoTone` → `Duotone`; `Tkey` → `TKey`.
- **Internalized:** `ValueAttribute`, `DictionaryExtensions`, `EnumExtensions`, and the
  `PositioningKind`/`ScalingKind` enums (the helper `…Extensions` namespace is no longer public as a
  result). Use the public `Transform` surface instead of internal helpers.
- **Tightened extensibility:** component method-serialization internals that were `protected` are now
  `private protected`/`internal`. Derive only from the documented public/protected hooks — the base
  component types and the `Animation`/`Transformation` model extension points remain public/protected.
- **Additive annotations:** `TextLayer.Text` and `CounterLayer.Text` now carry `[EditorRequired]`, so
  omitting `Text` in Razor raises the `RZ2012` advisory (the `string?` signature and C# callers are
  unchanged). `Icon.IconName` now documents its required identifier contract — a `fa-` prefixed icon
  name — but is intentionally **not** `[EditorRequired]`, because that annotation would propagate to
  the derived preset components.

## 7. Behavior changes

2.0 normalizes values **at construction** and makes serialization **pure** — 1.x clamped during
serialization, which mutated transform/options state on every render. This changes the outcome for
out-of-range and fractional inputs:

- **Transformations clamp at construction** (`Grow`/`Shrink`/`Up`/`Down`/`Left`/`Right` to `[1, 16]`;
  values emit with two decimals, invariant). `Scaling` **no longer rounds**, so `Shrink(3.5)` emits
  `shrink-3.50` (1.x rounded fractional scaling to an integer) and `Up(0.5)` emits `up-0.50`. The
  clamp runs once, so repeated `GetTransformation()` calls are idempotent.
- **Animations normalize at construction** through their options `init` accessors: unit-interval
  components (`Fade`/`BeatFade` opacity, `Flip.FlipX`/`FlipY`/`FlipZ`) clamp to `[0, 1]`, and
  `IterationCount` clamps to non-negative. Omitting `IterationCount` leaves Font Awesome's default
  (`infinite`). Serialization never re-clamps.
- **Duotone opacities** are clamped with the same rule, and the corrected CSS custom-property names
  `--fa-primary-opacity`/`--fa-secondary-opacity` are emitted (1.x emitted a broken
  `--fa-primary-opacity;` key with a stray `;`).
- **Duotone markup follows the FA 7 contract**: in Font Awesome 7, `fa-duotone` is the
  *family* prefix, so `IconStyle.Duotone` emits the canonical `fa-duotone fa-solid` pair, and the new
  `Typeface.Duotone`/`Typeface.SharpDuotone` family values pair with any `IconStyle`. Rendering Duotone
  or Sharp families requires a Pro asset kit; verify your kit before relying on two-tone output.

If you depended on a 1.x out-of-range value being thrown, or on fractional scaling being rounded,
pass an explicit in-range value instead.

## 8. Events

Event handlers are attached to the Blazor-owned boundary element, not the inner `<i>`. The public
event surface is `EventCallback<MouseEventArgs>`: `OnClick`, `OnDoubleClick`, `OnMouseOver`,
`OnMouseOut` (`MouseOver`/`MouseOut` were already absent).

## 9. Verification checklist

- [ ] Host targets `net8.0`/`net9.0`/`net10.0` and references `2.0.0`.
- [ ] CSS/JS references use the new asset names and load the config before Font Awesome.
- [ ] All `Animation`/`Transform` usage migrated to the `Animate`/`Transform` surfaces.
- [ ] No references to removed members (see §6).
- [ ] Reviewed the behavior changes for out-of-range/fractional inputs (see §7).
- [ ] Application builds with **zero warnings** and renders icons in your chosen FA mode.
