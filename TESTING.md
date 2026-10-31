# Testing Jenin.FontAwesome.Blazor

This document defines the test strategy for `Jenin.FontAwesome.Blazor`. It describes the testing
policy, what will be tested, the testability requirements the architecture must satisfy, and how the
tests will be run. See [CONTRIBUTING.md](CONTRIBUTING.md) for the contribution process.

## Policy

- **Framework: MSTest.** Tests use `MSTest.TestFramework`, `MSTest.TestAdapter`, and
  `Microsoft.NET.Test.Sdk`. Test discovery, attributes (`[TestClass]`, `[TestMethod]`,
  `[DataRow]`, `[TestCategory]`), and outcomes stay on MSTest.
- **Assertions: Jeninnet.Assertions.** All correctness assertions use the fluent `Should()`
  API from the `Jeninnet.Assertions` NuGet package (pinned centrally in
  `Directory.Packages.props`), called **directly** inside test methods — the same pattern the
  library's own consumer suite uses. The `Jeninnet.Assertions.MSTest` adapter package is
  deliberately *not* referenced: direct `Should()` keeps every assertion a single expression,
  and MSTest already reports the thrown `AssertionFailedException` as a test failure.
  `Assert.Inconclusive` is the sole remaining MSTest `Assert` member (test-outcome control,
  not a correctness assertion). Adopted 2026-10-31 by maintainer decision; the 1.0.0 package
  surface used here is `Be/NotBe`, `BeTrue/BeFalse`, `BeNull/NotBeNull`, `BeEmpty/NotBeEmpty`,
  `BeSameAs`, `BeOfType` (exact type) vs `BeAssignableTo` (MSTest `IsInstanceOfType` semantics),
  `Contain/NotContain`, `StartWith/EndWith`, `Equal` (order-sensitive) vs `BeEquivalentTo`,
  `HaveCount`, `BeGreaterThanOrEqualTo`, `ThrowExactly` via `FluentActions.Invoking`, and
  `because` messages. Reference-equality assertions use `ReferenceEquals(...).Should().BeTrue()`
  (`BeSameAs` exists only for single objects, not collections).
- **No other third-party test or assertion libraries.** xUnit, NUnit, Moq, FluentAssertions, bUnit, and
  similar packages are not used. Test doubles are hand-written when a seam needs one.
- **Deferred test project (introduced).** The test project was intentionally introduced *after* the
  library architecture and implementation stabilised, so that tests validate the designed contract
  rather than freezing an in-progress implementation. It lives at
  `tests/Jenin.FontAwesome.Blazor.Tests/`; the solution build, analyzers, sample applications, and the
  generated package remain part of the validation surface.
- **Testability is an architectural requirement.** Deterministic behaviour, explicit boundaries,
  separable responsibilities, and pure serialization are required so that the contract can be tested
  without a browser, a JavaScript runtime, or a running Font Awesome instance.

## Test project

| Item | Value |
|---|---|
| Name | `Jenin.FontAwesome.Blazor.Tests` |
| Location | `tests/Jenin.FontAwesome.Blazor.Tests/` |
| Target frameworks | `net8.0;net9.0;net10.0` (mirrors the library so all supported TFMs are exercised) |
| Package versions | Declared centrally in `Directory.Packages.props` (Central Package Management) |
| Project type | `Microsoft.NET.Sdk`, `IsTestProject=true`, `IsPackable=false` |
| References | The library project; no Font Awesome runtime, no browser host |

## What we test

Tests are grouped by category using `[TestCategory]` so categories can be run selectively.

### 1. Model serialization (`Model`)

The animation and transformation models are pure value objects with deterministic serialization, and
are the highest-value, lowest-cost tests.

- Animation classes: each concrete animation serializes its Font Awesome class
  (`Beat` → `fa-beat`, and so on) via `BuildClasses()`.
- Animation styles: `BuildStyles()` emits `--fa-animation-delay`, `--fa-animation-duration`,
  `--fa-animation-direction`, `--fa-animation-iteration-count`, and `--fa-animation-timing` only
  when the corresponding option is set; time values are formatted as invariant-culture milliseconds.
