#!/usr/bin/env pwsh
#
# Generates the AI-consumer entry point llms.txt (plus verification).
#
# llms.txt follows the llmstxt.org shape: H1 title, blockquote summary, H2 sections that link
# the detailed docs (the per-section "files"). Every link is verified to exist, so renaming or
# deleting a documented page fails CI instead of silently rotting the AI entry point.
#
# Links are repository-relative so they resolve both on GitHub and on the docs site (which
# mirrors these files); the generated API reference uses its canonical published URL.
#
# Usage:
#   pwsh ./eng/generate-llms-txt.ps1
#   pwsh ./eng/generate-llms-txt.ps1 -Verify
#
# Convention: when adding a documented surface, add its section
# to the $sections table below so it stays discoverable to AI consumers.

[CmdletBinding()]
param(
    [switch]$Verify
)

$ErrorAction = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$outPath = Join-Path $root 'llms.txt'

$sections = @(
    @{
        Title = 'Getting started'
        Body = @(
            '- [Installation and one-line setup](README.md#installation): install the package, wire assets with `AddJeninFontAwesome` + `MapJeninFontAwesomeAssets` + `<FontAwesomeHeadAssets/>`, import namespaces.'
            '- [Project templates](README.md#project-templates): `dotnet new jenin-fa-blazor` (Web App) and `jenin-fa-blazor-wasm` (standalone WebAssembly).'
            '- [Switching from another library](README.md#switching-from-another-library): MudBlazor, Blazorise, EasyAppDev, Blazicons, raw FA classes.'
        )
    },
    @{
        Title = 'Core rendering model'
        Body = @(
            '- `<Icon IconName="fa-house" IconStyle="IconStyle.Solid" />` is the core component; `IconNames.Solid.House` constants and `Jenin.FontAwesome.Blazor.Icons` preset components (`<HouseIcon />`) avoid magic strings.'
            '- SVG+JS (nest) is the default full-feature mode: duotone, layers, masks, power transforms, animations. Webfont mode renders through the typeface; engine-only tokens stay inert (see the README feature matrix).'
            '- Accessibility: bare icons are decorative (`aria-hidden`); add `Title`/`AriaLabel` to make an icon meaningful (`role="img"`).'
        )
    },
    @{
        Title = 'Cookbook (copy-paste recipes, each e2e-verified)'
        Body = @(
            '- [Cookbook page](samples/Jenin.FontAwesome.Blazor.Sample.Shared/Pages/Cookbook/Cookbook.razor): 15 recipes with live demos; each snippet file next to it is the copyable source.'
            '- Busy button, notification badge, empty state, icon list, stat card, animated trigger, duotone theme, accessible icon, icon toggle, avatar with status dot, star rating input, animated two-state, share buttons, timeline list, composed toast.'
            '- [Gallery of composed screens](samples/Jenin.FontAwesome.Blazor.Sample.Shared/Pages/Gallery/Gallery.razor): admin dashboard, notification center, file/activity timeline, settings with toggles, inbox.'
        )
    },
    @{
        Title = 'Migration guides'
        Body = @(
            '- [Migration index](docs/migration/index.md): each guide has an API mapping table, before/after diffs, deliberate differences, and a checklist.'
            '- From [MudBlazor](docs/migration/from-mudblazor.md), [Blazorise](docs/migration/from-blazorise.md), [EasyAppDev FA5/FA6](docs/migration/from-easyappdev.md), [Blazicons](docs/migration/from-blazicons.md), [raw Font Awesome classes](docs/migration/from-raw-fontawesome.md).'
        )
    },
    @{
        Title = 'Custom icons and assets'
        Body = @(
            '- [Custom icons](docs/custom-icons.md): register glyphs once with `RegisterCustomIcon` and use `<Icon>`; no FA assets required for rendering.'
            '- [Pro setup](docs/pro-setup.md): point the one-line setup at a kit or self-hosted Pro files; duotone/light/thin/sharp need licensed assets for full rendering.'
            '- [Kits](docs/hosting-with-kits.md): `UseFontAwesomeKit(code)`; custom kit glyphs resolve at runtime; suppress JFA1001 per usage with a justification.'
            '- [Trimming and bundle sizes](README.md#bundle-size-dashboard): generated per-mode numbers; `Svg` and `Icons` packages trim unused glyphs.'
        )
    },
    @{
        Title = 'UI suites and diagnostics'
        Body = @(
            '- [Using with UI suites](docs/using-with-ui-suites.md): `JeninMudIcon`/`MudIconMapper` for MudBlazor slots, `JeninBlazoriseIcon`/`BlazoriseIconMapper` for Blazorise; unmapped options fall back to each suite default.'
            '- [Analyzer diagnostics](docs/diagnostics.md): JFA1001 unknown name (Warning), JFA1002 raw class string in a name slot with code fix (Warning), JFA1003 Pro-only style without licensed assets (Warning). Tune via `dotnet_diagnostic.JFA100x.severity`.'
            '- [Generated API reference](https://tareknajem04.github.io/Jenin.FontAwesome.Blazor/api/): full public surface with XML documentation.'
        )
    }
)

function Get-ReferenceTargets([string]$line) {
    $targets = @()
    foreach($match in [regex]::Matches($line, '\]\(([^)]+)\)')) {
        $target = $match.Groups[1].Value
        if($target -notmatch '^(https?://|#|mailto:)') {
            $targets += $target.Split('#')[0]
        }
    }
    return $targets
}

$lines = @(
    '# Jenin.FontAwesome.Blazor'
    ''
    '> Free Font Awesome 7 components for Blazor: typed icons, animations, transforms, layers,'
    '> duotone themes, sprites, subsetting, inline SVG, an icon picker, UI-suite adapters, and'
    '> custom-glyph registration. Start with the setup below; follow links for depth.'
    ''
)

foreach($section in $sections) {
    $lines += "## $($section.Title)"
    $lines += ''
    foreach($line in $section.Body) {
        $lines += $line
        foreach($target in Get-ReferenceTargets $line) {
            $full = Join-Path $root $target
            if(-not (Test-Path -LiteralPath $full)) {
                throw "llms.txt references a missing file: $target"
            }
        }
    }
    $lines += ''
}

$lines += '_Generated by eng/generate-llms-txt.ps1 from the table above. Do not edit by hand._'
$content = ($lines -join "`r`n") + "`r`n"

if($Verify) {
    if(-not (Test-Path -LiteralPath $outPath)) {
        Write-Error "llms.txt is missing. Run pwsh ./eng/generate-llms-txt.ps1."
    }

    $current = [System.IO.File]::ReadAllText($outPath)
    if($current -ne $content) {
        Write-Error 'llms.txt is out of date (or references a missing file). Run pwsh ./eng/generate-llms-txt.ps1.'
    }

    Write-Host 'llms.txt is up to date.'
    return
}

[System.IO.File]::WriteAllText($outPath, $content)
Write-Host ("Wrote {0}." -f $outPath)
