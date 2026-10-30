//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Declares the Font Awesome glyph for an enum member, consumed by
/// <see cref="Components.IconFor{TValue}"/> so status values render without switch statements.
/// Only constant arguments are allowed: the name, an optional style, and an optional color.
/// </summary>
/// <param name="name">The Font Awesome icon name, including its style-independent <c>fa-</c> prefixed identifier (for example <c>fa-house</c>).</param>
/// <example>
/// <code>
/// public enum OrderStatus {
///     [Icon("fa-clock")]
///     Pending,
///     [Icon("fa-truck", IconStyle = IconStyle.Solid, Color = "blue")]
///     Shipped,
///     Cancelled,
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Field)]
public sealed class IconAttribute(string name) : Attribute {
    /// <summary>
    /// The Font Awesome icon name, including its style-independent <c>fa-</c> prefixed identifier.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// The icon style. Always concrete (never <c>null</c>: attribute arguments cannot be nullable),
    /// so a declared style behaves like a baked preset and wins over ambient defaults.
    /// </summary>
    public IconStyle Style { get; set; }

    /// <summary>
    /// The glyph color for the declared icon. <c>null</c> inherits the surrounding text color.
    /// </summary>
    public string? Color { get; set; }
}
