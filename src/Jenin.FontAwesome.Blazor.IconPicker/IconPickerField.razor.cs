//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.IconPicker;

/// <summary>
/// An <see cref="InputBase{TValue}"/>-derived icon field: type to filter the Free catalogue, pick
/// from a keyboard-operable dropdown, and bind the selected icon CSS class in any
/// <c>EditForm</c> with validation support.
/// </summary>
/// <remarks>
/// The field edits the icon CSS class (for example <c>fa-house</c>) as plain text and shows a glyph
/// preview of the current value. Arbitrary text is preserved as-is; pair with data annotations
/// (for example <c>[Required]</c> or a known-name check) when the value must resolve to an icon.
/// </remarks>
public partial class IconPickerField : InputBase<string> {
    private string _listId = string.Empty;
    private bool _open;
    private int _activeIndex;

    [Inject]
    private IServiceProvider? Services { get; set; }

    /// <summary>
    /// Placeholder and accessible name for the search box.
    /// </summary>
    [Parameter]
    public string Placeholder { get; set; } = "Search icons...";

    /// <summary>
    /// Accessible name for the field's results list.
    /// </summary>
    [Parameter]
    public string ResultsLabel { get; set; } = "Icon results";

    /// <summary>
    /// Text shown in the field when the search yields no results.
    /// </summary>
    [Parameter]
    public string NoResultsText { get; set; } = "No icons found.";

    /// <summary>
    /// Accessible name for the field's clear control.
    /// </summary>
    [Parameter]
    public string ClearLabel { get; set; } = "Clear selection";

    /// <summary>
    /// The maximum number of results rendered in the dropdown.
    /// </summary>
    [Parameter]
    public int MaxResults { get; set; } = 8;

    /// <summary>
    /// Gets or sets a value indicating whether a glyph preview of the current value is shown.
    /// </summary>
    [Parameter]
    public bool ShowPreview { get; set; } = true;

    /// <summary>
    /// The <c>id</c> rendered on the text box, so a <c>&lt;label for&gt;</c> can target it.
    /// Takes precedence over an <c>id</c> supplied through unmatched attributes.
    /// </summary>
    [Parameter]
    public string? InputId { get; set; }

    /// <summary>
    /// An optional metadata provider override for the field. When <c>null</c> the injected provider is used, falling back to
    /// <see cref="IconMetadataProvider.Default"/>.
    /// </summary>
    [Parameter]
    public IconMetadataProvider? Provider { get; set; }

    private IReadOnlyDictionary<string, object> ForwardedAttributes => AdditionalAttributes?
        .Where(static attribute => !string.Equals(attribute.Key, "class", StringComparison.OrdinalIgnoreCase)
                                   && !string.Equals(attribute.Key, "id", StringComparison.OrdinalIgnoreCase))
        .ToDictionary(static attribute => attribute.Key, static attribute => attribute.Value)
        ?? [];

    private IconMetadataProvider MetadataProvider =>
        Provider
        ?? (Services?.GetService(typeof(IconMetadataProvider)) as IconMetadataProvider)
        ?? IconMetadataProvider.Default;

    private IconMetadata? Selected => string.IsNullOrWhiteSpace(Value) ? null : MetadataProvider.Find(Value);

    private IReadOnlyList<IconMetadata> GetResults() => MetadataProvider.Search(CurrentValueAsString, style: null, MaxResults);

    /// <inheritdoc />
    protected override void OnInitialized() =>
        _listId = $"jenin-icon-field-{Guid.NewGuid():N}";

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, out string result, out string validationErrorMessage) {
        result = value ?? string.Empty;
        validationErrorMessage = null!;

        return true;
    }

    private void OnTextInput(ChangeEventArgs args) {
        ArgumentNullException.ThrowIfNull(args);

        CurrentValueAsString = args.Value?.ToString() ?? string.Empty;
        _activeIndex = 0;
        _open = true;
    }

    private void OnFocus() {
        if(GetResults().Count > 0) {
            _open = true;
        }
    }

    private void OnBlur() => _open = false;

    private void OnKeyDown(KeyboardEventArgs args) {
        ArgumentNullException.ThrowIfNull(args);

        var results = GetResults();

        switch(args.Key) {
            case "ArrowDown":
                if(!_open) {
                    _open = true;
                    _activeIndex = 0;
                } else if(results.Count > 0) {
                    _activeIndex = Math.Min(_activeIndex + 1, results.Count - 1);
                }

                break;
            case "ArrowUp":
                _activeIndex = Math.Max(_activeIndex - 1, 0);
                break;
            case "Enter":
                if(_open && (uint)_activeIndex < (uint)results.Count) {
                    Select(results[_activeIndex]);
                }

                break;
            case "Escape":
                _open = false;
                break;
            default:
                return;
        }
    }

    private void Select(IconMetadata icon) {
        ArgumentNullException.ThrowIfNull(icon);

        CurrentValue = icon.CssClass;
        _open = false;
    }

    private void Clear() {
        CurrentValueAsString = string.Empty;
        _open = false;
    }
}
