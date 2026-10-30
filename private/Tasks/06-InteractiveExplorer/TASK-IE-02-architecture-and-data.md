# TASK-IE-02 — Architecture & Data

**Status:** `DONE`
> **Governance reminder:** Follow `private/constitution.md`, `.editorconfig`, and
> `.gitattributes`. Before changing this task to `DONE`, fix all compiler/analyzer diagnostics in
> scope, run and record the applicable acceptance checks for all supported TFMs, and update this
> file plus the group index. Do not suppress or weaken diagnostics to bypass this gate.
**Group:** `06-InteractiveExplorer`
**Priority:** `high`
**Model:** `STRONG`

## Goal
Design data/search/state for 2163+ icons with fast filtering.

## Approach
- Use IconNames catalogue (Solid/Regular/Brands) as source of truth
- Build in-memory index: name, style(s), aliases/terms, fa-name
- Search: prefix + contains, case-insensitive; debounce
- State: filters (style, search), selected icon, preview props
- Routing: ?icon=fa-house&style=solid&...
- Virtualize grid (e.g. QuickGrid/Virtualize) for perf

## Deliverables
- Data service (catalogue loader/index)
- Search spec + perf notes
- State container (Cascading/Scoped)
- Routing plan

## Acceptance
- Sub-100ms filter on 2000+ items
- Deterministic, no network calls

> **Scope note (user decision):** executed as **architecture documentation only** — no product
> code lands in this task. Implementation follows in IE-03/04/05. The perf/determinism acceptance
> is verified by an executed prototype benchmark against the real shipped metadata (evidence in
> Deliverable B).

> **Reconciliation with IE-01:** the original approach line "IconNames as source of truth" is
> superseded per IE-01 audit decision D1: **`IconMetadataProvider` (same vendored FA 7.3.1
> metadata) is the runtime source of truth**; `IconNames` constants are compile-time-only and
> cannot be enumerated, so they cannot back a grid. Both are generated from `eng/metadata/`, so
> there is no drift risk.

---

# Deliverable A — Data service (catalogue loader/index)

## A.1 Components

```
ExplorerCatalogue            (lazy, shared, immutable after build)
├─ loader: IconMetadataProvider.Default.Icons   ← single embedded-JSON parse, reused
├─ entries: ExplorerIconEntry[] (ordinal-sorted by Name)
├─ byName: Dictionary<string, ExplorerIconEntry> (Ordinal, "fa-" stripped key)
└─ facetCounts: per-IconStyle counts (precomputed)

ExplorerIconEntry            (immutable record wrapping IconMetadata)
├─ Metadata                  (original IconMetadata reference)
├─ Name / CssClass / Label   (canonical, no duplicated state)
├─ Styles: IconStyle[]       (free facets: Solid/Regular/Brands)
├─ NameLower, LabelLower     (precomputed lowercase haystacks)
└─ TermsLower[], AliasesLower[]

ExplorerSearch               (pure static, stateless)
└─ Search(entries, query, facet) → IReadOnlyList<ExplorerIconEntry>
```

Sketch (illustrative signature, not committed code):

```csharp
public sealed class ExplorerCatalogue {
    public static ExplorerCatalogue Default { get; }        // Lazy<T>, thread-safe
    public IReadOnlyList<ExplorerIconEntry> Icons { get; }  // sorted, stable order
    public int CountFor(IconStyle style);                   // facet counts
    public ExplorerIconEntry? Find(string? nameOrClass);    // O(1), strips "fa-"
    public IReadOnlyList<ExplorerIconEntry> Search(string? query, IconStyle? facet);
}
```

## A.2 Key decisions

