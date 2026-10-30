#requires -Version 7
<#
.SYNOPSIS
  Verifies that the generated DocFX API metadata covers every documented assembly.

.DESCRIPTION
  Reads the assembly list from docs/docfx.json (the metadata inputs, one DLL per package)
  and checks the DocFX API manifest under docs/api. The manifest is produced by:
      dotnet docfx docs/docfx.json
  It is git-ignored, so this check runs after metadata generation in CI.

.PARAMETER ApiDirectory
  The generated DocFX API directory. Defaults to <repo>/docs/api.

.PARAMETER DocfxConfig
  The DocFX configuration that lists the documented assemblies. Defaults to <repo>/docs/docfx.json.

.OUTPUTS
  Prints the covered assembly count and exits 1 when an assembly is missing or the metadata is absent.

.EXAMPLE
  pwsh scripts/docs/check-api-reference.ps1
#>
param(
    [string]$ApiDirectory = (Join-Path (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path 'docs\api'),
    [string]$DocfxConfig = (Join-Path (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path 'docs\docfx.json')
)

$ErrorActionPreference = 'Stop'

$manifestPath = Join-Path $ApiDirectory '.manifest'
$tocPath = Join-Path $ApiDirectory 'toc.yml'

if (-not (Test-Path -LiteralPath $DocfxConfig)) {
    Write-Error "DocFX configuration not found at '$DocfxConfig'."
    exit 2
}

if (-not (Test-Path -LiteralPath $manifestPath)) {
    Write-Error "DocFX API metadata not found at '$ApiDirectory'. Run 'dotnet docfx docs/docfx.json' first."
    exit 2
}

if (-not (Test-Path -LiteralPath $tocPath)) {
    Write-Error "DocFX API table of contents not found at '$tocPath'."
    exit 2
}

$configuration = Get-Content -LiteralPath $DocfxConfig -Raw | ConvertFrom-Json
$assemblies = [System.Collections.Generic.List[string]]::new()
foreach ($metadata in @($configuration.metadata)) {
    foreach ($source in @($metadata.src)) {
        foreach ($file in @($source.files)) {
            if ($file -match '(?<assembly>[^/\\]+)\.dll$') {
                $name = $Matches['assembly']
                if (-not $assemblies.Contains($name)) {
                    [void]$assemblies.Add($name)
                }
            }
        }
    }
}

if ($assemblies.Count -eq 0) {
    Write-Error "No documented assemblies are listed in '$DocfxConfig'."
    exit 2
}

$manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
$uids = @($manifest.PSObject.Properties.Name)

$missing = [System.Collections.Generic.List[string]]::new()
foreach ($assembly in $assemblies) {
    $prefix = $assembly + '.'
    $found = $false
    foreach ($uid in $uids) {
        if ($uid -eq $assembly -or $uid.StartsWith($prefix, [System.StringComparison]::Ordinal)) {
            $found = $true
            break
        }
    }

    if (-not $found) {
        $missing.Add($assembly)
    }
}

if ($missing.Count -gt 0) {
    Write-Host "API REFERENCE CHECK FAILED ($($missing.Count) missing assembly/assemblies):" -ForegroundColor Red
    $missing | ForEach-Object { Write-Host "  $_" -ForegroundColor Red }
    exit 1
}

Write-Host "API reference OK: $($assemblies.Count) documented assemblies are represented in the generated metadata." -ForegroundColor Green
