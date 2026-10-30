//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.IconPicker;

/// <summary>
/// A lightweight, interop-free icon picker: type-to-search over the Font Awesome Free catalogue (by name,
/// alias, or keyword), filter by style, preview the results with the core <see cref="Icon"/> component,
/// and raise a selection.
/// </summary>
/// <remarks>
/// The component is presentation-only and ships no CSS or JavaScript. Style it via the emitted
/// <c>fa-icon-picker*</c> class names or <see cref="CssClass"/>. When
/// <c>AddJeninFontAwesomeIconPicker</c> is not called it falls back to <see cref="IconMetadataProvider.Default"/>.
/// </remarks>
public partial class IconPicker {
    private string? _query;
    private IconStyle? _styleFilter;

    [Inject]
    private IServiceProvider? Services { get; set; }

    /// <summary>
    /// The selected icon CSS class (for example <c>fa-house</c>), suitable for <see cref="Icon.IconName"/>.
    /// Supports two-way binding.
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// Raised when <see cref="Value"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<string?> ValueChanged { get; set; }

    /// <summary>
    /// Raised with the full <see cref="IconMetadata"/> when the user selects an icon.
    /// </summary>
    [Parameter]
    public EventCallback<IconMetadata> OnSelect { get; set; }

    /// <summary>
    /// The initial search text. Changing this after initialization does not update the search box.
    /// </summary>
    [Parameter]
    public string? SearchText { get; set; }

    /// <summary>
    /// The initial style filter; <c>null</c> shows all styles.
    /// </summary>
    [Parameter]
    public IconStyle? Style { get; set; }

    /// <summary>
    /// The maximum number of results to render. Values of zero or less mean unlimited.
    /// </summary>
    [Parameter]
    public int MaxResults { get; set; } = 60;

    /// <summary>
    /// Whether to render the human-readable label beneath each icon.
    /// </summary>
    [Parameter]
    public bool ShowLabels { get; set; } = true;

    /// <summary>
    /// Placeholder/accessible name for the search box.
    /// </summary>
    [Parameter]
    public string Placeholder { get; set; } = "Search icons...";

    /// <summary>
    /// Accessible name for the style filter.
    /// </summary>
    [Parameter]
    public string StyleFilterLabel { get; set; } = "Filter by style";

    /// <summary>
    /// Text shown for the "all styles" filter option.
    /// </summary>
    [Parameter]
    public string AllStylesText { get; set; } = "All styles";

    /// <summary>
    /// Accessible name for the picker's results list.
    /// </summary>
    [Parameter]
    public string ResultsLabel { get; set; } = "Icon results";

    /// <summary>
    /// Text shown in the picker when the search yields no results.
    /// </summary>
    [Parameter]
    public string NoResultsText { get; set; } = "No icons found.";

    /// <summary>
    /// An additional CSS class applied to the picker root.
    /// </summary>
    [Parameter]
    public string? CssClass { get; set; }

    /// <summary>
    /// An optional metadata provider override for the picker. When <c>null</c> the injected provider is used, falling back
    /// to <see cref="IconMetadataProvider.Default"/>.
    /// </summary>
    [Parameter]
    public IconMetadataProvider? Provider { get; set; }

    private IconMetadataProvider MetadataProvider =>
        Provider
        ?? (Services?.GetService(typeof(IconMetadataProvider)) as IconMetadataProvider)
        ?? IconMetadataProvider.Default;

    private string StyleFilterValue => _styleFilter switch {
        IconStyle.Solid => "solid",
        IconStyle.Regular => "regular",
        IconStyle.Brands => "brands",
        _ => string.Empty
    };

    /// <summary>
    /// Initializes the component by seeding the search query and style filter from the
    /// <see cref="SearchText"/> and <see cref="Style"/> parameters.
    /// </summary>
    protected override void OnInitialized() {
        _query = SearchText;
        _styleFilter = Style;
    }

    private IReadOnlyList<IconMetadata> GetResults() => MetadataProvider.Search(_query, _styleFilter, MaxResults);

    private void OnSearchInput(ChangeEventArgs args) {
        ArgumentNullException.ThrowIfNull(args);

        _query = args.Value?.ToString();
    }

    private void OnStyleChanged(ChangeEventArgs args) {
        ArgumentNullException.ThrowIfNull(args);

        _styleFilter = args.Value switch {
            "solid" => IconStyle.Solid,
            "regular" => IconStyle.Regular,
            "brands" => IconStyle.Brands,
            _ => null
        };
    }

    private async Task SelectAsync(IconMetadata icon) {
        ArgumentNullException.ThrowIfNull(icon);

        Value = icon.CssClass;

        await ValueChanged.InvokeAsync(icon.CssClass);
        await OnSelect.InvokeAsync(icon);
    }

    private bool IsSelected(IconMetadata icon) =>
        !string.IsNullOrEmpty(Value) && string.Equals(Value, icon.CssClass, StringComparison.OrdinalIgnoreCase);

    private string ItemCssClass(IconMetadata icon) {
        ArgumentNullException.ThrowIfNull(icon);

        var selected = IsSelected(icon) ? " is-selected" : string.Empty;
        var custom = icon.IsCustom ? " is-custom" : string.Empty;

        return "fa-icon-picker__item" + selected + custom;
    }
}