- Fluent purity: `SetDelay`, `SetDuration`, `SetDirection`, `SetIterationCount`, and
  `SetTimingFunction` return a new instance of the same runtime type and never mutate the source.
- Normalization: iteration count is clamped to non-negative; unit-interval opacity/vector values are
  clamped to `[0, 1]`; invalid inputs are normalized at construction.
- Transformation tokens: each transformation serializes to its ordered invariant token (`grow-6`,
  `shrink-8`, `rotate--90`, `flip-h`, `flip-v`, positioning tokens), formatted to the documented
  precision and invariant culture.
- Transformation composition: `Transform` preserves insertion order, space-joins tokens via
  `GetTransformation()`, and every operation returns a new instance without mutating the receiver.
- Value enums and extensions: `IconSize`, `IconStyle`, `IconRotate`, `IconStackSize`, `Typeface`,
  `BorderStyle`, `PullPosition`, and `AnimationDirection` map to their documented Font Awesome
  tokens; `DictionaryExtensions` (`AddOrUpdate`, `AddOrUpdateIf`, `AddOrUpdateIfNotNull`,
  `UpdateOrRemoveIf`, `JoinAsStyles`) honour null/empty/update semantics.
- Culture independence: serialization produces identical output under a culture that uses a
  comma decimal separator.

### 2. Rendering contracts (`Rendering`)

Rendering is tested through first-party Blazor primitives, without a browser:

- **`HtmlRenderer`** (`Microsoft.AspNetCore.Components.Web.HtmlRenderer`) renders a component to an
  HTML string; assert the produced markup, classes, styles, and attributes. It is the only rendering
  primitive used: render-tree types (`Microsoft.AspNetCore.Components.RenderTree.*`) emit analyzer
  warning `BL0006` and are avoided to keep the suite free of suppressions and warnings. As a result,
  boundary `@key` values, event bindings, and lifecycle ordering are not asserted directly.

Contract coverage:

- `Icon` renders a Blazor-owned boundary element (`<span>`) that carries no Font Awesome class, with
  the enhanceable element (`<i>`) nested inside and carrying the Font Awesome attribute contract.
- The full parameter matrix serializes to the expected classes and styles: size, fixed width, border
  (color, padding, radius, style, width), inverse/inverse color, background color, color, padding,
  margin, typeface, icon style, icon name, rotate and rotate angle, stack size, stack z-index, pull
  position and pull margin, mask (`data-fa-mask`, `data-fa-mask-id`), and duotone
  (`--fa-primary-*`, `--fa-secondary-*`, `fa-swap-opacity`).
- `AdditionalAttributes` (unmatched values) are forwarded to the rendered element.
- Events are attached only to the non-enhanceable boundary node, never to the enhanceable element.
  (Structural guard asserted by design review; not asserted by the HTML-only MSTest suite because it
  requires render-tree inspection, which emits `BL0006`.)
- Container and composition components render the expected structure: `IconContainer`,
  `IconLayersContainer`, `IconStackContainer`, `CounterLayer`, `TextLayer`, `IconList`, and
  `IconListItem`; the `IconContainer` cascading value reaches descendant `Icon` components.
- Boundary keying: identical parameters yield an identical boundary key, and a change to any
  Font Awesome-visible parameter changes the key (which recreates the subtree so Font Awesome
  re-enhances it). Not asserted by the HTML-only MSTest suite (render-tree inspection emits `BL0006`);
  the deterministic markup is asserted instead.

### 3. Static-asset and interop contract (`Contract`)

The 2.0 library is a component library with zero JavaScript interop, and it ships a closed set of
static assets. These are cheap, high-signal assertions:

- The library assembly contains no JavaScript interop: reflection asserts the absence of
  `IJSRuntime`, `JSRuntime`, `ElementReference` parameters, `InvokeVoidAsync`/`InvokeAsync` calls,
  `[JSInvokable]`, and JS module imports.
- The library ships exactly two static assets, `wwwroot/js/fontawesome-config-nest.js` and
  `wwwroot/js/fontawesome-config-webfont.js`, and no Font Awesome CSS, JavaScript, fonts, or SVGs.
