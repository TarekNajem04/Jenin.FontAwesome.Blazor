//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Data;

/// <summary>
/// The in-memory icon catalogue used by the explorer: an immutable, ordinal-sorted projection of
/// <see cref="IconMetadataProvider.Default"/> plus the faceted search algorithm described by the
/// explorer architecture document.
/// </summary>
/// <remarks>
/// <para>
/// The catalogue is built once (cold start measured well under the explorer's interactive budget) and
/// never mutates. Search runs entirely in memory with no network or file access, and the same query
/// always yields the same ordered result set, which keeps the UI deterministic.
/// </para>
/// <para>
/// Scoring mirrors <see cref="IconMetadataProvider.Search"/> so that a single-token query returns the
/// same icons the library search would, while multi-token queries apply an AND rule over per-token
/// first-match scores.
/// </para>
/// </remarks>
public sealed class ExplorerCatalogue {
    private static readonly string[] _emptyTokens = [];
    private static readonly IconStyle[] _styles = [IconStyle.Solid, IconStyle.Regular, IconStyle.Brands];

    private readonly List<ExplorerIconEntry> _icons;
    private readonly Dictionary<string, ExplorerIconEntry> _byName;
    private readonly Dictionary<IconStyle, int> _styleCounts;

    private ExplorerCatalogue() {
        var metadata = IconMetadataProvider.Default.Icons;
        var entries = new List<ExplorerIconEntry>(metadata.Count);
        var byName = new Dictionary<string, ExplorerIconEntry>(metadata.Count, StringComparer.OrdinalIgnoreCase);

        foreach(var item in metadata) {
            var entry = new ExplorerIconEntry(item);
            entries.Add(entry);
            byName[entry.Name] = entry;
        }

        entries.Sort(static (left, right) => string.CompareOrdinal(left.Name, right.Name));
        _icons = entries;
        _byName = byName;

        var counts = new Dictionary<IconStyle, int>();
        foreach(var style in _styles) {
            var count = 0;
            foreach(var entry in entries) {
                if(entry.SupportsStyle(style)) {
                    count++;
                }
            }

            counts[style] = count;
        }

        _styleCounts = counts;
    }

    /// <summary>
    /// Gets the shared catalogue instance, built from the embedded Font Awesome metadata.
    /// </summary>
    public static ExplorerCatalogue Default { get; } = new();

    /// <summary>
    /// Gets all icons in the catalogue, ordered by <see cref="string.CompareOrdinal(string, string)"/>
    /// on the canonical name.
    /// </summary>
    public IReadOnlyList<ExplorerIconEntry> Icons => _icons;

    /// <summary>
    /// Gets the total number of icons in the catalogue (the count shown when no style facet is applied).
    /// </summary>
    public int TotalCount => _icons.Count;

    /// <summary>
    /// Gets the number of icons available in the given Free style.
    /// </summary>
    /// <param name="style">The Free style to count.</param>
    /// <returns>The number of icons available in the style.</returns>
    public int CountForStyle(IconStyle style) => _styleCounts.TryGetValue(style, out var count) ? count : 0;

    /// <summary>
    /// Finds the unique icon identified by a canonical name or CSS class, accepting an optional
    /// <c>fa-</c> prefix and ignoring case.
    /// </summary>
    /// <param name="nameOrClass">The canonical name or CSS class to resolve.</param>
    /// <returns>The matching entry, or <c>null</c> when the value is empty or unknown.</returns>
    public ExplorerIconEntry? Find(string? nameOrClass) {
        if(string.IsNullOrWhiteSpace(nameOrClass)) {
            return null;
        }

        var name = nameOrClass.Trim();
        if(name.StartsWith("fa-", StringComparison.OrdinalIgnoreCase)) {
            name = name[3..];
        }

        return _byName.TryGetValue(name, out var entry) ? entry : null;
    }

    /// <summary>
    /// Searches the catalogue for the supplied query, optionally constrained to a single Free style.
    /// </summary>
    /// <param name="query">The raw query text; whitespace is trimmed and tokens are separated on whitespace.</param>
    /// <param name="style">When set, only icons available in the style are considered.</param>
    /// <returns>
    /// The matching entries ordered by descending score and then ascending canonical name. When the
    /// query is empty, every icon permitted by the style facet is returned in name order.
    /// </returns>
    public IReadOnlyList<ExplorerIconEntry> Search(string? query, IconStyle? style = null) {
        var tokens = Tokenize(query);
        var matches = new List<ScoredIcon>();

        foreach(var icon in _icons) {
            if(style is { } required && !icon.SupportsStyle(required)) {
                continue;
            }

            var score = Score(icon, tokens);
            if(score < 0) {
                continue;
            }

            matches.Add(new ScoredIcon(icon, score));
        }

        matches.Sort(static (left, right) => {
            var byScore = right.Score.CompareTo(left.Score);
            return byScore != 0 ? byScore : string.CompareOrdinal(left.Icon.Name, right.Icon.Name);
        });

        var results = new ExplorerIconEntry[matches.Count];
        for(var i = 0; i < matches.Count; i++) {
            results[i] = matches[i].Icon;
        }

        return results;
    }

    private static string[] Tokenize(string? query) {
        var trimmed = query?.Trim();
        return string.IsNullOrEmpty(trimmed)
            ? _emptyTokens
            : trimmed.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    private static int Score(ExplorerIconEntry icon, string[] tokens) {
        if(tokens.Length == 0) {
            return 0;
        }

        var total = 0;
        foreach(var token in tokens) {
            var tokenScore = ScoreToken(icon, token);
            if(tokenScore < 0) {
                return -1;
            }

            total += tokenScore;
        }

        return total;
    }

    private static int ScoreToken(ExplorerIconEntry icon, string token) {
        var metadata = icon.Metadata;

        if(metadata.Name.Equals(token, StringComparison.OrdinalIgnoreCase)
            || metadata.CssClass.Equals(token, StringComparison.OrdinalIgnoreCase)) {
            return 100;
        }

        if(metadata.Name.StartsWith(token, StringComparison.OrdinalIgnoreCase)) {
            return 80;
        }

        if(TryScoreList(metadata.Aliases, token, 70, 40, out var aliasScore)) {
            return aliasScore;
        }

        if(TryScoreList(metadata.SearchTerms, token, 60, 30, out var termScore)) {
            return termScore;
        }

        if(metadata.Name.Contains(token, StringComparison.OrdinalIgnoreCase)) {
            return 20;
        }

        return metadata.Label.Contains(token, StringComparison.OrdinalIgnoreCase) ? 10 : -1;
    }

    private static bool TryScoreList(IReadOnlyList<string> values, string token, int exactScore, int containsScore, out int score) {
        foreach(var value in values) {
            if(value.Equals(token, StringComparison.OrdinalIgnoreCase)) {
                score = exactScore;
                return true;
            }
        }

        foreach(var value in values) {
            if(value.Contains(token, StringComparison.OrdinalIgnoreCase)) {
                score = containsScore;
                return true;
            }
        }

        score = 0;
        return false;
    }

    private readonly record struct ScoredIcon(ExplorerIconEntry Icon, int Score);
}
