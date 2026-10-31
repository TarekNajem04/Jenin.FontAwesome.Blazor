//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Library-wide defaults applied to every component that does not set the value explicitly. A
/// <c>null</c> property means "no global default" and the built-in component default is used instead.
/// </summary>
/// <remarks>
/// Configure globally with <c>services.AddJeninFontAwesome(options =&gt; ...)</c>, or scope defaults to a
/// subtree with the <see cref="Components.FontAwesomeDefaults"/> component. Precedence is: explicit
/// component parameter &gt; <see cref="Components.FontAwesomeDefaults"/> cascade &gt; registered options &gt;
/// built-in default. The surface is purely host-side (no JavaScript interop).
/// </remarks>
public class FontAwesomeOptions {
    private Dictionary<string, CustomIconDefinition> _customIcons = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Consumer-registered custom glyphs by <c>IconName</c> (for example <c>fa-acme-widget</c>).
    /// Registered names render through <see cref="Components.Icon"/> as self-contained inline
    /// SVG in every hosting mode and appear in IconPicker search results distinctly marked as
    /// custom. Consumers own their glyphs (licensing included).
    /// </summary>
    public IReadOnlyDictionary<string, CustomIconDefinition> CustomIcons => _customIcons;

    /// <summary>
    /// Registers a custom glyph once; use it through the same <c>Icon</c> API, search metadata,
    /// and snippets as Font Awesome icons. Later registrations replace earlier ones for the
    /// same name.
    /// </summary>
    /// <param name="name">The icon name in <c>IconName</c> form (<c>fa-acme-widget</c>).</param>
    /// <param name="pathData">The SVG path data (<c>d</c> attribute) for the glyph.</param>
    /// <param name="viewBox">The SVG viewBox; defaults to <c>0 0 512 512</c>.</param>
    /// <param name="label">The human-readable label used by search and the picker.</param>
    /// <param name="searchTerms">Additional keywords used by search.</param>
    /// <returns>The registered definition, for assertion or reuse.</returns>
    public CustomIconDefinition RegisterCustomIcon(string name, string pathData, string? viewBox = null, string? label = null, IReadOnlyList<string>? searchTerms = null) {
        var definition = new CustomIconDefinition(name, pathData, viewBox, label, searchTerms);
        _customIcons[definition.Name] = definition;
        return definition;
    }

    /// <summary>
    /// Default icon style used when <c>Icon.IconStyle</c> is not set.
    /// </summary>
    public IconStyle? IconStyle { get; set; }

    /// <summary>
    /// Default icon typeface used when <c>Icon.Typeface</c> is not set.
    /// </summary>
    public Typeface? Typeface { get; set; }

    /// <summary>
    /// Default icon size used when <c>ElementBase.IconSize</c> is not set.
    /// </summary>
    public IconSize? IconSize { get; set; }

    /// <summary>
    /// Default accessibility mode used when <c>ElementBase.Decorative</c> is not set. When left
    /// <c>null</c> the accessibility mode is inferred per component.
    /// </summary>
    public bool? Decorative { get; set; }

    /// <summary>
    /// The delivery mode the one-line setup (<c>MapJeninFontAwesomeAssets</c> plus
    /// <see cref="Components.FontAwesomeHeadAssets"/>) wires assets for. <c>null</c> (the
    /// default) means <see cref="FontAwesomeAssetMode.SvgNest"/>.
    /// </summary>
    public FontAwesomeAssetMode? AssetMode { get; set; }

    /// <summary>
    /// The Font Awesome stylesheet URL the head assets component renders (host-owned: your
    /// self-hosted <c>all.css</c> or a CDN copy). <c>null</c> (the default) renders no
    /// stylesheet link.
    /// </summary>
    public string? StyleSheetUrl { get; set; }

    /// <summary>
    /// The Font Awesome runtime script URL the head assets component renders (host-owned: your
    /// self-hosted <c>all.min.js</c>, a Kit, or a CDN copy). <c>null</c> (the default) renders
    /// no runtime script; webfont CSS-only hosts leave this unset.
    /// </summary>
    public string? JavaScriptUrl { get; set; }

    /// <summary>
    /// The mode-config script URL the head assets component renders. When left <c>null</c> (the
    /// default) the component uses the mapped config endpoint when
    /// <c>MapJeninFontAwesomeAssets</c> ran, otherwise the matching static file under
    /// <c>_content/Jenin.FontAwesome.Blazor/js/</c>.
    /// </summary>
    public string? ConfigScriptUrl { get; set; }

