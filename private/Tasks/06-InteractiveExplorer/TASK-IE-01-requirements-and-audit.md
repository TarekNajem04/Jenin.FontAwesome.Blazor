# TASK-IE-01 — Requirements, UX, and API Surface Audit

**Status:** `DONE`
> **Governance reminder:** Follow `private/constitution.md`, `.editorconfig`, and
> `.gitattributes`. Before changing this task to `DONE`, fix all compiler/analyzer diagnostics in
> scope, run and record the applicable acceptance checks for all supported TFMs, and update this
> file plus the group index. Do not suppress or weaken diagnostics to bypass this gate.
**Group:** `06-InteractiveExplorer`
**Priority:** `high`
**Model:** `STRONG`

## Goal
Define requirements and user flows for an interactive Font Awesome explorer inspired by https://fontawesome.com/search, scoped to Jenin.FontAwesome.Blazor 2.0 API.

## Scope
- Interactive search, filtering (Style/Brands, Free styles including Duotone/Sharp), preview with live props
- Snippet generation (Razor + markup)
- Responsive, accessible, fast (large catalogue 2163+)

## Deliverables
- Requirements doc (must/should/could)
- Feature matrix vs current sample
- API coverage checklist (IconStyle/Typeface/IconNames, transforms, animations, layers/mask)
- User flows + wireframe description

## Acceptance
- Clear, actionable requirements
- All core features mapped to library API

---

# Deliverable 1 — Requirements (MoSCoW)

Context: the Explorer is a **separate sample project** (per group `INDEX.md` vision) that
*consumes* the 2.0 API. The core public API is frozen by
`tests/Jenin.FontAwesome.Blazor.Tests/Contract/PublicApiBaselineTests.cs`, so **no library API
changes** are permitted for this feature.

## Must

| # | Requirement | Notes |
|---|---|---|
| M1 | **Debounced text search** over name, label, aliases, and search terms; case-insensitive; prefix matches ranked above substring matches; empty query shows the full catalogue. | Use `IconMetadataProvider.Search` (already ships this ranking: exact 100 > prefix 80 > alias 70/40 > term 60/30 > name substring 20 > label 10). |
| M2 | **Style filter** with counts: All / Solid / Regular / Brands (the three Free styles). Duotone, Sharp, Light, Thin appear as **preview rendering options with "Pro" indicators**, not as catalogue facets — they have no separate Free icons. | Free metadata has no Pro style facets; `IconStyle`/`Typeface` enums do model them. |
| M3 | **Virtualized icon grid** of cards (live `<Icon>` preview + icon name), result count, empty state ("no icons match"), initial loading state; no layout shift while scrolling. | 1,992 unique icons / 2,163 style variants; must use `Virtualize`/QuickGrid (IE-02/IE-03). |
| M4 | **Live preview panel** bound to the selected icon, exposing every preview-relevant 2.0 parameter (full checklist in Deliverable 3): style, typeface, size/width, colors, rotation/flip, animation, transforms, border/pull/inverse, mask, duotone colors/opacity/swap-opacity, layers composite demo. | Controls map 1:1 to real component parameters — the preview *is* the rendered `<Icon>`. |
| M5 | **Snippet generation**: Razor `<Icon … />` reflecting only the parameters the user has changed (omit defaults), plus copy-to-clipboard with visible feedback. | Must compile as-is for typical cases (IE-05). |
| M6 | **Shareable URL state**: `?icon=fa-house&style=solid&…` encodes selection + filters + preview props; loading a URL restores full state; back/forward works. | Per IE-02 routing plan. |
| M7 | **Responsive + accessible**: usable at 360 px width; keyboard-operable (search, filters, grid, panel); visible focus; `aria-*` on interactive grids/panels; honors the library's own a11y model (`Title`/`AriaLabel`/`Decorative`); respects reduced-motion for preview animations. | Mirrors README accessibility section; IE-06 deepens this. |
| M8 | **Performance**: filter round-trip **sub-100 ms** over 2,000+ items, deterministic, **no network calls** (catalogue is embedded/assembly data); DOM cost bounded by virtualization. | Explicit IE-02 acceptance. |
| M9 | **Separate project** added to samples/solution; consumes core + IconPicker (+ Icons presets) packages. Zero changes to the frozen core public API. | Contract tests would fail otherwise. |
| M10 | **Honest rendering caveats**: when the host runs in webfont mode, show a notice that transforms/masks/layers are inert (README feature matrix); when a Pro style/typeface is selected under free assets, show a fallback/Pro notice instead of a blank preview. | README webfont vs SVG matrix; `IconStyle` Pro values. |

