#!/usr/bin/env pwsh
#
# Repeatable licensed-Pro render verification.
#
# Renders the family x style x mode matrix with the library's own components and asserts the
# emitted contract end to end in a real browser, for Free and for licensed Pro assets.
#
# Asset sourcing (nothing licensed is ever committed — fixtures read URLs from the environment):
#   * -AssetsDir <dir>  - a Font Awesome tree (css/all.css, js/all.min.js, webfonts/).
#     Defaults to $env:FA_PRO_ASSETS, else the repo's vendored Free 7.3.1 assets.
#   * -KitCss/-KitJs    - direct Kit/Pro stylesheet + runtime URLs (alternative to a directory).
#   * -Edition auto|free|pro - default auto: Pro woff2 (light/thin/duotone/sharp) present => pro.
#   * -SkipBrowser      - serve + SSR assertions only (no Playwright DOM checks).
#   * -KeepRunning      - leave fixture + asset servers up for manual inspection.
#
# CI runs the Free-only subset with zero secrets. The full licensed run is owner-triggered:
#   $env:FA_PRO_ASSETS = 'C:\fa-pro-kit'
#   pwsh ./eng/verify-pro-render.ps1
#
# Usage:
#   pwsh ./eng/verify-pro-render.ps1
#   pwsh ./eng/verify-pro-render.ps1 -AssetsDir C:\fa-pro-kit -Edition pro

[CmdletBinding()]
param(
    [string]$AssetsDir = '',
    [string]$KitCss = '',
    [string]$KitJs = '',
    [ValidateSet('auto', 'free', 'pro')]
    [string]$Edition = 'auto',
    [switch]$SkipBrowser,
    [switch]$KeepRunning
)

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$fixtureDir = Join-Path $root 'tests' 'e2e' 'fixtures' 'pro-matrix'
$vendoredFree = Join-Path $root 'samples' 'Jenin.FontAwesome.Blazor.Sample.Shared' 'wwwroot' 'fontawesome'

function Resolve-AssetFile([string]$dir, [string]$name) {
    $found = Get-ChildItem -Path $dir -Recurse -File -Filter $name -ErrorAction SilentlyContinue | Select-Object -First 1
    if($null -eq $found) { throw "Required asset '$name' not found under $dir." }
    return $found.FullName
}

$jobs = @()
$cssUrl = $KitCss
$jsUrl = $KitJs
$assetServerJob = $null

