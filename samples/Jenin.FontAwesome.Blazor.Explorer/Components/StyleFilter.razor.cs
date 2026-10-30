//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Components;

/// <summary>
/// The Free-style facet selector: an "All" option plus one toggle per Free style, each annotated with
/// the number of icons available in that style.
/// </summary>
public partial class StyleFilter {
    private static readonly (IconStyle Style, string Label, int Count)[] _options = [
        (IconStyle.Solid, "Solid", ExplorerCatalogue.Default.CountForStyle(IconStyle.Solid)),
        (IconStyle.Regular, "Regular", ExplorerCatalogue.Default.CountForStyle(IconStyle.Regular)),
        (IconStyle.Brands, "Brands", ExplorerCatalogue.Default.CountForStyle(IconStyle.Brands)),
    ];

    /// <summary>
    /// Gets or sets the active style facet, or <c>null</c> when all styles are shown.
    /// </summary>
    [Parameter]
    public IconStyle? Style { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the facet changes.
    /// </summary>
    [Parameter]
    public EventCallback<IconStyle?> StyleChanged { get; set; }

    /// <summary>
    /// Gets or sets the number of icons in the catalogue.
    /// </summary>
    [Parameter]
    public int TotalCount { get; set; }

    private string ButtonClass(IconStyle? option) => option == Style ? "explorer-filter__button is-active" : "explorer-filter__button";

    private Task SelectAsync(IconStyle? style) => StyleChanged.InvokeAsync(style);
}
