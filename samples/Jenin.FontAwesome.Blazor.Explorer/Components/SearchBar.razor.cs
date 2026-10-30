//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Components;

/// <summary>
/// The explorer search input: it reports every keystroke to the parent (which applies the debounce),
/// offers a clear affordance, and announces the current result count to assistive technology.
/// </summary>
public partial class SearchBar {
    /// <summary>
    /// Gets or sets the current query text.
    /// </summary>
    [Parameter]
    public string? Query { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the query text changes.
    /// </summary>
    [Parameter]
    public EventCallback<string?> QueryChanged { get; set; }

    /// <summary>
    /// Gets or sets the number of icons matching the current query and facet.
    /// </summary>
    [Parameter]
    public int ResultCount { get; set; }

    /// <summary>
    /// Gets or sets the number of icons in the catalogue.
    /// </summary>
    [Parameter]
    public int TotalCount { get; set; }

    private bool HasQuery => !string.IsNullOrEmpty(Query);

    private string Status => HasQuery
        ? $"{ResultCount} of {TotalCount} icons"
        : $"{TotalCount} icons";

    private Task OnInputAsync(ChangeEventArgs args) => QueryChanged.InvokeAsync(args.Value?.ToString());

    private Task OnKeyDownAsync(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs args) => (args.Key == "Escape") && HasQuery ? ClearAsync() : Task.CompletedTask;

    private Task ClearAsync() => QueryChanged.InvokeAsync(string.Empty);
}
