//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Components;

/// <summary>
/// The keyboard-shortcut help overlay. It lists the explorer's global shortcuts and is opened with
/// <c>?</c> (or the toolbar button) and dismissed with <c>Escape</c> or the close button.
/// </summary>
public partial class KeyboardHelp {
    private static readonly (string Key, string Description)[] _shortcuts =
    [
        ("/", "Focus the search box"),
        ("Esc", "Clear the search"),
        ("↑ ↓ ← →", "Move between icons"),
        ("Enter", "Select the focused icon"),
        ("C", "Copy the current snippet"),
        ("?", "Show or hide this help"),
    ];

    private const string MODULE_PATH = "./_content/Jenin.FontAwesome.Blazor.Explorer/js/explorer.js";

    /// <summary>
    /// Gets or sets a value indicating whether the overlay is shown.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the overlay is dismissed.
    /// </summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if(!Visible) {
            return;
        }

        try {
            var module = await JS.InvokeAsync<IJSObjectReference>("import", MODULE_PATH);
            await module.InvokeVoidAsync("focusSelector", ".explorer-help__close");
        }
        catch(JSException) {
            // Focusing the dialog is best-effort.
        }
    }

    private Task OnKeyDownAsync(KeyboardEventArgs args) => args.Key == "Escape" ? CloseAsync() : Task.CompletedTask;

    private Task CloseAsync() => OnClose.InvokeAsync();
}
