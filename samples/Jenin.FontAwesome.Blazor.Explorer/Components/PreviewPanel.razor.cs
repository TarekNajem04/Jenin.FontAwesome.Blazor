//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Components;

/// <summary>
/// The details pane for the selected icon: a large preview driven by <see cref="ExplorerPreviewState"/>
/// and the live control panel.
/// </summary>
public partial class PreviewPanel {
    private SnippetPanel? _snippets;

    /// <summary>
    /// Gets or sets the selected icon, or <c>null</c> when nothing is selected.
    /// </summary>
    [Parameter]
    public ExplorerIconEntry? Selected { get; set; }

    /// <summary>
    /// Gets or sets the live preview state.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public ExplorerPreviewState Preview { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the host renders icons with webfonts rather than the
    /// Font Awesome SVG engine, so the unsupported-notice can warn that SVG-only effects are inert.
    /// </summary>
    [Parameter]
    public bool WebFontMode { get; set; }

    private bool UsesSvgOnlyFeature =>
        Preview.UseTransform || Preview.UseMask || Preview.UseDuotone || Preview.UseDuotoneOpacity;

    private string FontAwesomeLink => Selected is null
        ? "https://fontawesome.com/search"
        : $"https://fontawesome.com/search?q={Uri.EscapeDataString(Selected.Name)}";

    /// <summary>
    /// Copies the current snippet to the clipboard, forwarding to the snippet panel. Exposed so the
    /// explorer's global <c>C</c> shortcut can trigger a copy regardless of focus.
    /// </summary>
    /// <returns>A task that completes when the copy has been attempted.</returns>
    public Task CopySnippetAsync() => _snippets?.CopyCurrentAsync() ?? Task.CompletedTask;

    private string? ColorValue => Preview.UseColor ? Preview.Color : null;

    private string? BackgroundColorValue => Preview.UseBackgroundColor ? Preview.BackgroundColor : null;

    private float? RotateAngle => Preview.Rotate == IconRotate.RotateBy ? Preview.RotateAngle : null;

    private string? PrimaryColor => Preview.UseDuotone ? Preview.DuotonePrimary : null;

    private string? SecondaryColor => Preview.UseDuotone ? Preview.DuotoneSecondary : null;

    private float? PrimaryOpacity => Preview.UseDuotoneOpacity ? Preview.DuotonePrimaryOpacity : null;

    private float? SecondaryOpacity => Preview.UseDuotoneOpacity ? Preview.DuotoneSecondaryOpacity : null;

    private string? MaskIconName => Preview.UseMask ? Preview.MaskIconName : null;

    private string StyleSummary => Selected is null ? string.Empty : string.Join(", ", Selected.Styles.Select(FormatStyle));

    private Animation? CurrentAnimation => PreviewIconFactory.BuildAnimation(Preview);

    private Transform? CurrentTransform => PreviewIconFactory.BuildTransform(Preview);

    private static string FormatStyle(IconStyle style) => style switch {
        IconStyle.Solid => "Solid",
        IconStyle.Regular => "Regular",
        IconStyle.Brands => "Brands",
        _ => style.ToString(),
    };
}
