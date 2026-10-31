//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// A consumer-registered custom glyph: a name plus inline SVG path data. Registered customs are
/// first-class citizens of the typed API — they render through <see cref="Components.Icon"/>,
/// appear in IconPicker search results, and generate the same usage snippets. Consumers own
/// their glyphs (licensing included); the library never bundles or redistributes path data.
/// </summary>
public sealed class CustomIconDefinition {
    /// <summary>
    /// The default viewBox applied when no explicit viewBox is supplied.
    /// </summary>
    public const string DEFAULT_VIEW_BOX = "0 0 512 512";

    /// <summary>
    /// Registers a custom glyph.
    /// </summary>
    /// <param name="name">The icon name in <c>IconName</c> form (<c>fa-acme-widget</c>).</param>
    /// <param name="pathData">The SVG path data (<c>d</c> attribute) for the glyph.</param>
    /// <param name="viewBox">The SVG viewBox; defaults to <see cref="DEFAULT_VIEW_BOX"/>.</param>
    /// <param name="label">The human-readable label used by search and the picker; defaults to the name.</param>
    /// <param name="searchTerms">Additional keywords used by search.</param>
    /// <exception cref="ArgumentException"><paramref name="name"/> or <paramref name="pathData"/> is missing or malformed.</exception>
    public CustomIconDefinition(string name, string pathData, string? viewBox = null, string? label = null, IReadOnlyList<string>? searchTerms = null) {
        if(string.IsNullOrWhiteSpace(name) || name.Trim().Length <= 3 || !name.Trim().StartsWith("fa-", StringComparison.Ordinal)) {
            throw new ArgumentException("A custom icon name is required in IconName form (for example \"fa-acme-widget\").", nameof(name));
        }

        if(string.IsNullOrWhiteSpace(pathData)) {
            throw new ArgumentException("SVG path data is required.", nameof(pathData));
        }

        Name = name.Trim();
        PathData = pathData;
        ViewBox = string.IsNullOrWhiteSpace(viewBox) ? DEFAULT_VIEW_BOX : viewBox.Trim();
        Label = string.IsNullOrWhiteSpace(label) ? Name : label.Trim();
        SearchTerms = searchTerms ?? [];
    }

    /// <summary>
    /// The icon name in <c>IconName</c> form (<c>fa-acme-widget</c>).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The bare icon name without the <c>fa-</c> prefix (for example <c>acme-widget</c>),
    /// matching the Font Awesome engine's <c>data-icon</c> convention.
    /// </summary>
    public string BareName => Name.StartsWith("fa-", StringComparison.Ordinal) ? Name[3..] : Name;

    /// <summary>
    /// The SVG path data (<c>d</c> attribute) for the glyph.
    /// </summary>
    public string PathData { get; }

    /// <summary>
    /// The SVG viewBox for the glyph.
    /// </summary>
    public string ViewBox { get; }

    /// <summary>
    /// The human-readable label used by search and the picker.
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// Additional keywords used by search.
    /// </summary>
    public IReadOnlyList<string> SearchTerms { get; }

    /// <summary>
    /// Generates the copy-paste <c>Icon</c> usage for this glyph.
    /// </summary>
    /// <returns>A Razor snippet such as <c>&lt;Icon IconName="fa-acme-widget" /&gt;</c>.</returns>
    public string ToRazorSnippet() => "<Icon IconName=\"" + Name + "\" />";
}
