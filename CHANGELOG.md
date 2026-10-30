# Changelog

All notable changes to `Jenin.FontAwesome.Blazor` are documented here.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project
adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [2.0.0]

2.0 is a **generation reset** and is intentionally **not backward compatible** with 1.x: removed
members are not shimmed and there are no `[Obsolete]` bridges. See
[MIGRATION.md](MIGRATION.md) for the complete 1.x → 2.0 upgrade guide.

### Added

- Immutable animation model: `Animate` factories, a single `Animation` base record, and per-animation
  options types deriving from `AnimationOptions`.
- Immutable transformation model: an instance-based `Transform` alongside the transformation value
  objects (`Scaling`, `Positioning`, `Rotating`, `FlipHorizontal`, `FlipVertical`).
- Host configuration: `AddJeninFontAwesome(...)` options and the cascading `<FontAwesomeDefaults>`
  component, with a documented precedence order.
- Accessibility parameters on every component (`Title`, `AriaLabel`, `Decorative`), decorative by
  default for `<Icon>` and opt-in for containers.
- Compile-time icon-name constants (`IconNames.Solid`/`Regular`/`Brands`), generated from the
  Font Awesome Free 7.3.1 metadata.
- `Jenin.FontAwesome.Blazor.IconPicker` package: an interop-free icon-metadata provider
  (`IconMetadataProvider`) and a ready-made `<IconPicker>` component.
- `Jenin.FontAwesome.Blazor.Icons` package: strongly-typed preset component for every Font Awesome
  Free icon.
- Trimming / Native AOT support in the seven compiled packages (`IsTrimmable`, `IsAotCompatible` declared
  per runtime/adapter package; the analyzer ships no runtime IL) with zero trim/AOT warnings plus a publish-level verification gate
  (`eng/verify-optional-trim.ps1`).
- Browser-level E2E verification as a required CI job (`e2e-browser`): the Playwright harness in
  `tests/e2e` boots the sample host and validates the real Font Awesome DOM replacement, the
  transform/mask/layering/accessibility tokens surviving it, RCL static-asset serving, re-enhancement
  after Blazor circuit re-renders, and a hidden Blazor error UI (`tests/e2e/README.md`).
- Code coverage gate: `coverlet.collector` collects Cobertura reports on the
  canonical net10.0 CI leg (`tests/coverage.runsettings`), `eng/verify-coverage.ps1` merges them into
  a single line-coverage figure and fails under the ratchet in `eng/coverage-threshold.txt`, and the
  report is published as the `coverage-report` artifact (`TESTING.md` documents the local workflow
  and the ratchet policy).
- `.NET` analyzers enforced at the `recommended` tier repo-wide (single source in
  `Directory.Build.props`), keeping the zero-warning/no-suppression build; the naming-opinion rule
  `CA1720` is pinned in `.editorconfig` because `Float` and `Double` are contract-mandated Font
  Awesome/CSS identifiers.
- FA 7.x token coverage: `PullPosition.Start/End` (`fa-pull-start`/`fa-pull-end`),
  `IconWidth` with `ElementBase.WidthMode` (`fa-width-auto`/`fa-width-fixed`), `CanvasPadding` with
  `ElementBase.CanvasPadding` (`fa-canvas-square`/`fa-canvas-roomy`), the 7.x animation tunables
  (`FloatOptions` Drift/Height/Squash/Stretch/Tilt, `BuzzOptions.Distance`, `JelloOptions.ScaleX/Y`,
  `SwingOptions.Angle`, `WagOptions.Angle`, `FlipOptions.Overshoot/AnticipationScale`,
  `BounceOptions.Anticipation`), and the documented `fa-layers-counter` custom properties
  (`--fa-counter-background-color/-border-radius/-padding`). The 2,163/1,992 icon-count difference is
  reconciled (style variants vs unique names; zero missing Free names) and the FA 7.3.1 token level
  is audited to full class/custom-property parity with recorded dispositions.
- Webfont-mode behavior documentation: a feature × mode support matrix in the README
  (`Webfont vs SVG: feature support matrix`) — engine-neutral markup keeps CSS-level features (sizing,
  rotation including `RotateBy`, animations, pull/stack/list, events) working in webfont mode while
  the SVG-engine-only tokens (`data-fa-transform`, `data-fa-mask`, layers) degrade into documented
  inert attributes; cross-linked from the webfont setup note, the asset requirements, and the
  Troubleshooting entry. Verified in-browser by the new `tests/e2e/webfont.spec.ts` (anchors stay
  un-replaced, webfont glyphs render, engine tokens stay inert).
- Pro-family values on `Typeface` (`Duotone` → `fa-duotone`, `SharpDuotone` → `fa-sharp-duotone`)
  pairing with any `IconStyle`, per the Font Awesome 7 family × style contract. A README
  subsection now documents the Pro requirement for Light/Thin/Duotone/Sharp styles and the measured
  Free-tier fallback behavior (paired style tokens render the classic style; family-only fragments
  render nothing).
- New optional package `Jenin.FontAwesome.Blazor.Svg`: 2,163 generated per-icon
  components (`HouseSolidIcon`, `GithubBrandsIcon`, ...) rendering standalone inline SVG with no
  Font Awesome JavaScript, CSS, fonts, or sprite file required. Unreferenced icons are linker-trimmed
  (verified per release by `eng/verify-optional-trim.ps1`); see the README section
  “Self-Contained Inline SVG Components” and the choosing-rendering table. Glyph artwork is
  redistributed under CC BY 4.0.
