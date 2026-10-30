//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Curated duotone palettes with light- and dark-mode layer colors. Pass one to
/// <see cref="Components.IconTheme"/>, or use its values as a starting point for custom colors.
/// </summary>
public static class DuotonePalettes {
    /// <summary>Blue info palette.</summary>
    public static DuotonePalette Info { get; } = new("Info", "#0d6efd", "#9ec5fe", "#8bb9fe", "#084298");

    /// <summary>Green success palette.</summary>
    public static DuotonePalette Success { get; } = new("Success", "#198754", "#a3cfbb", "#75b798", "#0f5132");

    /// <summary>Amber warning palette.</summary>
    public static DuotonePalette Warning { get; } = new("Warning", "#997404", "#ffe69c", "#ffda6a", "#664d03");

    /// <summary>Red danger palette.</summary>
    public static DuotonePalette Danger { get; } = new("Danger", "#dc3545", "#f1aeb5", "#ea868f", "#58151c");

    /// <summary>Violet brand palette.</summary>
    public static DuotonePalette Brand { get; } = new("Brand", "#6f42c1", "#c29ffa", "#b18cf5", "#3d2b7a");

    /// <summary>Gray neutral palette (inverts gracefully between modes).</summary>
    public static DuotonePalette Neutral { get; } = new("Neutral", "#495057", "#adb5bd", "#adb5bd", "#495057");
}
