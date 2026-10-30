<#
.SYNOPSIS
    Runs tests and generates a code coverage report.

.DESCRIPTION
    Executes the test suite, collects code coverage, and generates
    an HTML report using ReportGenerator.

    The script runs in preview mode by default.
    No commands are executed unless -Execute is specified.

.PARAMETER Execute
    Executes the coverage workflow.

.PARAMETER Clean
    Removes previous coverage artifacts before execution.

.PARAMETER Restore
    Restores NuGet packages before running tests.

.PARAMETER OpenReport
    Opens the generated HTML report after completion.

.PARAMETER InstallTools
    Automatically installs missing required tools.

.PARAMETER Configuration
    Specifies the coverage configuration file.

.EXAMPLE
    .\coverage.ps1

    Preview the coverage workflow.

.EXAMPLE
    .\coverage.ps1 -Execute

    Run tests and generate a coverage report.

.EXAMPLE
    .\coverage.ps1 -Execute -Clean -Restore

    Clean previous artifacts, restore packages, then run coverage.

.EXAMPLE
    .\coverage.ps1 -Execute -OpenReport

    Generate coverage and open the HTML report.

.NOTES
    Author:
        Tarek Najem

    Tool:
        Jenin FontAwesome Repository Coverage Tool
#>

[CmdletBinding(
    SupportsShouldProcess = $true,
    ConfirmImpact = 'Medium'
)]
param(

    [switch]
    $Execute,

    [switch]
    $Clean,

    [switch]
    $Restore,

    [switch]
    $OpenReport,

    [switch]
    $InstallTools,

    [string]
    $ConfigurationPath = (
        Join-Path $PSScriptRoot "coverage.config.json"
    )
)

Set-StrictMode -Version Latest

$ErrorActionPreference = 'Stop'

Import-Module -Name (Join-Path $PSScriptRoot ".." "common" "Common.psd1") -Force

$script:Stopwatch =
    [System.Diagnostics.Stopwatch]::StartNew()

