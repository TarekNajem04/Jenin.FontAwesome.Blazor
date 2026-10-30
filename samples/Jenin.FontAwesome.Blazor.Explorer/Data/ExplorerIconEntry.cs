//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Data;

/// <summary>
/// A single entry in the explorer catalogue: a projection of the immutable <see cref="IconMetadata"/>
/// exposed by <see cref="IconMetadataProvider"/> for use by the explorer UI.
/// </summary>
/// <remarks>
/// Instances are created once by <see cref="ExplorerCatalogue"/> and are safe to share across the
/// application. The entry intentionally stores only the metadata projection so that search does not
/// allocate per query.
/// </remarks>
public sealed class ExplorerIconEntry {
    internal ExplorerIconEntry(IconMetadata metadata) {
        ArgumentNullException.ThrowIfNull(metadata);

        Metadata = metadata;
        Name = metadata.Name;
        CssClass = metadata.CssClass;
        Label = metadata.Label;
        Styles = metadata.Styles;
    }

    /// <summary>
    /// The underlying Font Awesome metadata for the icon.
    /// </summary>
    public IconMetadata Metadata { get; }

    /// <summary>
    /// The canonical, style-independent icon name without the <c>fa-</c> prefix (for example <c>house</c>).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The CSS class accepted by <see cref="Icon.IconName"/> (for example <c>fa-house</c>).
    /// </summary>
    public string CssClass { get; }

    /// <summary>
    /// The human-readable Font Awesome label (for example <c>House</c>).
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// The Free styles in which this icon is available, in canonical order (solid, regular, brands).
    /// </summary>
    public IReadOnlyList<IconStyle> Styles { get; }

    /// <summary>
    /// The preferred style to render for this icon: the first available Free style, or
    /// <see cref="IconStyle.Solid"/> when no Free style is recorded.
    /// </summary>
    public IconStyle PrimaryStyle => Metadata.PrimaryStyle;

    /// <summary>
    /// Returns a value indicating whether this icon is available in the given style.
    /// </summary>
    /// <param name="style">The style to test.</param>
    /// <returns><c>true</c> when the style is available; otherwise <c>false</c>.</returns>
    public bool SupportsStyle(IconStyle style) => Metadata.SupportsStyle(style);

    /// <summary>
    /// Returns the canonical icon name.
    /// </summary>
    /// <returns>The canonical name.</returns>
    public override string ToString() => Name;
}
