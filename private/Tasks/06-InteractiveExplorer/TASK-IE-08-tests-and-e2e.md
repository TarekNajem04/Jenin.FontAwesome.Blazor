# TASK-IE-08 — Tests (unit/integration) + Playwright

**Status:** DONE
**Priority:** high
**Owner:** (group 06)
**Model:** STRONG

## Goal
Lock the concrete explorer behaviours behind automated tests. The explorer's correctness does not rest
on visual polish alone: its search must agree with the library's own search, its URL schema must round
trip, and the code it tells users to copy must be valid. Unit/integration tests cover the pure logic
(catalogue/search, routing, snippet generation, preview-state mapping); a Playwright spec covers the
end-to-end interaction on the real host (search, facet filtering, selection, keyboard shortcuts, theme
toggle), which the in-process MSTest suite cannot exercise.

## Deliverables
- New test project `tests\Jenin.FontAwesome.Blazor.Explorer.Tests` (MSTest) referencing the Explorer
  RCL, added to `Jenin.FontAwesome.Blazor.slnx` under `/tests/`.
- `InternalsVisibleTo("Jenin.FontAwesome.Blazor.Explorer.Tests")` on the Explorer RCL so the
  `internal` `SnippetGenerator` / `IconNamesReference` / `PreviewIconFactory` / `ExplorerIconEntry`
  can be exercised directly.
- Unit/integration tests:
  - **Catalogue/search:** totals, style counts, `Find` semantics (case/`fa-` prefix/trim/unknown),
    and the **parity contract** — for every single-token query the catalogue result set equals
    `IconMetadataProvider.Search` (normative IE-02 B.1).
  - **Routing:** `ExplorerRoute.Parse`/`Build` round trip, omit-defaults, unknown-key tolerance, URL
    encoding of special characters, and style-token validation.
  - **Snippets:** `SnippetGenerator` Razor / preset / HTML output for representative preview states,
    asserting defaults are omitted, `f`-suffixed float literals, single-quote attribute choice for
    values containing string literals, `data-fa-transform` emission, and the no-preset fallback.
  - **Preview-state mapping:** `PreviewIconFactory` animation/transform builders across the
    animation enum and transform flags.
- New Playwright spec `tests\e2e\explorer.spec.ts` covering `/explorer`: catalogue renders, search
  narrows results, facet filter, icon selection updates the preview + URL, global keyboard shortcuts
  (`/`, `?`, `C`), and the theme toggle.

## Acceptance
- `dotnet test tests\Jenin.FontAwesome.Blazor.Explorer.Tests` passes on net10.0.
- The single-token search parity assertion holds for the full alias/search-term vocabulary plus a
  sample of canonical names.
- `ExplorerRoute.Build(Parse(x))` reproduces the canonical URL for representative states.
- Generated snippets contain only valid tokens (floats suffixed, literals quoted) and match the state.
- The Playwright `explorer.spec.ts` passes against a Release host.
- Full solution build stays **0 warnings, 0 errors**; encoding gate passes; `build.ps1` succeeds.

## Dependencies
- IE-03 (Explorer project), IE-04 (preview), IE-05 (snippets), IE-06 (keyboard/theme), IE-07 (host).

## Outcome
- **New test project** `tests\Jenin.FontAwesome.Blazor.Explorer.Tests` (MSTest, **net10.0 only**,
  since the Explorer RCL is single-TFM), added to `Jenin.FontAwesome.Blazor.slnx` under `/tests/` with
  project references to core, IconPicker, and the Explorer RCL.
- **`InternalsVisibleTo`** added via `samples\...\Explorer\Properties\AssemblyInfo.cs` so the
  `internal` `SnippetGenerator` / `IconNamesReference` / `PreviewIconFactory` / `ExplorerIconEntry`
  are directly testable. (The attribute lives in `System.Runtime.CompilerServices`, not
  `System.Reflection`.)
- **`ExplorerCatalogueTests`** — totals, ordinal ordering, style counts, `Find` semantics
  (case/`fa-`/trim/unknown), empty-query ordering, style-facet behaviour, determinism, and the
  **parity contract**: every whitespace-free catalogue token (all names + aliases + search terms)
  returns the identical ordered result set as `IconMetadataProvider.Search`, with and without a style
  facet.
- **`ExplorerRouteTests`** — `Parse`/`Build` round trip, omit-defaults, unknown-key tolerance, URL
  decoding, style-token validation, blank-value handling, and base-path preservation.
- **`SnippetGeneratorTests`** — Razor/preset/HTML output, default omission, suffixed float literals,
  single-quote attribute selection for values containing string literals, transform expression,
  parameterless vs options animation, duotone custom properties, `IconNamesReference` group/fallback/
  digit-prefix rules.
- **`PreviewIconFactoryTests`** — animation builder covers every non-`None` enum value and maps
  options; transform builder grow/shrink/move/rotate/flip.
- **`tests\e2e\explorer.spec.ts`** — 10 Playwright tests over `/explorer`: catalogue renders,
  search narrows results, no-match empty state, style facet filters + URL, icon selection + URL deep
  link, `/` focus-search shortcut, `?` help dialog, theme toggle, arrow-key grid navigation.
- **`tests\e2e\.gitignore`** added so the Playwright `test-results/` output and `node_modules/` stay
  out of the tree.

## Evidence
- `dotnet test tests\Jenin.FontAwesome.Blazor.Explorer.Tests` → **59 passed** (net10.0).
- `dotnet test "Jenin.FontAwesome.Blazor.slnx" -c Release` → all suites green: core **292** (net8/9/10),
  IconPicker **38** (net8/9/10), Icons **8** (net8/9/10), Explorer **59** (net10.0).
- `dotnet build "Jenin.FontAwesome.Blazor.slnx" --no-restore -c Release` → **0 warnings, 0 errors**.
- `dotnet format "Jenin.FontAwesome.Blazor.slnx" --verify-no-changes --no-restore` → exit **0**
  (the Explorer RCL had never been formatted; whole-solution format applied and re-verified).
- `pwsh ./eng/verify-encoding.ps1 -Fix` → passed (333 governed files).
- `pwsh ./build.ps1 -Configuration Release` → Build succeeded (0/0).
- Playwright `explorer.spec.ts` → **10 passed** against the Release host.

## Notes
- The parity assertion is deliberately scoped to whitespace-free tokens: `IconMetadataProvider.Search`
  treats a multi-word metadata value (for example the term `digit one`) as a single comparison token,
  while the explorer tokenises the query on whitespace, so multi-word values are not single-token
  equivalents. The contract IE-02 B.1 defines is per single token, and the test covers the full
  name/alias/keyword vocabulary under that rule.
- The e2e spec follows the existing cold-circuit convention: the explorer emits virtualized spacers on
  prerender, so the suite re-clicks until the circuit is live and notes that only the visible slice of
  cards exists in the DOM (card assertions are made after a search narrows the result set).
- A pre-existing, unrelated `webfont.spec.ts` test (`power transforms stay inert engine tokens`)
  fails consistently on `dev` with this work stashed; it is a `getComputedStyle` timing flake on the
  `/transforms` page and is not caused by IE-08.
