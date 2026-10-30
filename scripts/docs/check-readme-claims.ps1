#requires -Version 7
<#
.SYNOPSIS
  Verifies that the package names and the package count claimed by README.md are backed by reality:
  every backticked "Jenin.FontAwesome.Blazor.*" name must exist as a project in the solution tree,
  every package documented by DocFX must be mentioned in the README, and the "all N packages"
  claim must match the documented package count.

.PARAMETER ReadmePath
  Repo README.md. Defaults to <repo>/README.md.

.PARAMETER DocfxConfig
  DocFX configuration listing the shipped packages. Defaults to <repo>/docs/docfx.json.

.OUTPUTS
  Prints the checked claims, then a summary. Exits 1 if any claim is not backed.

.EXAMPLE
  pwsh scripts/docs/check-readme-claims.ps1
#>
param(
    [string]$ReadmePath = (Join-Path (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path 'README.md'),
    [string]$DocfxConfig = (Join-Path (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path 'docs\docfx.json')
)

$ErrorActionPreference = 'Stop'

if (-not (Test-Path -LiteralPath $ReadmePath)) {
    Write-Error "README not found: $ReadmePath"
    exit 2
}

if (-not (Test-Path -LiteralPath $DocfxConfig)) {
    Write-Error "DocFX configuration not found: $DocfxConfig"
    exit 2
}

$repositoryRoot = Split-Path -Parent $ReadmePath

# ---------------------------------------------------------------- project-name claims
$projectDirectories = @('src', 'samples', 'tests', 'templates')
$knownProjects = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
foreach ($directory in $projectDirectories) {
    $path = Join-Path $repositoryRoot $directory
    if (-not (Test-Path -LiteralPath $path)) { continue }
    foreach ($project in Get-ChildItem $path -Recurse -Filter '*.csproj' -File) {
        [void]$knownProjects.Add($project.BaseName)
    }
}

$readme = [System.IO.File]::ReadAllText($ReadmePath)
$tokens = [regex]::Matches($readme, '`(Jenin\.FontAwesome\.Blazor[A-Za-z0-9._*]*)`') |
    ForEach-Object { $_.Groups[1].Value } |
    Sort-Object -Unique

$unresolved = [System.Collections.Generic.List[string]]::new()
foreach ($token in $tokens) {
    if ($token.EndsWith('.*')) {
        $prefix = $token.Substring(0, $token.Length - 2)
        $match = $knownProjects | Where-Object { $_.StartsWith($prefix, [System.StringComparison]::Ordinal) } | Select-Object -First 1
        if ($null -eq $match) {
            [void]$unresolved.Add("$token (no project with prefix '$prefix')")
        }
        continue
    }

    if (-not $knownProjects.Contains($token)) {
        [void]$unresolved.Add("$token (no such project)")
    }
}

# ---------------------------------------------------------------- documented-package claims
$configuration = Get-Content -LiteralPath $DocfxConfig -Raw | ConvertFrom-Json
$documentedPackages = [System.Collections.Generic.List[string]]::new()
foreach ($metadata in @($configuration.metadata)) {
    foreach ($source in @($metadata.src)) {
        foreach ($file in @($source.files)) {
            if ($file -match '(?<assembly>[^/\\]+)\.dll$') {
                $name = $Matches['assembly']
                if (-not $documentedPackages.Contains($name)) {
                    [void]$documentedPackages.Add($name)
                }
            }
        }
    }
}

$countFailures = [System.Collections.Generic.List[string]]::new()
foreach ($package in $documentedPackages) {
    if (-not $readme.Contains($package, [System.StringComparison]::Ordinal)) {
        [void]$countFailures.Add("README does not mention the documented package '$package'")
    }
}

$numberWords = @{
    'one' = 1; 'two' = 2; 'three' = 3; 'four' = 4; 'five' = 5;
    'six' = 6; 'seven' = 7; 'eight' = 8; 'nine' = 9; 'ten' = 10
}
$countMatch = [regex]::Match($readme, 'all\s+(?<count>\d+|one|two|three|four|five|six|seven|eight|nine|ten)\s+packages', 'IgnoreCase')
if ($countMatch.Success) {
    $rawCount = $countMatch.Groups['count'].Value.ToLowerInvariant()
    $claimed = if ($numberWords.ContainsKey($rawCount)) { $numberWords[$rawCount] } else { [int]$rawCount }
    if ($claimed -ne $documentedPackages.Count) {
        [void]$countFailures.Add("README claims $claimed packages, but $($documentedPackages.Count) packages are documented in docs/docfx.json")
    }
}
else {
    [void]$countFailures.Add("README does not state the 'all N packages' count that this gate verifies")
}

Write-Host "Checked $($tokens.Count) project-name claim(s) against $($knownProjects.Count) solution project(s)."
Write-Host "Package-count claim: $(if ($countMatch.Success) { $countMatch.Groups['count'].Value } else { '(absent)' }) vs $($documentedPackages.Count) documented package(s)."

if ($unresolved.Count -gt 0 -or $countFailures.Count -gt 0) {
    Write-Host ''
    Write-Host "README CLAIMS FAILED ($($unresolved.Count + $countFailures.Count) problem(s)):" -ForegroundColor Red
    $unresolved | ForEach-Object { Write-Host "  $_" -ForegroundColor Red }
    $countFailures | ForEach-Object { Write-Host "  $_" -ForegroundColor Red }
    exit 1
}

Write-Host 'README claims OK: every package name and the package count are backed by the repository.' -ForegroundColor Green
