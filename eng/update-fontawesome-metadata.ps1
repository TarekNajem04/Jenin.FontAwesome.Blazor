#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Refreshes the vendored, compact Font Awesome Free icon-name metadata.
.DESCRIPTION
    Downloads the official Font Awesome `metadata/icons.json` for a given release and derives two small,
    reviewable files:

      * a compact per-style canonical name list consumed by `eng/generate-icon-catalogue.ps1`; and
      * a richer per-icon metadata file (label, available Free styles, search terms, aliases) consumed by
        `eng/generate-icon-metadata.ps1`.

    Both derive from a single download of the multi-megabyte upstream JSON. Run this script only when
    intentionally upgrading the vendored Font Awesome version, then regenerate the catalogue and the
    picker metadata and review the diffs.

    Font Awesome Free icons are licensed under CC BY 4.0 (https://fontawesome.com/license/free); the
    attribution is recorded in the produced files.
.PARAMETER Version
    The Font Awesome release tag to vendor (default `7.3.1`).
.PARAMETER OutputPath
    The destination compact name-list file. Defaults to
    `eng/metadata/fontawesome-<Version>-free-icons.json`.
.PARAMETER MetadataOutputPath
    The destination rich per-icon metadata file. Defaults to
    `eng/metadata/fontawesome-<Version>-free-icon-metadata.json`.
.EXAMPLE
    pwsh ./eng/update-fontawesome-metadata.ps1
    pwsh ./eng/update-fontawesome-metadata.ps1 -Version 7.3.1
#>
[CmdletBinding()]
param(
    [string] $Version = '7.3.1',
    [string] $OutputPath,
    [string] $MetadataOutputPath
)

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot

if(-not $OutputPath) {
    $OutputPath = Join-Path $root "eng/metadata/fontawesome-$Version-free-icons.json"
}

if(-not $MetadataOutputPath) {
    $MetadataOutputPath = Join-Path $root "eng/metadata/fontawesome-$Version-free-icon-metadata.json"
}

$url = "https://raw.githubusercontent.com/FortAwesome/Font-Awesome/$Version/metadata/icons.json"
$temp = Join-Path ([System.IO.Path]::GetTempPath()) "fontawesome-$Version-icons.json"

Write-Host "Downloading $url"
Invoke-WebRequest -Uri $url -OutFile $temp -TimeoutSec 240

$icons = Get-Content -LiteralPath $temp -Raw | ConvertFrom-Json -AsHashtable

$styles = [ordered]@{}

foreach($style in 'solid', 'regular', 'brands') {
    $names = [System.Collections.Generic.List[string]]::new()

    foreach($key in $icons.Keys) {
        if($icons[$key].free -contains $style) {
            $names.Add($key)
        }
    }

    $names.Sort([System.StringComparer]::Ordinal)
    $styles[$style] = @($names)
}

$document = [ordered]@{
    version = $Version
    source  = "https://github.com/FortAwesome/Font-Awesome/blob/$Version/metadata/icons.json"
    license = 'Icons: CC BY 4.0 (https://fontawesome.com/license/free)'
    note    = 'Compact derived list of Font Awesome Free canonical icon class names per style. Regenerate with eng/update-fontawesome-metadata.ps1.'
    styles  = $styles
}

$freeIconNames = [System.Collections.Generic.List[string]]::new()

foreach($key in $icons.Keys) {
    if(@($icons[$key].free).Count -gt 0) {
        $freeIconNames.Add($key)
    }
}

$freeIconNames.Sort([System.StringComparer]::Ordinal)

$metadataIcons = [System.Collections.Generic.List[object]]::new()

foreach($name in $freeIconNames) {
    $icon = $icons[$name]
    $free = @($icon.free)

    $availableStyles = [System.Collections.Generic.List[string]]::new()

    foreach($style in 'solid', 'regular', 'brands') {
        if($free -contains $style) {
            $availableStyles.Add($style)
        }
    }

    $terms = [System.Collections.Generic.List[string]]::new()

    foreach($term in @($icon.search.terms)) {
        if($null -ne $term) {
            $terms.Add([string]$term)
        }
    }

    $aliases = [System.Collections.Generic.List[string]]::new()

    if($icon.aliases) {
        foreach($alias in @($icon.aliases)) {
            foreach($aliasName in @($alias.names)) {
                if($null -ne $aliasName) {
                    $aliases.Add([string]$aliasName)
                }
            }
        }
    }

    $metadataIcons.Add([ordered]@{
        name    = [string]$name
        label   = [string]$icon.label
        styles  = @($availableStyles.ToArray())
        terms   = @($terms.ToArray())
        aliases = @($aliases.ToArray())
    })
}

$metadataDocument = [ordered]@{
    version = $Version
    source  = "https://github.com/FortAwesome/Font-Awesome/blob/$Version/metadata/icons.json"
    license = 'Icons: CC BY 4.0 (https://fontawesome.com/license/free)'
    note    = 'Compact derived Font Awesome Free icon metadata (label, available styles, search terms, aliases) for the IconPicker package. Regenerate with eng/update-fontawesome-metadata.ps1.'
    icons   = @($metadataIcons.ToArray())
}

$json = ($document | ConvertTo-Json -Depth 6)
$json = $json.TrimEnd("`r", "`n") + "`n"

$metadataJson = ($metadataDocument | ConvertTo-Json -Depth 6)
$metadataJson = $metadataJson.TrimEnd("`r", "`n") + "`n"

$outputDirectory = Split-Path -Parent $OutputPath

if(-not (Test-Path -LiteralPath $outputDirectory)) {
    New-Item -ItemType Directory -Path $outputDirectory -Force | Out-Null
}

$metadataOutputDirectory = Split-Path -Parent $MetadataOutputPath

if(-not (Test-Path -LiteralPath $metadataOutputDirectory)) {
    New-Item -ItemType Directory -Path $metadataOutputDirectory -Force | Out-Null
}

$utf8NoBom = New-Object System.Text.UTF8Encoding($false)

[System.IO.File]::WriteAllText($OutputPath, $json, $utf8NoBom)
[System.IO.File]::WriteAllText($MetadataOutputPath, $metadataJson, $utf8NoBom)

$counts = 'solid={0}, regular={1}, brands={2}' -f $styles.solid.Count, $styles.regular.Count, $styles.brands.Count

Write-Host "Wrote $OutputPath ($counts)"
Write-Host "Wrote $MetadataOutputPath ($($metadataIcons.Count) icons)"
