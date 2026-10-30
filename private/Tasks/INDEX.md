# Tasks — Jenin.FontAwesome.Blazor Improvement Backlog

**Status:** `TODO` — implementation is substantially complete; release-readiness follow-up remains: 2 `TODO`, 1 `IN_PROGRESS`, 3 `BLOCKED`, and 4 `DEFERRED`, alongside 55 closed dispositions (53 `DONE`, 1 `RESOLVED`, 1 `WONTFIX`). 65 tasks total.
**Owner:** repository maintainer
**Basis:** [`00-Comparison-and-Gap-Analysis.md`](00-Comparison-and-Gap-Analysis.md)

This backlog captures the improvement ideas derived from the sibling-library comparison.
Folders run in numeric phase order (`00` → `14`); inside each folder the `INDEX.md` is the
group roll-up and task files run in numeric file order. Every task carries a `Status` field.
This folder is local-only (`private/` is not committed, published, or packaged).

## Status legend

| Status | Meaning |
|---|---|
| `TODO` | Identified, not started. |
| `IN_PROGRESS` | Actively being worked. |
| `DONE` | Complete and verified. |
| `BLOCKED` | Needs a decision, secret, environment, or upstream dependency. |
| `DEFERRED` | Deliberately postponed; rationale recorded. |
| `WONTFIX` | Considered and rejected; rationale recorded. |

Group roll-up is separate from task status. `OPEN` means at least one task still needs work or
verification. `RESOLVED` means every task in that group has an explicit disposition; it does not mean
that `DEFERRED` or `WONTFIX` features have been implemented.

## Master execution table

`Group` follows folder order (execution phase). `Order` is the execution sequence inside the
group (`—` = group roll-up row). `Status` is the task file's own `Status`.

