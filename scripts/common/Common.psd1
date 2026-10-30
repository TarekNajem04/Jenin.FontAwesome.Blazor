@{
    RootModule = 'Common.psm1'

    ModuleVersion = '1.0.0'

    GUID = 'd8f5c6b2-6d47-4d3c-9f3e-6a7f8c9b0d12'

    Author = 'Tarek Najem'

    CompanyName = 'Jeninnet'

    Description = 'Shared PowerShell utilities for Jenin FontAwesome repository scripts.'

    PowerShellVersion = '7.0'

    FunctionsToExport = @(
        'Write-ToolBanner',
        'Write-Section',
        'Write-Step',
        'Find-RepositoryRoot',
        'Read-JsonConfiguration',
        'Confirm-Action',
        'Write-Summary'
    )

    CmdletsToExport = @()

    VariablesToExport = @()

    AliasesToExport = @()
}
