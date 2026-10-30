//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Components;

/// <summary>
/// The virtualized icon grid: it arranges the supplied icons into fixed-height rows whose column
/// count follows the measured container width, and renders only the rows intersecting the viewport.
/// </summary>
/// <remarks>
/// Only the visible window of cards exists in the DOM, so the grid stays responsive for the full
/// catalogue. Card geometry is fixed (height and gap), which keeps scrolling free of layout shift.
/// The container width is measured by a small JavaScript module; when measurement is unavailable the
/// grid falls back to a fixed default column count.
/// </remarks>
public partial class IconGrid {
    /// <summary>
    /// The fixed height, in CSS pixels, of every grid row. Cards must fit inside one row.
    /// </summary>
    public const int ROW_HEIGHT = 132;

    private const int MIN_CARD_WIDTH = 132;
    private const int GAP = 12;
    private const int MAX_COLUMNS = 10;
    private const int DEFAULT_COLUMNS = 6;
    private const string MODULE_PATH = "./_content/Jenin.FontAwesome.Blazor.Explorer/js/explorer.js";

    private readonly List<ExplorerIconEntry[]> _rows = [];
    private ExplorerIconEntry[] _items = [];
    private int _columns = DEFAULT_COLUMNS;
    private bool _interopInitialized;
    private bool _resetScroll;
    private ElementReference _viewport;
    private IJSObjectReference? _module;
    private IJSObjectReference? _observer;
    private DotNetObjectReference<IconGrid>? _self;

    /// <summary>
    /// Gets or sets the icons to display, in result order.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IReadOnlyList<ExplorerIconEntry> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the currently selected icon, if any.
    /// </summary>
    [Parameter]
    public ExplorerIconEntry? Selected { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when a card is activated.
    /// </summary>
    [Parameter]
    public EventCallback<ExplorerIconEntry> OnSelect { get; set; }

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    private string RowStyle => $"grid-template-columns:repeat({_columns},minmax(0,1fr));height:{ROW_HEIGHT}px";

    /// <inheritdoc />
    protected override void OnParametersSet() {
        var items = Items as ExplorerIconEntry[] ?? [.. Items];

        // Entries are shared catalogue instances, so a content comparison distinguishes a genuinely
        // new result set (search or style filter) from a re-render with the same results (selection,
        // theme change). Only a new result set restarts at the first row; re-renders must keep the
        // user's scroll offset instead of yanking the grid back to the top.
        if(_items.Length == items.Length && _items.AsSpan().SequenceEqual(items)) {
            return;
        }

        _items = items;
        RebuildRows();

        // A new result set starts at the first row instead of keeping the previous scroll offset,
        // which would otherwise point at a different part of the list.
        _resetScroll = true;
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if(!_interopInitialized && _items.Length > 0) {
            try {
                _module = await JS.InvokeAsync<IJSObjectReference>("import", MODULE_PATH);
                _self = DotNetObjectReference.Create(this);
                _observer = await _module.InvokeAsync<IJSObjectReference>("observeWidth", _viewport, _self);
                _interopInitialized = true;
            }
            catch(JSException) {
                // Measurement is best-effort: keep the default column count when the module is unavailable.
                _interopInitialized = true;
            }
        }

        if(!_resetScroll) {
            return;
        }

        _resetScroll = false;

        if(_module is null) {
            return;
        }

        try {
            await _module.InvokeVoidAsync("scrollToTop", _viewport);
        }
        catch(JSException) {
            // Scrolling is best-effort; the user can still scroll manually.
        }
    }

    /// <summary>
    /// Receives the measured viewport width from JavaScript and recomputes the column count.
    /// </summary>
    /// <param name="width">The container width in CSS pixels.</param>
    /// <returns>A task that completes when the grid has been updated.</returns>
    [JSInvokable]
    public Task OnWidthChanged(int width) {
        var columns = width <= 0 ? _columns : Math.Clamp((width + GAP) / (MIN_CARD_WIDTH + GAP), 1, MAX_COLUMNS);
        if(columns == _columns) {
            return Task.CompletedTask;
        }

        _columns = columns;
        RebuildRows();
        return InvokeAsync(StateHasChanged);
    }

    private void RebuildRows() {
        _rows.Clear();
        for(var index = 0; index < _items.Length; index += _columns) {
            var length = Math.Min(_columns, _items.Length - index);
            var row = new ExplorerIconEntry[length];
            Array.Copy(_items, index, row, 0, length);
            _rows.Add(row);
        }
    }

    private bool IsSelected(ExplorerIconEntry entry) => ReferenceEquals(entry, Selected);

    private bool IsTabbable(ExplorerIconEntry entry) {
        if(Selected is not null) {
            return ReferenceEquals(entry, Selected);
        }

        return _items.Length > 0 && ReferenceEquals(_items[0], entry);
    }

    /// <summary>
    /// Handles arrow-key navigation across the virtualized grid. Focus movement is relative to the
    /// currently focused card (the grid renders only the visible window), so the handler issues a
    /// JavaScript focus move rather than tracking a global index. Selection stays on the native button
    /// activation so clicks and the keyboard share one path.
    /// </summary>
    /// <param name="args">The keyboard event raised on a card and bubbled to the grid.</param>
    /// <returns>A task that completes once focus has been moved.</returns>
    private async Task OnKeyDownAsync(KeyboardEventArgs args) {
        if(_items.Length == 0 || _module is null) {
            return;
        }

        try {
            switch(args.Key) {
                case "ArrowRight":
                    await _module.InvokeVoidAsync("moveCardFocus", 1);
                    break;
                case "ArrowLeft":
                    await _module.InvokeVoidAsync("moveCardFocus", -1);
                    break;
                case "ArrowDown":
                    await _module.InvokeVoidAsync("moveCardFocus", _columns);
                    break;
                case "ArrowUp":
                    await _module.InvokeVoidAsync("moveCardFocus", -_columns);
                    break;
                case "Home":
                    await _module.InvokeVoidAsync("moveCardFocusTo", "first");
                    break;
                case "End":
                    await _module.InvokeVoidAsync("moveCardFocusTo", "last");
                    break;
                default:
                    break;
            }
        }
        catch(JSException) {
            // Focus movement is best-effort; the roving tabindex still yields a keyboard path.
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync() {
        if(_observer is not null) {
            await TryDisposeAsync(_observer);
        }

        if(_module is not null) {
            await TryDisposeAsync(_module);
        }

        _self?.Dispose();
        GC.SuppressFinalize(this);
    }

    private static async Task TryDisposeAsync(IJSObjectReference reference) {
        try {
            await reference.DisposeAsync();
        }
        catch(JSException) {
            // The circuit may already be gone; disposal is best-effort.
        }
    }
}
