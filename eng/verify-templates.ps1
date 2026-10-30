#!/usr/bin/env pwsh
#
# Template-pack smoke verification.
#
# Packs the library and the template pack into a local feed, installs the templates, scaffolds
# each template with no edits, restores/builds/runs the scaffolded apps, and asserts a first
# icon renders (SSR markup for the Web App; served tags plus an optional headless-browser DOM
# check for WebAssembly). The mapped config endpoint is asserted in both FA modes.
#
# Pre-release the scaffolded projects resolve Jenin.FontAwesome.Blazor from the local feed;
# after 2.0 ships to nuget.org the same templates restore with no feed. The install/uninstall
# of the template package is scoped to this run (finally-block uninstall).
#
# Usage:
#   pwsh ./eng/verify-templates.ps1
#   pwsh ./eng/verify-templates.ps1 -SkipBrowser   # curl assertions only, no headless browser
#   pwsh ./eng/verify-templates.ps1 -KeepScaffold  # leave artifacts/template-smoke for inspection

[CmdletBinding()]
param(
    [switch]$SkipBrowser,
    [switch]$KeepScaffold
)

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$feedDir = Join-Path $root 'artifacts' 'localfeed'
# Scaffold OUTSIDE the repository: in-repo scaffolds would inherit the repo's
# Directory.Packages.props (central package management) and Directory.Build.props, which real
# consumers never see. The temp location also keeps the working tree clean.
$smokeRoot = Join-Path ([System.IO.Path]::GetTempPath()) 'jenin-fa-template-smoke'

function Read-LibraryVersion {
    [xml]$props = Get-Content -LiteralPath (Join-Path $root 'Directory.Build.props') -Raw
    $version = $props.Project.PropertyGroup.Version | Where-Object { $_ } | Select-Object -First 1
    if([string]::IsNullOrEmpty($version)) { throw 'Could not read <Version> from Directory.Build.props.' }
    return $version
}

function New-ScaffoldFeed([string]$dir, [string]$feed) {
    $config = @'
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="template-smoke-local" value="{0}" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
'@ -f $feed
    [System.IO.File]::WriteAllText((Join-Path $dir 'NuGet.config'), $config)
}

function Invoke-Scaffold([string]$template, [string]$name, [string]$target, [string[]]$extraArgs) {
    $dir = Join-Path $smokeRoot $target
    if(Test-Path -LiteralPath $dir) { Remove-Item -Recurse -Force -Path $dir }
    # Pre-seed NuGet.config so the template's restore post-action resolves the local feed.
    New-Item -ItemType Directory -Force -Path $dir | Out-Null
    New-ScaffoldFeed $dir $feedDir
    $allArgs = @($template, '-n', $name, '-o', $dir) + $extraArgs + @('--force')
    Write-Host ("==> dotnet new {0}" -f ($allArgs -join ' ')) -ForegroundColor Cyan
    # Discard stdout (it would otherwise leak into this function's return value); stderr
    # stays visible and the exit code still drives the failure below.
    $null = & dotnet new @allArgs
    if($LASTEXITCODE -ne 0) { throw "Scaffolding '$target' failed." }
    return $dir
}

function Assert-Contains([string]$haystack, [string]$needle, [string]$what) {
    if(-not $haystack.Contains($needle)) { throw "Assertion failed ($what): expected to find '$needle'." }
}

$jobs = @()
$installed = $false