    /// <summary>
    /// The Font Awesome Kit code configured via <see cref="UseFontAwesomeKit"/>, if any.
    /// Kits serve their own CSS, so <see cref="StyleSheetUrl"/> is cleared when a kit is set.
    /// Version pinning lives in the kit settings dashboard, not in the tag.
    /// </summary>
    public string? KitCode { get; set; }

    /// <summary>
    /// The Font Awesome kit runtime host. Kits are always served from this endpoint.
    /// </summary>
    private const string KIT_SCRIPT_HOST = "kit.fontawesome.com";

    /// <summary>
    /// Points the one-line setup at a Font Awesome Kit: sets <see cref="JavaScriptUrl"/> to the
    /// kit runtime (<c>https://kit.fontawesome.com/{kitCode}.js</c>) and clears
    /// <see cref="StyleSheetUrl"/> because kits serve their own CSS. The generated mode-config
    /// script still applies on top (it only sets <c>autoReplaceSvg</c>).
    /// </summary>
    /// <param name="kitCode">The kit code from the kit settings (alphanumeric, no URLs).</param>
    /// <exception cref="ArgumentException"><paramref name="kitCode"/> is empty or not a plain kit code.</exception>
    public void UseFontAwesomeKit(string kitCode) {
        if(string.IsNullOrWhiteSpace(kitCode)
            || kitCode.Any(static character => char.IsWhiteSpace(character) || character is '/' or ':' or '?' or '#' or '&' or '=')) {
            throw new ArgumentException("A Font Awesome kit code is required (for example \"a1b2c3d4e5\"); pass the code, not a URL.", nameof(kitCode));
        }

        KitCode = kitCode.Trim();
        JavaScriptUrl = new UriBuilder("https", KIT_SCRIPT_HOST) { Path = KitCode + ".js" }.Uri.AbsoluteUri;
        StyleSheetUrl = null;
    }

    /// <summary>
    /// The path <c>MapJeninFontAwesomeAssets</c> serves the generated mode-config script from.
    /// Defaults to <c>/_content/Jenin.FontAwesome.Blazor/js/fontawesome-config-auto.js</c>.
    /// </summary>
    public string ConfigEndpointPath {
        get => _configEndpointPath ?? ApplicationBuilderExtensions.DEFAULT_CONFIG_ENDPOINT_PATH;
        set => _configEndpointPath = value;
    }

    private string? _configEndpointPath;

    /// <summary>
    /// Returns a new options instance in which every value set on <paramref name="overrides"/> takes
    /// precedence over <paramref name="baseOptions"/>. Returns <c>null</c> only when both inputs are
    /// <c>null</c>.
    /// </summary>
    internal static FontAwesomeOptions? Merge(FontAwesomeOptions? baseOptions, FontAwesomeOptions? overrides) {
        if(baseOptions is null) {
            return overrides;
        }

        if(overrides is null) {
            return baseOptions;
        }

        return new FontAwesomeOptions {
            IconStyle = overrides.IconStyle ?? baseOptions.IconStyle,
            Typeface = overrides.Typeface ?? baseOptions.Typeface,
            IconSize = overrides.IconSize ?? baseOptions.IconSize,
            Decorative = overrides.Decorative ?? baseOptions.Decorative,
            AssetMode = overrides.AssetMode ?? baseOptions.AssetMode,
            KitCode = overrides.KitCode ?? baseOptions.KitCode,
            StyleSheetUrl = overrides.StyleSheetUrl ?? baseOptions.StyleSheetUrl,
            JavaScriptUrl = overrides.JavaScriptUrl ?? baseOptions.JavaScriptUrl,
            ConfigScriptUrl = overrides.ConfigScriptUrl ?? baseOptions.ConfigScriptUrl,
            _configEndpointPath = overrides._configEndpointPath ?? baseOptions._configEndpointPath,
            _customIcons = MergeCustomIcons(baseOptions._customIcons, overrides._customIcons),
        };
    }

    private static Dictionary<string, CustomIconDefinition> MergeCustomIcons(
        Dictionary<string, CustomIconDefinition> baseIcons,
        Dictionary<string, CustomIconDefinition> overrideIcons) {
        var merged = new Dictionary<string, CustomIconDefinition>(baseIcons, StringComparer.OrdinalIgnoreCase);

        foreach(var pair in overrideIcons) {
            merged[pair.Key] = pair.Value;
        }

        return merged;
    }
}
