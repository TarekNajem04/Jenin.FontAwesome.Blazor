<#
.SYNOPSIS
    Shared PowerShell utilities for Jenin FontAwesome repository scripts.

.DESCRIPTION
    Contains reusable functions shared by repository automation scripts.

.NOTES
    This module intentionally contains only common functionality
    used by multiple scripts.
#>

Set-StrictMode -Version Latest


function Write-ToolBanner {
    param(
        [Parameter(Mandatory)]
        [string]$Name,

        [string]$Version = "1.0"
    )

    Write-Host ""
    Write-Host "=============================================" -ForegroundColor Cyan
    Write-Host " Jenin FontAwesome Repository Tools" -ForegroundColor Cyan
    Write-Host "=============================================" -ForegroundColor Cyan
    Write-Host ""

    Write-Host "Tool    : $Name"
    Write-Host "Version : $Version"
    Write-Host ""
}


function Write-Section {
    param(
        [Parameter(Mandatory)]
        [string]$Message
    )

    Write-Host ""
    Write-Host "====== $Message ======" -ForegroundColor Cyan
}


function Write-Step {
    param(
        [Parameter(Mandatory)]
        [string]$Message,

        [ConsoleColor]$Color = [ConsoleColor]::Yellow
    )

    Write-Host $Message -ForegroundColor $Color
}


function Find-RepositoryRoot {
    param(
        [Parameter(Mandatory)]
        [string]$StartPath
    )

    $current = Get-Item -LiteralPath $StartPath

    while ($current) {

        $solution =
            Get-ChildItem `
                -Path $current.FullName `
                -Filter "*.sln*" `
                -File `
                -ErrorAction SilentlyContinue

        if ($solution) {
            return $current.FullName
        }

        $current = $current.Parent
    }

    throw @"
Unable to locate repository root.

No solution file (*.sln or *.slnx) was found.
The script must be executed inside a .NET solution directory.
"@
}


function Read-JsonConfiguration {
    param(
        [Parameter(Mandatory)]
        [string]$Path
    )

    if (-not (Test-Path -LiteralPath $Path)) {
        throw "Configuration file not found: $Path"
    }

    return Get-Content `
        -LiteralPath $Path `
        -Raw |
        ConvertFrom-Json
}


function Confirm-Action {
    param(
        [Parameter(Mandatory)]
        [string]$Message
    )

    $answer = Read-Host "$Message [y/N]"

    return $answer -match '^(y|yes)$'
}


function Write-Summary {
    param(
        [Parameter(Mandatory)]
        [hashtable]$Items
    )

    Write-Host ""
    Write-Host "=============================================" -ForegroundColor Cyan
    Write-Host "Summary"
    Write-Host "---------------------------------------------"

    foreach ($item in $Items.GetEnumerator()) {
        Write-Host ("{0,-15}: {1}" -f $item.Key, $item.Value)
    }

    Write-Host "============================================="
}
