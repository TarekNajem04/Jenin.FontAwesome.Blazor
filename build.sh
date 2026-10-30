#!/usr/bin/env bash
#
# Deterministic developer entry point for Jenin.FontAwesome.Blazor.
#
# Usage: ./build.sh [configuration] [target]
#   configuration: Debug (default) | Release
#   target:        restore | build (default) | format | pack | test | apidocs | clean
#
# The script pins the working directory to the repository root; the .NET SDK is selected by
# global.json, keeping results reproducible.
set -euo pipefail

cd "$(dirname "$0")"

configuration="${1:-Debug}"
target="${2:-build}"

solution="Jenin.FontAwesome.Blazor.slnx"
library="src/Jenin.FontAwesome.Blazor/Jenin.FontAwesome.Blazor.csproj"
icon_picker_library="src/Jenin.FontAwesome.Blazor.IconPicker/Jenin.FontAwesome.Blazor.IconPicker.csproj"
icons_library="src/Jenin.FontAwesome.Blazor.Icons/Jenin.FontAwesome.Blazor.Icons.csproj"
svg_library="src/Jenin.FontAwesome.Blazor.Svg/Jenin.FontAwesome.Blazor.Svg.csproj"

case "$target" in
    restore)
        dotnet restore "$solution"
        ;;
    build)
        dotnet build "$solution" -c "$configuration"
        ;;
    format)
        dotnet format "$solution" --verify-no-changes
        ;;
    pack)
        dotnet pack "$library" -c Release
        dotnet pack "$icon_picker_library" -c Release
        dotnet pack "$icons_library" -c Release
        dotnet pack "$svg_library" -c Release
        ;;
    test)
        test_project="$(find src tests -name '*Test*.csproj' -print -quit 2>/dev/null || true)"
        if [ -n "$test_project" ]; then
            dotnet test "$solution" -c "$configuration"
        else
            echo "No test project found; nothing to test yet."
        fi
        ;;
    apidocs)
        dotnet build "$library" -c Release
        dotnet build "$icon_picker_library" -c Release
        dotnet build "$icons_library" -c Release
        dotnet build "$svg_library" -c Release
        dotnet docfx docs/docfx.json
        ;;
    clean)
        dotnet clean "$solution" -c "$configuration"
        ;;
    *)
        echo "Unknown target: $target" >&2
        echo "Valid targets: restore | build | format | pack | test | apidocs | clean" >&2
        exit 2
        ;;
esac
