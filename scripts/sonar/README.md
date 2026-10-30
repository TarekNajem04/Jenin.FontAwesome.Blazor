# Sonar preview tooling

`preview.ps1` runs a SonarScanner analysis locally (against SonarCloud or a local SonarQube
server), then prints the open issues and the Quality Gate status.

Two targets are supported:

- **SonarCloud** (default) — needs a User Token; the Free plan only accepts master-branch analyses.
- **Local SonarQube** (`-Local`) — no cloud account needed; ideal before the repository is on GitHub.

## Token selection

| Target | Default token source | Override |
|---|---|---|
| SonarCloud (default) | `SONAR_TOKEN` | `-Token` |
| Local SonarQube (`-Local`) | `SONAR_TOKEN_LOCAL` | `-Token` |

An explicit `-Token` always wins. Create a User Token at
`<server> > My Account > Security > Tokens` (a fresh local SonarQube server logs in with
`admin`/`admin` until the password is changed).

## Prerequisites

- .NET SDK 10; the script installs `dotnet-sonarscanner` automatically when it is missing.
- Java 17 or newer (required by SonarScanner); the script resolves a JDK automatically from
  `SONAR_JAVA_PATH` (path to `java.exe`), `JAVA_HOME`, and the common install locations.
- Local mode: SonarQube Community Build running at <http://localhost:9000> (default).

## Local SonarQube (Community Build)

1. Download and unzip SonarQube Community Build (Java 21 required by current releases):
   <https://www.sonarsource.com/products/sonarqube/downloads/>
2. Start the server (point `JAVA_HOME` or the `PATH` at your JDK when Java is not on `PATH`):

   ```powershell
   $env:JAVA_HOME = "C:\Program Files\Android\openjdk\jdk-21.0.8"
   & "$env:USERPROFILE\Tools\SonarQube\bin\windows-x86-64\StartSonar.bat"
   ```

3. Open <http://localhost:9000> (first login `admin`/`admin`), change the password, then
   create a token: **My Account > Security > Generate Tokens**.
4. Stop the server:

   ```powershell
   Get-CimInstance Win32_Process -Filter "Name='java.exe'" |
       Where-Object { $_.CommandLine -match 'sonar-application' } |
       ForEach-Object { Stop-Process -Id $_.ProcessId }
   ```

## Usage

```powershell
# SonarCloud (default) — token from SONAR_TOKEN
$env:SONAR_TOKEN = "<cloud token>"
.\scripts\sonar\preview.ps1

# Local SonarQube — token from SONAR_TOKEN_LOCAL
$env:SONAR_TOKEN_LOCAL = "<local token>"
.\scripts\sonar\preview.ps1 -Local

# Local with tests and coverage (full CI simulation)
.\scripts\sonar\preview.ps1 -Local -WithCoverage

# Only print the issues and Quality Gate currently stored on the server
.\scripts\sonar\preview.ps1 -FetchIssues

# Write the open issues to a working file (default: private/sonar-issues.txt)
.\scripts\sonar\preview.ps1 -FetchIssues -ExportIssues

# ... or to an explicit path
.\scripts\sonar\preview.ps1 -FetchIssues -ExportIssues -ExportPath .\issues.txt

# Fail (exit 1) when the Quality Gate is not OK
.\scripts\sonar\preview.ps1 -Local -FailOnQualityGate
```

`-ServerUrl` and `-ProjectKey` override the defaults
(SonarCloud / `jeninnet-fontawesome-blazor`; with `-Local` the server URL defaults to
`http://localhost:9000`).

## Notes

- `-ExportIssues` writes one line per issue in the
  `<file>(<line>): <rule> [<severity>/<type>] <message>` shape plus a per-rule summary, so the
  list can be worked through offline (like a VS Error List export). The default path is
  `private/sonar-issues.txt`.
- Coverage requires `coverlet.collector` (referenced by the test project) and the
  `tests/coverage.opencover.runsettings` file; the script passes both to
  `dotnet test`.
- The scanner honors `.editorconfig` severities, so rules pinned there stay silent both
  locally and in CI.
- The master-branch warning applies to SonarCloud only. A local SonarQube server analyzes the
  working copy and has no branch concept, so branch parameters are omitted in local mode.
