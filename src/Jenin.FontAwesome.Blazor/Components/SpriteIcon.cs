//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Renders a Font Awesome glyph from a self-hosted SVG sprite with zero Font Awesome JavaScript:
/// a plain <c>&lt;svg&gt;&lt;use&gt;</c> reference that the browser resolves natively.
/// </summary>
/// <remarks>
/// <para>
/// The sprite is produced by <c>eng/fa_sprite.py</c> and served as a static file (for example
/// <c>wwwroot/fa-sprite.svg</c>). Because the element is a native <c>&lt;svg&gt;</c> (not an
/// <c>&lt;i&gt;</c> placeholder), the Font Awesome engine leaves it untouched, so the markup is
/// identical with or without the FA runtime, works offline, and survives strict Content Security
/// Policies that forbid the FA JavaScript.
/// </para>
/// <para>
/// Only the Free solid, regular, and brands glyphs included in the sprite can render; anything
/// else renders nothing. This is the documented trade-off of the runtime-free path (duotone,
/// layers, masks, and power transforms need the SVG engine). Decorative by default; supply
/// <see cref="ElementBase.Title"/> or <see cref="ElementBase.AriaLabel"/> to name it.
/// </para>
/// </remarks>
public class SpriteIcon : Element {
    /// <summary>
    /// The Font Awesome icon name, including its style-independent <c>fa-</c> prefixed identifier
    /// (for example <c>fa-house</c>). Combined with <see cref="IconStyle"/> it addresses the
    /// <c>fa-{style}-{name}</c> symbol in the sprite.
    /// </summary>
    [Parameter]
    public string? IconName { get; set; }

    /// <summary>
    /// Set icon style: selects the <c>fa-solid-*</c>, <c>fa-regular-*</c>, or <c>fa-brands-*</c>
    /// symbol. Other styles have no Free sprite glyphs and render nothing.
    /// </summary>
    [Parameter]
    public IconStyle IconStyle { get; set; } = IconStyle.Solid;

    /// <summary>
    /// The sprite document URL (for example <c>/fa-sprite.svg</c>). The symbol fragment
    /// <c>#fa-{style}-{name}</c> is appended to it.
    /// </summary>
    [Parameter]
    public string? Sprite { get; set; } = "/fa-sprite.svg";

    private string SymbolId {
        get {
            var name = IconName ?? string.Empty;

            if(name.StartsWith("fa-", StringComparison.OrdinalIgnoreCase)) {
                name = name[3..];
            }

            var style = IconStyle switch {
                IconStyle.Regular => "regular",
                IconStyle.Brands => "brands",
                _ => "solid",
            };

            return $"fa-{style}-{name}";
        }
    }

    private bool UsesDefaultName => string.IsNullOrWhiteSpace(Title) && string.IsNullOrWhiteSpace(AriaLabel);

    /// <summary>
    /// Builds the component's render tree: a sized <c>&lt;svg&gt;</c> with a
    /// <c>&lt;use&gt;</c> reference into the sprite document.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenElement(0, "svg");
        builder.AddAttribute(1, "viewBox", "0 0 512 512");
        builder.AddAttribute(2, "height", "1em");
        builder.AddAttribute(3, "style", "vertical-align:-0.125em;");
        AddAdditionalAttributesToRenderTree(builder, sequence: 4);

        if(UsesDefaultName) {
            builder.AddAttribute(5, "aria-hidden", "true");
        } else {
            builder.AddAttribute(5, "role", "img");
            builder.AddAttribute(6, "aria-label", !string.IsNullOrWhiteSpace(AriaLabel) ? AriaLabel.Trim() : Title!.Trim());
        }

        if(!string.IsNullOrWhiteSpace(Title)) {
            builder.OpenElement(7, "title");
            builder.AddContent(8, Title.Trim());
            builder.CloseElement();
        }

        builder.OpenElement(9, "use");
        builder.AddAttribute(10, "href", $"{Sprite}#{SymbolId}");
        builder.CloseElement();

        builder.CloseElement();
    }
}
