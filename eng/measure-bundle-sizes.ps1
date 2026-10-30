#!/usr/bin/env pwsh
#
# Bundle-size benchmark dashboard.
#
# Measures real publish output for fixture apps per delivery mode and rewrites the README
# dashboard table between the BUNDLE-SIZES markers, so evaluators asking "how much does it
# cost me?" get maintained numbers instead of a missing answer (EasyAppDev parity gap).
#
# Measured cells (all Release, net10.0-only; the TFM is recorded in the baseline):
#   * icons-1 / icons-10 - trimmed console host referencing 1 / 10 Icons preset components;
#     measures Jenin.FontAwesome.Blazor.Icons.dll inside the publish output (glyphs still come
#     from hosted Font Awesome assets, so this is the managed-code cost only).
#   * svg-1 / svg-10     - same host shape for the Svg package; the measured DLL carries the
#     inlined glyph paths, so this is the full per-icon payload (no JS/CSS/webfonts needed).
#   * subset-webfont     - the committed 2-icon fa-subset fixture (subset CSS + subset WOFF2s).
#   * sprite             - the committed 2-icon fa-sprite.svg fixture.
#   * status-quo-svgjs   - the vendored Font Awesome 7.3.1 assets an SVG+JS self-hoster ships
#     today (all.min.js + all.css + webfonts), independent of icon count.
#   * package-icons / package-svg - dotnet pack .nupkg bytes for the two optional packages.
#
# Each trimmed/file cell records raw bytes plus Brotli bytes using the .NET default encoder as
# a consistent comparator (not a wire-size promise: real servers vary quality and headers).
# .nupkg files are already compressed archives, so only raw bytes are recorded for them.
#
# Gate behavior: every cell is compared against the committed eng/bundle-sizes.json baseline.
# Growth beyond -ThresholdPercent fails the run so a human reviews the delta before it lands.
# Shrinkage never fails. Cells missing from the baseline are recorded, never failed.
#
# Usage:
#   pwsh ./eng/measure-bundle-sizes.ps1                  # measure + rewrite README table + gate
#   pwsh ./eng/measure-bundle-sizes.ps1 -Check           # measure + gate only (no file writes)
#   pwsh ./eng/measure-bundle-sizes.ps1 -UpdateBaseline  # measure + rewrite table + refresh baseline
#   pwsh ./eng/measure-bundle-sizes.ps1 -ThresholdPercent 10

[CmdletBinding()]
param(
    [switch]$Check,
    [switch]$UpdateBaseline,
    [double]$ThresholdPercent = 5.0
)

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$baselinePath = Join-Path $root 'eng' 'bundle-sizes.json'
$readmePath = Join-Path $root 'README.md'
$scratchRoot = Join-Path $root 'artifacts' 'bundle-size'

$iconsCsproj = (Resolve-Path (Join-Path $root 'src' 'Jenin.FontAwesome.Blazor.Icons' 'Jenin.FontAwesome.Blazor.Icons.csproj')).Path
$svgCsproj = (Resolve-Path (Join-Path $root 'src' 'Jenin.FontAwesome.Blazor.Svg' 'Jenin.FontAwesome.Blazor.Svg.csproj')).Path
$faDir = Join-Path $root 'samples' 'Jenin.FontAwesome.Blazor.Sample.Shared' 'wwwroot' 'fontawesome'
$subsetDir = Join-Path $root 'tests' 'e2e' 'fixtures' 'fa-subset'
$spriteFile = Join-Path $root 'tests' 'e2e' 'fixtures' 'fa-sprite' 'fa-sprite.svg'

# Ten FA Free icons present in both the Icons presets and the Svg components (the build of
# each fixture app fails loudly if a name drifts).
$tenIcons = @('House', 'Star', 'User', 'Gear', 'MagnifyingGlass', 'Heart', 'Check', 'Xmark', 'Bell')

