//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.IconPicker;

/// <summary>
/// Immutable metadata describing a single Font Awesome Free icon: its canonical name, human-readable
/// label, the Free styles it is available in, and the search terms and aliases used for discovery.
/// </summary>
/// <remarks>
/// Instances are produced by <see cref="IconMetadataProvider"/> from the vendored Font Awesome
/// metadata; they are not constructed directly by consumers.
/// </remarks>
public sealed class IconMetadata {
    internal IconMetadata(string name, string label, IReadOnlyList<IconStyle> styles, IReadOnlyList<string> searchTerms, IReadOnlyList<string> aliases, bool isCustom = false) {
        Name = name;
        Label = label;
        Styles = styles;
        SearchTerms = searchTerms;
        Aliases = aliases;
        IsCustom = isCustom;
    }

    /// <summary>
    /// The canonical, style-independent icon name without the <c>fa-</c> prefix (for example <c>house</c>).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The CSS class accepted by <see cref="Icon.IconName"/> (for example <c>fa-house</c>).
    /// </summary>
    public string CssClass => "fa-" + Name;

    /// <summary>
    /// Whether this icon is a consumer-registered custom glyph rather than a Font Awesome icon.
    /// Custom icons are style-agnostic and always included regardless of style filters.
    /// </summary>
    public bool IsCustom { get; }

    /// <summary>
    /// The human-readable Font Awesome label (for example <c>House</c>).
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// The Free styles in which this icon is available, in canonical order (solid, regular, brands).
    /// </summary>
    public IReadOnlyList<IconStyle> Styles { get; }

    /// <summary>
    /// The Font Awesome search keywords for this icon (excludes aliases).
    /// </summary>
    public IReadOnlyList<string> SearchTerms { get; }

    /// <summary>
    /// Alias names (for example <c>home</c> for <c>house</c>). Aliases are searchable but are not exposed
    /// as typed catalogue constants.
    /// </summary>
    public IReadOnlyList<string> Aliases { get; }

    /// <summary>
    /// The preferred style to render for this icon: the first available Free style, or
    /// <see cref="IconStyle.Solid"/> when no Free style is recorded.
    /// </summary>
    public IconStyle PrimaryStyle => Styles.Count > 0 ? Styles[0] : IconStyle.Solid;

    /// <summary>
    /// Returns a value indicating whether this icon is available in the given style.
    /// </summary>
    /// <param name="style">The style to test.</param>
    /// <returns><c>true</c> when the style is available; otherwise <c>false</c>.</returns>
    public bool SupportsStyle(IconStyle style) => Styles.Contains(style);

    /// <summary>
    /// Returns the icon name.
    /// </summary>
    /// <returns>The canonical name.</returns>
    public override string ToString() => Name;
}