try {

    Write-ToolBanner `
        -Name "Repository Coverage Tool"

    $repositoryRoot =
        Find-RepositoryRoot `
            -StartPath $PSScriptRoot

    $configuration =
        Read-JsonConfiguration `
            -Path $ConfigurationPath

    Write-Host "Repository:"
    Write-Host $repositoryRoot -ForegroundColor Green
    Write-Host ""

    if ($Execute) {

        Write-Host "Mode: Execute" `
            -ForegroundColor Yellow
    }
    else {

        Write-Host "Mode: Preview" `
            -ForegroundColor Cyan

        Write-Host "No commands will be executed."
    }

    Write-Host ""

    #
    # Environment preparation
    #

    if ($Clean) {

        Write-Host "Cleaning previous coverage artifacts..." `
            -ForegroundColor Yellow


        $cleanTargets = @(
            "TestResults",
            "TestCoverageReport"
        )


        foreach ($target in $cleanTargets) {

            $path =
                Join-Path `
                    $repositoryRoot `
                    $target


            if (Test-Path $path) {

                if ($PSCmdlet.ShouldProcess(
                        $path,
                        "Remove directory")) {

                    Remove-Item `
                        -Path $path `
                        -Recurse `
                        -Force

                    Write-Host "Removed: $path" `
                        -ForegroundColor Green
                }
            }
        }

        Write-Host ""
    }


    if ($Restore) {

        Write-Host "Restoring NuGet packages..." `
            -ForegroundColor Yellow


        if ($PSCmdlet.ShouldProcess(
                $repositoryRoot,
                "Run dotnet restore")) {

            dotnet restore

            if ($LASTEXITCODE -ne 0) {

                throw "dotnet restore failed."
            }
        }


        Write-Host ""
    }


    #
    # Check ReportGenerator
    #

    Write-Host "Checking ReportGenerator..." `
        -ForegroundColor Cyan


    $reportGenerator =
        Get-Command `
            "reportgenerator" `
            -ErrorAction SilentlyContinue


    if ($null -eq $reportGenerator) {

        Write-Host ""
        Write-Host "ReportGenerator is not installed." `
            -ForegroundColor Yellow

        if (-not $Execute) {

            Write-Host ""
            Write-Host "Preview mode: installation skipped." `
                -ForegroundColor DarkYellow

            Write-Host ""
            Write-Host "Run with:" `
                -ForegroundColor Cyan

            Write-Host "$($MyInvocation.MyCommand.Path) -Execute -InstallTools" `
                -ForegroundColor Green

        }
        else {

            if ($InstallTools) {

                $install = $true
            }
            else {

                $answer =
                    Read-Host `
                        "Do you want to install it? (Y/N)"

                $install =
                    $answer -match '^[Yy]'
            }


            if ($install) {

                Write-Host "Installing ReportGenerator..." `
                    -ForegroundColor Yellow

                dotnet tool install `
                    --global `
                    dotnet-reportgenerator-globaltool

                if ($LASTEXITCODE -ne 0) {

                    throw "Failed to install ReportGenerator."
                }
            }
            else {

                throw @"
ReportGenerator is required to generate coverage reports.

Install it or run the script again with:
-InstallTools
"@
            }    

        }
    }
    else {

        Write-Host "ReportGenerator found." `
            -ForegroundColor Green
    }

    Write-Host ""

    #
    # Load coverage configuration
    #

    Write-Host "Loading coverage configuration..." `
        -ForegroundColor Cyan


    $testCommand =
        $configuration.test.command


    $testArguments =
        $configuration.test.arguments


    $reportDirectory =
        Join-Path `
            $repositoryRoot `
            $configuration.coverage.reportDirectory


    $coverageDirectory =
        Join-Path `
            $repositoryRoot `
            $configuration.coverage.resultsDirectory


    $reportTypes =
        $configuration.coverage.reportTypes


    $assemblyFilters =
        $configuration.reportGenerator.assemblyFilters


    $reportGeneratorCommand =
        $configuration.reportGenerator.toolCommand

    #
    # Preview mode
    #

    if (-not $Execute) {

        Write-Host ""

        Write-Host "The following command will be executed:" `
            -ForegroundColor Cyan

        Write-Host ""

        Write-Host "dotnet test $testArguments" `
            -ForegroundColor Yellow

        Write-Host ""

        Write-Host "Report output:"
        Write-Host $reportDirectory

        Write-Host ""

        Write-Host "Run with:"
        Write-Host "$($MyInvocation.MyCommand.Path) -Execute" `
            -ForegroundColor Green

        exit 0
    }


    #
    # Run tests with coverage collection
    #

    Write-Host ""
    Write-Host "Running tests and collecting coverage..." `
        -ForegroundColor Cyan


    Push-Location $repositoryRoot

    try {

        dotnet test `
            $testArguments


        if ($LASTEXITCODE -ne 0) {

            throw "dotnet test failed."
        }

    }
    finally {

        Pop-Location
    }


    Write-Host ""

    Write-Host "Searching coverage files..." `
        -ForegroundColor Cyan


    $coverageFiles =
        Get-ChildItem `
            -Path $repositoryRoot `
            -Recurse `
            -Include "*.xml" `
            -ErrorAction SilentlyContinue |
        Where-Object {

            $_.FullName -match "TestResults" -and
            $_.Name -match "coverage|cobertura|opencover"

        }


    if ($null -eq $coverageFiles -or $coverageFiles.Count -eq 0) {

        throw @"
Coverage files were not generated.

Expected files:
- cobertura.xml
- coverage.opencover.xml
- *.coverage.xml
"@
    }


    Write-Host ""

    Write-Host "Coverage files found: $($coverageFiles.Count)" `
        -ForegroundColor Green

    foreach ($file in $coverageFiles) {

        Write-Host $file.FullName `
            -ForegroundColor DarkGray
    }


    Write-Host ""
    #
    # Generate coverage report
    #

    Write-Host "Generating coverage report..." `
        -ForegroundColor Cyan


    if (Test-Path $reportDirectory) {

        Remove-Item `
            -Path $reportDirectory `
            -Recurse `
            -Force
    }

    $reports =
        ($coverageFiles.FullName -join ";")


    $assemblyFilters =
        $configuration.reportGenerator.assemblyFilters -join ";"


    $reportTypes =
        $configuration.coverage.reportTypes -join ";"


    $classFilters = ""
    #$classFilters = $configuration.report.classFilters -join ";"

    $reportGeneratorCommand =
        $configuration.reportGenerator.toolCommand -join ";"


    $OpenReport =
        $configuration.behavior.openReport 


    $reportArguments = @(
        "-reports:$reports"
        "-targetdir:$reportDirectory"
        "-reporttypes:$reportTypes"
    )


    if (-not [string]::IsNullOrWhiteSpace($assemblyFilters)) {

        $reportArguments +=
            "-assemblyfilters:$assemblyFilters"
    }


    if (-not [string]::IsNullOrWhiteSpace($classFilters)) {

        $reportArguments +=
            "-classfilters:$classFilters"
    }


    Write-Host ""

    Write-Host "Running ReportGenerator..." `
        -ForegroundColor Cyan


    reportgenerator @reportArguments


    if ($LASTEXITCODE -ne 0) {

        throw "ReportGenerator failed."
    }


    #
    # Verify report
    #

    $indexFile =
        Join-Path `
            $reportDirectory `
            "index.html"


    if (-not (Test-Path $indexFile)) {

        throw @"
Coverage report was not generated.

Expected:
$indexFile
"@
    }


    Write-Host ""

    Write-Host "Coverage report generated successfully." `
        -ForegroundColor Green

    #
    # Open report
    #

    if ($OpenReport) {

        Write-Host ""
        Write-Host "Opening coverage report..." `
            -ForegroundColor Yellow

        Start-Process $indexFile
    }

    #
    # Summary
    #

    $script:Stopwatch.Stop()

    Write-Summary @{
        CoverageFiles = $coverageFiles.Count
        Report        = $indexFile
        Duration      = $script:Stopwatch.Elapsed
    }

    exit 0

}
catch {

    if ($script:Stopwatch.IsRunning) {

        $script:Stopwatch.Stop()
    }

    Write-Host ""

    Write-Host "Coverage failed:" `
        -ForegroundColor Red


    Write-Host $_.Exception.Message `
        -ForegroundColor Red

    exit 1
}
