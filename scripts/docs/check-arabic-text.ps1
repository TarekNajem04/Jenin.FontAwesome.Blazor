#requires -Version 7
<#
.SYNOPSIS
  Verifies that Arabic documentation contains no letters from other scripts and no
  replacement or hidden direction-control characters.

.DESCRIPTION
  ASCII text (code identifiers, links, English terms), Latin-script letters used in examples, and
  Arabic letters/marks are expected. Letters from Cyrillic, Greek, Hebrew, CJK, and other scripts
  usually indicate a bad paste or an encoding conversion. U+FFFD and hidden bidi controls are also
  rejected.

.PARAMETER RepositoryRoot
  Repository root. Defaults to two levels above this script.

.OUTPUTS
  Prints a summary. Exits 1 when a suspicious character is found.

.EXAMPLE
  pwsh scripts/docs/check-arabic-text.ps1
#>
param(
    [string]$RepositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path
)

$ErrorActionPreference = 'Stop'

$arabicRanges = @(
    @(0x0600, 0x06FF),
    @(0x0750, 0x077F),
    @(0x0870, 0x089F),
    @(0x08A0, 0x08FF),
    @(0xFB50, 0xFDFF),
    @(0xFE70, 0xFEFF)
)
$latinRanges = @(
    @(0x00C0, 0x024F),
    @(0x1E00, 0x1EFF),
    @(0x2C60, 0x2C7F),
    @(0xA720, 0xA7FF)
)
$hiddenControls = @(0x200B, 0x200E, 0x200F, 0xFEFF) + (0x202A..0x202E) + (0x2066..0x2069)

function Test-ArabicCodePoint([int]$CodePoint) {
    foreach ($range in $arabicRanges) {
        if ($CodePoint -ge $range[0] -and $CodePoint -le $range[1]) { return $true }
    }
    return $false
}

function Test-LatinCodePoint([int]$CodePoint) {
    foreach ($range in $latinRanges) {
        if ($CodePoint -ge $range[0] -and $CodePoint -le $range[1]) { return $true }
    }
    return $false
}

$arabicRoot = Join-Path $RepositoryRoot 'docs/ar'
if (-not (Test-Path -LiteralPath $arabicRoot)) {
    Write-Output 'Arabic text check: docs/ar does not exist; nothing to scan.'
    exit 0
}

$files = @(Get-ChildItem -LiteralPath $arabicRoot -Recurse -Filter '*.md' -File)
$violations = [System.Collections.Generic.List[object]]::new()
$lineCount = 0

foreach ($file in $files) {
    $relativePath = [System.IO.Path]::GetRelativePath($RepositoryRoot, $file.FullName)
    $lines = [System.IO.File]::ReadAllLines($file.FullName)
    $lineCount += $lines.Length

    for ($lineIndex = 0; $lineIndex -lt $lines.Length; $lineIndex++) {
        $line = $lines[$lineIndex]
        for ($column = 0; $column -lt $line.Length; $column++) {
            $codePoint = [int]$line[$column]
            if ($codePoint -lt 128) { continue }

            $category = [System.Globalization.CharUnicodeInfo]::GetUnicodeCategory($line[$column])
            $isLetter = $category -in @(
                [System.Globalization.UnicodeCategory]::UppercaseLetter,
                [System.Globalization.UnicodeCategory]::LowercaseLetter,
                [System.Globalization.UnicodeCategory]::TitlecaseLetter,
                [System.Globalization.UnicodeCategory]::ModifierLetter,
                [System.Globalization.UnicodeCategory]::OtherLetter
            )
            $isSurrogate = $category -eq [System.Globalization.UnicodeCategory]::Surrogate
            $isReplacement = $codePoint -eq 0xFFFD
            $isHiddenControl = $hiddenControls -contains $codePoint

            if ($isReplacement -or $isHiddenControl -or (($isLetter -or $isSurrogate) -and
                    -not (Test-ArabicCodePoint $codePoint) -and -not (Test-LatinCodePoint $codePoint))) {
                $context = $line.Trim()
                if ($context.Length -gt 90) { $context = $context.Substring(0, 90) + '...' }
                [void]$violations.Add([pscustomobject]@{
                        File    = $relativePath
                        Line    = $lineIndex + 1
                        Column  = $column + 1
                        Code    = 'U+{0:X4}' -f $codePoint
                        Kind    = if ($isReplacement) { 'replacement' }
                                  elseif ($isHiddenControl) { 'hidden-control' }
                                  else { 'foreign-letter' }
                        Context = $context
                    })
            }
        }
    }
}

if ($violations.Count -gt 0) {
    $violations | Format-Table -AutoSize | Out-String -Width 240 | Write-Output
    Write-Output "Arabic text check FAILED: $($violations.Count) suspicious character(s) in $($files.Count) file(s)."
    exit 1
}

Write-Output "Arabic text check OK: $($files.Count) file(s), $lineCount line(s); no foreign-script letters, replacement characters, or hidden controls."