- The config scripts remain non-destructive and idempotent: each sets only `autoReplaceSvg`
  (to `"nest"` and `false` respectively) while preserving unrelated `window.FontAwesomeConfig`
  options. JavaScript execution itself is out of scope for MSTest; it is covered by the runtime
  acceptance harness (sample applications).
- The packaged static-asset paths follow `_content/Jenin.FontAwesome.Blazor/js/<asset>`.

### 4. Regression and public-surface guards (`Regression`)

- Every defect fixed during implementation gains a regression test that fails before the fix and
  passes after it.
- The compiled public surface matches the documented contract: a reflection test asserts the exact
  set of public/protected types, that no `[Obsolete]` member is present, that `Icon.Animation` is
  typed `Animation?`, and that component serialization internals stay non-public.

### 5. Package contract (`Contract`, owned with release engineering)

Package contents (all target-framework assemblies, the two static assets, `README.md`, and
`LICENSES`, and the absence of any sample or test assemblies) are validated as part of the package
and release gates, not duplicated in the library test project.

**Optional-package trimming/AOT contract.** The three optional packages
(`Jenin.FontAwesome.Blazor.IconPicker`, `Jenin.FontAwesome.Blazor.Icons`,
`Jenin.FontAwesome.Blazor.Svg`) carry their own `IsTrimmable`/`IsAotCompatible` contract, verified
in two complementary ways (the `MudBlazor`/`Blazorise` adapters carry the same declarations,
build-verified; the analyzer ships no runtime IL and the template pack no binaries):

- MSTest `Contract` classes (`OptionalPackageTrimmingAotContractTests`) assert each package's csproj
  declaration and the compiled `AssemblyMetadata("IsTrimmable", "True")` on every supported TFM.
- The publish-level gate `pwsh ./eng/verify-optional-trim.ps1` publishes disposable probe
  executables with `PublishTrimmed=true; TrimMode=full` for every supported TFM and proves that:
  the IconPicker embedded metadata remains loadable end to end (manifest resource → JSON parse →
  search); a referenced generated preset component survives; an unreferenced preset is removed
  from the published assembly; and the same retention/removal contract holds for a referenced vs
  unreferenced generated inline-SVG component. In CI, the docs sample (a Blazor WebAssembly host
  that transitively references the optional packages) is additionally published trimmed by the
  sample gate.

## Testability requirements

These requirements keep the contract testable and must hold through implementation:

- Pure, deterministic serialization: `BuildClasses`, `BuildStyles`, and `GetTransformation` depend
  only on their inputs and never mutate state, read ambient culture, use the clock, or use
  randomness.
- No ambient global state in serialization: no `DateTime.Now`, `Random`, or generated identifiers on
  the serialization path.
- Serialization is separated from rendering: components serialise to values that tests can assert
  directly, and the rendered markup is a deterministic function of the parameters.
- Components render synchronously through the Blazor render tree and require no hosting model, no
  JavaScript, and no dependency injection beyond what Blazor provides.
- The public surface stays reflection-inspectable; internal seams remain `private protected` or
  `internal` so the contract is explicit.

## Conventions

- One test class per production type, in a namespace mirroring the production namespace
  (`Jenin.FontAwesome.Blazor.Tests.Animations`, and so on).
- Name tests by behaviour: `Method_State_ExpectedResult`.
- Use `[TestMethod]` with `[DataRow]` for parameterised cases (`[DataTestMethod]` is obsolete in
  MSTest 4 and would raise analyzer warning `MSTEST0044`).
- Use `[TestCategory]` with one of `Model`, `Rendering`, `Contract`, or `Regression`.
- Structure each test as Arrange / Act / Assert.

## Running the tests

Once the test project exists:

    dotnet test "Jenin.FontAwesome.Blazor.slnx"
    ./build.ps1 -Target Test
    ./build.sh Debug test

Tests run against every supported target framework. Continuous integration runs the same commands as
part of the quality gates.

## Code coverage

