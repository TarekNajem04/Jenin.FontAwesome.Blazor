//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Components;

/// <summary>
/// Shows the generated Razor, preset-component, and HTML snippets for the selected icon and the
/// current live-control values, with a copy-to-clipboard affordance and transient feedback.
/// </summary>
public partial class SnippetPanel {
    private const string MODULE_PATH = "./_content/Jenin.FontAwesome.Blazor.Explorer/js/explorer.js";

    private static readonly SnippetKind[] _kinds = [SnippetKind.Razor, SnippetKind.Preset, SnippetKind.Html];

    private SnippetKind _activeKind = SnippetKind.Razor;
    private bool _copied;

    /// <summary>
    /// Gets or sets the selected icon, or <c>null</c> when nothing is selected.
    /// </summary>
    [Parameter]
    public ExplorerIconEntry? Selected { get; set; }

    /// <summary>
    /// Gets or sets the live preview state used to build the snippets.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public ExplorerPreviewState Preview { get; set; } = default!;

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    private Snippet Current => Selected is null
        ? new Snippet(_activeKind, "razor", "// Select an icon to generate code.")
        : SnippetGenerator.Generate
          (
            Selected,
            Preview,
            _activeKind,
            PreviewIconFactory.BuildAnimation(Preview),
            PreviewIconFactory.BuildTransform(Preview)
          );

    private static string Label(SnippetKind kind) => kind switch {
        SnippetKind.Preset => "Preset component",
        SnippetKind.Html => "HTML",
        _ => "Razor",
    };

    private void SelectKind(SnippetKind kind) {
        _activeKind = kind;
        _copied = false;
    }

    private async Task CopyAsync() {
        var snippet = Current;
        try {
            var module = await JS.InvokeAsync<IJSObjectReference>("import", MODULE_PATH);
            await module.InvokeVoidAsync("copyToClipboard", snippet.Code);
        }
        catch(JSException) {
            return;
        }

        _copied = true;
        await Task.Delay(1500);
        _copied = false;
    }

    /// <summary>
    /// Copies the current snippet to the clipboard and shows transient feedback. Public so the
    /// explorer's global <c>C</c> shortcut can invoke it through the preview panel.
    /// </summary>
    /// <returns>A task that completes when the copy has been attempted.</returns>
    public Task CopyCurrentAsync() => CopyAsync();
}
