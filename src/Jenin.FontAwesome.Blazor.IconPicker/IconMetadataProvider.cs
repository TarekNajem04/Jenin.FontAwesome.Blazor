//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.IconPicker;

/// <summary>
/// Loads the embedded Font Awesome Free icon metadata and exposes it as a queryable, in-memory model.
/// The metadata is parsed once, lazily, and shared across all instances.
/// </summary>
/// <remarks>
/// No JavaScript interop is involved. Register with <c>AddJeninFontAwesomeIconPicker</c> or use
/// <see cref="Default"/> directly.
/// </remarks>
public sealed class IconMetadataProvider {
    private const string RESOURCE_SUFFIX = "Resources.fontawesome-7.3.1-free-icon-metadata.json";

    private static readonly Lazy<IconsAndIndex> _sharedIcons = new(LoadEmbedded, isThreadSafe: true);

    /// <summary>
    /// A shared provider backed by the embedded metadata.
    /// </summary>
    public static IconMetadataProvider Default { get; } = new();

    private readonly IconsAndIndex _data;
    private readonly IReadOnlyList<IconMetadata> _customs;
    private readonly Dictionary<string, IconMetadata>? _mergedIndex;

    /// <summary>
    /// Initializes a new instance backed by the embedded metadata.
    /// </summary>
    public IconMetadataProvider() {
        _data = _sharedIcons.Value;
        _customs = [];
        Icons = _data.Icons;
    }

    private IconMetadataProvider(IconsAndIndex data, IReadOnlyList<IconMetadata> customs) {
        _data = data;
        _customs = customs;
        Icons = data.Icons;

        if(customs.Count == 0) {
            return;
        }

        var merged = new List<IconMetadata>(data.Icons.Count + customs.Count);
        var index = new Dictionary<string, IconMetadata>(data.ByName, StringComparer.OrdinalIgnoreCase);
        var shadowed = new HashSet<string>(customs.Select(static icon => icon.Name), StringComparer.Ordinal);

        foreach(var icon in data.Icons.Where(icon => !shadowed.Contains(icon.Name))) {
            merged.Add(icon);
        }

        merged.AddRange(customs);
        merged.Sort(static (left, right) => string.CompareOrdinal(left.Name, right.Name));

        foreach(var icon in customs) {
            index[icon.Name] = icon;
        }

        _mergedIndex = index;
        Icons = merged;
    }

    /// <summary>
    /// Returns a provider that additionally serves the given consumer-registered custom glyphs.
    /// Customs are merged in ordinal name order, win on name collisions, are style-agnostic
    /// (always included regardless of style filters), and are distinctly marked via
    /// <see cref="IconMetadata.IsCustom"/>. Returns this instance when there is nothing to add.
    /// </summary>
    /// <param name="customIcons">The registered custom glyphs.</param>
    /// <returns>A provider serving the embedded catalogue plus the custom glyphs.</returns>
    public IconMetadataProvider WithCustomIcons(IEnumerable<CustomIconDefinition> customIcons) {
        ArgumentNullException.ThrowIfNull(customIcons);

        var customs = customIcons
            .Select(static definition => new IconMetadata(
                TrimFaPrefix(definition.Name),
                definition.Label,
                [],
                definition.SearchTerms,
                [],
                isCustom: true))
            .OrderBy(static icon => icon.Name, StringComparer.Ordinal)
            .ToList();

        if(customs.Count == 0 && _customs.Count == 0) {
            return this;
        }

        var combined = customs
            .Concat(_customs.Where(existing => !customs.Any(fresh => fresh.Name == existing.Name)))
            .OrderBy(static icon => icon.Name, StringComparer.Ordinal)
            .ToList();

        return new IconMetadataProvider(_data, combined);
    }

    private static string TrimFaPrefix(string name) =>
        name.StartsWith("fa-", StringComparison.OrdinalIgnoreCase) ? name[3..] : name;

    /// <summary>
    /// All known icons, ordered by canonical name (ordinal), including consumer-registered
    /// custom glyphs when present (see <see cref="WithCustomIcons"/>).
    /// </summary>
    public IReadOnlyList<IconMetadata> Icons { get; }

    /// <summary>
    /// Finds an icon by its canonical name or CSS class (for example <c>house</c> or <c>fa-house</c>).
    /// </summary>
    /// <param name="nameOrClass">The icon name or <c>fa-</c> prefixed CSS class.</param>
    /// <returns>The matching icon, or <c>null</c> when no icon matches.</returns>
    public IconMetadata? Find(string nameOrClass) {
        if(string.IsNullOrWhiteSpace(nameOrClass)) {
            return null;
        }

        var name = nameOrClass.Trim();

        if(name.StartsWith("fa-", StringComparison.OrdinalIgnoreCase)) {
            name = name[3..];
        }

        if(_mergedIndex is not null && _mergedIndex.TryGetValue(name, out var custom)) {
            return custom;
        }

        return _data.ByName.TryGetValue(name, out var icon) ? icon : null;
    }