| Group | Order | Task | Scope | Status |
|---|---|---|---|---|
| `00` (basis) | — | [`00-Comparison-and-Gap-Analysis.md`](00-Comparison-and-Gap-Analysis.md) | Sibling-library comparison, gaps G1–G16, method | Basis |
| `01-DeveloperExperience` | — | [`INDEX.md`](01-DeveloperExperience/INDEX.md) | Typed API, accessibility, config, picker, presets | `DONE` |
| `01-DeveloperExperience` | 1 | `TASK-DX-01-typed-icon-name-catalogue.md` | Strongly-typed icon-name catalogue | `DONE` |
| `01-DeveloperExperience` | 2 | `TASK-DX-02-accessibility-parameters.md` | Accessibility-first icon parameters | `DONE` |
| `01-DeveloperExperience` | 3 | `TASK-DX-03-global-configuration-di.md` | Global configuration & DI defaults | `DONE` |
| `01-DeveloperExperience` | 4 | `TASK-DX-04-icon-metadata-search-picker.md` | Icon metadata, search & IconPicker | `DONE` |
| `01-DeveloperExperience` | 5 | `TASK-DX-05-generated-preset-components.md` | Generated per-icon preset components | `DONE` |
| `02-PlatformCompatibility` | — | [`INDEX.md`](02-PlatformCompatibility/INDEX.md) | .NET 8, hosting models, AOT/trimming, RTL, inline SVG | `RESOLVED` |
| `02-PlatformCompatibility` | 1 | `TASK-PLAT-01-net8-target.md` | .NET 8 target (baseline amended) | `DONE` |
| `02-PlatformCompatibility` | 2 | `TASK-PLAT-02-blazor-web-app-hosting-models.md` | Blazor Web App hosting-model support | `DONE` |
| `02-PlatformCompatibility` | 3 | `TASK-PLAT-03-trimming-aot.md` | Trimming/AOT safety | `DONE` |
| `02-PlatformCompatibility` | 4 | `TASK-PLAT-04-rtl-localization.md` | RTL & localization | `WONTFIX` |
| `02-PlatformCompatibility` | 5 | `TASK-PLAT-05-inline-svg-rendering.md` | Inline SVG via optional `Svg` package (core unchanged) | `RESOLVED` |
| `02-PlatformCompatibility` | 6 | `TASK-PLAT-06-optional-package-trimming-aot.md` | Optional-package trimming/AOT contract | `DONE` |
| `03-Documentation` | — | [`INDEX.md`](03-Documentation/INDEX.md) | Docs site, README/migration, API reference | `DONE` |
| `03-Documentation` | 1 | `TASK-DOC-01-documentation-site.md` | Documentation site with live examples | `DONE` |
| `03-Documentation` | 2 | `TASK-DOC-02-readme-migration-consolidation.md` | README & migration guide consolidation | `DONE` |
| `03-Documentation` | 3 | `TASK-DOC-03-xml-docs-api-reference.md` | XML docs & generated API reference | `DONE` |
| `04-QualityRelease` | — | [`INDEX.md`](04-QualityRelease/INDEX.md) | Browser tests, coverage, signing/publish, analyzers | `DEFERRED` |
| `04-QualityRelease` | 1 | `TASK-QA-01-e2e-verification-ci.md` | Browser/E2E verification in CI | `DONE` |
| `04-QualityRelease` | 2 | `TASK-QA-02-coverage-gate.md` | Code coverage gate & report | `DONE` |
| `04-QualityRelease` | 3 | `TASK-QA-03-signing-release-automation.md` | NuGet signing & release automation | `DEFERRED` |
| `04-QualityRelease` | 4 | `TASK-QA-04-analyzer-strictness.md` | Analyzer strictness to `recommended` | `DONE` |
| `05-FeatureCompleteness` | — | [`INDEX.md`](05-FeatureCompleteness/INDEX.md) | FA token/catalogue audit, webfont behavior, Pro verification | `DONE` |
| `05-FeatureCompleteness` | 1 | `TASK-FA-01-fa7-token-coverage-audit.md` | FA 7.x token coverage audit | `DONE` |
| `05-FeatureCompleteness` | 2 | `TASK-FA-02-webfont-degradation.md` | Webfont-mode degradation handling | `DONE` |
| `05-FeatureCompleteness` | 3 | `TASK-FA-03-pro-styles-verification.md` | Pro styles verification | `DONE` |
| `06-InteractiveExplorer` | — | [`INDEX.md`](06-InteractiveExplorer/INDEX.md) | Interactive explorer (search, preview, snippets) | `DONE` |
| `06-InteractiveExplorer` | 1 | `TASK-IE-01-requirements-and-audit.md` | Requirements, UX, API surface audit, scope | `DONE` |
| `06-InteractiveExplorer` | 2 | `TASK-IE-02-architecture-and-data.md` | Data model, search/index, routing, state | `DONE` |
| `06-InteractiveExplorer` | 3 | `TASK-IE-03-ui-shell-and-search.md` | Layout, filters, search, virtualized grid | `DONE` |
| `06-InteractiveExplorer` | 4 | `TASK-IE-04-icon-preview-controls.md` | Live controls for all core features | `DONE` |
| `06-InteractiveExplorer` | 5 | `TASK-IE-05-snippet-generator.md` | Razor/markup/code generation + copy | `DONE` |
| `06-InteractiveExplorer` | 6 | `TASK-IE-06-theming-and-accessibility.md` | Dark/light, a11y, keyboard nav | `DONE` |
| `06-InteractiveExplorer` | 7 | `TASK-IE-07-samples-integration.md` | Samples integration, navigation | `DONE` |
| `06-InteractiveExplorer` | 8 | `TASK-IE-08-tests-and-e2e.md` | Unit/integration + Playwright smoke tests | `DONE` |
| `06-InteractiveExplorer` | 9 | `TASK-IE-09-polish-and-docs.md` | README update, screenshots, final QA | `DONE` |
| `07-ReleaseAndAdoption` | — | [`INDEX.md`](07-ReleaseAndAdoption/INDEX.md) | Ship 2.0 (release-time tasks held by owner until all else closes) | `DEFERRED` |
| `07-ReleaseAndAdoption` | 1 | `TASK-REL-01-publish-2.0-packages.md` | Publish the 2.0.0 packages to nuget.org | `DEFERRED` |
| `07-ReleaseAndAdoption` | 2 | `TASK-REL-02-nuget-discoverability-metadata.md` | Icon, tags, README, release-notes links | `DONE` |
| `07-ReleaseAndAdoption` | 3 | `TASK-REL-03-deprecate-stale-1x-line.md` | Deprecate 1.x with a migration pointer | `DEFERRED` |
| `07-ReleaseAndAdoption` | 4 | `TASK-REL-04-publish-docs-site-automatically.md` | CI-deployed public docs/demo site | `DEFERRED` |
| `08-ZeroSetupAndTemplates` | — | [`INDEX.md`](08-ZeroSetupAndTemplates/INDEX.md) | One-line setup, templates, migration guides, analyzer | `DONE` |
| `08-ZeroSetupAndTemplates` | 1 | `TASK-SET-01-one-line-setup-asset-wiring.md` | One service call + one map call, no script tags | `DONE` |
| `08-ZeroSetupAndTemplates` | 2 | `TASK-SET-02-dotnet-new-templates.md` | Template packages with FA wired and smoke-tested | `DONE` |
| `08-ZeroSetupAndTemplates` | 3 | `TASK-SET-03-competitor-migration-guides.md` | MudBlazor/Blazorise/EasyAppDev/Blazicons/raw-class guides | `DONE` |
| `08-ZeroSetupAndTemplates` | 4 | `TASK-SET-04-roslyn-analyzer-code-fixes.md` | Analyzer + code fixes for the typed API | `DONE` |
| `09-NativeSvgPackage` | — | [`INDEX.md`](09-NativeSvgPackage/INDEX.md) | Inline-SVG revisit, optional package, size benchmarks | `DONE` |
| `09-NativeSvgPackage` | 1 | `TASK-SVG-01-inline-svg-feasibility-spike.md` | Measured spike + amendment proposal | `DONE` |
| `09-NativeSvgPackage` | 2 | `TASK-SVG-02-optional-inline-svg-package.md` | `Jenin.FontAwesome.Blazor.Svg` on amendment | `DONE` |
| `09-NativeSvgPackage` | 3 | `TASK-SVG-03-bundle-size-benchmark-dashboard.md` | CI-regenerated size table per mode | `DONE` |
| `10-ProKitsAndCustomIcons` | — | [`INDEX.md`](10-ProKitsAndCustomIcons/INDEX.md) | Licensed-Pro proof, Kit integration, custom-icon registration | `BLOCKED` |
| `10-ProKitsAndCustomIcons` | 1 | `TASK-PRO-01-licensed-pro-render-verification.md` | Repeatable licensed-Pro render proof | `BLOCKED` |
| `10-ProKitsAndCustomIcons` | 2 | `TASK-PRO-02-font-awesome-kit-integration.md` | Kit setup path + custom Kit icons | `DONE` |
| `10-ProKitsAndCustomIcons` | 3 | `TASK-PRO-03-consumer-custom-icon-registration.md` | Registered glyphs through the same API | `DONE` |
| `11-EcosystemAndShowcase` | — | [`INDEX.md`](11-EcosystemAndShowcase/INDEX.md) | Suite adapters, llms.txt + cookbook, gallery, reduced motion | `DONE` |
| `11-EcosystemAndShowcase` | 1 | `TASK-ECO-01-ui-suite-icon-adapters.md` | MudBlazor/Blazorise icon-slot adapters | `DONE` |
| `11-EcosystemAndShowcase` | 2 | `TASK-ECO-02-llms-txt-recipe-cookbook.md` | `llms.txt` + 12-recipe cookbook, e2e-verified | `DONE` |
| `11-EcosystemAndShowcase` | 3 | `TASK-ECO-03-real-world-example-gallery.md` | Composed screens with visual tests | `DONE` |
| `11-EcosystemAndShowcase` | 4 | `TASK-ECO-04-reduced-motion-support.md` | `prefers-reduced-motion` for animations | `DONE` |
| `12-ComposedComponents` | — | [`INDEX.md`](12-ComposedComponents/INDEX.md) | Buttons, indicators, inputs, blocks, IconFor (Phase 1) | `DONE` |
| `12-ComposedComponents` | 1 | `TASK-UI-01-accessible-icon-button-family.md` | `IconButton`/`IconToggleButton`/`FavoriteButton` | `DONE` |
| `12-ComposedComponents` | 2 | `TASK-UI-02-live-indicators.md` | `NotificationBadge`/`StatusDot`/`IconProgress`/`AvatarIcon` | `DONE` |
| `12-ComposedComponents` | 3 | `TASK-UI-03-form-ready-inputs.md` | `IconPickerField` + `IconRating` | `DONE` |
| `12-ComposedComponents` | 4 | `TASK-UI-04-content-blocks.md` | `StatCard`/`EmptyState`/`ActivityItem`/`ShareButtons` | `DONE` |
| `12-ComposedComponents` | 5 | `TASK-UI-05-enum-driven-iconfor.md` | Attribute-declared enum → icon rendering | `DONE` |
| `13-AdvancedRendering` | — | [`INDEX.md`](13-AdvancedRendering/INDEX.md) | Triggers, transitions, duotone, sprite, subsetter, SVG strings (Phase 1) | `DONE` |
| `13-AdvancedRendering` | 1 | `TASK-FX-01-animation-triggers.md` | Hover/parent-hover/focus/scroll-reveal triggers | `DONE` |
| `13-AdvancedRendering` | 2 | `TASK-FX-02-animated-icon-state-transitions.md` | `IconTransition`/`TwoStateIcon` animated swaps | `DONE` |
| `13-AdvancedRendering` | 3 | `TASK-FX-03-themed-duotone-palettes.md` | `DuotonePalettes` + cascading `IconTheme` | `DONE` |
| `13-AdvancedRendering` | 4 | `TASK-FX-04-svg-sprite-use-mode.md` | Single-request `<svg><use>` sprite mode | `DONE` |
| `13-AdvancedRendering` | 5 | `TASK-FX-05-subsetter-usage-report.md` | Used-icon subsetter + savings report | `DONE` |
| `13-AdvancedRendering` | 6 | `TASK-FX-06-server-side-svg-strings.md` | Standalone SVG strings for email/PDF/report | `DONE` |
| `14-ReleaseReadiness` | — | [`INDEX.md`](14-ReleaseReadiness/INDEX.md) | Resolve package inventory and close pre-release quality, metadata, docs, and deployment gaps | `IN_PROGRESS` |
| `14-ReleaseReadiness` | 1 | `TASK-RDY-01-package-inventory-and-release-scope.md` | Owner decision: which packable package IDs belong in 2.0.0 | `BLOCKED` |
| `14-ReleaseReadiness` | 2 | `TASK-RDY-02-package-metadata-and-licensing.md` | Align NuGet license/readme/icon/tags for the approved package set | `TODO` |
| `14-ReleaseReadiness` | 3 | `TASK-RDY-03-composed-icon-base-api.md` | Make the public composed-icon base genuinely extensible | `DONE` |
| `14-ReleaseReadiness` | 4 | `TASK-RDY-04-docs-and-changelog-consistency.md` | Reconcile TFMs, install guidance, API reference scope, links, and 2.0 notes | `TODO` |
| `14-ReleaseReadiness` | 5 | `TASK-RDY-05-ci-gate-parity.md` | Run the documented encoding/generator/template/size gates in CI | `IN_PROGRESS` |
| `14-ReleaseReadiness` | 6 | `TASK-RDY-06-remote-preflight.md` | Validate CI, Pages, and OIDC setup on a clean remote branch without publishing | `BLOCKED` |

