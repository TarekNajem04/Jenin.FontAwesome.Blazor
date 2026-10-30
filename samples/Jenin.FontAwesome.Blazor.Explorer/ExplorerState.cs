//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer;

/// <summary>
/// The mutable, component-owned state of the explorer. A single instance lives for the lifetime of
/// the <see cref="Components.Explorer"/> component and is projected to and from the URL by
/// <see cref="Routing.ExplorerRoute"/>.
/// </summary>
/// <remarks>
/// State is owned by the explorer component rather than registered in dependency injection: the
/// component is the only writer, which keeps server-side prerendering and interactive upgrades from
/// sharing a throwaway scope, and there is no cross-route lifetime to manage.
/// </remarks>
public sealed class ExplorerState {
    /// <summary>
    /// Gets or sets the current search text.
    /// </summary>
    public string Query { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the active style facet, or <c>null</c> to search every style.
    /// </summary>
    public IconStyle? Style { get; set; }

    /// <summary>
    /// Gets or sets the selected icon, or <c>null</c> when nothing is selected.
    /// </summary>
    public ExplorerIconEntry? Selected { get; set; }

    /// <summary>
    /// Gets the live preview state.
    /// </summary>
    public ExplorerPreviewState Preview { get; } = new();

    /// <summary>
    /// Projects the state into the URL-addressable <see cref="ExplorerRouteState"/>.
    /// </summary>
    /// <returns>The route state describing the current explorer view.</returns>
    public ExplorerRouteState ToRouteState() => new(Query, Style, Selected?.Name);
}
