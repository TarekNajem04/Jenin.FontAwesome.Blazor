//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Provides subtree-scoped default values for descendant components without any DI registration.
/// Nested <see cref="FontAwesomeDefaults"/> components merge with, and override, their ancestors.
/// </summary>
/// <remarks>
/// Precedence for a value is: explicit component parameter &gt; nearest <see cref="FontAwesomeDefaults"/>
/// &gt; values registered with <c>AddJeninFontAwesome</c> &gt; built-in default.
/// </remarks>
public class FontAwesomeDefaults : ComponentBase {
    /// <summary>
    /// Ambient defaults received from an enclosing <see cref="FontAwesomeDefaults"/>, if any.
    /// </summary>
    [CascadingParameter]
    private FontAwesomeOptions? ParentDefaults { get; set; }

    /// <summary>
    /// Default icon style for the subtree.
    /// </summary>
    [Parameter]
    public IconStyle? IconStyle { get; set; }

    /// <summary>
    /// Default icon typeface for the subtree.
    /// </summary>
    [Parameter]
    public Typeface? Typeface { get; set; }

    /// <summary>
    /// Default icon size for the subtree.
    /// </summary>
    [Parameter]
    public IconSize? IconSize { get; set; }

    /// <summary>
    /// Default accessibility mode for the subtree.
    /// </summary>
    [Parameter]
    public bool? Decorative { get; set; }

    /// <summary>
    /// The subtree that receives the defaults.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Builds the component's render tree: a <see cref="CascadingValue{TValue}"/> that supplies
    /// the merged defaults to the child content.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        var options = FontAwesomeOptions.Merge(ParentDefaults, new FontAwesomeOptions {
            IconStyle = IconStyle,
            Typeface = Typeface,
            IconSize = IconSize,
            Decorative = Decorative,
        });

        builder.OpenComponent<CascadingValue<FontAwesomeOptions>>(0);
        builder.AddAttribute(1, "Value", options);
        builder.AddAttribute(2, "IsFixed", false);
        builder.AddAttribute(3, "ChildContent", ChildContent);
        builder.CloseComponent();
    }
}
