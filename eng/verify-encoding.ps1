#!/usr/bin/env pwsh
#
# Encoding guard.
#
# `dotnet format` does not apply the .editorconfig charset/end_of_line/insert_final_newline rules to
# .razor (and does not fully enforce them for some generated files), which previously let 41 files
# drift. This script enforces the governed outcome for every source file in the repository:
#
#   * UTF-8 with BOM
#   * CRLF line endings (no bare LF)
#   * exactly one trailing final newline
#
# Usage: pwsh ./eng/verify-encoding.ps1
#        pwsh ./eng/verify-encoding.ps1 -Fix   # rewrite offending files in place

[CmdletBinding()]
param(
    [switch]$Fix
)

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot

$governedExtensions = @('.cs', '.csx', '.vb', '.vbx', '.razor')
$scanRoots = @('src', 'samples', 'tests')
$skipDirectories = @('bin', 'obj')

function Test-Violation {
    param([string]$Path)

    $bytes = [System.IO.File]::ReadAllBytes($Path)
    $hasBom = $bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF
    $text = [System.Text.Encoding]::UTF8.GetString($bytes)
    $withoutCrlf = $text.Replace("`r`n", [string]::Empty)
    $hasCrlf = $text.Contains("`r`n")
    $hasBareLf = $withoutCrlf.Contains("`n")
    $hasFinalNewline = $text.EndsWith("`n")

    return -not ($hasBom -and $hasCrlf -and (-not $hasBareLf) -and $hasFinalNewline)
}

function Repair-File {
    param([string]$Path)

    $text = [System.IO.File]::ReadAllText($Path)
    $text = $text.Replace("`r`n", "`n").Replace("`n", "`r`n").TrimEnd("`r", "`n") + "`r`n"
    $utf8Bom = New-Object System.Text.UTF8Encoding($true)
    [System.IO.File]::WriteAllText($Path, $text, $utf8Bom)
}

$files = foreach($scanRoot in $scanRoots) {
    $fullRoot = Join-Path $root $scanRoot

    if(Test-Path -LiteralPath $fullRoot) {
        Get-ChildItem -LiteralPath $fullRoot -Recurse -File |
            Where-Object {
                if($governedExtensions -notcontains $_.Extension) {
                    return $false
                }

                $segments = $_.FullName -split '[\\/]'

                return -not ($segments | Where-Object { $skipDirectories -contains $_ })
            }
    }
}

$checked = 0
$violations = @()

foreach($file in $files) {
    $checked++

    if(Test-Violation -Path $file.FullName) {
        $violations += $file.FullName

        if($Fix) {
            Repair-File -Path $file.FullName
        }
    }
}

if($violations.Count -gt 0) {
    if($Fix) {
        Write-Host "Repaired $($violations.Count) file(s):"
        $violations | ForEach-Object { Write-Host "  $_" }
        exit 0
    }

    Write-Host "Encoding violations ($($violations.Count) of $checked governed files):"
    $violations | ForEach-Object { Write-Host "  $_" }
    Write-Host 'Every governed file must be UTF-8 BOM + CRLF + a single final newline. Run: pwsh ./eng/verify-encoding.ps1 -Fix'
    exit 1
}

Write-Host "Encoding check passed: $checked governed files are UTF-8 BOM + CRLF + final newline."