Tallies: 65 task files — 53 `DONE`, 1 `RESOLVED` (PLAT-05, delivered by the optional
`Svg` package), 1 `WONTFIX` (PLAT-04), 3 `BLOCKED` (PRO-01 licensed Pro assets; RDY-01 package
inventory decision; RDY-06 remote owner setup), 4 `DEFERRED` (QA-03 first-publish verify; REL-01/
REL-03/REL-04: owner hold on publishing until every other task closes), 2 `TODO` (RDY-02, RDY-04),
and 1 `IN_PROGRESS` (RDY-05 CI gates). Groups: 9 `DONE` (01, 03, 05, 06,
08, 09, 11, 12, 13), 1 `RESOLVED` (02), 1 `BLOCKED` (10 PRO-01), 2 `DEFERRED` (04 QA-03, 07),
1 `IN_PROGRESS` (14).
`private/` stays gitignored; only the tracked master `INDEX.md` is committed.

## Summary of gaps → tasks

- G1 strongly-typed names → `TASK-DX-01`
- G3 accessibility → `TASK-DX-02`
- G4 DI/config → `TASK-DX-03`
- G5 metadata/picker → `TASK-DX-04`
- G2 per-icon presets → `TASK-DX-05`
- G6 .NET 8 → `TASK-PLAT-01`
- G7 hosting models → `TASK-PLAT-02`
- G8 trimming/AOT → `TASK-PLAT-03`; inline-SVG/no-FA-runtime → `TASK-PLAT-05`
- G16 optional-package trimming/AOT evidence → `TASK-PLAT-06`
- G11 RTL/localization → `TASK-PLAT-04`
- G9 docs site → `TASK-DOC-01`
- (D9-05) README/migration → `TASK-DOC-02`
- G10 API docs → `TASK-DOC-03`
- G12 e2e in CI → `TASK-QA-01`
- G13 coverage gate → `TASK-QA-02`
- (D9-02) signing/publish → `TASK-QA-03`
- (D9-06) analyzer strictness → `TASK-QA-04` (delivered — `recommended` adopted; see the task file)
- FA token coverage → `TASK-FA-01` (delivered — 2,163/1,992 reconciled; audit matrix in `private/reports/FA-01-fa7-token-coverage-audit.md`)
- webfont degradation → `TASK-FA-02` (delivered — README feature × mode matrix cross-linked from setup + troubleshooting; webfont e2e spec 6/6)
- Pro-style verification → `TASK-FA-03` (delivered — FA 7 family × style contract verified; `fa-duotone` emission defect fixed with `Typeface.Duotone`/`SharpDuotone` added; Free-tier fallback measured live)
- Source-generator implementation → no gap: deterministic generators already produce the public typed catalogues and preset components; no consumer capability is missing.
- Adoption refresh (2026-10-25): public package still 1.2.0 → `TASK-REL-01`…`TASK-REL-04`
  (`REL-02` `DONE`, the rest release-time `DEFERRED` per owner hold); manual setup friction →
  `TASK-SET-01`/`TASK-SET-02` (both `DONE` 2026-10-26); switching costs → `TASK-SET-03`
  (`DONE` 2026-10-26); IDE guidance → `TASK-SET-04` (`DONE` 2026-10-26); inline-SVG objection →
  `TASK-SVG-01` (proposal) + `TASK-SVG-02` (`DONE` 2026-10-26 under the granted amendment) + honest
  numbers via `TASK-SVG-03` (`DONE` 2026-10-26); Pro/Kit/custom glyphs → `TASK-PRO-01` (`BLOCKED`
  on owner Pro assets), `TASK-PRO-02`/`TASK-PRO-03` (`DONE` 2026-10-26); suite embedding →
  `TASK-ECO-01` (`DONE`); AI-era docs + recipes → `TASK-ECO-02` (`DONE`); composed screens →
  `TASK-ECO-03` (`DONE`); motion safety → `TASK-ECO-04` (`DONE`).
