#!/usr/bin/env pwsh
#
# Font Awesome usage scanner.
#
# Scans .razor and .cs sources for statically known icon references and emits a JSON manifest
# consumed by eng/fa_subset.py. Dynamic references (IconName="@...") cannot be resolved and are
# reported as warnings — they are never silently dropped; add those icons to --ExtraIcons.
#
# Recognized patterns:
#   IconName="fa-..." / MaskIconName="fa-..." / [Icon("fa-...")]   -> literal name
#   IconNames.(Solid|Regular|Brands).Member                         -> name + style
#   <XxxIcon ... /> component tags (generated presets)              -> name + baked style
#   IconName="@..." (any @-expression)                             -> dynamic warning
#
# Usage: pwsh ./eng/Find-FontAwesomeUsage.ps1 -Path samples -Output eng/used-icons.json
#        pwsh ./eng/Find-FontAwesomeUsage.ps1 -Path samples -Output eng/used-icons.json -ExtraIcons fa-bell,fa-star

[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$Path,

    [Parameter(Mandatory)]
    [string]$Output,

    [string[]]$ExtraIcons = @()
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
$presetsPath = Join-Path $repoRoot 'src/Jenin.FontAwesome.Blazor.Icons/GeneratedIconComponents.cs'

# Map generated preset component name -> (css class, style) from the checked-in source.
$presetMap = @{}
if (Test-Path -LiteralPath $presetsPath) {
    $presetSource = Get-Content -LiteralPath $presetsPath -Raw
    $matches = [regex]::Matches($presetSource, 'class (\w+) : Icon\s*\{\s*[^}]*?IconName = "(fa-[a-z0-9-]+)";\s*[^}]*?IconStyle = IconStyle\.(\w+)', [System.Text.RegularExpressions.RegexOptions]::Singleline)

    foreach ($match in $matches) {
        $presetMap[$match.Groups[1].Value] = @{
            Name = $match.Groups[2].Value
            Style = $match.Groups[3].Value.ToLowerInvariant()
        }
    }
}

function ConvertTo-KebabCase([string]$value) {
    $stepped = [regex]::Replace($value, '(?<!^)([A-Z])', '-$1')
    return $stepped.ToLowerInvariant().TrimStart('-').TrimStart('_')
}

$names = @{}
$dynamic = [System.Collections.Generic.List[object]]::new()
$files = Get-ChildItem -LiteralPath $Path -Recurse -Include '*.razor', '*.cs' -File |
    Where-Object { $_.FullName -notmatch '\\(obj|bin)\\' }

foreach ($file in $files) {
    $text = Get-Content -LiteralPath $file.FullName -Raw
    $relative = [System.IO.Path]::GetRelativePath($Path, $file.FullName)

    # Literal names: IconName="fa-x", MaskIconName="fa-x", [Icon("fa-x")].
    # Styles accumulate per name; entries stay style-free when unknown.
    $addName = {
        param([string]$Name, [string]$Style = '')

        if (-not $names.ContainsKey($Name)) {
            $names[$Name] = [System.Collections.Generic.HashSet[string]]::new()
        }

        if ($Style -ne '') {
            $null = $names[$Name].Add($Style)
        }
    }

    foreach ($match in [regex]::Matches($text, '(?:IconName|MaskIconName)\s*=\s*"(fa-[a-z0-9-]+)"')) {
        & $addName $match.Groups[1].Value
    }

    foreach ($match in [regex]::Matches($text, '\[Icon\("(fa-[a-z0-9-]+)"')) {
        & $addName $match.Groups[1].Value
    }

    # Typed catalogue references carry their style: IconNames.Solid.House.
    foreach ($match in [regex]::Matches($text, 'IconNames\.(Solid|Regular|Brands)\.([A-Za-z0-9_]+)')) {
        & $addName "fa-$(ConvertTo-KebabCase $match.Groups[2].Value)" $match.Groups[1].Value.ToLowerInvariant()
    }

    # Generated preset components: <HouseIcon .../> -> name + baked style.
    foreach ($match in [regex]::Matches($text, '<([A-Z][A-Za-z0-9]*)Icon[\s/>]')) {
        $component = "$($match.Groups[1].Value)Icon"

        if ($presetMap.ContainsKey($component)) {
            $entry = $presetMap[$component]
            & $addName $entry.Name $entry.Style
        }
    }

    # Dynamic references cannot be resolved statically: warn, never silently drop.
    foreach ($match in [regex]::Matches($text, '(?:IconName|MaskIconName)\s*=\s*"@')) {
        $line = ($text.Substring(0, $match.Index) -split "`n").Count
        $dynamic.Add(@{ File = $relative; Line = $line })
    }
}

foreach ($extra in $ExtraIcons) {
    $clean = $extra.Trim().ToLowerInvariant()

    if ($clean -ne '') {
        if (-not $names.ContainsKey($clean)) {
            $names[$clean] = [System.Collections.Generic.HashSet[string]]::new()
        }
    }
}

$entries = @(foreach ($key in ($names.Keys | Sort-Object)) {
    [ordered]@{ name = $key; styles = @($names[$key] | Sort-Object) }
})

$result = [ordered]@{
    version = '1'
    scanned = $Path
    names = @($entries)
    dynamic = @($dynamic)
}

$directory = Split-Path -Parent $Output

if ($directory -ne '' -and -not (Test-Path -LiteralPath $directory)) {
    New-Item -ItemType Directory -Path $directory | Out-Null
}

$result | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $Output -Encoding utf8NoBOM

$unresolved = $dynamic.Count
Write-Host "Used icons: $($entries.Count); dynamic references needing review: $unresolved -> $Output"