Coverage is measured with `coverlet.collector` and gated in CI. The metric is
line coverage merged across the four test projects on the canonical net10.0 leg, so core, IconPicker,
Icons, and Svg are counted once each (a line covered by any run counts as covered):

- Collector configuration: `tests/coverage.runsettings` (Cobertura output, generated files and the
  test assemblies excluded from the measurement).
- Threshold ratchet: `eng/coverage-threshold.txt` (a single percentage line). CI fails under it; when
  real coverage improves (through new tests or deleted dead code), deliberately raise the ratchet in
  the same change.
- Gate script: `pwsh ./eng/verify-coverage.ps1` (expects the collector reports under
  `artifacts/coverage` and emits the merged rate).

Local workflow:

    Remove-Item artifacts/coverage -Recurse -Force
    dotnet test tests/Jenin.FontAwesome.Blazor.Tests/Jenin.FontAwesome.Blazor.Tests.csproj -f net10.0 -c Release --collect:"XPlat Code Coverage" --settings tests/coverage.runsettings --results-directory artifacts/coverage
    dotnet test tests/Jenin.FontAwesome.Blazor.IconPicker.Tests/Jenin.FontAwesome.Blazor.IconPicker.Tests.csproj -f net10.0 -c Release --collect:"XPlat Code Coverage" --settings tests/coverage.runsettings --results-directory artifacts/coverage
    dotnet test tests/Jenin.FontAwesome.Blazor.Icons.Tests/Jenin.FontAwesome.Blazor.Icons.Tests.csproj -f net10.0 -c Release --collect:"XPlat Code Coverage" --settings tests/coverage.runsettings --results-directory artifacts/coverage
    dotnet test tests/Jenin.FontAwesome.Blazor.Svg.Tests/Jenin.FontAwesome.Blazor.Svg.Tests.csproj -f net10.0 -c Release --collect:"XPlat Code Coverage" --settings tests/coverage.runsettings --results-directory artifacts/coverage
    pwsh ./eng/verify-coverage.ps1

CI runs the identical collection after the Test step, fails the job under the threshold, and uploads
`artifacts/coverage/**/coverage.cobertura.xml` as the `coverage-report` artifact (a hosted coverage
service such as Codecov can consume the same Cobertura files later if wanted). Reports must come from
one fresh collection: always delete `artifacts/coverage` before collecting, otherwise stale files
inflate the union.

Branch coverage is deliberately not gated: Cobertura reports are merged across runs of separate test
projects, and neither unioning branches across reports nor pretending per-report branch rates
aggregate faithfully is trustworthy; line coverage is the stable, deterministic signal here.

## Bundle-size dashboard

Consumer-facing size numbers are measured, not estimated. The gate script
`pwsh ./eng/measure-bundle-sizes.ps1` publishes disposable trimmed fixture apps (1-icon and
10-icon hosts for the Icons and Svg packages, net10.0/Release), measures the committed
2-icon subset/sprite fixtures and the vendored Font Awesome 7.3.1 status-quo assets, packs both
optional packages, and rewrites the dashboard table in `README.md` between the
`BUNDLE-SIZES` markers. Every cell records raw bytes plus Brotli bytes (the .NET default
encoder as a consistent comparator) and is compared against the committed
`eng/bundle-sizes.json` baseline: growth beyond `+5%` (override with `-ThresholdPercent`)
fails the gate so a human reviews the delta before it lands; shrinkage never fails.

Local workflow:

    pwsh ./eng/measure-bundle-sizes.ps1 -Check            # measure + gate only, no file writes
    pwsh ./eng/measure-bundle-sizes.ps1 -UpdateBaseline   # measure + rewrite README + refresh baseline

CI runs the `-Check` leg after the Test step. Re-bless the baseline only after reviewing the
delta (`-UpdateBaseline`), and record the new numbers as the updated baseline.

## Template smoke

The `dotnet new` templates (`templates/`) are verified end to end by
`pwsh ./eng/verify-templates.ps1`: it packs the library and the template pack into a local
feed, installs the templates, scaffolds each one with no edits (Web App default,
Web App `--FaMode Webfont`, standalone WASM), builds, runs, and asserts a first icon renders —
SSR markup plus the generated tags and mapped config for the Web App, served tags plus a
headless-browser DOM check for WebAssembly (skipped with `-SkipBrowser` when Playwright is
unavailable). The install is undone in a finally block; pass `-KeepScaffold` to inspect the
scaffolded apps under the temp directory. CI runs this leg after the Test step so template
drift fails the job before release.