function Get-BrotliLength([byte[]]$bytes) {
    # NOTE: the MemoryStream Length must be read while it is still open (leaveOpen=$true on
    # the encoder); PowerShell surfaces a closed stream's Length as $null instead of throwing.
    $ms = New-Object System.IO.MemoryStream
    try {
        $encoder = New-Object System.IO.Compression.BrotliStream($ms, [System.IO.Compression.CompressionMode]::Compress, $true)
        try {
            $encoder.Write($bytes, 0, $bytes.Length)
        }
        finally {
            $encoder.Close()
        }
        return $ms.Length
    }
    finally {
        $ms.Close()
    }
}

function Measure-FileCell([string]$path) {
    $bytes = [System.IO.File]::ReadAllBytes($path)
    return @{ raw = $bytes.Length; brotli = (Get-BrotliLength $bytes) }
}

function Measure-FilesCell([string[]]$paths) {
    $total = 0
    $buffer = New-Object System.Collections.Generic.List[byte]
    foreach($path in $paths) {
        $bytes = [System.IO.File]::ReadAllBytes($path)
        $total += $bytes.Length
        $buffer.AddRange($bytes)
    }
    return @{ raw = $total; brotli = (Get-BrotliLength $buffer.ToArray()); files = @($paths | ForEach-Object { Split-Path -Leaf $_ }) }
}

$probeCsproj = @'
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <OutputType>Exe</OutputType>
        <AssemblyName>BundleSizeProbe</AssemblyName>
        <RootNamespace>BundleSizeProbe</RootNamespace>
        <TargetFramework>net10.0</TargetFramework>
        <PublishTrimmed>true</PublishTrimmed>
        <TrimMode>full</TrimMode>
        <InvariantGlobalization>true</InvariantGlobalization>
        <GenerateDocumentationFile>false</GenerateDocumentationFile>
        <Nullable>enable</Nullable>
    </PropertyGroup>

    <ItemGroup>
        <ProjectReference Include="{0}" />
    </ItemGroup>

</Project>
'@

function New-BundleProbe([string]$name, [string]$projectReference, [string]$program) {
    $probeDir = Join-Path $scratchRoot $name
    New-Item -ItemType Directory -Force -Path $probeDir | Out-Null
    [System.IO.File]::WriteAllText((Join-Path $probeDir 'BundleSizeProbe.csproj'), ($probeCsproj -f $projectReference))
    [System.IO.File]::WriteAllText((Join-Path $probeDir 'Program.cs'), $program)
    return $probeDir
}

function Measure-TrimmedDll([string]$name, [string]$projectReference, [string]$program, [string]$dllName) {
    $probeDir = New-BundleProbe $name $projectReference $program
    $publishDir = Join-Path $probeDir 'publish'
    # Discard stdout (it would otherwise leak into this function's return value); stderr
    # stays visible and the exit code still drives the failure below.
    $null = dotnet publish (Join-Path $probeDir 'BundleSizeProbe.csproj') -c Release -o $publishDir --nologo
    if($LASTEXITCODE -ne 0) { throw "Bundle-size probe publish failed for $name." }
    $dllPath = Join-Path $publishDir $dllName
    if(-not (Test-Path -LiteralPath $dllPath)) { throw "Expected published DLL missing: $dllPath." }
    return (Measure-FileCell $dllPath)
}

function Format-Bytes([long]$bytes) {
    # Invariant culture: the rewritten README table must be byte-identical on every machine
    # regardless of OS locale, otherwise the checked-in table drifts between runs.
    $inv = [System.Globalization.CultureInfo]::InvariantCulture
    if($bytes -ge 1048576) { return [string]::Format($inv, '{0:N1} MB', ($bytes / 1048576)) }
    if($bytes -ge 1024) { return [string]::Format($inv, '{0:N1} KB', ($bytes / 1024)) }
    return [string]::Format($inv, '{0:N0} B', $bytes)
}

