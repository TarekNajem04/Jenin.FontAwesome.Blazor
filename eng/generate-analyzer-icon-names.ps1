#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Generates the embedded known-icon-name list for the analyzer package.
.DESCRIPTION
    Reads the vendored rich metadata (`eng/metadata/fontawesome-7.3.1-free-icon-metadata.json`)
    and emits `KnownIconNames.txt`: one icon name per line, bare and `fa-`-prefixed forms,
    canonical names plus Font Awesome aliases, in ordinal order. The analyzer
    (`src/Jenin.FontAwesome.Blazor.Analyzers`) embeds this file and uses it as the JFA1001
    known-name source, so the diagnostic can never drift from the vendored catalogue.

    The generator is deterministic and supports a `-Verify` mode used by CI to fail when the
    committed file drifts from the vendored metadata.
.PARAMETER MetadataPath
    The vendored rich metadata file. Defaults to `eng/metadata/fontawesome-7.3.1-free-icon-metadata.json`.
.PARAMETER StylesPath
    The vendored per-style metadata file. Defaults to `eng/metadata/fontawesome-7.3.1-free-icons.json`.
.PARAMETER OutputPath
    The generated name list. Defaults to
    `src/Jenin.FontAwesome.Blazor.Analyzers/KnownIconNames.txt`.
.PARAMETER Verify
    When set, performs no writes and exits with a non-zero code if the committed file differs from the
    freshly generated output.
.EXAMPLE
    pwsh ./eng/generate-analyzer-icon-names.ps1
    pwsh ./eng/generate-analyzer-icon-names.ps1 -Verify
#>
[CmdletBinding()]
param(
    [string] $MetadataPath,
    [string] $StylesPath,
    [string] $OutputPath,
    [switch] $Verify
)

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot

if(-not $MetadataPath) {
    $MetadataPath = Join-Path $root 'eng/metadata/fontawesome-7.3.1-free-icon-metadata.json'
}

if(-not $StylesPath) {
    $StylesPath = Join-Path $root 'eng/metadata/fontawesome-7.3.1-free-icons.json'
}

$analyzerDir = Join-Path $root 'src' 'Jenin.FontAwesome.Blazor.Analyzers'

if(-not $OutputPath) {
    $OutputPath = Join-Path $analyzerDir 'KnownIconNames.txt'
}

$StylesOutputPath = Join-Path $analyzerDir 'KnownIconStyles.txt'
$AliasesOutputPath = Join-Path $analyzerDir 'KnownIconAliases.txt'

function Get-FileBytes([string[]] $Lines) {
    $content = ($Lines -join "`r`n") + "`r`n"
    return [System.Text.Encoding]::GetEncoding('utf-8').GetBytes($content)
}

function Test-FileBytes([string] $Path, [byte[]] $Bytes) {
    if(-not (Test-Path -LiteralPath $Path)) {
        Write-Error "Generated file is missing: $Path. Run pwsh ./eng/generate-analyzer-icon-names.ps1."
    }

    $current = [System.IO.File]::ReadAllBytes($Path)
    if($current.Length -ne $Bytes.Length) {
        return $false
    }

    for($i = 0; $i -lt $current.Length; $i++) {
        if($current[$i] -ne $Bytes[$i]) { return $false }
    }

    return $true
}

$metadata = Get-Content -LiteralPath $MetadataPath -Raw | ConvertFrom-Json
$styles = Get-Content -LiteralPath $StylesPath -Raw | ConvertFrom-Json

$names = New-Object System.Collections.Generic.HashSet[string]
$aliasNameLines = New-Object System.Collections.Generic.HashSet[string]

foreach($icon in $metadata.icons) {
    [void]$names.Add($icon.name)
    [void]$names.Add(('fa-' + $icon.name))
    foreach($alias in $icon.aliases) {
        $aliasName = if($alias -is [string]) { $alias } else { $alias.name }
        if(-not [string]::IsNullOrEmpty($aliasName)) {
            [void]$aliasNameLines.Add('fa-' + $aliasName + '=' + 'fa-' + $icon.name)
            [void]$names.Add($aliasName)
            [void]$names.Add(('fa-' + $aliasName))
        }
    }
}

$styleLines = New-Object System.Collections.Generic.List[string]
foreach($property in $styles.styles.PSObject.Properties) {
    foreach($name in $property.Value) {
        $styleLines.Add(($property.Name + ':fa-' + $name))
    }
}

$sortedNames = $names | Sort-Object -Culture '' -CaseSensitive
$sortedStyles = $styleLines | Sort-Object -Culture '' -CaseSensitive -Unique
$sortedAliases = $aliasNameLines | Sort-Object -Culture '' -CaseSensitive

$nameBytes = Get-FileBytes $sortedNames
$styleBytes = Get-FileBytes $sortedStyles
$aliasBytes = Get-FileBytes $sortedAliases

if($Verify) {
    $ok = (Test-FileBytes $OutputPath $nameBytes) `
        -and (Test-FileBytes $StylesOutputPath $styleBytes) `
        -and (Test-FileBytes $AliasesOutputPath $aliasBytes)

    if(-not $ok) {
        Write-Error 'Analyzer catalogue files are out of date. Run pwsh ./eng/generate-analyzer-icon-names.ps1.'
    }

    Write-Host ("Analyzer catalogue files are up to date ({0} names, {1} style entries, {2} aliases)." -f $sortedNames.Count, $sortedStyles.Count, $sortedAliases.Count)
    return
}

[System.IO.File]::WriteAllBytes($OutputPath, $nameBytes)
[System.IO.File]::WriteAllBytes($StylesOutputPath, $styleBytes)
[System.IO.File]::WriteAllBytes($AliasesOutputPath, $aliasBytes)
Write-Host ("Wrote {0} ({1} names), {2} ({3} style entries), {4} ({5} aliases)." -f $OutputPath, $sortedNames.Count, $StylesOutputPath, $sortedStyles.Count, $AliasesOutputPath, $sortedAliases.Count)
