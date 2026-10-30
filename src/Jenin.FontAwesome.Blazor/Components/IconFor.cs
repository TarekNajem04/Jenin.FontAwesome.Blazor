//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Renders the glyph declared on an enum value by <see cref="IconAttribute"/>, so status fields
/// never need a switch statement. Values without a declaration render <see cref="FallbackIconName"/>.
/// </summary>
/// <remarks>
/// <para>
/// Attribute lookup follows the trim-safe pattern used across the library: the enum type is a
/// generic type parameter annotated for public-field preservation, and no dynamic code paths
/// are involved. Combined (flags) values do not resolve to a single member and fall back.
/// </para>
/// <para>
/// The glyph is always decorative; status icons are expected to accompany visible text carrying
/// the meaning. Use <see cref="Icon"/> directly when the icon itself must be announced.
/// </para>
/// </remarks>
/// <typeparam name="TValue">The enum type carrying <see cref="IconAttribute"/> declarations.</typeparam>
public class IconFor<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] TValue> : Element
    where TValue : struct, Enum {
    /// <summary>
    /// The enum value to render.
    /// </summary>
    [Parameter]
    public TValue Value { get; set; }

    /// <summary>
    /// The Font Awesome animation applied to the resolved glyph.
    /// </summary>
    [Parameter]
    public Animation? Animation { get; set; }

    /// <summary>
    /// The fallback glyph rendered when <see cref="Value"/> declares no icon.
    /// </summary>
    [Parameter]
    public string? FallbackIconName { get; set; } = "fa-circle-question";

    /// <summary>
    /// The icon style used for <see cref="FallbackIconName"/>.
    /// </summary>
    [Parameter]
    public IconStyle FallbackIconStyle { get; set; } = IconStyle.Solid;

    private static IconAttribute? ResolveAttribute(TValue value) =>
        typeof(TValue).GetField(value.ToString())?.GetCustomAttribute<IconAttribute>();

    private IconAttribute? Declared => ResolveAttribute(Value);

    private bool IsFallback => string.IsNullOrWhiteSpace(Declared?.Name);

    private string? ResolvedIconName => IsFallback ? FallbackIconName : Declared!.Name;

    private IconStyle ResolvedIconStyle =>
        IsFallback ? FallbackIconStyle : Declared!.Style;

    private string? ResolvedColor => Declared?.Color;

    /// <summary>
    /// Builds the component's render tree: the declared glyph, or the fallback glyph.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenComponent<Icon>(0);
        builder.AddAttribute(1, nameof(Icon.IconName), ResolvedIconName);
        builder.AddAttribute(2, nameof(Icon.IconStyle), ResolvedIconStyle);
        builder.AddAttribute(3, nameof(Icon.Decorative), true);

        if(!string.IsNullOrEmpty(ResolvedColor)) {
            builder.AddAttribute(4, nameof(Icon.Color), ResolvedColor);
        }

        if(Animation is not null) {
            builder.AddAttribute(5, nameof(Icon.Animation), Animation);
        }

        builder.CloseComponent();
    }
}