- Differentiate-first (2026-10-26, owner directive: no release before distinctive): release work
  gated in `07` (Phase 2); Phase-1 differentiation → button family `TASK-UI-01`, live indicators
  `TASK-UI-02`, form inputs `TASK-UI-03`, content blocks `TASK-UI-04`, enum `IconFor`
  `TASK-UI-05`; animation triggers `TASK-FX-01`, state transitions `TASK-FX-02`, duotone themes
  `TASK-FX-03`, sprite mode `TASK-FX-04`, subsetter + usage report `TASK-FX-05`, server-side SVG
  strings `TASK-FX-06` (all `DONE`).
- Phase-2 release (2026-10-27): prepared and held. `TASK-REL-02` `DONE`
  (FA7/accessibility/trimming/AOT tags + 4/4 local pack proof); `TASK-REL-01` `DEFERRED` (release
  workflow now packs/publishes 4 packages + symbols; fresh net8/9/10 consumers build; waits on the
  trusted-publishing setup (`NUGET_USER`, `production`, nuget.org policy) + go-ahead + `v2.0.0`);
  `TASK-REL-04` `DEFERRED` (the `docs-build.yml` DocFX + Pages-artifact pipeline is ported and
  smoke-tested 4/4; Pages source must switch to GitHub Actions); `TASK-REL-03` `DEFERRED` (after REL-01).
  Owner policy: no publishing until every other task closes. The same audit repaired the copied CI
  workflow (wrong branch, nonexistent projects, missing e2e job, un-installable SDK pin) and the
  coverage gate's double-counted file union (91.23%, ratchet 64→90); see
  `TASK-QA-01`/`TASK-QA-02`. `RDY-03` later made the base genuinely extensible
  (`protected abstract`, documented contract, external-derivation proof); the baseline was
  regenerated again without new warnings.
