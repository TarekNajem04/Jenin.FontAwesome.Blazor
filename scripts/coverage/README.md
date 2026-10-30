# Jenin FontAwesome Repository Coverage Tool

`coverage.ps1` runs the repository test suite with XPlat Code Coverage, collects the emitted coverage files, and generates a browsable HTML report with ReportGenerator. It is the local counterpart to the coverage data the repository produces in CI and for Sonar analysis.

The script is **preview-only by default**: it prints the commands it would run and exits without touching the working tree. Pass `-Execute` to run the workflow.

## Workflow

An executed run performs these steps:

1.  Finds the repository root by walking upward to the solution files.
2.  Optionally removes previous artifacts (`TestResults`, `TestCoverageReport`).
3.  Optionally restores NuGet packages.
4.  Verifies that `reportgenerator` is available, installing it when `-InstallTools` is supplied.
5.  Runs `dotnet test --collect:"XPlat Code Coverage"` from the repository root.
6.  Searches `TestResults` for `*.xml` files whose names contain `coverage`, `cobertura`, or `opencover`.
7.  Replaces the report directory and runs ReportGenerator with the configured report types and assembly filters.
8.  Verifies that `index.html` exists, optionally opens it, and prints a summary.

## Prerequisites

- PowerShell 7 or later.

- .NET 10 SDK.

- ReportGenerator installed globally:

  ``` lang-powershell
  dotnet tool install --global dotnet-reportgenerator-globaltool
  ```

  Alternatively, pass `-Execute -InstallTools` to let the script install it.

## Usage

Preview the workflow (default):

```powershell
./scripts/coverage/coverage.ps1
```

Run the tests and generate the report:

```powershell
./scripts/coverage/coverage.ps1 -Execute
```

Clean old artifacts, restore packages, and then run coverage:

```powershell
./scripts/coverage/coverage.ps1 -Execute -Clean -Restore
```

Install ReportGenerator automatically when it is missing:

```powershell
./scripts/coverage/coverage.ps1 -Execute -InstallTools
```

Use an alternative configuration file:

```powershell
./scripts/coverage/coverage.ps1 -Execute -ConfigurationPath ./scripts/coverage/coverage.config.json
```

## Parameters

| Parameter | Type | Purpose |
|----|----|----|
| `-Execute` | switch | Runs the workflow. Without it the script stays in preview mode and exits `0`. |
| `-Clean` | switch | Removes `TestResults` and `TestCoverageReport` under the repository root before running. |
| `-Restore` | switch | Runs `dotnet restore` at the repository root before testing. |
| `-InstallTools` | switch | Installs `dotnet-reportgenerator-globaltool` globally when `reportgenerator` is not found. |
| `-ConfigurationPath` | string | Path to the JSON configuration file. Defaults to `coverage.config.json` next to the script. |
| `-OpenReport` | switch | Accepted for command-line symmetry; the effective open behavior is read from `behavior.openReport` in the configuration file. |

## Configuration

The default configuration is [`coverage.config.json`](coverage.config.json), with a companion JSON schema at [`coverage.schema.json`](coverage.schema.json) for editor assistance:

```json
{
  "coverage": {
    "resultsDirectory": "TestResults",
    "reportDirectory": "artifacts/coverage-report",
    "reportTypes": ["Html", "HtmlSummary", "TextSummary"]
  },
  "test": {
    "command": "dotnet test",
    "arguments": ["--collect:XPlat Code Coverage", "--settings", "tests/coverage.runsettings"]
  },
  "reportGenerator": {
    "toolCommand": "reportgenerator",
    "assemblyFilters": [
      "+Jenin.FontAwesome.Blazor*",
      "-*.Tests*"
    ]
  },
  "behavior": {
    "openReport": true
  }
}
```

| Key | Meaning |
|----|----|
| `coverage.resultsDirectory` | Directory searched for coverage XML files, relative to the repository root. |
| `coverage.reportDirectory` | ReportGenerator target directory. Replaced on every executed run. |
| `coverage.reportTypes` | ReportGenerator output formats; `Html`, `HtmlSummary`, and `TextSummary` by default. |
| `test.arguments` | Arguments appended to `dotnet test`. The script invokes `dotnet test` directly. |
| `reportGenerator.toolCommand` | Executable used to run ReportGenerator. |
| `reportGenerator.assemblyFilters` | Include (`+`) and exclude (`-`) filters using ReportGenerator's filtering syntax. |
| `behavior.openReport` | Opens `index.html` after a successful run when `true`. |

The results and report paths are resolved from the repository root, not from the script folder.

## Output

| Path | Contents |
|----|----|
| `TestResults/**` | Raw coverage XML emitted by the test run. |
| `artifacts/coverage-report/index.html` | Browsable HTML report with per-assembly and per-class coverage. |
| `artifacts/coverage-report/Summary.txt` | Text summary produced by `TextSummary`. |

The script exits `0` on success and `1` on failure. Failures include a failed `dotnet test`, missing coverage files, a ReportGenerator failure, or a missing `index.html`.

## Troubleshooting

| Symptom | Cause | Remedy |
|----|----|----|
| `ReportGenerator is not installed.` | The global tool is not on `PATH` | Run with `-InstallTools`, or install `dotnet-reportgenerator-globaltool` globally |
| `Coverage files were not generated.` | Tests failed, were skipped, or the coverage collector did not run | Run `dotnet test --collect:"XPlat Code Coverage"` directly and inspect the output |
| `Coverage failed: dotnet test failed.` | A test failure or a restore/build problem | Fix the test or restore failure first; the script intentionally stops |
| `ReportGenerator failed.` | Invalid report types or filters, or an unreadable coverage file | Validate the configuration against the schema and inspect the generated XML |
| The report does not open automatically | `behavior.openReport` is `false` | Set it to `true` or open `artifacts/coverage/index.html` manually |
| Stale results | Previous run output was reused | Use `-Clean` and delete `artifacts/coverage-report` |

## Related tooling

- [Common PowerShell module](../common/README.md) — shared banner, repository-root, and configuration helpers.
- [Clean tool](../clean/README.md) — removes generated files and directories.
- [Format tool](../format/README.md) — applies and verifies .NET formatting.
- [Sonar preview tooling](../sonar/README.md) — runs the SonarScanner analysis used by CI.
- [Repository README](../../README.md) — build, test, and packaging entry points.

## License

MIT — see [`LICENSE`](../../LICENSES).