- New `Jenin.FontAwesome.Blazor.Templates` package: `jenin-fa-blazor` (Blazor Web App) and
  `jenin-fa-blazor-wasm` (standalone WebAssembly) starters with the library installed,
  configured, and showing a first icon (`--Framework net8.0|net9.0|net10.0`, `--FaMode`,
  `--JfaVersion`; pre-release restore via a local pack feed). Every template is smoke-verified
  end to end by `eng/verify-templates.ps1` (scaffold → build → run → first-icon assertion,
  including a headless-browser DOM check for WebAssembly).
- New `Jenin.FontAwesome.Blazor.Analyzers` package: Roslyn analyzer + code fixes for the typed
  API — unknown icon names (`JFA1001`), raw class strings in name positions with a fix to the
  typed equivalent (`JFA1002`), and Pro-only styles without licensed assets (`JFA1003`);
  `.editorconfig`-tunable and dogfooded on every shipped compilation. See `docs/diagnostics.md`.
- New adapter packages `Jenin.FontAwesome.Blazor.MudBlazor` (`MudIconMapper`, `JeninMudIcon`)
  and `Jenin.FontAwesome.Blazor.Blazorise` (`JeninBlazoriseIcon`) mapping typed icon names into
  suite icon slots, verified against real suite components by `eng/verify-suites.ps1`.
  See `docs/using-with-ui-suites.md`.
- One-line setup: `AddJeninFontAwesome()` plus `MapJeninFontAwesomeAssets(...)` serving the
  generated mode-config endpoint plus `<FontAwesomeHeadAssets />` in `<head>` — no hand-written
  `<script>` tags — with an automatic static-config fallback for standalone WebAssembly hosts
  and a documented manual-wiring path.
- Composed components in the core package: icon buttons (`IconButton`, `IconToggleButton`,
  `FavoriteButton`), live indicators (`NotificationBadge`, `StatusDot`, `IconProgress`,
  `AvatarIcon` + `AvatarPresence`), form inputs (`IconRating`, plus `IconPickerField` in the
  IconPicker package), content blocks (`StatCard`, `EmptyState`, `ActivityItem`, `ShareButtons`
  + `StatDelta`), and enum-driven rendering (`[Icon]` + `IconFor<T>`). See the README Component
  Catalog and the composed sample screens (`docs/gallery.md`).
- Font Awesome Kit support (`UseFontAwesomeKit`, including custom Kit glyphs through search,
  picker, and snippets; `docs/hosting-with-kits.md` and the KitDemo sample) and consumer
  custom-icon registration through the same typed API (`docs/custom-icons.md`); licensed-Pro
  render verification stays a local/manual gate (`eng/verify-pro-render.ps1`,
  `docs/pro-setup.md`) pending owner Pro assets.
- Advanced rendering: hover/parent-hover/focus/scroll-reveal `AnimationTrigger` (`Icon.Trigger`),
  `IconTransition`/`TwoStateIcon` animated state swaps, theme-aware `DuotonePalettes` with
  cascading `IconTheme` scopes, single-request `SpriteIcon` + `eng/fa_sprite.py` sprite builder,
  a used-icon subsetter with savings report (`eng/Find-FontAwesomeUsage.ps1`, `eng/fa_subset.py`),
  and `IconSvg.ToSvgString` standalone SVG strings for email/PDF/report targets.
### Changed

- Targets **.NET 8.0, .NET 9.0, and .NET 10.0** (.NET 8 support was restored by maintainer decision before release).
- Markup targets **Font Awesome 7.x**; samples ship Font Awesome Free 7.3.1.
- Values are normalized at construction and serialization is pure/idempotent (see Behavior changes).
- Event handlers are attached to the Blazor-owned boundary element and exposed as
  `EventCallback<MouseEventArgs>` (`OnClick`, `OnDoubleClick`, `OnMouseOver`, `OnMouseOut`).

### Removed

See [MIGRATION.md §6](MIGRATION.md#6-removed-and-renamed-members) for the full list. Highlights:
`DurationUnit` and the unit-based delay/duration API, the `IAnimation`/`IFluentAnimation` interface
family, `AutoReplaceSvg`/`ProcessingSvgMethod`, `ForceRebuildRenderTree`, the `ElementEventArgs<T>` /
`ElementMouseEventArgs` event-argument wrappers, `TransformExtensions`, and the `fontawesome-utilities.js` /
`fontawesome-config-utilities.js` assets. The `Jenin.FontAwesome.Blazor.Extensions` namespace is no
longer public.

### Fixed

- Impure serialization that previously mutated transform/animation state (clamping write-back) on
  every render.
- Duotone opacity CSS custom-property key typo: `--fa-primary-opacity;` is now the correct
  `--fa-primary-opacity`.
- `IconStyle.Duotone` now emits the Font Awesome 7 canonical Duotone markup (`fa-duotone fa-solid`):
  in FA 7 the bare `fa-duotone` class is the Duotone *family prefix* (incomplete without a style
  class), which rendered nothing under Free assets. Pair other duotone styles through
  `Typeface.Duotone`/`Typeface.SharpDuotone` with any style value.

### Behavior changes

Documented in full in [MIGRATION.md §7](MIGRATION.md#7-behavior-changes):

- Transformations clamp to `[1, 16]` at construction and `Scaling` no longer rounds, so `Shrink(3.5)`
  emits `shrink-3.50` (1.x rounded to an integer).
- Animations normalize unit-interval components to `[0, 1]` and `IterationCount` to non-negative at
  construction; omitting `IterationCount` leaves Font Awesome's `infinite` default.

[Unreleased]: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/compare/v2.0.0...HEAD
[2.0.0]: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/releases/tag/v2.0.0