try {
    $version = Read-LibraryVersion
    Write-Host "Library version: $version" -ForegroundColor Green

    if(Test-Path -LiteralPath $feedDir) { Remove-Item -Recurse -Force -Path $feedDir }
    if(Test-Path -LiteralPath $smokeRoot) { Remove-Item -Recurse -Force -Path $smokeRoot }
    New-Item -ItemType Directory -Force -Path $feedDir | Out-Null
    New-Item -ItemType Directory -Force -Path $smokeRoot | Out-Null

    $null = dotnet pack (Join-Path $root 'src' 'Jenin.FontAwesome.Blazor' 'Jenin.FontAwesome.Blazor.csproj') -c Release -o $feedDir --nologo
    if($LASTEXITCODE -ne 0) { throw 'Library pack failed.' }
    $templateNupkg = Join-Path $feedDir "Jenin.FontAwesome.Blazor.Templates.$version.nupkg"
    $null = dotnet pack (Join-Path $root 'templates' 'Jenin.FontAwesome.Blazor.Templates.csproj') -c Release -o $feedDir --nologo
    if($LASTEXITCODE -ne 0) { throw 'Template pack failed.' }
    if(-not (Test-Path -LiteralPath $templateNupkg)) { throw "Template package not found: $templateNupkg." }

    $null = dotnet new install $templateNupkg
    if($LASTEXITCODE -ne 0) { throw 'Template install failed.' }
    $installed = $true

    # --- Scaffold each template with no edits -------------------------------------
    $webAppDir = Invoke-Scaffold 'jenin-fa-blazor' 'ScaffoldWeb' 'webapp' @()
    $webAppWebfontDir = Invoke-Scaffold 'jenin-fa-blazor' 'ScaffoldWebfont' 'webapp-webfont' @('--FaMode', 'Webfont')
    $wasmDir = Invoke-Scaffold 'jenin-fa-blazor-wasm' 'ScaffoldWasm' 'wasm' @()

    foreach($dir in @($webAppDir, $webAppWebfontDir, $wasmDir)) {
        $proj = Get-ChildItem -Path $dir -Filter '*.csproj' -Recurse | Select-Object -First 1
        if($null -eq $proj) { throw "No project found in $dir." }
        Write-Host ("==> dotnet build {0}" -f $proj.FullName) -ForegroundColor Cyan
        $null = dotnet build $proj.FullName -c Release --nologo
        if($LASTEXITCODE -ne 0) { throw "Build failed for $dir." }
    }

    function Start-Scaffold([string]$dir, [int]$port) {
        $proj = Get-ChildItem -Path $dir -Filter '*.csproj' -Recurse | Select-Object -First 1
        $job = Start-Job -ScriptBlock {
            param($project, $port)
            & dotnet run --project $project -c Release --no-build --urls "http://localhost:$port"
        } -ArgumentList $proj.FullName, $port
        return $job
    }

    function Wait-WasmAppUrl($job) {
        # The WebAssembly dev-server ignores --urls and prints its own endpoint.
        for($i = 0; $i -lt 40; $i++) {
            Start-Sleep -Seconds 3
            $text = (Receive-Job -Job $job | Out-String)
            $match = [regex]::Match($text, 'App url:\s*(http://127\.0\.0\.1:\d+)')
            if($match.Success) { return $match.Groups[1].Value }
        }
        throw 'WASM dev-server did not print an App url.'
    }

    function Wait-ForHttp([string]$url) {
        for($i = 0; $i -lt 30; $i++) {
            try {
                return (Invoke-WebRequest -Uri $url -UseBasicParsing -TimeoutSec 10).Content
            }
            catch {
                Start-Sleep -Seconds 3
            }
        }
        throw "Server did not respond at $url."
    }

    # --- Web App (SvgNest default): tags + icon + nest config ----------------------
    $jobs += Start-Scaffold $webAppDir 5061
    $home1 = Wait-ForHttp 'http://localhost:5061/'
    Assert-Contains $home1 'fa-house' 'WebApp home shows the first icon'
    Assert-Contains $home1 '/_content/Jenin.FontAwesome.Blazor/js/fontawesome-config-auto.js' 'WebApp head points at the mapped config endpoint'
    Assert-Contains $home1 'cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@7.3.1/css/all.min.css' 'WebApp head renders the stylesheet'
    $config1 = (Invoke-WebRequest -Uri 'http://localhost:5061/_content/Jenin.FontAwesome.Blazor/js/fontawesome-config-auto.js' -UseBasicParsing -TimeoutSec 15).Content
    Assert-Contains $config1 'autoReplaceSvg: "nest"' 'WebApp default serves the nest config'
    Write-Host 'WebApp (SvgNest) smoke PASSED.' -ForegroundColor Green

    # --- Web App (Webfont): same tags, false config --------------------------------
    $jobs += Start-Scaffold $webAppWebfontDir 5062
    $home2 = Wait-ForHttp 'http://localhost:5062/'
    Assert-Contains $home2 'fa-house' 'WebApp webfont home shows the first icon'
    $config2 = (Invoke-WebRequest -Uri 'http://localhost:5062/_content/Jenin.FontAwesome.Blazor/js/fontawesome-config-auto.js' -UseBasicParsing -TimeoutSec 15).Content
    Assert-Contains $config2 'autoReplaceSvg: false' 'WebApp --FaMode Webfont serves the webfont config'
    Write-Host 'WebApp (Webfont) smoke PASSED.' -ForegroundColor Green

    # --- WebAssembly: tags served; DOM check needs a browser -----------------------
    $wasmJob = Start-Scaffold $wasmDir 5063
    $jobs += $wasmJob
    $wasmBase = Wait-WasmAppUrl $wasmJob
    Write-Host "WASM dev-server at $wasmBase" -ForegroundColor Cyan
    $index = (Invoke-WebRequest -Uri "$wasmBase/" -UseBasicParsing -TimeoutSec 15).Content
    Assert-Contains $index 'fontawesome-config-nest.js' 'WASM index.html references the static nest config'
    Assert-Contains $index 'blazor.webassembly.js' 'WASM index.html boots the runtime'
    Write-Host 'WebAssembly static smoke PASSED.' -ForegroundColor Green

    if(-not $SkipBrowser) {
        $playwrightLib = Join-Path $root 'tests' 'e2e' 'node_modules' '@playwright' 'test'
        if(Test-Path -LiteralPath $playwrightLib) {
            $checkScript = Join-Path ([System.IO.Path]::GetTempPath()) 'wasm-dom-check.cjs'
            [System.IO.File]::WriteAllText($checkScript, @'
const { chromium } = require(process.argv[2]);
const baseUrl = process.argv[3];
(async () => {
    const browser = await chromium.launch();
    try {
        const page = await browser.newPage();
        await page.goto(baseUrl + '/', { waitUntil: 'load', timeout: 60000 });
        await page.waitForSelector('.fa-house', { timeout: 120000 });
        const count = await page.locator('.fa-house').count();
        if(count < 1) { throw new Error('No .fa-house element rendered.'); }
        console.log(`WASM DOM check PASSED (${count} .fa-house element(s)).`);
    }
    finally {
        await browser.close();
    }
})().catch((error) => { console.error('WASM DOM check FAILED: ' + error.message); process.exit(1); });
'@)
            & node $checkScript $playwrightLib $wasmBase
            if($LASTEXITCODE -ne 0) { throw 'WebAssembly headless-browser DOM check failed.' }
        }
        else {
            Write-Host 'Playwright not installed under tests/e2e/node_modules; skipping the WASM DOM check (static tags verified).' -ForegroundColor Yellow
        }
    }
}
finally {
    foreach($job in $jobs) {
        try {
            Stop-Job -Job $job -ErrorAction SilentlyContinue
            Remove-Job -Job $job -Force -ErrorAction SilentlyContinue
        }
        catch {
        }
    }
    if($installed) {
        & dotnet new uninstall Jenin.FontAwesome.Blazor.Templates
    }
    if(-not $KeepScaffold -and (Test-Path -LiteralPath $smokeRoot)) {
        Remove-Item -Recurse -Force -Path $smokeRoot
    }
}

Write-Host 'Template smoke verification PASSED.' -ForegroundColor Green
