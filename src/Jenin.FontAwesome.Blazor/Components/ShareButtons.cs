//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Brand-icon share links (X, LinkedIn, WhatsApp, Telegram, mail) with documented share intents.
/// Links open in a new tab with <c>noopener</c> and carry accessible names.
/// </summary>
/// <remarks>
/// <para>
/// Share URLs follow each provider's documented web intent (verified 2026-10-26):
/// X <c>twitter.com/intent/tweet</c>, LinkedIn <c>sharing/share-offsite</c>, WhatsApp
/// <c>wa.me</c>, Telegram <c>t.me/share/url</c>, and <c>mailto:</c>. Values are URL-encoded.
/// </para>
/// </remarks>
public class ShareButtons : Element {
    /// <summary>
    /// The URL being shared.
    /// </summary>
    [Parameter]
    public string? Url { get; set; }

    /// <summary>
    /// The share text accompanying <see cref="Url"/>.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Accessible name for the share group.
    /// </summary>
    [Parameter]
    public string? ShareLabel { get; set; } = "Share";

    /// <summary>
    /// Gets or sets a value indicating whether the X (Twitter) link is rendered.
    /// </summary>
    [Parameter]
    public bool IncludeX { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the LinkedIn link is rendered.
    /// </summary>
    [Parameter]
    public bool IncludeLinkedIn { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the WhatsApp link is rendered.
    /// </summary>
    [Parameter]
    public bool IncludeWhatsApp { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the Telegram link is rendered.
    /// </summary>
    [Parameter]
    public bool IncludeTelegram { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the mail link is rendered.
    /// </summary>
    [Parameter]
    public bool IncludeMail { get; set; } = true;

    private string EncodedUrl => Uri.EscapeDataString(Url ?? string.Empty);

    private string EncodedText => Uri.EscapeDataString(Text ?? string.Empty);

    private protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) {
        styles = base.AddStyles(styles);

        styles["display"] = "inline-flex";
        styles["gap"] = "0.5rem";
        styles["align-items"] = "center";

        return styles;
    }

    private protected override void BuildAccessibilityAttributes() {
        AdditionalAttributes ??= [];

        _ = AdditionalAttributes.AddOrUpdate("role", "group");

        if(!string.IsNullOrWhiteSpace(ShareLabel)) {
            _ = AdditionalAttributes.AddOrUpdate("aria-label", ShareLabel.Trim());
        }
    }

    /// <summary>
    /// Builds the component's render tree: the enabled brand share links.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenElement(0, "span");
        AddAdditionalAttributesToRenderTree(builder, sequence: 1);
        AddEventsToRenderTree(builder, sequence: 2);

        var sequence = 10;

        if(IncludeX) {
            AddShareLink(builder, ref sequence, "Share on X", "fa-x-twitter",
                $"https://twitter.com/intent/tweet?url={EncodedUrl}&text={EncodedText}");
        }

        if(IncludeLinkedIn) {
            AddShareLink(builder, ref sequence, "Share on LinkedIn", "fa-linkedin",
                $"https://www.linkedin.com/sharing/share-offsite/?url={EncodedUrl}");
        }

        if(IncludeWhatsApp) {
            AddShareLink(builder, ref sequence, "Share on WhatsApp", "fa-whatsapp",
                $"https://wa.me/?text={EncodedText}%20{EncodedUrl}");
        }

        if(IncludeTelegram) {
            AddShareLink(builder, ref sequence, "Share on Telegram", "fa-telegram",
                $"https://t.me/share/url?url={EncodedUrl}&text={EncodedText}");
        }

        if(IncludeMail) {
            AddShareLink(builder, ref sequence, "Share by email", "fa-envelope",
                $"mailto:?subject={EncodedText}&body={EncodedUrl}");
        }

        builder.CloseElement();
    }

    private static void AddShareLink(RenderTreeBuilder builder, ref int sequence, string label, string icon, string href) {
        builder.OpenElement(sequence, "a");
        builder.AddAttribute(sequence + 1, "href", href);
        builder.AddAttribute(sequence + 2, "target", "_blank");
        builder.AddAttribute(sequence + 3, "rel", "noopener noreferrer");
        builder.AddAttribute(sequence + 4, "aria-label", label);
        builder.AddAttribute(sequence + 5, "title", label);

        builder.OpenComponent<Icon>(sequence + 6);
        builder.AddAttribute(sequence + 7, nameof(Icon.IconName), icon);
        builder.AddAttribute(sequence + 8, nameof(Icon.IconStyle), IconStyle.Brands);
        builder.AddAttribute(sequence + 9, nameof(Icon.Decorative), true);
        builder.CloseComponent();

        builder.CloseElement();

        sequence += 10;
    }
}