## Should

| # | Requirement | Notes |
|---|---|---|
| S1 | Dark/light theme toggle, consistent with sample styling. | IE-06. |
| S2 | Power-user keyboard flow: `/` focuses search, `Esc` clears, arrow keys move between cards, `Enter` selects, `C` copies snippet. | IE-06. |
| S3 | Snippet output variants: Razor `<Icon …/>`, preset component (`<HouseIcon />`), and typed name reference (`IconNames.Solid.House`) for the name literal. | Ties the three catalogue representations together (README's IconCatalogue comparison). |
| S4 | Per-card **style availability badges** (which Free styles the icon supports) derived from `IconMetadata.Styles`. | |
| S5 | Random/"surprise me" icon picker; "clear all" resets every control to defaults. | |
| S6 | Last-copied snippet + recent selections persisted per session (browser storage in the sample host only). | Sample-side JS allowed; library stays interop-free. |
| S7 | Config panel demoing `AddJeninFontAwesome` options / `<FontAwesomeDefaults>` precedence as it affects the preview. | Bonus educational value; must not mutate global sample state outside the explorer route. |

## Could

| # | Requirement | Notes |
|---|---|---|
| C1 | Full animation playground: all 14 `Animate.*` factories with per-animation options (scale, rebound, pulse, drift…), duration/delay/direction/iteration/timing. | IE-04 may promote parts of this. |
| C2 | Composite builders: layers (`CounterLayer`/`TextLayer`), stack, and list examples generated as snippets too. | |
| C3 | Pin/compare two icons side by side. | |
| C4 | "View on fontawesome.com" deep link + license/attribution footer (CC BY 4.0 metadata notice). | |
| C5 | Keyboard-shortcut help overlay (`?`). | |
| C6 | i18n-ready strings (all UI text localized in one place). | |

**Explicitly out of scope:** Pro-only catalogue icons (we ship Free metadata only), modifying the
core library/public API, shipping FA assets (host-supplied), webfont-mode feature emulation.

---

# Deliverable 2 — Feature matrix vs current sample

| Capability | Current sample | `<IconPicker>` package | Explorer (target) | Library API ready? |
|---|---|---|---|---|
| Full-catalogue text search | partial (picker only) | yes (ranked, 60-result cap) | yes (virtualized, unlimited) | ✅ `IconMetadataProvider.Search` |
| Alias/keyword search | partial | yes | yes | ✅ metadata `a`/`t` |
| Style filter (Free styles) | partial (picker select) | yes (`Style` param) | yes (toggle buttons + counts) | ✅ `IconMetadata.Styles` |
| Pro style/typeface as *facet* | no | no | no — rendered as preview options w/ Pro badge (M2) | ✅ enums exist, no free data |
| Virtualized result grid | no (60-row cap) | no | yes | ➖ app concern (`Virtualize`) |
| Live preview of selected icon | no | tiny in-list preview | full-size panel | ✅ `<Icon>` |
| Live prop controls (size/color/rotate/…) | scattered across ~18 demo pages | no | one control surface | ✅ full 2.0 parameter set |
| Animation controls | demo pages only | no | yes (preset + tuning) | ✅ `Animate.*` + options records |
| Transform controls | demo page only | no | yes (fluent builder UI) | ✅ `Transform` |
| Mask / layers / stack demos | demo pages only | no | yes (composite mode) | ✅ mask + layer components |
| Duotone color/opacity controls | demo page only (Pro) | no | yes + Pro caveat | ✅ `Primary*`/`Secondary*`/`SwapOpacity` |
| Snippet generation + copy | no (static `CodeSnippet` embeds) | no | yes, state-driven | ✅ app-side |
| Deep-linkable state (`?icon=…`) | no | no | yes | ➖ app concern |
| Dark/light theming | no | n/a (ships no CSS) | yes | ➖ app concern |
| Keyboard navigation of results | no | no | yes | ➖ app concern |
| Result count / empty state | partial (picker no-results text) | yes (`NoResultsText`) | yes + counts | ✅ |
| Documented parameter tables | yes (`IconBrowser.razor`, `IconCatalogue`) | — | auto-generate from selected props | ✅ |
| Preset components / `IconNames` demo | yes (catalogue page) | no | shown in snippet variants | ✅ Icons package |

**Gap summary:** the sample demonstrates features *in isolation* across static pages, and the only
search UI (`/icon-browser`) is a capped picker with no preview controls or snippet output. The
Explorer combines search + full live control surface + snippet generation + shareable state —
nothing in the current sample covers that combination.

---

# Deliverable 3 — API coverage checklist

Legend: **P** = preview control required (M4), **S** = snippet output (M5), **F** = filter/facet (M2),
`—` = app-only concern. Source: `src/Jenin.FontAwesome.Blazor/`.

## Core `<Icon>` parameters (`Components/Icon.cs`)

- [ ] `IconName` (string, `IconNames.*`) — P, S *(value from selection; not user-editable free text)*
- [ ] `IconStyle` (`Solid/Regular/Light/Thin/Duotone/Brands`) — P, S, F (Free subset only)
- [ ] `Typeface` (`Classic/Sharp/Duotone/SharpDuotone`) — P, S (Pro badges + M10 caveat)
- [ ] `Rotate` (`IconRotate`) + `RotateAngle` (with `RotateBy`) — P, S
- [ ] `CombiningRotate` — P, S
- [ ] `Animation` (`Animate.Beat/Fade/BeatFade/Bounce/Flip/Shake/Spin/SpinSnap/Buzz/Float/Jello/Flip360/Swing/Wag`) — P, S *(C1 for full options)*
- [ ] `PullPosition`/`PullMargin` — P, S
- [ ] `StackSize`/`StackZIndex` — P, S *(only meaningful inside a stack)*
- [ ] `MaskIconStyle`/`MaskIconName`/`MaskId` — P, S *(SVG-only, M10)*
- [ ] `PrimaryColor`/`PrimaryOpacity`/`SecondaryColor`/`SecondaryOpacity` — P, S
- [ ] `SwapOpacity` — P, S
- [ ] `Transform` (`Transformations.Transform`) — P, S

## Inherited shared surface (`Components/ElementBase.cs`, `Element.cs`)

- [ ] `Color`, `BackgroundColor`, `CssClass` — P, S
- [ ] `IconSize` (`X01`–`X10`, `xs`–`2xl`, Inherits) — P, S
- [ ] `WidthMode` (`Auto`/`Fixed`) and `FixedWidth` — P, S
- [ ] `CanvasPadding` (`Square`/`Roomy`) — P, S
- [ ] `Bordered` + `BorderColor/BorderPadding/BorderRadius/BorderWidth/BorderStyle` — P, S
- [ ] `Padding`, `Margin` — P, S
- [ ] `Inverse`, `InverseColor` — P, S
- [ ] `Title`, `AriaLabel`, `Decorative` — P, S *(preview must keep a11y tokens; default decorative per README)*
- [ ] Mouse events (`OnClick`…) — *(not previewable as props; C-level "events demo" only if ever needed)*

## Containers / composites

- [ ] `IconContainer` + `IconLayersContainer` — P, S
- [ ] `CounterLayer` (`Text` required, counter positioning/scale/padding…) — P, S
- [ ] `TextLayer` (`Text` required) — P, S
- [ ] `IconStackContainer` (`VerticalAlign`, `StackSize` on child) — P, S
- [ ] `IconList`/`IconListItem` (`OrderedList`, `ItemMargin/Width`) — S *(C2)*

## Transform API (`Transformations/`)

- [ ] `Grow`, `Shrink`, `Up`, `Down`, `Left`, `Right`, `Rotate`, `FlipV`, `FlipH`, `FlipBoth`, `Add` — P (builder UI), S (serialized `data-fa-transform` tokens)
- [ ] Immutability/fluent chaining respected by state model (clone-per-change)

## Animation API (`Animations/`)

- [ ] Base options on every animation: `Delay`, `Duration`, `Direction`, `IterationCount`, `TimingFunction` — P, S
- [ ] Per-animation extras (`BeatOptions.Scale`, `SpinOptions.Pulse/Reverse`, `BounceOptions.*`, …) — C1
- [ ] Reduced-motion handling in preview (M7)

## Configuration

- [ ] `AddJeninFontAwesome(Action<FontAwesomeOptions>)` / `FontAwesomeDefaults` cascade — S7 awareness; Explorer must **not** rely on it mutating other pages
- [ ] Precedence (explicit param > defaults cascade > DI > built-in) documented in snippet help text where relevant

## Data / discovery (`Jenin.FontAwesome.Blazor.IconPicker`)

- [ ] `IconMetadataProvider.Default.Icons` — enumerate catalogue (**source of truth for the grid**)
- [ ] `IconMetadataProvider.Search(query, style, maxResults)` — M1 (`maxResults: 0` = unlimited)
- [ ] `IconMetadataProvider.Find(nameOrClass)` — deep-link resolution (`?icon=fa-house`)
- [ ] `IconMetadata.{Name, CssClass, Label, Styles, SearchTerms, Aliases, PrimaryStyle, SupportsStyle}` — M1/F/S4
- [ ] `IconNames.Solid/Regular/Brands.*` — snippet constants (S3); **note: consts are not enumerable** → never used as the grid data source
- [ ] `Jenin.FontAwesome.Blazor.Icons` presets (`HouseIcon`) — snippet variant (S3)

## Audit findings & constraints

1. **Data source decision:** grid/search driven by `IconMetadataProvider` (1,992 icons, labels, styles,
   aliases, terms, ranked search already implemented and trim/AOT-safe). `IconNames` (2,163 consts)
   is compile-time only and powers snippet output/typed references. This resolves the IE-02 note
   "use IconNames as source of truth" → *metadata JSON is the runtime source of truth; both derive
   from the same vendored FA 7.3.1 metadata (`eng/metadata/`), so they cannot drift.*
2. **Free vs Pro:** searchable catalogue = Free only (Solid 1,422 / Regular 169 / Brands 572 style
   entries). Duotone/Sharp/Light/Thin are *rendering* selections with Pro indicators (M2/M10).
3. **Webfont mode:** transforms, masks, and layers are inert (README matrix) → preview banner (M10);
   still generates correct markup/snippets.
4. **Frozen API:** all work lands in a new sample project + docs; any core change would trip
   `PublicApiBaselineTests`.
5. **Interop:** the library has none; the Explorer *app* may use JS (clipboard, URL/state storage)
   — keep it in the sample host's `wwwroot`, never in the library.
6. **Rendering precondition:** previews only render where host FA assets exist (sample hosts vendor
   FA under `Sample.Shared/wwwroot/fontawesome/`); the Explorer must ship in such a host.
7. **E2E:** extend `tests/e2e` Playwright pattern (target `localhost:5000`, Chromium) in IE-08.
8. **Licensing:** keep the CC BY 4.0 attribution already embedded in the metadata resource visible
   in the Explorer footer (C4).

---

# Deliverable 4 — User flows + wireframe description

## Flows

**F1 — Discover (primary).**
Land on `/explorer` → (optional: press `/`) → type query (debounced ~150 ms) → results re-rank live
(count updates) → optionally toggle style filters → move focus through cards (arrows) → `Enter`/
click selects → preview panel renders the icon with default props → URL updates to `?icon=…`.

**F2 — Tune and copy.**
From F1 → adjust controls (e.g. `IconSize=Large`, `Rotate=Deg090`, `Animate.Spin`) → preview updates
on every change (single `<Icon>` re-render) → snippet pane shows exact Razor for the changed props
only → `Copy` (or `C`) → button shows "Copied ✓" for ~2 s → user pastes into their app.

**F3 — Deep link / share.**
Paste `…/explorer?icon=fa-bug&style=brands&size=2xl&anim=beat` → app restores selection, filters,
and preview props before first paint → recipient sees identical state; Back returns to prior
filter state.

**F4 — Unsupported-combo handling.**
User selects Typeface=`Sharp` (Pro) or enables a transform in webfont mode → preview shows inline
notice ("Pro style — falls back under free assets" / "SVG mode required for transforms") while
snippet still emits the code → user understands without a blank/broken icon.

**F5 — Keyboard-only.**
`/` focus search → type → `Tab`/`→` into grid → arrows move card focus (virtualization keeps DOM
small) → `Enter` select → `Tab` through preview controls → `C` copy → `Esc` clears query.

## Wireframe (desktop ≥ 992 px)

```
┌────────────────────────────────────────────────────────────────────────────┐
│ Header: title   [ Search icons………… ] [All|Solid|Regular|Brands]  247 results│
├──────────────────────────────────────────────────┬─────────────────────────┤
│  Icon grid (virtualized)                         │  Preview panel          │
│  ┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐     │  ┌───────────────────┐  │
│  │  [ico] │ │  [ico] │ │  [ico] │ │  [ico] │     │  │   large preview   │  │
│  │ house  │ │ bug    │ │ bell   │ │ star   │     │  │   [ selected ]    │  │
│  └────────┘ └────────┘ └────────┘ └────────┘     │  ├───────────────────┤  │
│  ┌────────┐ ┌────────┐ ┌────────┐ ┌────────┐     │  │ Style ▾ Typeface ▾│  │
│  │  [ico] │ │  [ico] │ │  [ico] │ │  [ico] │     │  │ Size  Color  Width│  │
│  │  …     │ │  …     │ │  …     │ │  …     │     │  │ Rotate Flip Border│  │
│  └────────┘ └────────┘ └────────┘ └────────┘     │  │ Anim  Transform   │  │
│  ← virtualization spacer →                      │  │ Mask  Duotone     │  │
│                                                  │  ├───────────────────┤  │
│  empty state: "No icons match 'xyz'"             │  │ Razor | Icon ▾    │  │
│                                                  │  │ <Icon … /> [Copy] │  │
└──────────────────────────────────────────────────┴─────────────────────────┘
```

- **≤ 768 px:** single column — sticky search/filter bar on top, grid below; selecting a card opens
  the preview panel as a full-screen sheet (back closes, preserving filters); snippet pane sits at
  the bottom of the sheet.
- **Components (IE-03):** `Explorer` (owns state + URL sync), `SearchBar`, `StyleFilter`,
  `IconGrid` (Virtualize), `IconCard`, `PreviewPanel` (hosts `PreviewControls` from IE-04 and
  `SnippetPane` from IE-05), `UnsupportedNotice` (M10).
- **States:** initial (skeleton cards, one pass only — data is embedded), populated, empty,
  copied-feedback, unsupported-combo banner.

---

## Resolution
Requirements, feature matrix, API coverage checklist, and user flows/wireframes delivered above,
fulfilling all four deliverables and both acceptance criteria (clear actionable MoSCoW
requirements; every core library feature mapped to its API member).

Key decisions:
- **Data source:** `IconMetadataProvider` runtime catalogue (1,992 icons) for grid/search;
  `IconNames`/presets only for snippet output — both generated from the same vendored FA 7.3.1
  metadata, so no drift risk.
- **Facet model:** free style toggles (All/Solid/Regular/Brands) are the only catalogue facets;
  Duotone/Sharp/Light/Thin are preview options with Pro indicators, plus explicit unsupported-state
  messaging (M10) for Pro and webfont-mode limitations.
- **Architecture constraints locked in for IE-02+:** separate project, frozen public API,
  sub-100 ms no-network filtering, virtualized grid, URL-encoded state, zero library interop
  (sample-host JS only).
- Snippet output covers the three catalogue representations (`<Icon …/>`, preset component,
  `IconNames` constant) to mirror the IconCatalogue docs page.

Evidence: audit performed against `README.md`, `src/Jenin.FontAwesome.Blazor/**` (Icon,
ElementBase, Transformations, Animations, IconNames), `src/...IconPicker/**`
(`IconMetadataProvider.Search`), `src/...Icons/GeneratedIconComponents.cs`,
`samples/...Sample.Shared/Pages/**` (incl. `IconBrowser/IconBrowser.razor`,
`IconCatalogue/`), `tests/e2e/**`, and `tests/...Contract/PublicApiBaselineTests.cs`.
Documentation-only change: no build/test gate applies; group `INDEX.md` updated to `IN_PROGRESS`
with per-task status.

## Follow-ups
- IE-02 must reconcile its "IconNames as source of truth" wording with the metadata-provider
  decision recorded here.
- Confirm C1 animation-options scope is folded into IE-04 or explicitly deferred.
- Register group `06-InteractiveExplorer` in the root `private/Tasks/INDEX.md` groups table when
  the first code task (IE-02/03) lands.
