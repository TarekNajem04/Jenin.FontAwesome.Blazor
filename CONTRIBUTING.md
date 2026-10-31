# Contributing to Jenin.FontAwesome.Blazor

Thank you for your interest in Jenin.FontAwesome.Blazor: Blazor components, analyzers,
and templates for Font Awesome Free (plus optional licensed-Pro support).

## Current Project Status

Jenin.FontAwesome.Blazor 2.0.0 is released. The public API surface, CI/CD pipelines, quality
gates, and release process are now at a stable baseline; source-code contributions remain
limited to project maintainers until external contributions are opened publicly.

## How You Can Contribute Today

Community participation is highly appreciated.

The most valuable contributions at this stage are:

* Bug reports
* Feature requests
* Design discussions
* API design feedback
* Documentation improvements
* Real-world usage feedback

Please use GitHub Issues or Discussions to share ideas and suggestions.

## Pull Requests

At the current stage of the project:

* Pull requests containing source-code changes may not be accepted.
* Pull requests that improve documentation may be considered on a case-by-case basis.
* Large design proposals should begin as an Issue or Discussion before implementation.
* Dependabot dependency PRs are auto-approved; minor and patch updates auto-merge (squash).
  Major version bumps are approved but never auto-merged — they need a human decision.

Once the project reaches a stable baseline, external code contributions will be opened publicly.

### Branch and merge policy

* The default branch is `main`. All changes land through pull requests — direct pushes are restricted.
* Merges use squash with the PR title as the commit message; head branches are deleted on merge.
* Every pull request must satisfy the required checks (format, build and test on
  Windows/Ubuntu/macOS, package validation, SonarCloud analysis) and a CODEOWNERS review.
* Keep the change focused and limited to one concern. Every pull request is expected to meet
  the checklist in `.github/PULL_REQUEST_TEMPLATE.md`:
  * `dotnet build "Jenin.FontAwesome.Blazor.slnx"` succeeds with zero errors and zero warnings.
  * Formatting and analyzers are clean (`dotnet format Jenin.FontAwesome.Blazor.slnx --verify-no-changes --no-restore`).
  * No analyzer diagnostic was suppressed or downgraded, and no `.editorconfig` rule was weakened.
  * Documentation and samples were updated where behavior changed.

## Project Structure

```text
src/          # Shipped libraries (core, IconPicker, Icons, Svg, adapters, analyzers)
tests/        # MSTest suites, contract tests, and Playwright end-to-end tests
samples/      # Runnable Blazor samples, including the docs site and the explorer
templates/    # `dotnet new` template pack
docs/         # Guides and the published documentation site sources
eng/          # Deterministic generators and verification gates (see eng/README.md)
scripts/      # Repository maintenance tooling (see scripts/README.md)
.github/      # CI workflows and pull-request automation
```

## Design Philosophy

The architecture emphasizes:

* Host-owned Font Awesome assets: the library emits markup and ships no runtime, fonts, or interop bridge
* A typed, discoverable Blazor API surface with deterministic rendering
* Trimmability and Native AOT compatibility, verified per package at publish level
* Deterministic generation: every generated file is byte-reproducible and CI-verified
* Long-term API stability enforced by a committed public-API baseline

When evaluating proposals, maintainability and long-term API stability are generally preferred over short-term convenience.

## Development Standards

The project follows modern .NET engineering practices including:

* Automated CI validation
* Static analysis and quality gates (including SonarCloud)
* Package validation
* Public API surface baselines
* Cross-platform testing (Windows, Ubuntu, macOS)

Any future code contributions will be expected to satisfy these requirements.

## Testing

Run the checks described in [TESTING.md](TESTING.md): build, unit and contract tests on all
supported target frameworks, formatting verification, encoding verification, and the
generator `-Verify` gates. Analyzer behavior is documented in [docs/diagnostics.md](docs/diagnostics.md).

Thank you for helping improve Jenin.FontAwesome.Blazor.