| # | Decision | Rationale |
|---|---|---|
| D1 | **Catalogue source = `IconMetadataProvider.Default.Icons`** (embedded `Resources/fontawesome-7.3.1-free-icon-metadata.json`, 361,316 bytes), *not* a second JSON parse and *not* `IconNames` consts. | One lazy parse shared with `IconPicker`; labels/aliases/terms only exist here; `IconNames` is not enumerable at runtime. |
| D2 | Index is **built once, lazily, immutable** (`Lazy<T>`, no invalidation). | Catalogue changes only with a package upgrade → rebuild = new process/version. No cache-eviction complexity. |
| D3 | **Precompute lowercase haystacks** (`NameLower`, `LabelLower`, `TermsLower`, `AliasesLower`) at index build; scoring uses `StringComparison.Ordinal` only. | Case-insensitive compare per token per field at query time is the hot path; pre-lowering removes `OrdinalIgnoreCase` overhead and guarantees culture-invariant behavior. |
| D4 | **Multi-token AND search** (see B) as an Explorer-side extension of `IconMetadataProvider.Search`; single-token ranking is byte-for-byte the same weight table. | Provider treats the whole query as one string — `"arrow right"` returns 0 results there; a search box must support multi-word. |
| D5 | Grid enumerates **unique icons (1,992)**; style facet counts (Solid 1,422 / Regular 169 / Brands 572 = 2,163 style entries) are shown as filter badges. | Reconciles the "2163+ icons" goal with the 1,992-unique reality (TASK-FA-01 accounting). |
| D6 | **No network, no JS interop, no reflection**: file/resource read at first use only; trim/AOT-safe (mirrors provider's `JsonDocument` approach). | Constitution §3-III boundary; provider precedent. |
| D7 | Virtualization (`Virtualize`/QuickGrid) is a *rendering* concern (IE-03); the data service always returns the **complete** result list so counts are exact. | Perf budget (B) makes full-result computation cheap; avoids count/`Virtualize` mismatch bugs. |

## A.3 Data flow

```
host start ─► first search/select
              └► ExplorerCatalogue.Default (lazy)
                   └► IconMetadataProvider.Default.Icons   [one embedded-JSON parse]
                        └► build entries + lowercase fields + byName + facetCounts
                             └► ExplorerSearch.Search(query, facet) → IReadOnlyList
                                  └► Explorer page renders via Virtualize (IE-03)
```

---

# Deliverable B — Search spec + perf notes

## B.1 Search specification (normative)

1. **Normalize:** `query.Trim()` → `ToLowerInvariant()`; split on any whitespace
   (`RemoveEmptyEntries`). Empty/null query ⇒ all icons (facet still applies).
2. **Facet pre-filter:** if a style facet is selected, drop icons where `!SupportsStyle(facet)`
   *before* scoring.
3. **Match semantics:** **every token must match** (AND) the icon at least once; an icon that
   fails any token is excluded.
4. **Per-token score** (first match wins, identical weights to `IconMetadataProvider.Score`,
   `IconMetadataProvider.cs:102-134`):

   | Weight | Field |
   |---|---|
   | 100 | exact `Name` or `CssClass` (incl. `fa-`-prefixed query) |
   | 80 | `Name` starts with token |
   | 70 / 40 | alias exact / alias contains |
   | 60 / 30 | search term exact / term contains |
   | 20 | `Name` contains |
   | 10 | `Label` contains |
   | −1 | no match → exclude icon for this token |

5. **Icon score** = Σ per-token scores.
6. **Order:** score **descending**, tie-break `string.CompareOrdinal(Name, Name)` ascending —
   total order ⇒ deterministic; identical to the provider's tie-break.
7. **Debounce:** input changes coalesce; one `Search` per **150 ms** idle (leading flush optional,
   never parallel searches).
8. **Single-token equivalence guarantee:** for single-token queries the Explorer result sequence
   equals `IconMetadataProvider.Search(query, style, 0)` — same weights, same ordering. To be
   asserted as a contract test in IE-08.

## B.2 Complexity

`O(I × T × F)` worst case — icons 1,992 × tokens (≤ ~6 after split) × fields (aliases ≤ few,
terms ≤ ~30 avg 11) — all ordinal scans over pre-lowered arrays; plus one `O(n log n)` sort of
candidates. No allocations per icon beyond the candidate tuples.

## B.3 Perf notes — **executed evidence**

Prototype benchmark implementing exactly A/B above against the **real shipped metadata file**
(`src/Jenin.FontAwesome.Blazor.IconPicker/Resources/fontawesome-7.3.1-free-icon-metadata.json`,
361,316 bytes; 1,992 icons; 614 aliases; 21,972 search terms).

- **Command:** `dotnet run -c Release` in a throwaway console project
  (`C:\Users\Tarek\AppData\Local\Temp\opencode\ie02-bench`, net10.0, runtime 10.0.13,
  Windows, 12 logical CPUs) — run **twice**.
- **Exit code:** `0` both runs (harness fails the process on any non-determinism or ≥100 ms avg).

| Scenario | run 1 avg ms | run 2 avg ms | max ms (both) | results |
|---|---|---|---|---|
| cold load + index build (one-time) | 36.34 | — | — | 1,992 icons |
| empty query, no facet (full list + sort) | 0.613 | 0.877 | 6.13 | 1,992 |
| `house` | 1.169 | 1.541 | 5.28 | 49 |
| `ar` (broad substring) | 1.432 | 1.181 | 12.64 | 1,228 |
| `arrow right` (2 tokens, AND) | 0.766 | 1.382 | 2.66 | 46 |
| `social media brand logo` (4 tokens) | 0.781 | 1.383 | 2.30 | 0 |
| facet only `brands` | 0.082 | 0.115 | 0.41 | 572 |
| facet only `regular` | 0.037 | 0.052 | 0.37 | 169 |
| `ar` + facet `regular` | 0.071 | 0.112 | 0.33 | 101 |
| `Find("fa-house")` dict lookup | 0.000 | 0.000 | 0.001 | 1 |

**Results:**
- ✅ **Sub-100 ms:** worst single-query average **1.54 ms** (~65× margin); worst observed
  single call 12.64 ms (first-call/GC outlier) — still <100 ms. Cold index build 36.34 ms once.
- ✅ **Deterministic:** identical SHA-256 result hashes for every scenario across all 200
  iterations **and across both process runs** (`DETERMINISM: PASS`, identical hashes printed).
- ✅ **No network:** benchmark reads only a local file; runtime design reads only the embedded
  assembly resource (D6). Facet counts confirmed: Solid=1,422, Regular=169, Brands=572,
  unique=1,992 — matches IE-01/`CHANGELOG` accounting.

**Budget guidance for IE-03:** with a 150 ms debounce, worst-case measured compute (≈1.5 ms) is
<1% of the debounce window — remaining budget goes to rendering, which the virtualized grid
bounds independently. No result memoization required (measure before adding).

---

# Deliverable C — State container (Cascading/Scoped)

## C.1 Decision

**Chosen: component-owned state + `CascadingValue<ExplorerState>`** — the `Explorer` page root
owns one mutable `ExplorerState` instance (per render/circuit lifetime) and cascades it;
children mutate via `EventCallback` handlers owned by the root, which then re-renders and syncs
the URL.

**Rejected alternative: scoped DI service (`AddScoped<ExplorerState>`).**
- Requires `StateChanged` event + manual `StateHasChanged` subscriptions in every consumer.
- Static SSR/prerender instantiates a throwaway scope → two instances (prerender + interactive)
  and subscription-disposal plumbing for zero benefit.
- DI registration leaks explorer-only state into the host service graph (IE-07 integration cost).
- Cascading works identically in all four Blazor render modes with no DI lifetime reasoning.

Revisit only if IE-07 needs the same state across *multiple* routes (then scoped + explicit
dispose). Recorded as a decision point for IE-07.

## C.2 State shape

```csharp
sealed class ExplorerState {
    // filters / discovery
    string   Query;            // raw search box text
    IconStyle? Facet;          // null = All

    // selection
    ExplorerIconEntry? Selected;   // null ⇒ no preview

    // preview props (mirrors <Icon> parameters; only these serialize to the URL)
    IconStyle?  IStyle;  Typeface? Typeface;  IconSize? Size;  IconWidth? Width;
    CanvasPadding? Padding;  string? Color;  string? BgColor;
    IconRotate? Rotate;  float? RotateAngle;
    Animation?  Animation;  Transform? Transform;
    string? MaskIcon;  IconStyle? MaskStyle;
    string? PrimaryColor; float? PrimaryOpacity; string? SecondaryColor; float? SecondaryOpacity;
    bool SwapOpacity;  PullPosition? Pull;  bool? Bordered;  bool? Inverse;
    // reserved for IE-04/05: composite mode (layers/stack/list), title/aria overrides
}
```

Rules:
- Child components never mutate the instance directly for *shared* changes — they call
  `EventCallback`s (`OnQueryChanged`, `OnFacetChanged`, `OnSelectedChanged`,
  `OnPreviewChanged`) so the root can (a) trigger `Search`, (b) debounce-sync the URL.
- Preview-prop edits are local-first: `PreviewPanel` may bind privately and publish
  committed values upward; either way the root owns URL serialization.
- Derived data (result list, counts) is computed by the root on state change — never stored
  (avoids dual-source-of-truth staleness).

## C.3 Update loop

```
user input → child EventCallback → root mutates ExplorerState
           → recompute Search (debounced 150 ms for query; immediate for facet/select)
           → StateHasChanged (root) → cascaded re-render
           → URL sync (Deliverable D, debounced 300 ms)
external navigation (Back/Forward / deep link)
           → OnParametersSet → parse query string → replace state → recompute (no re-write of URL)
```

Guard: a `bool _syncingFromUrl` flag suppresses URL writes during `OnParametersSet` parsing to
prevent navigation ping-pong.

---

# Deliverable D — Routing plan

## D.1 Query-string schema

Only **non-default** values are written (short URLs, defaults recoverable). Keys are stable,
lowercase, and use **CSS-facing tokens** (not C# member names) so URLs are readable and survive
enum refactors.

| Key | Meaning | Values / format | Maps to API |
|---|---|---|---|
| `q` | search query | raw text (URL-encoded) | state `Query` |
| `style` | facet | `all` \| `solid` \| `regular` \| `brands` (omit = all) | `IconStyle` facet |
| `icon` | selected icon | `fa-house` or `house` (canonicalized to `fa-…`) | `IconMetadataProvider.Find` → `Icon.IconName` |
| `istyle` | preview style | `solid` \| `regular` \| `light` \| `thin` \| `duotone` \| `brands` | `Icon.IconStyle` |
| `tf` | typeface | `classic` \| `sharp` \| `duotone` \| `sharp-duotone` | `Icon.Typeface` |
| `size` | size | `2xs`…`10x`, `xs`, `sm`, `lg`, `xl`, `2xl`, `inherit` | `Icon.IconSize` |
| `width` | width mode | `auto` \| `fixed` | `Icon.WidthMode` |
| `pad` | canvas padding | `square` \| `roomy` | `Icon.CanvasPadding` |
| `color` / `bg` | colors | percent-encoded CSS color | `Icon.Color` / `Icon.BackgroundColor` |
| `rot` | rotation/flip | `90` \| `180` \| `270` \| `fh` \| `fv` \| `fb` \| `by:<float>` | `Icon.Rotate` (+ `RotateAngle` for `by:`) |
| `xform` | transform chain | canonical `data-fa-transform` tokens, spaces → `+` (e.g. `grow-6+rotate--30`) | `Transform.GetTransformation()` round-trip |
| `anim` | animation name | `beat` \| `fade` \| `beat-fade` \| `bounce` \| `flip` \| `shake` \| `spin` \| `spin-snap` \| `buzz` \| `float` \| `jello` \| `flip-360` \| `swing` \| `wag` | `Animate.*` factory |
| `animdur` / `animdelay` | durations | .NET-independent `500ms`, `1.5s` | `AnimationOptions.Duration/Delay` |
| `animdir` / `animiter` | direction / count | `normal`…`alternate-reverse` / float | `AnimationOptions.Direction/IterationCount` |
| `mask` / `maskid` | mask | `<style>:<name>` (e.g. `solid:lock`) / id | `Icon.MaskIconStyle`+`MaskIconName` / `MaskId` |
| `duo-p`, `duo-p-o`, `duo-s`, `duo-s-o` | duotone colors/opacity | color / `0`–`1` | `Primary*/Secondary*` params |
| `swap` | swap opacity | `1` | `Icon.SwapOpacity` |
| `pull` | pull | `left` \| `right` \| `start` \| `end` | `Icon.PullPosition` |
| `bd` / `inv` | border / inverse | `1` | `Icon.Bordered` / `Icon.Inverse` |
| `fw` | fixed width | `1` | `Icon.FixedWidth` |
| `title` / `aria` | a11y text | URL-encoded | `Icon.Title` / `Icon.AriaLabel` |
| `v` | schema version | `1` (reserved) | forward compatibility |

Unknown keys are **ignored on read, dropped on write** (forward/backward compatible).
Composite (layers/stack/list) state is **reserved** for IE-04/05 (`cx=…` key family) — not
designed here.

## D.2 Navigation behavior

- **Write path:** all state→URL syncs are **debounced 300 ms** and use
  `NavigationManager.NavigateTo(uri, replace: true)` **except** discrete selection/facet events
  (`icon`, `style` changes) which use `push` — so typing never spams history, while
  Back/Forward still steps between meaningful selections (IE-01 M6).
- **Read path:** `OnParametersSet` parses the query string, sets `_syncingFromUrl`, rebuilds
  state (via `Find` for `icon`; invalid/unknown values silently fall back to defaults, never
  throw), recomputes results, clears the flag.
- **Canonicalization:** `icon=house` → `fa-house`; `style=all` omitted; defaults omitted —
  parse(write(parse(uri))) is a fixed point (round-trip stable).
- **Deep links render correct on first paint** (state derived during prerender, no redirect).

## D.3 Examples

```
/explorer
/explorer?q=arrow&style=solid
/explorer?icon=fa-house&istyle=regular&size=2xl&rot=90&anim=beat&animdur=1s
/explorer?icon=fa-bug&style=brands&xform=grow-6+rotate--30&color=%23e3342f
```

---

## Acceptance evidence

| Criterion | Result | Evidence |
|---|---|---|
| Sub-100 ms filter on 2000+ items | ✅ PASS | Executed prototype benchmark, 2 runs × 200 iterations/scenario: worst avg **1.54 ms**, worst single call 12.64 ms over 1,992 icons (table B.3); harness exits non-zero on violation |
| Deterministic | ✅ PASS | SHA-256 result hashes identical across iterations **and** across both process runs (`DETERMINISM: PASS`) |
| No network calls | ✅ PASS | Design (D6) + benchmark read only a local file / embedded resource; no HTTP/socket code in pipeline |
| All four deliverables present | ✅ | A (data service design), B (search spec + perf evidence), C (state container decision), D (routing plan) |

**Commands executed:**
```
dotnet run -c Release        # workdir: %TEMP%\opencode\ie02-bench → EXIT=0
dotnet run -c Release --no-build  # second run → EXIT=0, identical hashes
git status --short           # only private/Tasks/06-InteractiveExplorer/* modified
```
Documentation-only task: no repository build/test gate applies (no source changes outside
`private/`).

## Resolution
Architecture and data design delivered as documents in this file (doc-only scope chosen by the
user; implementation deferred to IE-03/04/05). Decisions of record:

1. **D1–D7 data service decisions** — reuse `IconMetadataProvider` as loader, immutable lazy
   index, precomputed lowercase haystacks, multi-token AND search as a superset of provider
   ranking, 1,992-unique grid with 2,163 style-entry facet counts, zero network/interop,
   full-list results with virtualization kept a rendering concern.
2. **Search spec B.1 is normative for IE-03/IE-08**, including the single-token equivalence
   guarantee with `IconMetadataProvider.Search` (to become a contract test).
3. **State container: cascading component-owned** (scoped DI explicitly rejected; revisit only
   if IE-07 needs cross-route state).
4. **Routing schema D.1/D.2** — CSS-token values, omit-defaults, ignore-unknown, debounced
   replace-on-type / push-on-discrete-change, ping-pong guard.
5. **Acceptance verified by executed evidence**, not inspection: two benchmark runs, exit 0,
   deterministic hashes, worst-case 1.54 ms vs 100 ms budget.

## Follow-ups
- IE-03: implement per this spec; add contract test for B.1 single-token equivalence (with IE-08).
- IE-04/05: claim reserved routing keys (`cx=` composite family) and extend `ExplorerState`
  preview fields; update D.1 table when they do.
- IE-07: confirm or overturn the cascading-vs-scoped state decision if Explorer spans routes.
- Benchmark prototype lives only in `%TEMP%\opencode\ie02-bench` (local-only, not committed);
  IE-08 should fold the timing/determinism assertions into the Explorer test project instead.
