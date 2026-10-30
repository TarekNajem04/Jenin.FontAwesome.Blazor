#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deterministic developer entry point for Jenin.FontAwesome.Blazor.
.DESCRIPTION
    Wraps the common restore/build/format/pack/test/clean tasks so that local development and CI use
    the same commands. The script pins the working directory to the repository root, and the .NET SDK
    is selected by global.json, keeping results reproducible.
.PARAMETER Target
    The task to run: Restore, Build (default), Format, Pack, Test, or Clean.
.PARAMETER Configuration
    The build configuration: Debug (default) or Release.
.EXAMPLE
    ./build.ps1
    ./build.ps1 -Configuration Release
    ./build.ps1 -Target Format
#>
[CmdletBinding()]
param(
    [ValidateSet('Restore', 'Build', 'Format', 'Pack', 'Test', 'Clean')]
    [string] $Target = 'Build',

    [ValidateSet('Debug', 'Release')]
    [string] $Configuration = 'Debug'
)

$ErrorActionPreference = 'Stop'
Set-Location -LiteralPath $PSScriptRoot

$solution = 'Jenin.FontAwesome.Blazor.slnx'
$library = 'src/Jenin.FontAwesome.Blazor/Jenin.FontAwesome.Blazor.csproj'
$iconPickerLibrary = 'src/Jenin.FontAwesome.Blazor.IconPicker/Jenin.FontAwesome.Blazor.IconPicker.csproj'
$iconsLibrary = 'src/Jenin.FontAwesome.Blazor.Icons/Jenin.FontAwesome.Blazor.Icons.csproj'
$svgLibrary = 'src/Jenin.FontAwesome.Blazor.Svg/Jenin.FontAwesome.Blazor.Svg.csproj'
$analyzersLibrary = 'src/Jenin.FontAwesome.Blazor.Analyzers/Jenin.FontAwesome.Blazor.Analyzers.csproj'
$mudBlazorLibrary = 'src/Jenin.FontAwesome.Blazor.MudBlazor/Jenin.FontAwesome.Blazor.MudBlazor.csproj'
$blazoriseLibrary = 'src/Jenin.FontAwesome.Blazor.Blazorise/Jenin.FontAwesome.Blazor.Blazorise.csproj'
$templatesPack = 'templates/Jenin.FontAwesome.Blazor.Templates.csproj'

switch ($Target) {
    'Restore' {
        dotnet restore $solution
    }
    'Build' {
        dotnet build $solution -c $Configuration
    }
    'Format' {
        dotnet format $solution --verify-no-changes
    }
    'Pack' {
dotnet pack $library -c Release
dotnet pack $iconPickerLibrary -c Release
dotnet pack $iconsLibrary -c Release
dotnet pack $svgLibrary -c Release
dotnet pack $analyzersLibrary -c Release
dotnet pack $mudBlazorLibrary -c Release
dotnet pack $blazoriseLibrary -c Release
dotnet pack $templatesPack -c Release
    }
    'Test' {
        $testRoots = @('src', 'tests') | Where-Object { Test-Path -LiteralPath $_ }
        $testProjects = Get-ChildItem -Path $testRoots -Recurse -Filter '*.csproj' |
            Where-Object { $_.BaseName -like '*Test*' }
        if (-not $testProjects) {
            Write-Host 'No test project found; nothing to test yet.'
            return
        }
        dotnet test $solution -c $Configuration
    }
    'Clean' {
        dotnet clean $solution -c $Configuration
    }
}
