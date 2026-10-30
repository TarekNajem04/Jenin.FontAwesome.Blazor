# Repository scripts

PowerShell tooling for repository maintenance. Each script is preview-first unless its README says otherwise, and each folder documents its own parameters, configuration, and outputs.

| Script | Purpose | Documentation |
|----|----|----|
| `scripts/common` | Shared banner, repository-root, configuration, and summary helpers | [README](common/README.md) |
| `scripts/clean` | Removes generated files and directories, including docs/API output and IDE caches | [README](clean/README.md) |
| `scripts/coverage` | Runs tests with XPlat Code Coverage and generates an HTML report | [README](coverage/README.md) |
| `scripts/format` | Applies and verifies .NET formatting | [README](format/README.md) |
| `scripts/sonar` | Runs a local SonarScanner preview (SonarCloud or local SonarQube) | [README](sonar/README.md) |
| `scripts/badges` | Generates the repository README badges | `generate.ps1` |
| `scripts/docs` | Verifies documentation links, root README claims, generated API coverage, and Arabic text | `check-doc-links.ps1`, `check-readme-claims.ps1`, `check-api-reference.ps1`, `check-arabic-text.ps1` |

The documentation scripts are part of the documentation gate:

```powershell
pwsh scripts/docs/check-doc-links.ps1
pwsh scripts/docs/check-readme-claims.ps1
pwsh scripts/docs/check-api-reference.ps1
```

The API coverage check runs after DocFX generation and verifies that every documented assembly (from `docs/docfx.json`) appears in the generated manifest.

The coverage and formatting scripts are documented in their folders. They are not required to build the library unless you are reproducing the CI checks locally.

## Related documentation

- [Repository README](../README.md)
- [Documentation contribution guide](../CONTRIBUTING.md)
- [Coverage tool](coverage/README.md)
- [Format tool](format/README.md)
- [Sonar preview](sonar/README.md)

## License

MIT — see [`LICENSE`](../LICENSES).
