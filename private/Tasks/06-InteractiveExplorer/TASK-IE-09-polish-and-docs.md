# TASK-IE-09-polish-and-docs — Polish, Screenshots, README, QA

**Status:** DONE
**Priority:** medium
**Owner:** (group 06)
**Model:** STRONG

## Goal
Finish group 06 by making the interactive explorer discoverable and self-documenting: a dedicated
README for the Explorer sample project and a section in the root README that introduces the explorer,
explains how to run it, and documents its behaviour (search, facets, URL state, keyboard shortcuts),
then a final QA pass over the whole group.

## Scope (agreed with the user)
- **Explorer README** — a new `samples/Jenin.FontAwesome.Blazor.Explorer/README.md`.
- **Root README section** — a new "Interactive Icon Explorer" section in the main `README.md`.

The following IE-01 requirements were **explicitly deselected** for this task and remain unimplemented
(would be a future group): per-card style-availability badges (S4), random "surprise me" + clear-all
(S5), session persistence of recent selections (S6), the `AddJeninFontAwesome` config-panel demo (S7),
pin/compare (C3), and committed screenshots (binary image capture is not produced by this task).

## Deliverables
- `samples/Jenin.FontAwesome.Blazor.Explorer/README.md` — project README covering purpose, features,
  search semantics, URL schema, keyboard shortcuts, theming/accessibility, how to run, and the
  architecture map.
- Root `README.md` — an "Interactive Icon Explorer" section (placed after "Icon Discovery &
  IconPicker") with a short intro, how to run the sample, feature bullets, and a link to the project
  README.

## Acceptance
- Both README files exist, are accurate against the implemented behaviour (verified against the
  running sample and the source), and follow the existing documentation style and encoding rules.
- The root README builds the explorer as a sample and the navigation already links it ("Icon explorer").
- Final group QA passes: `dotnet format --verify-no-changes`, encoding check, solution build,
  full test run, and the `explorer.spec.ts` Playwright suite.

## Dependencies
- IE-01 (requirements + MoSCoW), IE-02 (search/route spec), IE-03 (UI shell), IE-04 (preview),
  IE-05 (snippets), IE-06 (theming/keyboard), IE-07 (samples integration), IE-08 (tests/e2e).

## Notes
- The explorer is a **sample RCL** (`samples/Jenin.FontAwesome.Blazor.Explorer`, `net10.0`), not a
  shipped package; it is the only place in the repository that is allowed to use JavaScript.
- The runtime catalogue is the Font Awesome Free 7.3.1 metadata (1,992 icons) exposed through
  `IconMetadataProvider` from the optional IconPicker package.

## Outcome
- **`samples/Jenin.FontAwesome.Blazor.Explorer/README.md`** — new project README with the purpose and
  sample-vs-package note, how to run it (`dotnet run --project samples/Jenin.FontAwesome.Blazor.Sample`,
  route `/explorer`), the feature list, the URL schema table (`q`/`style`/`icon`), the keyboard-shortcut
  table, the accessibility summary, an architecture map (component → file), the test instructions, and
  the CC BY 4.0 attribution.
- **Root `README.md`** — new "Interactive Icon Explorer" section inserted after "Icon Discovery &
  IconPicker": short intro, sample note, run command, feature bullets (search/facets/virtualized grid/
  live preview/snippets/deep links/keyboard/theme), and a link to the project README.
- Both documents were written against the implemented behaviour (verified against the running sample and
  the source: `Explorer.razor.cs`, `IconGrid.razor.cs`, `ExplorerRoute.cs`, `App.razor`, `NavMenu.razor`).

## Evidence
- `pwsh ./eng/verify-encoding.ps1` → passed (333 governed files).
- `dotnet format "Jenin.FontAwesome.Blazor.slnx" --verify-no-changes --no-restore` → exit **0**.
- `dotnet build "Jenin.FontAwesome.Blazor.slnx" --no-restore -c Release` → **0 warnings, 0 errors**.
- `dotnet test "Jenin.FontAwesome.Blazor.slnx" -c Release --no-build` → all suites green: core **292**
  (net8/9/10), IconPicker **38** (net8/9/10), Icons **8** (net8/9/10), Explorer **59** (net10.0).
- Playwright `explorer.spec.ts` → **10 passed** against the Release host.

## Notes
- The deselected IE-01 requirements (S4 style badges, S5 surprise/clear-all, S6 session persistence,
  S7 config-panel demo, C3 pin/compare, committed screenshots) are recorded in this file's Scope section
  as deferred; they are not part of group 06 as delivered.
- The documentation intentionally describes only the three URL keys (`q`/`style`/`icon`) that are
  actually implemented; preview style/typeface are not URL-addressable (extended keys were deferred in
  IE-04/IE-05).