    /// <summary>
    /// Searches the catalogue by canonical name, alias, or keyword, optionally restricted to a style.
    /// Results are ranked (exact name, then name prefix, then alias/keyword matches) and stable.
    /// Consumer-registered custom glyphs are always included (they are style-agnostic) and carry
    /// <see cref="IconMetadata.IsCustom"/>.
    /// </summary>
    /// <param name="query">The search text. <c>null</c> or empty returns all matching icons.</param>
    /// <param name="style">An optional style the icon must be available in.</param>
    /// <param name="maxResults">The maximum number of results (0 or negative means unlimited).</param>
    /// <returns>The matching icons in rank order.</returns>
    public IReadOnlyList<IconMetadata> Search(string? query, IconStyle? style = null, int maxResults = 0) {
        var normalized = query?.Trim();
        var hasQuery = !string.IsNullOrEmpty(normalized);
        var candidates = new List<RankedIcon>();

        foreach(var icon in Icons) {
            if(style.HasValue && !icon.IsCustom && !icon.SupportsStyle(style.Value)) {
                continue;
            }

            var score = 0;

            if(hasQuery) {
                score = Score(icon, normalized!);

                if(score < 0) {
                    continue;
                }
            }

            candidates.Add(new RankedIcon(icon, score));
        }

        candidates.Sort(static (left, right) => {
            var byScore = right.Rank.CompareTo(left.Rank);

            return byScore != 0 ? byScore : string.CompareOrdinal(left.Metadata.Name, right.Metadata.Name);
        });

        var results = maxResults > 0 && candidates.Count > maxResults
            ? candidates.GetRange(0, maxResults)
            : candidates;

        return [.. results.Select(static ranked => ranked.Metadata)];
    }

    private static int Score(IconMetadata icon, string query) {
        if(icon.Name.Equals(query, StringComparison.OrdinalIgnoreCase)
           || icon.CssClass.Equals(query, StringComparison.OrdinalIgnoreCase)) {
            return 100;
        }

        if(query.StartsWith("fa-", StringComparison.OrdinalIgnoreCase)
           && icon.Name.Equals(query[3..], StringComparison.OrdinalIgnoreCase)) {
            return 100;
        }

        if(icon.Name.StartsWith(query, StringComparison.OrdinalIgnoreCase)) {
            return 80;
        }

        if(Matches(icon.Aliases, query, exactScore: 70, containsScore: 40, out var aliasScore)) {
            return aliasScore;
        }

        if(Matches(icon.SearchTerms, query, exactScore: 60, containsScore: 30, out var termScore)) {
            return termScore;
        }

        if(icon.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) {
            return 20;
        }

        if(icon.Label.Contains(query, StringComparison.OrdinalIgnoreCase)) {
            return 10;
        }

        return -1;
    }

    private static bool Matches(IReadOnlyList<string> values, string query, int exactScore, int containsScore, out int score) {
        if(values.Any(value => value.Equals(query, StringComparison.OrdinalIgnoreCase))) {
            score = exactScore;

            return true;
        }

        if(values.Any(value => value.Contains(query, StringComparison.OrdinalIgnoreCase))) {
            score = containsScore;

            return true;
        }

        score = -1;

        return false;
    }

    private static IconsAndIndex LoadEmbedded() {
        var assembly = typeof(IconMetadataProvider).Assembly;
        var resourceName = Array.Find(assembly.GetManifestResourceNames(),
            static name => name.EndsWith(RESOURCE_SUFFIX, StringComparison.Ordinal))
            ?? throw new InvalidOperationException($"Embedded icon metadata resource ('*{RESOURCE_SUFFIX}') was not found in '{assembly.FullName}'.");

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Embedded icon metadata resource '{resourceName}' could not be opened.");

        return Parse(stream);
    }

    private static IconsAndIndex Parse(Stream stream) {
        using var document = JsonDocument.Parse(stream);
        var icons = new List<IconMetadata>();
        var index = new Dictionary<string, IconMetadata>(StringComparer.OrdinalIgnoreCase);

        if(!document.RootElement.TryGetProperty("icons", out var iconsElement)) {
            return new IconsAndIndex(icons, index);
        }

        foreach(var element in iconsElement.EnumerateArray()) {
            var name = element.GetProperty("n").GetString()!;
            var label = element.TryGetProperty("l", out var labelElement) ? labelElement.GetString() ?? name : name;
            var styles = ParseStyles(element);
            var terms = ParseStrings(element, "t");
            var aliases = ParseStrings(element, "a");

            var icon = new IconMetadata(name, label, styles, terms, aliases);

            icons.Add(icon);
            index[name] = icon;
        }

        icons.Sort(static (left, right) => string.CompareOrdinal(left.Name, right.Name));

        return new IconsAndIndex(icons, index);
    }

    private static List<IconStyle> ParseStyles(JsonElement element) {
        if(!element.TryGetProperty("s", out var stylesElement)) {
            return [];
        }

        var styles = new List<IconStyle>();

        foreach(var styleElement in stylesElement.EnumerateArray()) {
            var mapped = styleElement.GetString() switch {
                "solid" => IconStyle.Solid,
                "regular" => IconStyle.Regular,
                "brands" => IconStyle.Brands,
                _ => (IconStyle?)null
            };

            if(mapped is { } value) {
                styles.Add(value);
            }
        }

        return styles;
    }

    private static List<string> ParseStrings(JsonElement element, string propertyName) {
        if(!element.TryGetProperty(propertyName, out var arrayElement)) {
            return [];
        }

        var values = new List<string>();

        foreach(var item in arrayElement.EnumerateArray()) {
            if(item.GetString() is { } value) {
                values.Add(value);
            }
        }

        return values;
    }

    private sealed record IconsAndIndex(IReadOnlyList<IconMetadata> Icons, Dictionary<string, IconMetadata> ByName);

    private readonly record struct RankedIcon(IconMetadata Metadata, int Rank);
}
