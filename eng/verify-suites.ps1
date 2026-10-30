#!/usr/bin/env pwsh
#
# UI-suite adapter verification.
#
# Boots the suite-adapters fixture (real MudBlazor + Blazorise icon components fed by the
# adapter packages) and asserts the emitted contract in a real browser. FA glyphs resolve
# against the vendored tree (FA_VENDOR_DIR, defaulting to this repo's vendored Free 7.3.1);
# no live suites, kits, or network beyond localhost are involved.
#
# Usage:
#   pwsh ./eng/verify-suites.ps1
#   pwsh ./eng/verify-suites.ps1 -KeepRunning

[CmdletBinding()]
param(
    [switch]$KeepRunning
)

$ErrorAction = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$fixtureDir = Join-Path $root 'tests' 'e2e' 'fixtures' 'suite-adapters'
$vendorDir = Join-Path $root 'samples' 'Jenin.FontAwesome.Blazor.Sample.Shared' 'wwwroot' 'fontawesome'

$jobs = @()

try {
    Write-Host 'Building the suite-adapters fixture...' -ForegroundColor Cyan
    $null = dotnet build (Join-Path $fixtureDir 'suite-adapters.csproj') -c Release --nologo
    if($LASTEXITCODE -ne 0) { throw 'Suite-adapters fixture build failed.' }

    $job = Start-Job -ScriptBlock {
        param($dir, $port, $vendor)
        $env:FA_VENDOR_DIR = $vendor
        Set-Location $dir
        & dotnet run --project suite-adapters.csproj -c Release --no-build --urls "http://localhost:$port"
    } -ArgumentList $fixtureDir, 5006, $vendorDir
    $jobs += $job

    $page = $null
    for($i = 0; $i -lt 40; $i++) {
        try {
            Start-Sleep -Seconds 3
            $page = (Invoke-WebRequest -Uri 'http://localhost:5006/mud' -UseBasicParsing -TimeoutSec 10).Content
            if($page -match 'MudBlazor adapter') { break }
            $page = $null
        }
        catch {
            $page = $null
        }
    }
    if($null -eq $page) { throw 'Suite-adapters fixture did not respond.' }
    Write-Host 'Suite-adapters fixture serves both adapter pages.' -ForegroundColor Green

    $env:SAMPLE_URL = 'http://localhost:5006'
    # Marker consumed by suites.spec.ts: the spec skips unless the fixture host is up, so a
    # plain `npm test` (main sample host) never runs it against the wrong pages.
    $env:SUITES_FIXTURE = '1'
    try {
        Push-Location (Join-Path $root 'tests' 'e2e')
        & npx playwright test suites.spec.ts
        if($LASTEXITCODE -ne 0) { throw 'Playwright suites.spec.ts FAILED.' }
    }
    finally {
        Pop-Location
        Remove-Item Env:SAMPLE_URL -ErrorAction SilentlyContinue
        Remove-Item Env:SUITES_FIXTURE -ErrorAction SilentlyContinue
    }

    Write-Host 'UI-suite adapter verification PASSED.' -ForegroundColor Green
}
finally {
    if($KeepRunning) {
        Write-Host 'Keeping the fixture up for inspection (http://localhost:5006/{mud,blazorise}).' -ForegroundColor Yellow
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
