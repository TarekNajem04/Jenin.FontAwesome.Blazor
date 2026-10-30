//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Shared plumbing for composed components that let a consumer-selected style/typeface flow to
/// their child <see cref="Icon"/> and render either that icon or a custom fragment.
/// Internal on purpose: it is an implementation detail, not public API.
/// </summary>
internal static class IconForwarding {
    internal static (IconStyle? Style, Typeface? Typeface) Capture(ParameterView parameters) {
        var style = parameters.TryGetValue<IconStyle>("IconStyle", out var capturedStyle) ? capturedStyle : (IconStyle?)null;
        var typeface = parameters.TryGetValue<Typeface>("Typeface", out var capturedTypeface) ? capturedTypeface : (Typeface?)null;

        return (style, typeface);
    }

    internal static IconStyle ResolveStyle(IconStyle? consumer, FontAwesomeOptions? defaults, IconStyle declared) =>
        consumer ?? defaults?.IconStyle ?? declared;

    internal static Typeface ResolveTypeface(Typeface? consumer, FontAwesomeOptions? defaults, Typeface declared) =>
        consumer ?? defaults?.Typeface ?? declared;

    internal static void AddGlyph(RenderTreeBuilder builder, int sequence, string? iconName, IconStyle style, Typeface typeface, string? color, RenderFragment? fragment) {
        ArgumentNullException.ThrowIfNull(builder);

        if(fragment is not null) {
            builder.AddContent(sequence, fragment);
            return;
        }

        builder.OpenComponent<Icon>(sequence);
        builder.AddAttribute(sequence + 1, nameof(Icon.IconName), iconName);
        builder.AddAttribute(sequence + 2, nameof(Icon.IconStyle), style);
        builder.AddAttribute(sequence + 3, nameof(Icon.Typeface), typeface);
        builder.AddAttribute(sequence + 4, nameof(Icon.Decorative), true);

        if(!string.IsNullOrEmpty(color)) {
            builder.AddAttribute(sequence + 5, nameof(Icon.Color), color);
        }

        builder.CloseComponent();
    }
}