if(Test-Path -LiteralPath $scratchRoot) {
    Remove-Item -Recurse -Force -Path $scratchRoot
}
New-Item -ItemType Directory -Force -Path $scratchRoot | Out-Null

try {
    # --- Trimmed managed-code cost per package -------------------------------------
    $iconsNs = 'Jenin.FontAwesome.Blazor.Icons'
    $svgNs = 'Jenin.FontAwesome.Blazor.Svg'

    $iconsOneProgram = "// Generated by eng/measure-bundle-sizes.ps1. Disposable benchmark fixture.`nusing System;`nusing $iconsNs;`n`nvar icon = new HouseIcon();`nConsole.WriteLine(icon.IconName);`n"
    $iconsTenProgram = "// Generated by eng/measure-bundle-sizes.ps1. Disposable benchmark fixture.`nusing System;`nusing Jenin.FontAwesome.Blazor.Components;`nusing $iconsNs;`n`nIcon[] icons = { $(($tenIcons | ForEach-Object { "new ${_}Icon()" }) -join ', '), new GithubIcon() };`nConsole.WriteLine(icons.Length);`n"

    $svgOneProgram = "// Generated by eng/measure-bundle-sizes.ps1. Disposable benchmark fixture.`nusing System;`nusing $svgNs;`n`nvar icon = new HouseSolidIcon();`nConsole.WriteLine(icon.GetType().Name);`n"
    $svgTenProgram = "// Generated by eng/measure-bundle-sizes.ps1. Disposable benchmark fixture.`nusing System;`nusing $svgNs;`n`nSvgIcon[] icons = { $(($tenIcons | ForEach-Object { "new ${_}SolidIcon()" }) -join ', '), new GithubBrandsIcon() };`nConsole.WriteLine(icons.Length);`n"

    Write-Host '==> Measuring trimmed fixture apps (net10.0, Release)...' -ForegroundColor Cyan
    $cells = @{}
    $cells['icons-1'] = Measure-TrimmedDll 'icons-1' $iconsCsproj $iconsOneProgram 'Jenin.FontAwesome.Blazor.Icons.dll'
    Write-Host ("    icons-1: {0:N0} B" -f $cells['icons-1'].raw)
    $cells['icons-10'] = Measure-TrimmedDll 'icons-10' $iconsCsproj $iconsTenProgram 'Jenin.FontAwesome.Blazor.Icons.dll'
    Write-Host ("    icons-10: {0:N0} B" -f $cells['icons-10'].raw)
    $cells['svg-1'] = Measure-TrimmedDll 'svg-1' $svgCsproj $svgOneProgram 'Jenin.FontAwesome.Blazor.Svg.dll'
    Write-Host ("    svg-1: {0:N0} B" -f $cells['svg-1'].raw)
    $cells['svg-10'] = Measure-TrimmedDll 'svg-10' $svgCsproj $svgTenProgram 'Jenin.FontAwesome.Blazor.Svg.dll'
    Write-Host ("    svg-10: {0:N0} B" -f $cells['svg-10'].raw)

    # --- Committed webfont/sprite fixtures (no publish needed) ----------------------
    $subsetFiles = @((Join-Path $subsetDir 'fontawesome-subset.css')) + @(Get-ChildItem -Path (Join-Path $subsetDir '*.woff2') -File | ForEach-Object { $_.FullName })
    $cells['subset-webfont'] = Measure-FilesCell $subsetFiles
    Write-Host ("    subset-webfont: {0:N0} B" -f $cells['subset-webfont'].raw)
    $cells['sprite'] = Measure-FileCell $spriteFile
    Write-Host ("    sprite: {0:N0} B" -f $cells['sprite'].raw)

    # --- Status quo: what an SVG+JS self-hoster ships today -------------------------
    $statusQuoFiles = @(
        (Join-Path $faDir 'js' 'all.min.js'),
        (Join-Path $faDir 'css' 'all.css')
    ) + @(Get-ChildItem -Path (Join-Path $faDir 'webfonts' '*.woff2') -File | ForEach-Object { $_.FullName })
    $cells['status-quo-svgjs'] = Measure-FilesCell $statusQuoFiles
    Write-Host ("    status-quo-svgjs: {0:N0} B" -f $cells['status-quo-svgjs'].raw)

    # --- NuGet package bytes ----------------------------------------------------------
    $packDir = Join-Path $scratchRoot 'packages'
    New-Item -ItemType Directory -Force -Path $packDir | Out-Null
    $null = dotnet pack (Split-Path -Parent $iconsCsproj) -c Release -o $packDir --nologo
    if($LASTEXITCODE -ne 0) { throw 'dotnet pack failed for the Icons package.' }
    $null = dotnet pack (Split-Path -Parent $svgCsproj) -c Release -o $packDir --nologo
    if($LASTEXITCODE -ne 0) { throw 'dotnet pack failed for the Svg package.' }
    $iconsNupkg = Get-ChildItem -Path (Join-Path $packDir 'Jenin.FontAwesome.Blazor.Icons.*.nupkg') -File | Select-Object -First 1
    $svgNupkg = Get-ChildItem -Path (Join-Path $packDir 'Jenin.FontAwesome.Blazor.Svg.*.nupkg') -File | Select-Object -First 1
    $cells['package-icons'] = @{ raw = $iconsNupkg.Length }
    $cells['package-svg'] = @{ raw = $svgNupkg.Length }
    Write-Host ("    packages: Icons={0:N0} B, Svg={1:N0} B" -f $cells['package-icons'].raw, $cells['package-svg'].raw)
}
finally {
    if(Test-Path -LiteralPath $scratchRoot) {
        Remove-Item -Recurse -Force -Path $scratchRoot
    }
}

