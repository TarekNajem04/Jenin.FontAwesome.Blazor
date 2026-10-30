# Contributing to Jenin.FontAwesome.Blazor

Thank you for your interest in Jenin.FontAwesome.Blazor: Blazor components, analyzers,
and templates for Font Awesome Free (plus optional licensed-Pro support).

## How you can contribute

Community participation is highly appreciated. The most valuable contributions at this stage are:

* Bug reports
* Feature requests
* Design discussions
* Documentation improvements
* Real-world usage feedback

Please use GitHub Issues or Discussions to share ideas and suggestions.

## Pull requests

Keep the change focused and limited to one concern. Every pull request is expected to meet
the checklist in `.github/PULL_REQUEST_TEMPLATE.md`:

* `dotnet build "Jenin.FontAwesome.Blazor.slnx"` succeeds with zero errors and zero warnings.
* Formatting and analyzers are clean (`./build.ps1 -Target Format` or equivalent `dotnet format` verify).
* No analyzer diagnostic was suppressed or downgraded, and no `.editorconfig` rule was weakened.
* Documentation and samples were updated where behavior changed.

## Project structure

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

## Testing

Run the checks described in [TESTING.md](TESTING.md): build, unit and contract tests on all
supported target frameworks, formatting verification, encoding verification, and the
generator `-Verify` gates. Analyzer behavior is documented in [docs/diagnostics.md](docs/diagnostics.md).
