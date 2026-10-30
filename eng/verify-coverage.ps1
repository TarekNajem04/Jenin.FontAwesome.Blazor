#!/usr/bin/env pwsh
#
# Code coverage gate.
#
# Merges every Cobertura report produced by the XPlat Code Coverage collector (coverlet.collector,
# configured by tests/coverage.runsettings) into a single line-coverage figure and fails when it
# falls below the ratchet in eng/coverage-threshold.txt. The gate can only be an under-approximation:
# a line counts as executed when any run hits it.
#
# The merge is a set-union over (source file, line) keyed reports so that the canonical net10.0 leg,
# which runs the three test projects in parallel concerns (core / IconPicker / Icons), is measured
# as one metric instead of three blended report-level rates. Reports must come from one fresh
# collection; delete artifacts/coverage before collecting (CI does this; TESTING.md documents the
# local workflow).
#
# The script also fails when a generated file leaks back into the metric: the catalogue
# (src/Jenin.FontAwesome.Blazor/IconNames.cs) and the per-icon preset components
# (src/Jenin.FontAwesome.Blazor.Icons/GeneratedIconComponents.cs and
# src/Jenin.FontAwesome.Blazor.Svg/GeneratedIconComponents.cs, matched by leaf name) are excluded
# by tests/coverage.runsettings and regenerated from Font Awesome metadata (eng/generate-* gates).
#
# Usage:
#   pwsh ./eng/verify-coverage.ps1
#   pwsh ./eng/verify-coverage.ps1 -Path 'artifacts/coverage/**/coverage.cobertura.xml'
#   pwsh ./eng/verify-coverage.ps1 -Threshold 64

[CmdletBinding()]
param(
    [string]$Path = 'artifacts/coverage/**/coverage.cobertura.xml',
    [double]$Threshold = -1
)

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot

$thresholdFile = Join-Path $root 'eng' 'coverage-threshold.txt'
if($Threshold -lt 0) {
    if(-not (Test-Path -LiteralPath $thresholdFile)) {
        Write-Error "Coverage threshold file is missing: $thresholdFile"
    }

    $Threshold = [double](Get-Content -LiteralPath $thresholdFile | Where-Object { $_.Trim() -ne '' } | Select-Object -First 1)
}

$reports = Get-ChildItem -Path (Join-Path $root $Path) -File -ErrorAction SilentlyContinue
if(-not $reports) {
    Write-Host "No coverage reports matched '$Path'."
    Write-Host 'Collect coverage first, for example:'
    Write-Host '  Remove-Item artifacts/coverage -Recurse -Force'
    Write-Host '  dotnet test tests/Jenin.FontAwesome.Blazor.Tests/Jenin.FontAwesome.Blazor.Tests.csproj -f net10.0 -c Release --collect:"XPlat Code Coverage" --settings tests/coverage.runsettings --results-directory artifacts/coverage'
    exit 1
}

$generatedNames = @('IconNames.cs', 'GeneratedIconComponents.cs')

$lines = @{}
$leaks = New-Object System.Collections.Generic.HashSet[string]

foreach($report in $reports) {
    [xml]$xml = Get-Content -LiteralPath $report.FullName -Raw

    # Coverlet emits paths relative to the report's <source> root. A test project that lives in
    # src/ (the core tests) reports "Jenin.FontAwesome.Blazor/Components/Icon.cs"; a test project
    # that lives in src/<project>/ reports "Components/Icon.cs". Keying raw filenames would count
    # the same source line twice (once per format) and deflate the rate, so canonicalize every
    # file to "<project>/<path within the project>" before unioning.
    $reportProject = $null
    foreach($source in $xml.SelectNodes('//sources/source')) {
        $normalizedSource = ([string]$source.InnerText) -replace '\\', '/'

        if($normalizedSource -match '/src/(?<project>Jenin\.FontAwesome\.Blazor[^/]*)/?$') {
            $reportProject = $matches['project']
            break
        }
    }

    foreach($class in $xml.SelectNodes('//class')) {
        $file = [string]$class.filename
        if([string]::IsNullOrEmpty($file)) {
            continue
        }

        $leaf = Split-Path -Leaf $file
        if($generatedNames -contains $leaf) {
            [void]$leaks.Add($file)
        }

        $canonicalFile = $file -replace '\\', '/'
        $srcIndex = $canonicalFile.IndexOf('/src/')
        if($srcIndex -ge 0) {
            $canonicalFile = $canonicalFile.Substring($srcIndex + 5)
        }
        $canonicalFile = $canonicalFile.TrimStart('/')

        if($canonicalFile -notmatch '^Jenin\.FontAwesome\.Blazor[^/]*/') {
            $project = if($reportProject) { $reportProject } else { 'Jenin.FontAwesome.Blazor' }
            $canonicalFile = "$project/$canonicalFile"
        }

        foreach($line in $class.SelectNodes('lines/line')) {
            $key = "$canonicalFile#$($line.number)"
            if(-not $lines.ContainsKey($key)) {
                $lines[$key] = $false
            }

            if([int]$line.hits -gt 0) {
                $lines[$key] = $true
            }
        }
    }
}

if($lines.Count -eq 0) {
    Write-Host 'Coverage reports contained no measured lines; the collection is broken.'
    Write-Host 'Check tests/coverage.runsettings and the coverlet.collector package references.'
    exit 1
}

$covered = ($lines.Values | Where-Object { $_ }).Count
$rate = [Math]::Round(100 * $covered / $lines.Count, 2)

Write-Host "Coverage: $covered/$($lines.Count) lines = $rate% (threshold: $Threshold%, reports: $($reports.Count))"

if($leaks.Count -gt 0) {
    Write-Host 'Generated files leaked into the coverage metric:'
    $leaks | ForEach-Object { Write-Host "  $_" }
    Write-Host 'Fix tests/coverage.runsettings (ExcludeByFile) before trusting this gate.'
    exit 1
}

if($rate -lt $Threshold) {
    Write-Host "Coverage gate failed: $rate% < $Threshold%."
    Write-Host "Coverage must not regress below eng/coverage-threshold.txt; add tests or raise coverage, then set a new ratchet deliberately."
    exit 1
}

Write-Host 'Coverage gate passed.'