# --- Baseline gate ------------------------------------------------------------------
$baseline = $null
if(Test-Path -LiteralPath $baselinePath) {
    $baseline = Get-Content -LiteralPath $baselinePath -Raw | ConvertFrom-Json -AsHashtable
}

$failures = @()
if($null -ne $baseline -and $baseline.ContainsKey('cells')) {
    foreach($key in $cells.Keys) {
        if(-not $baseline.cells.ContainsKey($key)) {
            Write-Host ("    [new cell] {0}: {1:N0} B (no baseline yet)" -f $key, $cells[$key].raw)
            continue
        }
        foreach($metric in @('raw', 'brotli')) {
            if(-not $cells[$key].ContainsKey($metric) -or -not $baseline.cells[$key].ContainsKey($metric)) { continue }
            $old = [double]$baseline.cells[$key][$metric]
            $new = [double]$cells[$key][$metric]
            if($old -le 0) { continue }
            $delta = 100.0 * ($new - $old) / $old
            if($delta -gt $ThresholdPercent) {
                $failures += ("{0}.{1}: {2:N0} -> {3:N0} B (+{4:N1}% over +{5}% threshold)" -f $key, $metric, $old, $new, $delta, $ThresholdPercent)
            }
        }
    }
}

$measuredAt = (Get-Date).ToUniversalTime().ToString('yyyy-MM-dd')
$results = [ordered]@{
    generated_utc = $measuredAt
    tfm = 'net10.0'
    configuration = 'Release'
    threshold_pct = $ThresholdPercent
    sources = [ordered]@{
        fontAwesome = '7.3.1 Free (vendored under samples/.../wwwroot/fontawesome)'
        subset_fixture = 'tests/e2e/fixtures/fa-subset (2 icons: house + star)'
        sprite_fixture = 'tests/e2e/fixtures/fa-sprite/fa-sprite.svg (2 icons: house + star)'
        brotli = '.NET default encoder; consistent comparator, not a wire-size promise'
        peers = 'Third-party trimmed-size tables live in vendor docs and are not copied here so they cannot go stale; the status-quo row is this repo''s own dated baseline.'
    }
    cells = $cells
}