## Roslyn analyzer

The optional analyzer package (`src/Jenin.FontAwesome.Blazor.Analyzers`) ships
three diagnostics (`JFA1001` unknown name, `JFA1002` raw class string + code fix, `JFA1003`
Pro-only style) documented in `docs/diagnostics.md`. Two gates keep it honest:

- `pwsh ./eng/generate-analyzer-icon-names.ps1 -Verify` fails CI when the embedded
  known-name catalogue drifts from `eng/metadata`.
- The dogfood gate: `Directory.Build.targets` applies the in-repo analyzer to every shipped
  compilation (src + samples; tests are excluded because their suites intentionally cover
  invalid inputs and the full style matrix). A full `dotnet build` must report zero `JFA`
  findings; intentional demo suppressions carry a justification comment.

Analyzer behavior is proven by `tests/Jenin.FontAwesome.Blazor.Analyzers.Tests` (Roslyn SDK
harnesses, all TFMs): diagnostics on C# and Razor-generated shapes, no-diagnostic cases, and
code-fix application.

## Pro render verification

The family × style × mode matrix (`tests/e2e/fixtures/pro-matrix`) is verified
by `pwsh ./eng/verify-pro-render.ps1`: it serves an asset tree locally (vendored Free 7.3.1
by default, or `$env:FA_PRO_ASSETS` for a licensed kit — never committed), auto-detects the
edition from the present webfonts, boots the fixture in nest and webfont modes, and asserts
the emitted contract in a real browser (`tests/e2e/promatrix.spec.ts`). Neither leg runs in
CI (local/manual gate: Pro assets come from the environment, and group 10 `PRO-01` is pending
owner-licensed verification) — run the Free-only subset locally with no secrets; the full
licensed run is owner-triggered. See `docs/pro-setup.md`.

## Cookbook and llms.txt

The cookbook (`samples/.../Pages/Cookbook`) is the copy-paste surface: 15
`<Snippet>` pages, each rendering its demo plus its own source, asserted by
`tests/e2e/cookbook.spec.ts` (one test per recipe plus focused behavior checks).

The gallery (`samples/.../Pages/Gallery`) composes five full screens from the
same components; `tests/e2e/gallery.spec.ts` asserts each screen's markers plus two
cross-cutting guarantees: a 480px mobile viewport with no horizontal overflow, and the
dark theme applied from stored settings.

`llms.txt` is the AI entry point (llmstxt.org shape: summary + section links). It is
generated by `pwsh ./eng/generate-llms-txt.ps1`, which also verifies that every linked
file exists; `pwsh ./eng/generate-llms-txt.ps1 -Verify` fails CI on drift or dead links.
When adding documented surface, add its section to the generator's table so it stays
discoverable to AI consumers.

## UI-suite adapter verification

The MudBlazor/Blazorise adapters (`src/Jenin.FontAwesome.Blazor.{MudBlazor,Blazorise}`) are verified at two levels: mapper/component unit tests in
`tests/Jenin.FontAwesome.Blazor.{MudBlazor,Blazorise}.Tests` (all TFMs, including
suite-aware `TestHost` support), and `pwsh ./eng/verify-suites.ps1`, which builds and
boots the `tests/e2e/fixtures/suite-adapters` host (real suite components, vendored FA
7.3.1 CSS, no FA JS) and runs `tests/e2e/suites.spec.ts`: mapped markup per slot,
title/custom-name passthrough, and real glyph resolution. See
`docs/using-with-ui-suites.md`.

## Out of scope

- The test project and individual test cases (introduced after implementation stabilises).
- Continuous-integration wiring (quality gates).
- Sample-application acceptance testing (the runtime harness).
- Package and release validation (release engineering).
- JavaScript execution tests (a JavaScript runtime is not part of the .NET test suite).