- Completion audit (2026-10-28): no `TODO` task predates the readiness review. All 65 files have a disposition: 53 `DONE` (incl. RDY-03 with external-derivation proof), `PLAT-05` `RESOLVED`, `QA-03` `DEFERRED` (no author cert; OIDC publish path; first-publish verify pending), `PRO-01`/`RDY-01`/`RDY-06` `BLOCKED` (no licensed Pro assets / package-scope decision / remote owner setup), `REL-01`/`REL-03`/`REL-04` `DEFERRED` as release-time work, 2 `TODO` (RDY-02, RDY-04), 1 `IN_PROGRESS` (RDY-05 CI gates), and `PLAT-04` `WONTFIX` (documented decision).
- Quality hardening (2026-10-27): local SonarQube Community Build 26.10 is green — **0 open
  issues**, **0.0% duplication**, **Quality Gate OK**; overall coverage **82.7%** (line 90.4%,
  branch 69.3%) and the canonical four-package coverage gate **91.4% ≥ 90**. The 15 Roslyn/IDE
  findings were fixed and the shared composed-icon rendering was deduplicated into
  `ComposedIconElement` + internal helpers (public API baseline regenerated). The repository
  scripts (clean/coverage/format/docs/badges/sonar) are ported; docs deploy through the DocFX +
  Pages-artifact workflow and packages publish through OIDC trusted publishing. Build, full test
  suite, formatting, encoding, docs checks, and the coverage/trim gates are green.