function Cell-Text([string]$key) {
    if(-not $cells.ContainsKey($key)) { return '—' }
    $text = Format-Bytes $cells[$key].raw
    if($cells[$key].ContainsKey('brotli')) {
        $text += (' ({0} Brotli)' -f (Format-Bytes $cells[$key].brotli))
    }
    return $text
}

$table = @()
$table += '<!-- BUNDLE-SIZES:START (generated by eng/measure-bundle-sizes.ps1; do not edit by hand) -->'
$table += ''
$table += ('Measured {0} on net10.0/Release (baseline: `eng/bundle-sizes.json`, gate: +{1}% growth fails):' -f $measuredAt, $ThresholdPercent)
$table += ''
$table += '| Scenario | Icons presets (trimmed DLL) | Inline SVG (trimmed DLL) | Subset webfont | Sprite (`<use>`) | Status quo (SVG+JS self-host) |'
$table += '|---|---|---|---|---|---|'
$table += ('| Trimmed app, 1 icon (`fa-house`) | {0} | {1} | — | — | — |' -f (Cell-Text 'icons-1'), (Cell-Text 'svg-1'))
$table += ('| Trimmed app, 10 icons | {0} | {1} | — | — | — |' -f (Cell-Text 'icons-10'), (Cell-Text 'svg-10'))
$table += ('| Self-hosted payload, 2-icon fixture | — | — | {0} | {1} | {2} |' -f (Cell-Text 'subset-webfont'), (Cell-Text 'sprite'), (Cell-Text 'status-quo-svgjs'))
$table += ('| NuGet package (`.nupkg`) | {0} | {1} | — | — | — |' -f (Cell-Text 'package-icons'), (Cell-Text 'package-svg'))
$table += ''
$table += 'Reading guide: the Icons column is managed-code cost only (glyphs still load from your'
$table += 'hosted Font Awesome assets); the Inline-SVG column is the whole payload (no JS/CSS/fonts).'
$table += 'Status quo ships the full engine regardless of icon count — the comparison point that makes'
$table += 'subset/sprite/inline-SVG numbers meaningful.'
$table += '<!-- BUNDLE-SIZES:END -->'
$tableText = ($table -join "`r`n")

if(-not $Check) {
    $readme = [System.IO.File]::ReadAllText($readmePath)
    $startMarker = '<!-- BUNDLE-SIZES:START'
    $endMarker = '<!-- BUNDLE-SIZES:END -->'
    $start = $readme.IndexOf($startMarker)
    $end = $readme.IndexOf($endMarker)
    if($start -ge 0 -and $end -gt $start) {
        $end += $endMarker.Length
        $readme = $readme.Substring(0, $start) + $tableText + $readme.Substring($end)
    }
    else {
        $anchor = '## Troubleshooting'
        $idx = $readme.IndexOf($anchor)
        if($idx -lt 0) { throw 'README anchor for the dashboard table not found.' }
        $section = '### Bundle-size dashboard' + "`r`n`r`n" + $tableText + "`r`n`r`n"
        $readme = $readme.Substring(0, $idx) + $section + $readme.Substring($idx)
    }
    [System.IO.File]::WriteAllText($readmePath, $readme)
    Write-Host 'README dashboard table updated.' -ForegroundColor Green
}

if($UpdateBaseline) {
    ($results | ConvertTo-Json -Depth 6) + "`r`n" | Set-Content -LiteralPath $baselinePath -NoNewline -Encoding utf8
    Write-Host ("Baseline written to {0}." -f $baselinePath) -ForegroundColor Green
}

if($failures.Count -gt 0) {
    Write-Host 'Bundle-size gate FAILED: cells grew beyond the threshold:' -ForegroundColor Red
    $failures | ForEach-Object { Write-Host ("  {0}" -f $_) -ForegroundColor Red }
    Write-Host 'Review the delta, then re-run with -UpdateBaseline to bless it.'
    exit 1
}

Write-Host 'Bundle-size gate PASSED.' -ForegroundColor Green
