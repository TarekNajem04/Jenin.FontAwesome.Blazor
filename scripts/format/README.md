# Jenin FontAwesome Repository Format Tool

The `format.ps1` script is a repository maintenance tool used to apply and validate .NET code formatting rules across the solution.

The tool provides a consistent way for all contributors to run:

- `dotnet format`
- `dotnet format whitespace`
- `dotnet format style`
- `dotnet format analyzers`

using a shared configuration.

---

# Design Goals

This tool was created to ensure:

- Consistent formatting across the repository.
- Easy onboarding for new contributors.
- Repeatable formatting behavior.
- Compatibility with local development and CI/CD pipelines.

The formatting rules are stored separately from the script to avoid embedding project-specific settings inside PowerShell code.

---

# Folder Structure

```text
format/
│
├── format.ps1
├── format.config.json
├── format.schema.json
└── README.md
````

---

# Files

## format.ps1

The main formatting script.

Responsibilities:

* Locate the repository root.
* Detect the solution file (`*.sln` or `*.slnx`).
* Load formatting configuration.
* Execute configured `dotnet format` commands.
* Provide preview mode before applying changes.

---

## format.config.json

Contains formatting preferences.

Example:

```json
{
  "$schema": "./format.schema.json",

  "version": 1,

  "dotnet": {
    "verbosity": "diagnostic",
    "severity": "warn"
  },

  "commands": {
    "format": true,
    "whitespace": true,
    "style": true,
    "analyzers": true
  }
}
```

---

## format.schema.json

JSON schema used by editors to validate:

* Configuration structure.
* Allowed values.
* Supported options.

This improves the editing experience in Visual Studio and VS Code.

---

# Safety First

The formatting tool runs in **Preview mode by default**.

Running:

```powershell
.\scripts\format\format.ps1
```

does not modify any file.

It only displays the commands that would be executed.

Example:

```text
Mode: Preview

dotnet format Jenin.FontAwesome.Blazor.slnx --severity warn --verbosity diagnostic

dotnet format whitespace Jenin.FontAwesome.Blazor.slnx --verbosity diagnostic

dotnet format style Jenin.FontAwesome.Blazor.slnx --severity warn --verbosity diagnostic

dotnet format analyzers Jenin.FontAwesome.Blazor.slnx --severity warn --verbosity diagnostic
```

---

# Applying Formatting

To apply formatting changes:

```powershell
.\scripts\format\format.ps1 -Execute
```

The script executes the configured formatting commands.

---

# Verify Formatting

For CI/CD pipelines or validation before creating a pull request:

```powershell
.\scripts\format\format.ps1 -Verify
```

This runs formatting verification without modifying files.

The command fails if the solution is not correctly formatted.

---

# Selecting a Solution

By default, the script searches automatically for:

```text
*.sln
*.slnx
```

If multiple solutions exist, a specific solution can be selected:

```powershell
.\scripts\format\format.ps1 `
    -Solution Jenin.FontAwesome.Blazor.slnx
```

---

# Verbose Output

To see detailed execution information:

```powershell
.\scripts\format\format.ps1 -Verbose
```

Example:

```text
VERBOSE: Checking:
C:\Projects\Jenin.FontAwesome.Blazor

VERBOSE:
Solution found:
Jenin.FontAwesome.Blazor.slnx
```

---

# Configuration

The commands executed by the script are controlled by:

```text
format.config.json
```

Example:

```json
"commands": {
    "format": true,
    "whitespace": true,
    "style": true,
    "analyzers": true
}
```

---

## Disable Specific Formatting Steps

For example, to run only style formatting:

```json
"commands": {
    "format": false,
    "whitespace": false,
    "style": true,
    "analyzers": false
}
```

No PowerShell changes are required.

---

# Formatting Commands

## General Formatting

Equivalent command:

```powershell
dotnet format
```

Handles general formatting corrections.

---

## Whitespace Formatting

Equivalent command:

```powershell
dotnet format whitespace
```

Handles:

* Spaces.
* Indentation.
* Line endings.

---

## Style Formatting

Equivalent command:

```powershell
dotnet format style
```

Handles:

* C# style rules.
* EditorConfig preferences.
* Code simplifications.

---

## Analyzer Formatting

Equivalent command:

```powershell
dotnet format analyzers
```

Handles analyzer-based code fixes.

---

# Recommended Workflow

Before committing changes:

## 1. Preview formatting

```powershell
.\scripts\format\format.ps1
```

Review the commands.

---

## 2. Apply formatting

```powershell
.\scripts\format\format.ps1 -Execute
```

---

## 3. Run tests

```powershell
dotnet test
```

---

## 4. Commit changes

Only commit formatting changes that are intentional.

---

# CI/CD Usage

For automated validation:

```powershell
.\scripts\format\format.ps1 -Verify
```

A failed verification indicates that formatting changes are required.

---

# Troubleshooting

## Solution Not Found

Error:

```text
Unable to locate repository root.
```

Ensure that:

* The script is executed inside the repository.
* A solution file exists:

```text
*.sln
```

or:

```text
*.slnx
```

---

## Configuration File Missing

Ensure this file exists:

```text
scripts/format/format.config.json
```

or provide another configuration:

```powershell
.\scripts\format\format.ps1 `
    -Configuration .\my-format.json
```

---

# Design Principles

## Safe by Default

No files are modified without:

```powershell
-Execute
```

---

## Configuration Driven

Formatting behavior is controlled through JSON configuration.

---

## Repository Independent

The tool discovers the repository structure automatically.

---

## Contributor Friendly

Every contributor can use the same formatting process without remembering complex commands.

---

# Author

Tarek Najem

# Tool

Jenin FontAwesome Repository Format Tool
