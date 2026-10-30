//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Scopes duotone layer colors to a subtree: every duotone <see cref="Icon"/> inside inherits the
/// palette unless it sets its own <c>PrimaryColor</c>/<c>SecondaryColor</c>. Light and dark
/// variants follow the host's <c>data-theme="dark"</c> marker (the convention used by the samples
/// and Bootstrap 5.3+); without it the light values apply.
/// </summary>
/// <remarks>
/// <para>
/// Resolution order per layer: explicit per-mode color (<see cref="LightPrimary"/>,
/// <see cref="LightSecondary"/>, <see cref="DarkPrimary"/>, <see cref="DarkSecondary"/>), then the
/// single-pair shortcut (<see cref="PrimaryColor"/>, <see cref="SecondaryColor"/>, applied to both
/// modes), then <see cref="Palette"/>. Unspecified dark values fall back to their light counterparts.
/// </para>
/// </remarks>
public class IconTheme : Element {
    /// <summary>
    /// The themed content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The palette supplying all four layer colors unless overridden below.
    /// </summary>
    [Parameter]
    public DuotonePalette? Palette { get; set; }

    /// <summary>
    /// A single primary color applied to both modes unless a per-mode value wins.
    /// </summary>
    [Parameter]
    public string? PrimaryColor { get; set; }

    /// <summary>
    /// A single secondary color applied to both modes unless a per-mode value wins.
    /// </summary>
    [Parameter]
    public string? SecondaryColor { get; set; }

    /// <summary>
    /// The light-mode primary layer color.
    /// </summary>
    [Parameter]
    public string? LightPrimary { get; set; }

    /// <summary>
    /// The light-mode secondary layer color.
    /// </summary>
    [Parameter]
    public string? LightSecondary { get; set; }

    /// <summary>
    /// The dark-mode primary layer color. Defaults to the resolved light value.
    /// </summary>
    [Parameter]
    public string? DarkPrimary { get; set; }

    /// <summary>
    /// The dark-mode secondary layer color. Defaults to the resolved light value.
    /// </summary>
    [Parameter]
    public string? DarkSecondary { get; set; }

    private string? ResolvedLightPrimary => LightPrimary ?? PrimaryColor ?? Palette?.LightPrimary;

    private string? ResolvedLightSecondary => LightSecondary ?? SecondaryColor ?? Palette?.LightSecondary;

    private string? ResolvedDarkPrimary => DarkPrimary ?? PrimaryColor ?? Palette?.DarkPrimary ?? ResolvedLightPrimary;

    private string? ResolvedDarkSecondary => DarkSecondary ?? SecondaryColor ?? Palette?.DarkSecondary ?? ResolvedLightSecondary;

    private bool HasTheme => ResolvedLightPrimary is not null
                             || ResolvedLightSecondary is not null
                             || ResolvedDarkPrimary is not null
                             || ResolvedDarkSecondary is not null;

    /// <summary>
    /// Applies parameter changes before rendering: prepends the theme marker class when the
    /// theme carries at least one color. An empty theme stays a plain wrapper so it never
    /// interrupts inherited values.
    /// </summary>
    protected override void OnParametersSet() {
        base.OnParametersSet();

        if(!HasTheme) {
            return;
        }

        AdditionalAttributes ??= [];

        var existing = Convert.ToString(AdditionalAttributes.TryGetValue("class", out var current) ? current : null, CultureInfo.InvariantCulture);
        var merged = string.Join(' ', new[] { "jfa-duotone-theme", existing }.Where(static part => !string.IsNullOrWhiteSpace(part)));

        _ = AdditionalAttributes.SetOrRemoveIfEmpty("class", merged);
    }

    private protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) {
        styles = base.AddStyles(styles);

        return styles.AddOrUpdateIfNotNull("--duo-primary", ResolvedLightPrimary)
                     .AddOrUpdateIfNotNull("--duo-secondary", ResolvedLightSecondary)
                     .AddOrUpdateIfNotNull("--duo-primary-dark", ResolvedDarkPrimary)
                     .AddOrUpdateIfNotNull("--duo-secondary-dark", ResolvedDarkSecondary);
    }

    /// <summary>
    /// Builds the component's render tree: a scope element carrying the layer colors as custom
    /// properties, consumed by the companion stylesheet.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenElement(0, "span");
        AddAdditionalAttributesToRenderTree(builder, sequence: 1);
        AddEventsToRenderTree(builder, sequence: 2);
        builder.AddContent(6, ChildContent);
        builder.CloseElement();
    }
}
