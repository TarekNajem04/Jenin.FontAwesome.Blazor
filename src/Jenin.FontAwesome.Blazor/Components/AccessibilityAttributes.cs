//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Shared accessible-name/role emission for composed components that expose a status or image
/// role. Internal on purpose: it is an implementation detail, not public API.
/// </summary>
internal static class AccessibilityAttributes {
    internal static void ApplyNameRole(Dictionary<string, object> attributes, string role, string? name, bool isDecorative, string? title) {
        ArgumentNullException.ThrowIfNull(attributes);

        if(!string.IsNullOrWhiteSpace(name)) {
            _ = attributes.AddOrUpdate("role", role);
            _ = attributes.AddOrUpdate("aria-label", name.Trim());
        } else if(isDecorative) {
            _ = attributes.AddOrUpdate("aria-hidden", "true");
        }

        if(!string.IsNullOrWhiteSpace(title)) {
            _ = attributes.AddOrUpdate("title", title.Trim());
        }
    }
}
