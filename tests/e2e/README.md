# End-to-end (browser) tests

This suite closes the no-live-browser/e2e-verification gap and is a required CI gate (job `e2e-browser`). It runs the Playwright harness against
a real Blazor host and verifies what the in-process MSTest suite cannot:

- Font Awesome's client-side DOM replacement (placeholder `<i class="fa-...">` → `<svg>`), i.e. the
  real auto-replace contract the library emits markup for (`enhancement.spec.ts`, `smoke.spec.ts`).
- Transform, mask, layering, and accessibility tokens surviving that replacement on
  `/transforms`, `/masking`, `/layering`, and `/accessibility`.
- Webfont mode (`autoReplaceSvg: false`) by swapping in the packaged webfont config: anchors stay
  un-replaced `<i>` elements and render webfont glyphs, CSS rotations/flips/rotate-by keep working,
  and the SVG-engine-only tokens (`data-fa-transform`, `data-fa-mask`, `.fa-layers`) stay inert
  (`webfont.spec.ts` — feature x mode matrix).
- The server round-trip: a parameter change through the Blazor circuit replaces the boundary
  subtree and the freshly emitted placeholder is re-enhanced (`interaction.spec.ts`).
- That the RCL static assets (`/_content/Jenin.FontAwesome.Blazor/js/...`) are served by a running
  host in the requested environment.
- That the Blazor error UI stays hidden while the circuit is healthy.

The harness intentionally does not add a browser dependency to the default `dotnet` build; the
folder stays outside the solution and runs through its own Node.js toolchain.

## CI integration

The `e2e-browser` job in `.github/workflows/ci.yml` skips downloading browser binaries when the
cache is warm (keyed on `tests/e2e/package-lock.json`), runs `npm ci` + `npx playwright install
--with-deps chromium`, and lets Playwright boot the sample host itself (`npm test`). On failure the
job uploads `tests/e2e/test-results` as an artifact.

## Running locally

```bash
cd tests/e2e
npm ci
npx playwright install --with-deps chromium
# Option A: let Playwright boot the sample host (default)
npm test
# Option B: reuse an already-running host
SAMPLE_URL=http://localhost:5000 npm test
```

## Fixture-scoped specs

Two specs need dedicated fixture hosts instead of the main sample and skip by default:

- `suites.spec.ts` targets the suite-adapters fixture (`/mud`, `/blazorise`) — it runs only when
  `SUITES_FIXTURE` is set, which `eng/verify-suites.ps1` does after booting that fixture.
- `promatrix.spec.ts` targets the pro-matrix fixture (`PRO_MATRIX_MODE`) — it runs only under
  `eng/verify-pro-render.ps1`, which boots the fixture per mode.

A plain `npm test` (main sample host) skips both, so the default suite stays green without the
fixtures; the fixture gates prove those specs in CI (`verify-gates`) and locally respectively.

## Deployed docs-site smoke

`docs-site.spec.ts` is opt-in: it skips unless `DOCS_SITE_URL` is set, so the default suite never
depends on a public deployment. Use it after a GitHub Pages run (or against a local base-path
build) to verify the sub-path layout:

```bash
# Public deployment (also suppresses the default web server)
SAMPLE_URL=https://tareknajem04.github.io/Jenin.FontAwesome.Blazor/ \
DOCS_SITE_URL=https://tareknajem04.github.io/Jenin.FontAwesome.Blazor/ \
npx playwright test docs-site.spec.ts
```

To reproduce the CI layout locally, publish `samples/Jenin.FontAwesome.Blazor.Docs`, rewrite
`<base href>` to `/Jenin.FontAwesome.Blazor/`, copy `index.html` to `404.html`, add `.nojekyll`
and the DocFX output under `api/`, then serve the parent directory (the app must be reachable at
`/Jenin.FontAwesome.Blazor/`).

## Troubleshooting

- If the sample fails to boot, run
  `dotnet run --project ../samples/Jenin.FontAwesome.Blazor.Sample/Jenin.FontAwesome.Blazor.Sample.csproj -c Release --urls http://localhost:5000`
  manually and point `SAMPLE_URL` at it; console output (and the `blazor-error-ui`) will explain
  start failures.
- Cold-boot blips on CodeBuild-like runners are swallowed by the re-click loop in
  `interaction.spec.ts`; unrelated flakes should be re-run before diagnosing.
