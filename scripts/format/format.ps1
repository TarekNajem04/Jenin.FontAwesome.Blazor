<#
.SYNOPSIS
    Runs dotnet format commands for the repository.

.DESCRIPTION
    Executes configured dotnet format operations based on format.config.json.

    The script runs in preview mode by default.
    Use -Execute to apply formatting changes.

.PARAMETER Execute
    Executes formatting commands.

.PARAMETER Verify
    Runs dotnet format in verification mode.
    Useful for CI/CD pipelines.

.PARAMETER Configuration
    Specifies the format configuration file.

.PARAMETER Solution
    Specifies a solution file manually.
    If omitted, the script searches automatically.

.EXAMPLE
    .\format.ps1

    Shows the formatting operations that would run.

.EXAMPLE
    .\format.ps1 -Execute

    Applies formatting changes.

.EXAMPLE
    .\format.ps1 -Verify

    Verifies that the solution is already formatted.

.EXAMPLE
    .\format.ps1 -Solution Jenin.FontAwesome.Blazor.slnx -Execute

    Formats a specific solution.

.NOTES
    Author:
        Tarek Najem

    Tool:
        Jenin FontAwesome Repository Format Tool
#>


[CmdletBinding()]
param(

    [switch]
    $Execute,


    [switch]
    $Verify,


    [string]
    $Configuration = (Join-Path $PSScriptRoot "format.config.json"),


    [string]
    $Solution
)


Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"



function Write-Banner {

    Write-Host ""
    Write-Host "=============================================" -ForegroundColor Cyan
    Write-Host " Jenin FontAwesome Repository Format Tool" -ForegroundColor Cyan
    Write-Host "=============================================" -ForegroundColor Cyan
    Write-Host ""
}



function Find-RepositoryRoot {

    param(
        [Parameter(Mandatory)]
        [string]$StartPath
    )


    $current = (Get-Item -LiteralPath $StartPath).FullName


    while ($true) {

        Write-Verbose "Checking: $current"


        $solutionFile = Get-ChildItem `
            -Path $current `
            -File `
            -Filter "*.sln" `
            -ErrorAction SilentlyContinue |
            Select-Object -First 1


        if (-not $solutionFile) {

            $solutionFile = Get-ChildItem `
                -Path $current `
                -File `
                -Filter "*.slnx" `
                -ErrorAction SilentlyContinue |
                Select-Object -First 1
        }


        if ($solutionFile) {

            Write-Verbose "Solution found: $($solutionFile.FullName)"

            return @{
                Root = $current
                Solution = $solutionFile.FullName
            }
        }


        $parent = [System.IO.Directory]::GetParent($current)


        if ($null -eq $parent) {
            break
        }


        $current = $parent.FullName
    }


    throw "Unable to locate repository root. No solution file was found."
}



function Read-FormatConfiguration {

    param(
        [Parameter(Mandatory)]
        [string]$Path
    )


    if (-not (Test-Path $Path)) {

        throw "Configuration file not found: $Path"
    }


    return Get-Content `
        -Path $Path `
        -Raw |
        ConvertFrom-Json
}



function Invoke-FormatCommand {

    param(
        [Parameter(Mandatory)]
        [string[]]$Arguments
    )


    $command = "dotnet format " + ($Arguments -join " ")


    if (-not $Execute -and -not $Verify) {

        Write-Host "Preview:" -ForegroundColor Yellow
        Write-Host $command

        return
    }


    Write-Host ""
    Write-Host "Running:" -ForegroundColor Green
    Write-Host $command
    Write-Host ""


    & dotnet format @Arguments


    if ($LASTEXITCODE -ne 0) {

        throw "dotnet format failed with exit code $LASTEXITCODE."
    }
}



try {

    $timer = [System.Diagnostics.Stopwatch]::StartNew()


    Write-Banner


    $solutionInfo = Find-RepositoryRoot `
        -StartPath $PSScriptRoot


    if ($Solution) {

        $solutionPath = Join-Path `
            $solutionInfo.Root `
            $Solution
    }
    else {

        $solutionPath = $solutionInfo.Solution
    }


    if (-not (Test-Path $solutionPath)) {

        throw "Solution file not found: $solutionPath"
    }


    $config = Read-FormatConfiguration `
        -Path $Configuration



    Write-Host "Solution:"
    Write-Host $solutionPath -ForegroundColor Green
    Write-Host ""


    if ($Verify) {

        Write-Host "Mode: Verify" -ForegroundColor Cyan
    }
    elseif ($Execute) {

        Write-Host "Mode: Execute" -ForegroundColor Red
    }
    else {

        Write-Host "Mode: Preview" -ForegroundColor Cyan
        Write-Host "No changes will be made."
    }


    Write-Host ""



    $commonArguments = @(
        $solutionPath
    )


    if ($Verify) {

        $commonArguments += "--verify-no-changes"
    }

    $verifyArguments = @()
    if ($Verify) {

        $verifyArguments = @("--verify-no-changes")
    }



    if ($config.commands.format) {

        Invoke-FormatCommand `
            -Arguments (
                $commonArguments +
                @(
                    "--severity",
                    $config.dotnet.severity,
                    "--verbosity",
                    $config.dotnet.verbosity
                )
            )
    }



    if ($config.commands.whitespace) {

        Invoke-FormatCommand `
            -Arguments (
                @(
                    "whitespace",
                    $solutionPath,
                    "--verbosity",
                    $config.dotnet.verbosity
                ) + $verifyArguments
            )
    }



    if ($config.commands.style) {

        Invoke-FormatCommand `
            -Arguments (
                @(
                    "style",
                    $solutionPath,
                    "--severity",
                    $config.dotnet.severity,
                    "--verbosity",
                    $config.dotnet.verbosity
                ) + $verifyArguments
            )
    }



    if ($config.commands.analyzers) {

        Invoke-FormatCommand `
            -Arguments (
                @(
                    "analyzers",
                    $solutionPath,
                    "--severity",
                    $config.dotnet.severity,
                    "--verbosity",
                    $config.dotnet.verbosity
                ) + $verifyArguments
            )
    }


    $timer.Stop()


    Write-Host ""
    Write-Host "============================================="
    Write-Host "Format Summary"
    Write-Host "---------------------------------------------"
    Write-Host "Solution : $solutionPath"
    Write-Host "Duration : $($timer.Elapsed)"
    Write-Host "============================================="



    if (-not $Execute -and -not $Verify) {

        Write-Host ""
        Write-Host "To apply formatting, run this script again with the " -ForegroundColor Green -NoNewline
        Write-Host "-Execute" -ForegroundColor Red -NoNewline
        Write-Host " parameter:" -ForegroundColor Green
        Write-Host ""

        Write-Host "Example:" -ForegroundColor Cyan
        Write-Host "$($MyInvocation.MyCommand.Path)" -ForegroundColor Yellow  -NoNewline
        Write-Host " -Execute" -ForegroundColor Red

    }


    exit 0

}
catch {

    Write-Host ""
    Write-Host "Format failed:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red

    exit 1
}
