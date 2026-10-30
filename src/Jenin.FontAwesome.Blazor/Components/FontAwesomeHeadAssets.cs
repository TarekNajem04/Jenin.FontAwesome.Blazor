//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Renders the Font Awesome asset tags for the configured delivery mode: the host-owned
/// stylesheet link, the mode-config script, and the host-owned runtime script. Place once in
/// <c>&lt;head&gt;</c> (or the root component's head) together with
/// <c>services.AddJeninFontAwesome(...)</c> and <c>app.MapJeninFontAwesomeAssets(...)</c> for
/// the one-line setup — no hand-written <c>&lt;script&gt;</c> tags needed.
/// </summary>
/// <remarks>
/// Plain markup is rendered (no <c>HeadContent</c>), so the component works in every hosting
/// model including static server-side rendering. Each parameter falls back to the registered
/// <see cref="FontAwesomeOptions"/> when left <c>null</c>; a <c>null</c> stylesheet or runtime
/// URL renders no tag (host-owned assets stay host-owned and are never fetched by default).
/// </remarks>
public class FontAwesomeHeadAssets : ComponentBase {
    /// <summary>
    /// Service provider used to resolve options registered with <c>AddJeninFontAwesome</c>.
    /// </summary>
    [Inject]
    private protected IServiceProvider? Services { get; set; }

    /// <summary>
    /// The delivery mode to render tags for. <c>null</c> (the default) uses the registered
    /// options, else <see cref="FontAwesomeAssetMode.SvgNest"/>.
    /// </summary>
    [Parameter]
    public FontAwesomeAssetMode? Mode { get; set; }

    /// <summary>
    /// The Font Awesome stylesheet URL. <c>null</c> (the default) uses the registered options;
    /// no link is rendered when the effective value is <c>null</c>.
    /// </summary>
    [Parameter]
    public string? StyleSheetUrl { get; set; }

    /// <summary>
    /// The Font Awesome runtime script URL. <c>null</c> (the default) uses the registered
    /// options; no script is rendered when the effective value is <c>null</c>.
    /// </summary>
    [Parameter]
    public string? JavaScriptUrl { get; set; }

    /// <summary>
    /// The mode-config script URL. <c>null</c> (the default) uses the registered options, else
    /// the mapped config endpoint when <c>MapJeninFontAwesomeAssets</c> ran, else the matching
    /// static file under <c>_content/Jenin.FontAwesome.Blazor/js/</c>.
    /// </summary>
    [Parameter]
    public string? ConfigScriptUrl { get; set; }

    /// <summary>
    /// Builds the component's render tree: the stylesheet link (when configured), the
    /// mode-config script, and the runtime script (when configured), in load order.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        var options = Services?.GetService(typeof(FontAwesomeOptions)) as FontAwesomeOptions;
        var mode = Mode ?? options?.AssetMode ?? FontAwesomeAssetMode.SvgNest;
        var styleSheet = StyleSheetUrl ?? options?.StyleSheetUrl;
        var javaScript = JavaScriptUrl ?? options?.JavaScriptUrl;
        var config = ConfigScriptUrl
            ?? options?.ConfigScriptUrl
            ?? (mode == FontAwesomeAssetMode.Webfont
                ? "_content/Jenin.FontAwesome.Blazor/js/fontawesome-config-webfont.js"
                : "_content/Jenin.FontAwesome.Blazor/js/fontawesome-config-nest.js");

        var sequence = 0;

        if(!string.IsNullOrEmpty(styleSheet)) {
            builder.OpenElement(sequence++, "link");
            builder.AddAttribute(sequence++, "rel", "stylesheet");
            builder.AddAttribute(sequence++, "href", styleSheet);
            builder.CloseElement();
        }

        builder.OpenElement(sequence++, "script");
        builder.AddAttribute(sequence++, "src", config);
        builder.AddAttribute(sequence++, "type", "text/javascript");
        builder.CloseElement();

        if(!string.IsNullOrEmpty(javaScript)) {
            builder.OpenElement(sequence++, "script");
            builder.AddAttribute(sequence++, "src", javaScript);
            builder.AddAttribute(sequence++, "type", "text/javascript");
            builder.CloseElement();
        }
    }
}
