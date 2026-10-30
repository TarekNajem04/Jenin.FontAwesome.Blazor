//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Base class for composed components that render a leading icon marker and forward the
/// consumer-selected icon style/typeface to it (consumer value, then ambient defaults, then the
/// component default).
/// </summary>
/// <remarks>
/// This is a supported extension point. To build a composed icon component, derive from this
/// class, declare the <c>IconName</c>/<c>IconStyle</c>/<c>Typeface</c>/<c>IconColor</c>/
/// <c>IconFragment</c> parameters your component needs, implement
/// <see cref="AddComponentStyles(Dictionary{string, string})"/>, and call
/// <see cref="AddRootAndMarker(RenderTreeBuilder, string, string, IconStyle, Typeface, string, RenderFragment)"/>
/// followed by your body content and <c>builder.CloseElement()</c> in
/// <c>BuildRenderTree</c>.
/// </remarks>
public abstract class ComposedIconElement : Element {
    private IconStyle? _iconStyleFromConsumer;
    private Typeface? _typefaceFromConsumer;

    /// <summary>
    /// Adds the component-specific layout styles after the shared styles.
    /// </summary>
    /// <param name="styles">The style dictionary to update.</param>
    protected abstract void AddComponentStyles(Dictionary<string, string> styles);

    /// <summary>
    /// Opens the root element and renders the leading icon marker. The caller renders its body and
    /// closes the root element afterwards.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    /// <param name="markerStyle">The inline style applied to the marker wrapper.</param>
    /// <param name="iconName">The marker icon name.</param>
    /// <param name="iconStyle">The component's declared icon style.</param>
    /// <param name="typeface">The component's declared typeface.</param>
    /// <param name="iconColor">The marker icon color, or <c>null</c> to inherit.</param>
    /// <param name="iconFragment">Custom marker content that replaces the built-in icon.</param>
    protected void AddRootAndMarker(RenderTreeBuilder builder, string markerStyle, string? iconName, IconStyle iconStyle, Typeface typeface, string? iconColor, RenderFragment? iconFragment) {
        ArgumentNullException.ThrowIfNull(builder);

        var resolvedStyle = IconForwarding.ResolveStyle(_iconStyleFromConsumer, ResolvedDefaults, iconStyle);
        var resolvedTypeface = IconForwarding.ResolveTypeface(_typefaceFromConsumer, ResolvedDefaults, typeface);

        builder.OpenElement(0, "div");
        AddAdditionalAttributesToRenderTree(builder, sequence: 1);
        AddEventsToRenderTree(builder, sequence: 2);

        builder.OpenComponent<IconMarker>(7);
        builder.AddAttribute(8, nameof(IconMarker.MarkerStyle), markerStyle);
        builder.AddAttribute(9, nameof(IconMarker.IconName), iconName);
        builder.AddAttribute(10, nameof(IconMarker.IconStyle), resolvedStyle);
        builder.AddAttribute(11, nameof(IconMarker.Typeface), resolvedTypeface);
        builder.AddAttribute(12, nameof(IconMarker.IconColor), iconColor);
        builder.AddAttribute(13, nameof(IconMarker.IconFragment), iconFragment);
        builder.CloseComponent();
    }

    private protected override void CaptureParameters(ParameterView parameters) {
        base.CaptureParameters(parameters);

        (_iconStyleFromConsumer, _typefaceFromConsumer) = IconForwarding.Capture(parameters);
    }

    private protected sealed override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) {
        styles = base.AddStyles(styles);

        AddComponentStyles(styles);

        return styles;
    }
}
