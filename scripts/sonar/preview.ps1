<#
.SYNOPSIS
    Previews Sonar analysis locally before pushing to GitHub.

.DESCRIPTION
    Runs a SonarScanner analysis locally (SonarCloud or a local SonarQube
    server), then prints the open issues and the Quality Gate status a CI
    analysis would report.

    Two targets are supported:

    SonarCloud (default)
        Analyzes the working copy against SonarCloud.

    Local SonarQube (-Local)
        Analyzes the working copy against a local SonarQube server
        (default http://localhost:9000). No cloud account is needed.

    Two modes are available:

    -FetchIssues
        Queries the server only. Read-only, no local analysis is run.

    Default
        Builds the solution, runs the analysis, then prints the report.
        Tests and coverage are skipped unless -WithCoverage is specified.

    Notes:
    - The token is taken from -Token when passed; otherwise from the
      SONAR_TOKEN_LOCAL environment variable with -Local, and from the
      SONAR_TOKEN environment variable without it. Create a User Token at
      <server> > My Account > Security > Tokens.
    - The SonarCloud Free plan only accepts master branch analyses, and a
      local analysis replaces the latest master branch snapshot.
      The next CI run restores it.

.PARAMETER Local
    Targets a local SonarQube server (default http://localhost:9000)
    instead of SonarCloud. Also switches the default token variable to
    SONAR_TOKEN_LOCAL.

.PARAMETER FetchIssues
    Fetches the open issues currently reported by the server
    without running a local analysis.

.PARAMETER ExportIssues
    Writes the fetched open issues to a text file so the list can be worked through
    offline (default path: private/sonar-issues.txt). Combine with -FetchIssues for a
    read-only export.

.PARAMETER ExportPath
    Overrides the export path used by -ExportIssues.

.PARAMETER WithCoverage
    Runs the test suite with coverage like CI does.
    Required to preview the coverage condition of the Quality Gate.

.PARAMETER FailOnQualityGate
    Exits with code 1 when the Quality Gate status is not OK,
    simulating sonar.qualitygate.wait=true used by CI.

.PARAMETER Token
    Sonar token. When omitted, the environment variable for the selected
    target is used (SONAR_TOKEN_LOCAL with -Local, SONAR_TOKEN otherwise).

.PARAMETER ServerUrl
    Overrides the server URL (defaults to https://sonarcloud.io, or
    http://localhost:9000 with -Local).

.PARAMETER ProjectKey
    Overrides the Sonar project key (defaults to jeninnet-fontawesome-blazor).

.EXAMPLE
    .\preview.ps1 -FetchIssues

    Lists the open issues currently stored in SonarCloud.

.EXAMPLE
    .\preview.ps1 -FetchIssues -ExportIssues

    Lists the open issues and writes them to private/sonar-issues.txt (read-only).

.EXAMPLE
    .\preview.ps1

    Builds and analyzes the working copy against SonarCloud, then prints the report.

.EXAMPLE
    .\preview.ps1 -Local -WithCoverage -FailOnQualityGate

    Full local simulation of the CI quality gate against a local SonarQube server.

.NOTES
    Author:
        Tarek Najem

    Tool:
        Jenin FontAwesome Sonar Preview Tool
#>


[CmdletBinding()]
param(

    [switch]
    $Local,


    [switch]
    $FetchIssues,


    [switch]
    $ExportIssues,


    [string]
    $ExportPath,


    [switch]
    $WithCoverage,


    [switch]
    $FailOnQualityGate,


    [string]
    $Token,


    [string]
    $ServerUrl,


    [string]
    $ProjectKey
)


Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"


$script:Organization = "jeninnet"
$script:ProjectKey = if ([string]::IsNullOrWhiteSpace($ProjectKey)) { "jeninnet-fontawesome-blazor" } else { $ProjectKey }
$script:SonarHostUrl = if (-not [string]::IsNullOrWhiteSpace($ServerUrl)) {
    $ServerUrl
}
elseif ($Local) {
    "http://localhost:9000"
}
else {
    "https://sonarcloud.io"
}
$script:Target = if ($Local) { "Local SonarQube" } else { "SonarCloud" }
$script:TokenVariable = if ($Local) { "SONAR_TOKEN_LOCAL" } else { "SONAR_TOKEN" }


# Token selection: an explicit -Token always wins; otherwise the target decides the variable.
if ([string]::IsNullOrWhiteSpace($Token)) {
    $Token = if ($Local) { $env:SONAR_TOKEN_LOCAL } else { $env:SONAR_TOKEN }
}


Import-Module -Name (Join-Path $PSScriptRoot ".." "common" "Common.psd1") -Force


function Get-SonarApi {

    param(
        [Parameter(Mandatory)]
        [string]$Path,

        [string]$Query
    )


    $uri = "$script:SonarHostUrl/$Path"

    if (-not [string]::IsNullOrEmpty($Query)) {
        $uri = "$uri`?$Query"
    }


    $attempts = @(
        "Bearer $Token",
        "Basic $([Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("${Token}:")))"
    )


    foreach ($attempt in $attempts) {

        try {

            $headers = @{ Authorization = $attempt }

            return Invoke-RestMethod -Uri $uri -Headers $headers -Method Get
        }
        catch {

            $statusCode = 0

            # Connection failures carry no Response; strict mode forbids the bare access.
            $response = $null

            if ($_.Exception.PSObject.Properties["Response"]) {
                $response = $_.Exception.Response
            }

            if ($null -ne $response) {
                $statusCode = [int]$response.StatusCode
            }

            if ($statusCode -eq 401 -or $statusCode -eq 403) {
                continue
            }

            throw "$script:Target API request failed: $($_.Exception.Message)"
        }
    }

    throw "The $script:Target server rejected the token. Create a User Token (My Account > Security > Tokens) and pass it with -Token or `$env:$($script:TokenVariable)."
}


function Test-SonarToken {

    $result = Get-SonarApi -Path "api/authentication/validate"

    if (-not $result.valid) {
        throw "The $script:Target token is not valid. Create a User Token (My Account > Security > Tokens)."
    }
}


function Get-SonarIssueList {

    param(
        [string]
        $BranchName = "master"
    )


    # A local SonarQube server has no branch concept; the parameter is SonarCloud-only.
    $branchQuery = if ($Local) { "" } else { "&branch=$BranchName" }
    $query = "componentKeys=$script:ProjectKey&resolved=false&ps=500$branchQuery"
    $result = Get-SonarApi -Path "api/issues/search" -Query $query

    return , @($result.issues)
}


function Get-SonarQualityGate {

    param(
        [string]
        $BranchName = "master"
    )


    $branchQuery = if ($Local) { "" } else { "&branch=$BranchName" }
    $query = "projectKey=$script:ProjectKey$branchQuery"

    # WAITING and NONE both mean "no definitive result yet": the compute engine may still be
    # processing the report, and a first analysis reports NONE until the gate is computed.
    foreach ($attempt in 1..24) {

        $result = Get-SonarApi -Path "api/qualitygates/project_status" -Query $query
        $status = $result.projectStatus.status

        if ($status -notin @("WAITING", "NONE")) {
            return $result.projectStatus
        }

        Start-Sleep -Seconds 5
    }

    return $result.projectStatus
}


function Wait-SonarAnalysis {

    param(
        [string]
        $TaskId,

        [int]
        $TimeoutSeconds = 300
    )


    Write-Step "Waiting for $script:Target to process the analysis..."


    if ([string]::IsNullOrWhiteSpace($TaskId)) {

        # Fallback for scanner output shapes without a task URL: poll the component activity until
        # no task is pending. Weaker than the task-id wait (the task may not be visible yet).
        Start-Sleep -Seconds 5

        $deadline = (Get-Date).AddSeconds($TimeoutSeconds)

        while ((Get-Date) -lt $deadline) {

            $activity = Get-SonarApi -Path "api/ce/activity" -Query "component=$script:ProjectKey&ps=10"

            $tasks = @()

            if ($activity.PSObject.Properties["tasks"] -and $activity.tasks) {
                $tasks = @($activity.tasks)
            }

            $activeTask = $tasks | Where-Object {
                $_.status -in @("PENDING", "IN_PROGRESS", "RUNNING")
            } | Select-Object -First 1

            if ($null -eq $activeTask) {
                return
            }

            Start-Sleep -Seconds 5
        }

        Write-Host "Timed out waiting for $script:Target processing. Results may be stale." -ForegroundColor Yellow

        return
    }


    # The report URL printed by the scanner names the exact compute-engine task: waiting for that
    # task removes the race where the previous analysis's gate status and issues are read as current.
    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)

    while ((Get-Date) -lt $deadline) {

        $task = Get-SonarApi -Path "api/ce/task" -Query "id=$TaskId"

        switch ($task.task.status) {

            "SUCCESS" {
                return
            }

            "FAILED" {
                throw "The $script:Target analysis task failed (status FAILED). Check the server's background tasks for details."
            }

            "CANCELED" {
                throw "The $script:Target analysis task was canceled."
            }
        }

        Start-Sleep -Seconds 5
    }

    Write-Host "Timed out waiting for $script:Target processing. Results may be stale." -ForegroundColor Yellow
}


function Export-IssueList {

    param(
        [array]
        $Issues
    )


    $repositoryRoot = Find-RepositoryRoot -StartPath $PSScriptRoot

    $path = if ([string]::IsNullOrWhiteSpace($ExportPath)) {
        Join-Path $repositoryRoot "private\sonar-issues.txt"
    }
    else {
        $ExportPath
    }

    $directory = Split-Path -Parent $path

    if ($directory -and -not (Test-Path $directory)) {
        New-Item -ItemType Directory -Force -Path $directory | Out-Null
    }


    $lines = [System.Collections.Generic.List[string]]::new()
    [void]$lines.Add("# $script:ProjectKey - open SonarQube issues")
    [void]$lines.Add("# Target   : $script:Target ($script:SonarHostUrl)")
    [void]$lines.Add("# Exported : $((Get-Date).ToString('yyyy-MM-dd HH:mm:ss'))")
    [void]$lines.Add("# Issues   : $($Issues.Count)")
    [void]$lines.Add("# Format   : <file>(<line>): <rule> [<severity>/<type>] <message>")
    [void]$lines.Add("#")

    foreach ($issue in ($Issues | Sort-Object -Property component, line)) {

        $file = $issue.component -replace "^$($script:ProjectKey):", ""
        $line = if ($issue.PSObject.Properties["line"] -and $issue.line) { $issue.line } else { 0 }

        [void]$lines.Add("${file}(${line}): $($issue.rule) [$($issue.severity)/$($issue.type)] $($issue.message)")
    }

    [void]$lines.Add("#")
    [void]$lines.Add("# Summary by rule:")
    foreach ($group in ($Issues | Group-Object rule | Sort-Object Count -Descending)) {
        [void]$lines.Add(("#   {0,3}x {1}" -f $group.Count, $group.Name))
    }


    # A working document outside the repository rules: LF endings, no BOM (Byte Order Mark).
    [System.IO.File]::WriteAllText($path, ($lines -join "`n") + "`n", [System.Text.UTF8Encoding]::new($false))

    Write-Step "Issues exported to $path" -Color Green
}


function Write-IssueList {

    param(
        [array]
        $Issues
    )


    if ($Issues.Count -eq 0) {

        Write-Host ""
        Write-Host "No open issues found." -ForegroundColor Green

        return
    }


    $sorted = $Issues | Sort-Object -Property component, line

    foreach ($issue in $sorted) {

        $file = $issue.component -replace "^$($script:ProjectKey):", ""

        $line = ""

        if ($issue.PSObject.Properties["line"] -and $issue.line) {
            $line = ":$($issue.line)"
        }

        $color = switch ($issue.severity) {
            "BLOCKER" { "Red" }
            "CRITICAL" { "Red" }
            "MAJOR" { "Yellow" }
            "MINOR" { "DarkYellow" }
            default { "Gray" }
        }

        Write-Host ""
        Write-Host "$file$line" -ForegroundColor Cyan -NoNewline
        Write-Host "  [$($issue.severity)] $($issue.rule)" -ForegroundColor $color
        Write-Host "  $($issue.message)"
    }


    Write-Host ""
    Write-Host "Open issues: $($Issues.Count)" -ForegroundColor Cyan

    foreach ($group in ($Issues | Group-Object severity | Sort-Object Name)) {
        Write-Host ("  {0,-10}: {1}" -f $group.Name, $group.Count)
    }
}


function Write-QualityGate {

    param(
        [Parameter(Mandatory)]
        $Gate
    )


    $color = switch ($Gate.status) {
        "OK" { "Green" }
        "WARN" { "Yellow" }
        "ERROR" { "Red" }
        default { "Yellow" }
    }

    Write-Host ""
    Write-Host "Quality Gate: $($Gate.status)" -ForegroundColor $color

    if ($Gate.status -eq "NONE") {
        Write-Host "  The gate could not be evaluated yet (no definitive status returned)." -ForegroundColor Yellow
        Write-Host "  Verify that a quality gate is associated with the project, then re-run -FetchIssues." -ForegroundColor Yellow
    }


    foreach ($condition in $Gate.conditions) {

        $passed = $condition.status -eq "OK"
        $symbol = if ($passed) { "OK  " } else { "FAIL" }
        $conditionColor = if ($passed) { "Green" } else { "Red" }
        $actual = ""

        if ($condition.PSObject.Properties["actualValue"] -and $condition.actualValue) {
            $actual = "$($condition.actualValue)"
        }

        Write-Host ("  [{0}] {1}: {2} (threshold {3})" -f `
                $symbol, $condition.metricKey, $actual, $condition.errorThreshold) -ForegroundColor $conditionColor
    }
}


function Test-JavaVersion {

    param(
        [Parameter(Mandatory)]
        [string]$JavaExe,


        [int]
        $MinimumMajor = 17
    )


    $versionOutput = & $JavaExe -version 2>&1 | Out-String

    if ($versionOutput -match 'version "(?<major>\d+)') {
        return [int]$Matches.major -ge $MinimumMajor
    }

    return $false
}


function Resolve-JavaHome {

    $candidates = @()


    # Explicit override: SONAR_JAVA_PATH points at the java executable to use.
    if ($env:SONAR_JAVA_PATH) {

        $javaPath = $env:SONAR_JAVA_PATH

        if (Test-Path $javaPath) {

            $binDirectory = Split-Path -Parent $javaPath

            if ((Split-Path -Leaf $binDirectory) -ieq "bin") {
                $candidates += (Split-Path -Parent $binDirectory)
            }
            else {
                $candidates += $binDirectory
            }
        }
    }


    if ($env:JAVA_HOME) {
        $candidates += $env:JAVA_HOME
    }


    $patterns = @()


    if (${env:ProgramFiles}) {

        $patterns += @(
            (Join-Path ${env:ProgramFiles} "Android\openjdk\jdk-*"),
            (Join-Path ${env:ProgramFiles} "Microsoft\jdk-*"),
            (Join-Path ${env:ProgramFiles} "Eclipse Adoptium\jdk-*"),
            (Join-Path ${env:ProgramFiles} "Amazon Corretto\jdk*"),
            (Join-Path ${env:ProgramFiles} "Zulu\zulu-*")
        )
    }


    if ($env:LOCALAPPDATA) {
        $patterns += (Join-Path $env:LOCALAPPDATA "Programs\Android Studio\jbr")
    }


    if ($env:USERPROFILE) {
        $patterns += (Join-Path $env:USERPROFILE ".jdks\*")
    }


    foreach ($pattern in $patterns) {

        foreach ($item in @(Get-Item $pattern -ErrorAction SilentlyContinue)) {

            if ($null -ne $item) {
                $candidates += $item.FullName
            }
        }
    }


    $candidates = $candidates | Where-Object { $_ } | Select-Object -Unique

    foreach ($candidate in $candidates) {

        $javaExe = Join-Path $candidate "bin\java.exe"

        if ((Test-Path $javaExe) -and (Test-JavaVersion -JavaExe $javaExe)) {
            return $candidate
        }
    }

    throw "Java 17 or newer is required by SonarScanner but was not found. Install it with: winget install Microsoft.OpenJDK.21"
}


function Confirm-SonarScanner {

    $toolsDirectory = Join-Path ([Environment]::GetFolderPath("UserProfile")) ".dotnet\tools"
    $scannerShim = Join-Path $toolsDirectory "dotnet-sonarscanner.exe"

    if (-not (Test-Path $scannerShim)) {

        Write-Step "Installing SonarScanner for .NET..."

        & dotnet tool install --global dotnet-sonarscanner

        if ($LASTEXITCODE -ne 0) {
            throw "Failed to install dotnet-sonarscanner."
        }
    }


    # The tools directory is not necessarily on this process's PATH: a tool installed in an
    # earlier session updates the user PATH, but already-running shells keep their old copy.
    if (($env:Path -split [IO.Path]::PathSeparator) -notcontains $toolsDirectory) {
        $env:Path = "$toolsDirectory$([IO.Path]::PathSeparator)$env:Path"
    }
}


function Invoke-DotNetCommand {

    param(
        [Parameter(Mandatory)]
        [string]$Label,

        [Parameter(Mandatory)]
        [string[]]$Arguments
    )


    Write-Step "$Label..."

    & dotnet @Arguments

    if ($LASTEXITCODE -ne 0) {
        throw "$Label failed (exit code $LASTEXITCODE)."
    }
}


try {

    Write-ToolBanner -Name "Sonar Preview Tool"


    if ([string]::IsNullOrWhiteSpace($Token)) {

        throw "No $script:Target token found. Set `$env:$($script:TokenVariable) or pass -Token. Create a User Token (My Account > Security > Tokens)."
    }


    Test-SonarToken

    Write-Step "Token accepted." -Color Green


    if ($FetchIssues) {

        $issues = Get-SonarIssueList

        if ($ExportIssues) {
            Export-IssueList -Issues $issues
        }

        Write-Section "Open Issues"
        Write-IssueList -Issues $issues

        Write-Section "Quality Gate"
        Write-QualityGate -Gate (Get-SonarQualityGate)

        exit 0
    }


    $repositoryRoot = Find-RepositoryRoot -StartPath $PSScriptRoot

    Write-Section "Environment"
    Write-Host "Repository : $repositoryRoot"
    Write-Host "Target     : $script:Target"
    Write-Host "Server     : $script:SonarHostUrl"
    Write-Host "Project    : $script:ProjectKey"


    $javaHome = Resolve-JavaHome

    $env:JAVA_HOME = $javaHome
    $env:Path = (Join-Path $javaHome "bin") + [IO.Path]::PathSeparator + $env:Path

    Write-Host "JAVA_HOME  : $javaHome"


    Confirm-SonarScanner


    $currentBranch = (& git -C $repositoryRoot rev-parse --abbrev-ref HEAD 2>$null | Out-String).Trim()

    if (-not $Local -and $currentBranch -and $currentBranch -ne "master") {

        Write-Host ""
        Write-Host "Warning: git branch is '$currentBranch'." -ForegroundColor Yellow
        Write-Host "The SonarCloud Free plan only accepts master branch analyses," -ForegroundColor Yellow
        Write-Host "so results are posted to the master branch." -ForegroundColor Yellow
    }


    Push-Location $repositoryRoot


    $resultsDirectory = Join-Path $repositoryRoot "TestResults"

    if (Test-Path $resultsDirectory) {

        Write-Step "Cleaning previous TestResults (CI starts from a clean checkout)..."
        Remove-Item -Path $resultsDirectory -Recurse -Force
    }


    $beginArguments = @(
        "sonarscanner",
        "begin",
        "/k:$script:ProjectKey"
    )

    # SonarCloud requires the organization; a local SonarQube server does not use one.
    if (-not $Local) {
        $beginArguments += "/o:$script:Organization"
    }

    $beginArguments += @(
        "/d:sonar.host.url=$script:SonarHostUrl",
        "/d:sonar.token=$Token",
        "/d:sonar.cs.vstest.reportsPaths=TestResults/**/*.trx",
        "/d:sonar.cs.opencover.reportsPaths=**/coverage.opencover.xml",
        "/d:sonar.coverage.exclusions=**/*.ps1,**/*.md,**/*.js,**/*.css,**/wwwroot/**,docs/**,scripts/**,.github/**,eng/**,templates/**,badges/**,private/**",
        "/d:sonar.scm.provider=git",
        "/d:sonar.scm.forceReloadAll=true",
        "/d:sonar.exclusions=samples/**,templates/**,docs/**,scripts/**,.github/**,eng/**,badges/**,private/**,**/*.ps1,**/*.md,**/bin/**,**/obj/**,**/TestResults/**",
        "/d:sonar.test.exclusions=tests/**"
    )

    $failureMessage = ""
    $analysisStarted = $false
    $ceTaskId = $null


    try {

        Invoke-DotNetCommand -Label "Begin Sonar analysis" -Arguments $beginArguments

        $analysisStarted = $true


        try {

            Invoke-DotNetCommand -Label "Restore dependencies" -Arguments @("restore")

            Invoke-DotNetCommand -Label "Build solution" -Arguments @("build", "-c", "Release", "--no-restore")


            if ($WithCoverage) {

                Invoke-DotNetCommand -Label "Run tests with coverage" -Arguments @(
                    "test",
                    "-c", "Release",
                    "--no-build",
                    "--settings", "tests/coverage.opencover.runsettings",
                    "--collect:XPlat Code Coverage",
                    "--logger", "trx;LogFileName=results.trx",
                    "--results-directory", "TestResults"
                )
            }
            else {

                Write-Step "Tests skipped (use -WithCoverage to include them)."
            }
        }
        catch {

            $failureMessage = $_.Exception.Message
        }
    }
    finally {

        if ($analysisStarted) {

            Write-Step "End Sonar analysis..."

            $scannerOutput = @()

            & dotnet sonarscanner end "/d:sonar.token=$Token" 2>&1 | Tee-Object -Variable scannerOutput

            # The scanner prints the compute-engine task URL; its id is the precise completion barrier.
            if (($scannerOutput | Out-String) -match "api/ce/task\?id=(?<id>[0-9a-fA-F-]+)") {
                $ceTaskId = $Matches["id"]
            }

            if ($LASTEXITCODE -ne 0 -and [string]::IsNullOrEmpty($failureMessage)) {
                $failureMessage = "SonarScanner end failed (exit code $LASTEXITCODE)."

                if (($scannerOutput | Out-String) -match "MissingObjectException|Missing blob") {
                    $failureMessage += " The scanner's git blame could not read the repository objects;" +
                        " refresh the local object store with 'git repack -adf' and re-run."
                }
            }
        }
    }


    Pop-Location


    if (-not [string]::IsNullOrEmpty($failureMessage)) {
        throw $failureMessage
    }


    Wait-SonarAnalysis -TaskId $ceTaskId


    # The gate polling doubles as a second readiness check: a first analysis reports NONE until
    # the gate is computed.
    $gate = Get-SonarQualityGate


    Write-Section "Open Issues"
    $issues = Get-SonarIssueList

    if ($ExportIssues) {
        Export-IssueList -Issues $issues
    }

    Write-IssueList -Issues $issues


    Write-Section "Quality Gate"
    Write-QualityGate -Gate $gate


    Write-Summary -Items @{
        "Target"       = $script:Target
        "Project"      = $script:ProjectKey
        "Open Issues"  = $issues.Count
        "Quality Gate" = $gate.status
    }


    Write-Host ""
    Write-Host "Dashboard:" -ForegroundColor Cyan

    if ($Local) {
        Write-Host "  $script:SonarHostUrl/dashboard?id=$script:ProjectKey"
    }
    else {
        Write-Host "  $script:SonarHostUrl/project/issues?id=$script:ProjectKey"
        Write-Host "  $script:SonarHostUrl/summary/new_code?id=$script:ProjectKey"
    }


    if ($FailOnQualityGate -and $gate.status -ne "OK") {
        exit 1
    }

    exit 0
}
catch {

    Write-Host ""
    Write-Host "Sonar preview failed:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red

    exit 1
}
