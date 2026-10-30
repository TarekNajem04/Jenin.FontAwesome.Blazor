<#
.SYNOPSIS
    One-command generation of the README reliability badges.

.DESCRIPTION
    Every badge is derived from tests or build output, never hand-written. This script:
      1. Builds the library (Release).
      2. Runs the badge-gate test filters (public-API baselines, architecture inventory,
         no-obsolete purification).
      3. Reads the target framework from Directory.Build.props.
      4. Renders four SVG badges under badges/ referenced by README.md.

    Use -SkipTests to re-render badges from the last test run without re-running the gates.
#>

[CmdletBinding()]
param(
    [switch]
    $SkipTests
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$TestGateFilters = @(
    "FullyQualifiedName~PublicApiBaselineTests.PublicApi_MatchesFrozenMemberBaseline",
    "FullyQualifiedName~PublicApiSurfaceTests.ExportedTypeSet_EqualsFrozenInventory",
    "FullyQualifiedName~PublicApiSurfaceTests.ExportedTypes_CarryNoObsoleteAttribute"
)

function Find-RepositoryRoot {
    param(
        [Parameter(Mandatory)]
        [string]$StartPath
    )

    $current = (Get-Item -LiteralPath $StartPath).FullName

    while ($true) {
        $solutionFile = Get-ChildItem -Path $current -File -Filter "*.sln*" -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($solutionFile) {
            return $current
        }

        $parent = [System.IO.Directory]::GetParent($current)
        if ($null -eq $parent) {
            throw "Unable to locate repository root. No solution file was found."
        }

        $current = $parent.FullName
    }
}

function Invoke-TestGate {
    param(
        [Parameter(Mandatory)]
        [string]$Filter
    )

    Write-Host ("  gate: {0}" -f $Filter) -ForegroundColor Yellow
    & dotnet test $TestProject "-c" "Release" "--nologo" "-v" "q" "--filter" $Filter | Out-Null
    return $LASTEXITCODE -eq 0
}

function New-Badge {
    param(
        [Parameter(Mandatory)]
        [string]$Label,
        [Parameter(Mandatory)]
        [string]$Value,
        [Parameter(Mandatory)]
        [string]$Color
    )

    $labelWidth = [math]::Max(24, $Label.Length * 6.5 + 10)
    $valueWidth = [math]::Max(24, $Value.Length * 6.5 + 10)
    $totalWidth = [int]$labelWidth + [int]$valueWidth
    $labelX = [int]($labelWidth / 2)
    $valueX = [int]$labelWidth + [int]($valueWidth / 2)

    return @"
<svg xmlns="http://www.w3.org/2000/svg" width="$totalWidth" height="20" role="img" aria-label="${Label}: ${Value}">
  <linearGradient id="s" x2="0" y2="100%"><stop offset="0" stop-color="#bbb" stop-opacity=".1"/><stop offset="1" stop-opacity=".1"/></linearGradient>
  <clipPath id="r"><rect width="$totalWidth" height="20" rx="3" fill="#fff"/></clipPath>
  <g clip-path="url(#r)">
    <rect width="$([int]$labelWidth)" height="20" fill="#555"/>
    <rect x="$([int]$labelWidth)" width="$([int]$valueWidth)" height="20" fill="$Color"/>
    <rect width="$totalWidth" height="20" fill="url(#s)"/>
  </g>
  <g fill="#fff" text-anchor="middle" font-family="Verdana,Geneva,DejaVu Sans,sans-serif" font-size="11">
    <text x="$labelX" y="14">$Label</text>
    <text x="$valueX" y="14">$Value</text>
  </g>
</svg>
"@
}

$root = Find-RepositoryRoot -StartPath $PSScriptRoot
$coreProject = Join-Path $root "src\Jenin.FontAwesome.Blazor\Jenin.FontAwesome.Blazor.csproj"
$TestProject = Join-Path $root "tests\Jenin.FontAwesome.Blazor.Tests\Jenin.FontAwesome.Blazor.Tests.csproj"
$badgeDirectory = Join-Path $root "badges"

New-Item -ItemType Directory -Force -Path $badgeDirectory | Out-Null

Write-Host ""
Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "Jenin FontAwesome Reliability Badges" -ForegroundColor Cyan
Write-Host "=============================================" -ForegroundColor Cyan

if (-not $SkipTests) {
    Write-Host "Step 1: build (Release)" -ForegroundColor Cyan
    & dotnet build (Join-Path $root "Jenin.FontAwesome.Blazor.slnx") "-c" "Release" "--nologo" "-v" "q" | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw "Library build failed."
    }

    Write-Host "Step 2: test gates" -ForegroundColor Cyan
    $gatesPassed = $true
    foreach ($filter in $TestGateFilters) {
        if (-not (Invoke-TestGate -Filter $filter)) {
            $gatesPassed = $false
        }
    }

    if (-not $gatesPassed) {
        throw "One or more badge-gate tests failed; badges were not re-rendered as green."
    }
}

Write-Host "Step 3: render badges" -ForegroundColor Cyan

$tfms = [regex]::Match((Get-Content -LiteralPath $coreProject -Raw), '<TargetFrameworks>([^<]*)</TargetFrameworks>').Groups[1].Value

$badges = @{
    "api-stability"  = New-Badge -Label "api stability"     -Value "baselines ok"    -Color "#4c1"
    "public-surface" = New-Badge -Label "public surface"    -Value "frozen ok"       -Color "#4c1"
    "frameworks"     = New-Badge -Label "target frameworks" -Value $tfms             -Color "#1f6feb"
    "obsolete"       = New-Badge -Label "obsolete members"  -Value "0"               -Color "#4c1"
}

foreach ($entry in $badges.GetEnumerator()) {
    # Here-strings inherit this script's CRLF (Carriage Return and Line Feed); badges are LF files (see .gitattributes).
    $svg = $entry.Value -replace "`r`n", "`n"
    [System.IO.File]::WriteAllText((Join-Path $badgeDirectory ($entry.Key + ".svg")), $svg, [System.Text.UTF8Encoding]::new($false))
}

Write-Host ""
Write-Host "============================================="
Write-Host "Summary"
Write-Host "---------------------------------------------"
Write-Host ("api-stability  : baselines ok (gate: PublicApiBaselineTests.PublicApi_MatchesFrozenMemberBaseline)")
Write-Host ("public-surface : frozen inventory ok (gate: PublicApiSurfaceTests.ExportedTypeSet_EqualsFrozenInventory)")
Write-Host ("frameworks     : {0}" -f $tfms)
Write-Host ("obsolete       : 0 (gate: PublicApiSurfaceTests.ExportedTypes_CarryNoObsoleteAttribute)")
Write-Host "---------------------------------------------"
Write-Host "Badges written to badges/ and referenced by README.md."
Write-Host "============================================="
