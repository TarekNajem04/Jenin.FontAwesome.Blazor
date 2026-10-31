# Jenin.FontAwesome.Blazor v2.0.0 — Generation Reset Release

v2.0.0 marks a generation reset for Jenin.FontAwesome.Blazor.

This release rebuilds the component model on immutable value objects, adds seven companion
packages alongside the core library, and moves markup to Font Awesome 7.x. It is intentionally
**not backward compatible** with 1.x: removed members have no shims and no `[Obsolete]` bridges.

## Highlights

### ⚠️ Breaking generation reset

- 1.x animation interfaces (`IAnimation`, fluent-unit operations, `DurationUnit`) are replaced
  by immutable records plus a single `Animate` factory with per-animation options
  (`BeatOptions`, `BounceOptions`, …) and `TimeSpan` timing.
- 1.x static `Transform.*` factories / `TransformExtensions` are replaced by an immutable
  instance `Transform` (`new Transform().Grow(6).Rotate(-30)`).
- Removed: `AutoReplaceSvg`, `ProcessingSvgMethod`, `ForceRebuildRenderTree`, `Clicked` /
  `DoubleClicked`, `ElementEventArgs<T>` wrappers, `IconId` / DOM ids, `JSRuntime` injection,
  `DurationUnit`, `fontawesome-utilities.js` / `fontawesome-config-utilities.js`. The
  `Jenin.FontAwesome.Blazor.Extensions` namespace is no longer public.
- Renamed: `ElemenWithTransformations` → `ElementWithTransformations`, `Typefaces` →
  `Typeface`, `DuoTone` → `Duotone`.

### 📦 Eight packages on nuget.org

- Core `Jenin.FontAwesome.Blazor` plus `IconPicker`, `Icons` (1,992 per-icon presets),
  `Svg` (2,163 standalone inline-SVG components, no FA runtime needed), `Analyzers`
  (`JFA1001`/`JFA1002`/`JFA1003`), `MudBlazor` / `Blazorise` adapters, and `dotnet new`
  templates (`jenin-fa-blazor`, `jenin-fa-blazor-wasm`).
- One-line setup: `AddJeninFontAwesome()` + `MapJeninFontAwesomeAssets(...)` +
  `<FontAwesomeHeadAssets />` — no hand-written `<script>` tags, with a static-config
  fallback for standalone WebAssembly.

### ✨ What you can build in 2.0

- Typed icons (`IconNames.*`, `<HouseIcon />`), 14 animation families with triggers
  (hover / parent-hover / focus / scroll-reveal), transitions (`IconTransition` /
  `TwoStateIcon`), theme-aware duotone palettes (`IconTheme` / `DuotonePalettes`).
- Composed blocks: `IconButton`, `IconToggleButton`, `FavoriteButton`, `NotificationBadge`,
  `StatusDot`, `IconProgress`, `AvatarIcon`, `IconRating`, `StatCard`, `EmptyState`,
  `ActivityItem`, `ShareButtons`, enum-driven `IconFor`.
- No-JS paths: `SpriteIcon` + sprite builder, `IconSvg.ToSvgString` for email/PDF, subsetter
  (`Find-FontAwesomeUsage.ps1` + `fa_subset.py`), Kit + custom-icon registration.
- Targets **.NET 8.0 / 9.0 / 10.0**, Font Awesome **7.x** (samples use Free 7.3.1),
  trimming / Native AOT clean, browser-level E2E + coverage gates in CI.

## What this release is about

v2.0.0 is not a feature drop on top of 1.x.

It represents a shift to an **immutable, engine-neutral core**: values normalize at
construction, serialization is pure and idempotent, and the same markup works across
SVG+JS, webfont, sprite, and inline-SVG paths (engine-only tokens degrade to documented
inert attributes in webfont mode).

> Normalize at construction → serialize purely → verify in a real browser.

## Compatibility

**Breaking.** Do not upgrade the package version alone — the code will not compile or
render as before.

What you must do:

1. Point the package reference to `2.0.0` and the target framework to
   `net8.0`, `net9.0`, or `net10.0`.
2. Read **`MIGRATION.md`** (§1–§9) and apply it end to end: namespaces, `Animate` /
   `Transform` rewrites, new asset names (`fontawesome-config-nest.js` /
   `fontawesome-config-webfont.js`, or the generated `fontawesome-config-auto.js` via the
   one-line setup), events on the boundary element, and the §7 behavior changes
   (clamping, `Shrink(3.5)` → `shrink-3.50`, duotone markup `fa-duotone fa-solid`).
3. Finish with the **§9 verification checklist** — build with zero warnings and confirm
   icons in your chosen FA mode.

Full API details: `CHANGELOG.md`. Everyday usage: `README.md`.

## Documentation

- Upgrade guide: `MIGRATION.md` (1.x → 2.0, including §6 removed/renamed list and §7
  behavior changes).
- Release details: `CHANGELOG.md` (§ Added / Changed / Removed / Fixed / Behavior changes).
- Setup, feature matrix, hosting with Kits, custom icons, Pro verification, UI-suite
  adapters, diagnostics: `README.md` + `docs/`.

---

**Jenin.FontAwesome.Blazor v2.0.0**

Free Font Awesome components for ASP.NET Blazor.

**Full Changelog**: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/compare/v1.0.1...v2.0.0