- Release-readiness review (2026-10-28): no additional product features are recommended before
  2.0. The audit found a package-scope mismatch (8 packable IDs vs 4 in the publish workflow),
  Svg license metadata that says MIT despite bundled CC BY 4.0 artwork, a public but externally
  non-derivable `ComposedIconElement`, README/API-reference/CHANGELOG inconsistencies, and CI
  gates documented but not wired. Six tasks (`TASK-RDY-01`…`TASK-RDY-06`; the embargo-guard draft was removed as contradicting the normal auto-deploy flow) track those closure items. Decisions
  recorded 2026-10-28: no publish until all tasks close; OIDC trusted publishing instead of an
  author-signing certificate; no Pro license available so the licensed proof stays blocked with the
  fallback documented; RDY-03 executed (protected contract + external proof). Existing task files
  keep their history; only this master index gains the new group and totals.

## Conventions

- One task per file, named `<ID>-<short-slug>.md`, grouped in a numbered folder with its own `INDEX.md`.
- Each task file starts with `# <ID> — <Title>`, then a `**Status:**` line, the governance reminder,
  group, priority, suggested prompt filename, and recommended model, followed by `## Gap`, `## Mission`,
  `## Acceptance criteria`, and `## Dependencies / References`.
- Statuses are manually maintained by the maintainer. Update the task file, the group `INDEX.md`,
  **and the `Order`/`Status` cells of the master execution table above** when a status changes.
- Every task file must remind contributors to follow `private/constitution.md`, `.editorconfig`, and
  `.gitattributes`.
- Change a task to `DONE` only after its acceptance criteria pass, all applicable analyzer/compiler
  diagnostics in the change scope are fixed, relevant checks run for all supported TFMs, and actual
  commands/results are recorded in the task and group index. Do not suppress or weaken analyzer rules
  to pass this gate. Use `WONTFIX`, `DEFERRED`, or `BLOCKED` when the feature is not implemented.
- Keep work cohesive and small: state one user-visible outcome per task, preserve the ratified
  architecture, use a focused red/green/refactor loop for behavior changes, and avoid speculative
  abstractions or unrelated refactoring.
