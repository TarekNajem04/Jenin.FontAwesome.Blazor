//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Components;

/// <summary>
/// A single selectable icon tile rendered inside the explorer grid.
/// </summary>
public partial class IconCard {
    /// <summary>
    /// Gets or sets the catalogue entry rendered by the card.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public ExplorerIconEntry Entry { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the card represents the currently selected icon.
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the card is activated.
    /// </summary>
    [Parameter]
    public EventCallback<ExplorerIconEntry> OnSelect { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this card is the current roving-tabindex stop, so that
    /// Tab lands on exactly one card while the arrow keys move within the grid.
    /// </summary>
    [Parameter]
    public bool Tabbable { get; set; }

    private string CssClass => Selected ? "explorer-card is-selected" : "explorer-card";

    private Task SelectAsync() => OnSelect.InvokeAsync(Entry);
}
