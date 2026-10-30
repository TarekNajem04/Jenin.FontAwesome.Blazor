//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Routing;

/// <summary>
/// Reads and writes the explorer state as URL query-string values so that a search or selection can
/// be shared, bookmarked, and traversed with the browser's back and forward buttons.
/// </summary>
/// <remarks>
/// <para>
/// The schema is deliberately small and additive: <c>q</c> for the query, <c>style</c> for the style
/// facet, and <c>icon</c> for the selected icon. Unknown keys are ignored on read and defaults are
/// omitted on write, so an explorer URL with no state is simply the route path.
/// </para>
/// </remarks>
public static class ExplorerRoute {
    private const string QUERY_KEY = "q";
    private const string STYLE_KEY = "style";
    private const string ICON_KEY = "icon";

    /// <summary>
    /// Parses the query-string values of the current location into an <see cref="ExplorerRouteState"/>.
    /// </summary>
    /// <param name="queryString">The raw query string, including the leading <c>?</c> when present.</param>
    /// <returns>The parsed state; unrecognised or malformed values are ignored.</returns>
    public static ExplorerRouteState Parse(string? queryString) {
        if(string.IsNullOrEmpty(queryString) || queryString == "?") {
            return ExplorerRouteState.Empty;
        }

        string? query = null;
        string? styleToken = null;
        string? icon = null;

        foreach(var pair in queryString.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries)) {
            var separator = pair.IndexOf('=', StringComparison.Ordinal);
            var rawKey = separator < 0 ? pair : pair[..separator];
            var rawValue = separator < 0 ? string.Empty : pair[(separator + 1)..];
            var key = Decode(rawKey);
            var value = Decode(rawValue);

            switch(key) {
                case QUERY_KEY:
                    query = value;
                    break;
                case STYLE_KEY:
                    styleToken = value;
                    break;
                case ICON_KEY:
                    icon = value;
                    break;
                default:
                    break;
            }
        }

        return new ExplorerRouteState(query ?? string.Empty, ParseStyle(styleToken), string.IsNullOrWhiteSpace(icon) ? null : icon);
    }

    /// <summary>
    /// Builds an absolute explorer URI for the supplied state, omitting default and unset values.
    /// </summary>
    /// <param name="navigation">The navigation manager used to resolve the base address and format the query string.</param>
    /// <param name="state">The state to encode.</param>
    /// <returns>The absolute URI for the explorer route representing <paramref name="state"/>.</returns>
    public static string Build(NavigationManager navigation, ExplorerRouteState state) {
        ArgumentNullException.ThrowIfNull(navigation);
        ArgumentNullException.ThrowIfNull(state);

        var parameters = new Dictionary<string, object?>(3, StringComparer.Ordinal) {
            [QUERY_KEY] = string.IsNullOrWhiteSpace(state.Query) ? null : state.Query.Trim(),
            [STYLE_KEY] = state.Style is { } style ? FormatStyle(style) : null,
            [ICON_KEY] = string.IsNullOrWhiteSpace(state.Icon) ? null : state.Icon,
        };

        var path = navigation.BaseUri.TrimEnd('/') + "/explorer";
        return navigation.GetUriWithQueryParameters(path, parameters);
    }

    private static string Decode(string value) => Uri.UnescapeDataString(value.Replace('+', ' '));

    private static IconStyle? ParseStyle(string? token) {
        if(string.Equals(token, "solid", StringComparison.OrdinalIgnoreCase)) {
            return IconStyle.Solid;
        }

        if(string.Equals(token, "regular", StringComparison.OrdinalIgnoreCase)) {
            return IconStyle.Regular;
        }

        return string.Equals(token, "brands", StringComparison.OrdinalIgnoreCase) ? IconStyle.Brands : null;
    }

    private static string FormatStyle(IconStyle style) => style switch {
        IconStyle.Solid => "solid",
        IconStyle.Regular => "regular",
        IconStyle.Brands => "brands",
        _ => style.ToString(),
    };
}
