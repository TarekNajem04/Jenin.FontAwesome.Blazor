//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Routing;

/// <summary>
/// The URL-addressable state of the explorer: the search query, the optional style facet, and the
/// optional selected icon name.
/// </summary>
/// <param name="Query">The search text, or an empty string when unset.</param>
/// <param name="Style">The active style facet, or <c>null</c> when showing all styles.</param>
/// <param name="Icon">The canonical name of the selected icon, or <c>null</c> when nothing is selected.</param>
public sealed record ExplorerRouteState(string Query, IconStyle? Style, string? Icon) {
    /// <summary>
    /// Gets the empty route state used when no query-string values are present.
    /// </summary>
    public static ExplorerRouteState Empty { get; } = new(string.Empty, null, null);
}