try {
    if([string]::IsNullOrEmpty($cssUrl)) {
        if([string]::IsNullOrEmpty($AssetsDir)) {
            $AssetsDir = $env:FA_PRO_ASSETS
        }
        if([string]::IsNullOrEmpty($AssetsDir)) {
            $AssetsDir = $vendoredFree
            Write-Host 'No Pro assets configured; verifying the Free-only subset (vendored 7.3.1).' -ForegroundColor Yellow
        }

        $cssPath = Resolve-AssetFile $AssetsDir 'all.css'
        $cssRoot = Split-Path -Parent (Split-Path -Parent $cssPath)
        $jsPath = Join-Path $cssRoot 'js' 'all.min.js'

        # Static asset server for the (possibly licensed, always external) tree.
        $assetJob = Start-Job -ScriptBlock {
            param($serveDir, $port)
            & python -m http.server $port --directory $serveDir --bind 127.0.0.1
        } -ArgumentList $cssRoot, 5059
        $jobs += $assetJob
        $assetServerJob = $assetJob

        $cssUrl = 'http://127.0.0.1:5059/css/all.css'
        if(Test-Path -LiteralPath $jsPath) {
            $jsUrl = 'http://127.0.0.1:5059/js/all.min.js'
        }

        if($Edition -eq 'auto') {
            $proFonts = @(Get-ChildItem -Path (Join-Path $cssRoot 'webfonts') -File -ErrorAction SilentlyContinue |
                Where-Object { $_.Name -match 'light|thin|duotone|sharp' })
            $Edition = if($proFonts.Count -gt 0) { 'pro' } else { 'free' }
        }
    }
    elseif($Edition -eq 'auto') {
        $Edition = 'pro'
    }

    Write-Host ("Edition under test: {0}" -f $Edition) -ForegroundColor Cyan
    Write-Host ("CSS: {0}" -f $cssUrl) -ForegroundColor Cyan
    if(-not [string]::IsNullOrEmpty($jsUrl)) {
        Write-Host ("JS:  {0}" -f $jsUrl) -ForegroundColor Cyan
    }

    $fixtureProj = Join-Path $fixtureDir 'pro-matrix.csproj'
    $modes = @(@{ Mode = 'nest'; Port = 5005 }, @{ Mode = 'webfont'; Port = 5006 })

    foreach($run in $modes) {
        $fixtureJob = Start-Job -ScriptBlock {
            param($project, $port, $css, $js, $edition, $mode)
            $env:FA_ASSET_CSS = $css
            $env:FA_ASSET_JS = $js
            $env:FA_EDITION = $edition
            $env:PRO_MATRIX_MODE = $mode
            Set-Location (Split-Path -Parent $project)
            & dotnet run --project $project -c Release --urls "http://localhost:$port"
        } -ArgumentList $fixtureProj, $run.Port, $cssUrl, $jsUrl, $Edition, $run.Mode
        $jobs += $fixtureJob

        $pageHtml = $null
        for($i = 0; $i -lt 40; $i++) {
            try {
                Start-Sleep -Seconds 3
                $pageHtml = (Invoke-WebRequest -Uri ("http://localhost:{0}/" -f $run.Port) -UseBasicParsing -TimeoutSec 10).Content
                if($pageHtml -match 'data-edition="(free|pro)"') { break }
                $pageHtml = $null
            }
            catch {
                $pageHtml = $null
            }
        }
        if($null -eq $pageHtml) { throw ("Fixture host ({0}) did not respond." -f $run.Mode) }

        $servedEdition = $Matches[1]
        if($servedEdition -ne $Edition) { throw "Fixture reports edition '$servedEdition', expected '$Edition'." }
        if($pageHtml -match 'data-mode="([^"]+)"' -and $Matches[1] -ne $run.Mode) {
            throw ("Fixture reports mode '{0}', expected '{1}'." -f $Matches[1], $run.Mode)
        }
        if($pageHtml -notmatch 'fa-light fa-camera') { throw ("Matrix markup missing from fixture ({0})." -f $run.Mode) }
        Write-Host ("Fixture serves the matrix ({0})." -f $run.Mode) -ForegroundColor Green

        if(-not $SkipBrowser) {
            $env:SAMPLE_URL = ("http://localhost:{0}" -f $run.Port)
            try {
                Push-Location (Join-Path $root 'tests' 'e2e')
                & npx playwright test promatrix.spec.ts
                if($LASTEXITCODE -ne 0) { throw ("Playwright pro-matrix spec FAILED ({0})." -f $run.Mode) }
            }
            finally {
                Pop-Location
                Remove-Item Env:SAMPLE_URL -ErrorAction SilentlyContinue
            }
        }

        if(-not $KeepRunning) {
            try {
                Stop-Job -Job $fixtureJob -ErrorAction SilentlyContinue
                Remove-Job -Job $fixtureJob -Force -ErrorAction SilentlyContinue
                $jobs = @($jobs | Where-Object { $_.Id -ne $fixtureJob.Id })
            }
            catch {
            }
        }
    }

    Write-Host ("Pro render verification PASSED (edition: {0})." -f $Edition) -ForegroundColor Green
    Write-Host 'Record the family x style x mode results before finishing.' -ForegroundColor Cyan
}
finally {
    if($KeepRunning) {
        Write-Host 'Keeping servers running for inspection (fixture :5005, assets :5059).' -ForegroundColor Yellow
    }
    else {
        foreach($job in $jobs) {
            try {
                Stop-Job -Job $job -ErrorAction SilentlyContinue
                Remove-Job -Job $job -Force -ErrorAction SilentlyContinue
            }
            catch {
            }
        }
    }
}
