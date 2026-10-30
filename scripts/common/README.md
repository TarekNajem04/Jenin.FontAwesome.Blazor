# Common PowerShell Module

Shared PowerShell utilities used by Jenin FontAwesome repository automation scripts.

## Purpose

This module provides common functionality to keep repository scripts:

- Consistent.
- Maintainable.
- Easy to extend.

## Design Rules

- Only shared functionality belongs here.
- Avoid adding helpers for future assumptions.
- Keep functions focused and reusable.
- Changes should benefit multiple scripts.

## Usage

Import the module:

```powershell
Import-Module "./scripts/common/Common.psd1"
````

Example:

```powershell
Write-ToolBanner -Name "Coverage"
```

## Functions

| Function                 | Description                                |
| ------------------------ | ------------------------------------------ |
| `Write-ToolBanner`       | Displays tool information                  |
| `Write-Section`          | Displays execution sections                |
| `Write-Step`             | Displays execution steps                   |
| `Find-RepositoryRoot`    | Finds repository root using solution files |
| `Read-JsonConfiguration` | Loads JSON configuration files             |
| `Confirm-Action`         | Requests user confirmation                 |
| `Write-Summary`          | Displays execution summary                 |
