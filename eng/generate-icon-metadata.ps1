#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Generates the compact, embedded icon-metadata resource for the IconPicker package.
.DESCRIPTION
    Reads the rich per-icon metadata vendored by `eng/update-fontawesome-metadata.ps1` and emits a
    minified JSON resource embedded into `Jenin.FontAwesome.Blazor.IconPicker`. The compact form uses
    short keys (v/icons/n/l/s/t/a) and omits empty term/alias arrays to keep the embedded resource small.

    The generator is deterministic (icons kept in ordinal name order, short-key order fixed) and supports
    a `-Verify` mode used by CI to fail when the committed resource drifts from the vendored metadata.
.PARAMETER MetadataPath
    The vendored rich metadata file. Defaults to `eng/metadata/fontawesome-7.3.1-free-icon-metadata.json`.
.PARAMETER OutputPath
    The generated embedded resource. Defaults to
    `src/Jenin.FontAwesome.Blazor.IconPicker/Resources/fontawesome-7.3.1-free-icon-metadata.json`.
.PARAMETER Verify
    When set, performs no writes and exits with a non-zero code if the committed resource differs from the
    freshly generated output.
.EXAMPLE
    pwsh ./eng/generate-icon-metadata.ps1
    pwsh ./eng/generate-icon-metadata.ps1 -Verify
#>
[CmdletBinding()]
param(
    [string] $MetadataPath,
    [string] $OutputPath,
    [switch] $Verify
)

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot

if(-not $MetadataPath) {
    $MetadataPath = Join-Path $root 'eng/metadata/fontawesome-7.3.1-free-icon-metadata.json'
}

if(-not $OutputPath) {
    $OutputPath = Join-Path $root 'src/Jenin.FontAwesome.Blazor.IconPicker/Resources/fontawesome-7.3.1-free-icon-metadata.json'
}

$metadata = Get-Content -LiteralPath $MetadataPath -Raw | ConvertFrom-Json -AsHashtable

$icons = [System.Collections.Generic.List[object]]::new()

foreach($icon in @($metadata.icons)) {
    $styles = [System.Collections.Generic.List[string]]::new()

    foreach($style in @($icon.styles)) {
        $styles.Add([string]$style)
    }

    $terms = [System.Collections.Generic.List[string]]::new()

    foreach($term in @($icon.terms)) {
        $terms.Add([string]$term)
    }

    $aliases = [System.Collections.Generic.List[string]]::new()

    foreach($alias in @($icon.aliases)) {
        $aliases.Add([string]$alias)
    }

    $record = [ordered]@{
        n = [string]$icon.name
        l = [string]$icon.label
        s = @($styles.ToArray())
    }

    if($terms.Count -gt 0) {
        $record.t = @($terms.ToArray())
    }

    if($aliases.Count -gt 0) {
        $record.a = @($aliases.ToArray())
    }

    $icons.Add($record)
}

$document = [ordered]@{
    v           = [string]$metadata.version
    license     = 'CC BY 4.0'
    attribution = "Font Awesome Free $($metadata.version) by @fontawesome - https://fontawesome.com. License - https://fontawesome.com/license/free (Icons: CC BY 4.0). Copyright Fonticons, Inc."
    icons       = @($icons.ToArray())
}

$content = ($document | ConvertTo-Json -Compress -Depth 8) + "`n"

if($Verify) {
    if(-not (Test-Path -LiteralPath $OutputPath)) {
        Write-Host "Missing generated icon metadata resource: $OutputPath"
        exit 1
    }

    $existing = [System.IO.File]::ReadAllText($OutputPath)

    if($existing -ne $content) {
        Write-Host 'The icon metadata resource is out of date. Run: pwsh ./eng/generate-icon-metadata.ps1'
        exit 1
    }

    Write-Host 'Icon metadata resource is up to date.'
    exit 0
}

$outputDirectory = Split-Path -Parent $OutputPath

if(-not (Test-Path -LiteralPath $outputDirectory)) {
    New-Item -ItemType Directory -Path $outputDirectory -Force | Out-Null
}

[System.IO.File]::WriteAllText($OutputPath, $content, (New-Object System.Text.UTF8Encoding($false)))

Write-Host "Wrote $OutputPath (Font Awesome $($metadata.version); $($icons.Count) icons, $((Get-Item -LiteralPath $OutputPath).Length) bytes)"
