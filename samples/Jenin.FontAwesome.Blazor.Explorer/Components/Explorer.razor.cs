//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Components;

/// <summary>
/// The interactive Font Awesome explorer: search, style facets, a virtualized result grid, and a
/// details pane, with the whole view addressable through the URL.
/// </summary>
/// <remarks>
/// <para>
/// The component owns the <see cref="ExplorerState"/> and is its only writer; children receive the
/// current values through parameters and report changes through callbacks. This keeps re-rendering
/// predictable (a cascading mutable object would not reliably notify children) and needs no
/// dependency-injection scope that outlives the page.
/// </para>
/// <para>
/// Typing is debounced before the catalogue is queried and before the URL is updated with a history
/// replacement; discrete changes (style facet, icon selection) update the URL with a pushed history
/// entry so the browser's back button walks the user's choices.
/// </para>
/// </remarks>
public partial class Explorer : IAsyncDisposable {
    private const int QUERY_DEBOUNCE_MS = 150;
    private const string MODULE_PATH = "./_content/Jenin.FontAwesome.Blazor.Explorer/js/explorer.js";

    private readonly ExplorerCatalogue _catalogue = ExplorerCatalogue.Default;
    private readonly ExplorerState _state = new();

    private IReadOnlyList<ExplorerIconEntry> _results = [];
    private int _queryVersion;
    private bool _initialized;
    private bool _isDark;
    private bool _helpVisible;
    private bool _themeInitialized;
    private bool _inheritsHostTheme;
    private PreviewPanel? _preview;
    private IJSObjectReference? _module;
    private DotNetObjectReference<Explorer>? _themeObserverRef;

    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if(firstRender) {
            try {
                _module = await JS.InvokeAsync<IJSObjectReference>("import", MODULE_PATH);
            }
            catch(JSException) {
                // Keyboard and theme interop are best-effort.
            }
        }

        if(_themeInitialized || _module is null) {
            return;
        }

        _themeInitialized = true;
        await InitializeThemeAsync();
    }

    private async Task InitializeThemeAsync() {
        if(_module is not { } module) {
            return;
        }

        try {
            // A host page that owns a theme of its own wins: the explorer follows it and hides its
            // own switch, so the user only ever deals with a single light/dark control.
            var hostTheme = await module.InvokeAsync<string?>("getHostTheme");
            if(!string.IsNullOrWhiteSpace(hostTheme)) {
                _inheritsHostTheme = true;
                _isDark = string.Equals(hostTheme, "dark", StringComparison.OrdinalIgnoreCase);
                await module.InvokeVoidAsync("setRootTheme", hostTheme);
                await StartHostThemeObservationAsync();
                await InvokeAsync(StateHasChanged);
                return;
            }

            var stored = await module.InvokeAsync<string?>("getStoredTheme");
            var dark = stored switch {
                "dark" => true,
                "light" => false,
                _ => await module.InvokeAsync<bool>("systemPrefersDark"),
            };

            _isDark = dark;
            await module.InvokeVoidAsync("applyTheme", stored);
            await InvokeAsync(StateHasChanged);
        }
        catch(JSException) {
            // Theme initialisation is best-effort; the CSS colour-scheme media query still applies.
        }
    }

    private async Task StartHostThemeObservationAsync() {
        if(_module is null) {
            return;
        }

        try {
            _themeObserverRef = DotNetObjectReference.Create(this);
            await _module.InvokeVoidAsync("observeHostTheme", _themeObserverRef);
        }
        catch(JSException) {
            // Observation is best-effort; the theme still applies from the initial value.
        }
    }

    /// <summary>
    /// Called from JavaScript when the host page changes the theme it publishes on the document
    /// element. The explorer mirrors the change when it inherits the host theme.
    /// </summary>
    /// <param name="theme">The host theme, or <c>null</c> when the host cleared it.</param>
    /// <returns>A task that completes when the explorer has caught up with the host.</returns>
    [JSInvokable]
    public async Task OnHostThemeChanged(string? theme) {
        if(!_inheritsHostTheme) {
            return;
        }

        var dark = string.Equals(theme, "dark", StringComparison.OrdinalIgnoreCase);
        if(dark == _isDark) {
            return;
        }

        _isDark = dark;

        if(_module is not null) {
            try {
                await _module.InvokeVoidAsync("setRootTheme", theme);
            }
            catch(JSException) {
                // Applying the theme is best-effort.
            }
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task OnThemeChangedAsync(bool dark) {
        if(_inheritsHostTheme) {
            // The host owns the theme, so the explorer's own switch stays out of the way.
            return;
        }

        _isDark = dark;
        if(_module is not null) {
            try {
                await _module.InvokeVoidAsync("applyTheme", dark ? "dark" : "light");
            }
            catch(JSException) {
                // Applying the theme is best-effort.
            }
        }
    }

    private Task OpenHelpAsync() {
        _helpVisible = true;
        return Task.CompletedTask;
    }

    private Task CloseHelpAsync() {
        _helpVisible = false;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the explorer-wide keyboard shortcuts. Keys pressed inside a text field are ignored so
    /// typing (including the plain <c>C</c> character) is never intercepted; the search field owns its
    /// own <c>Escape</c> handling.
    /// </summary>
    /// <param name="args">The keyboard event bubbled to the explorer root.</param>
    /// <returns>A task that completes when the shortcut has been handled.</returns>
    private async Task OnGlobalKeyDownAsync(KeyboardEventArgs args) {
        if(_module is null) {
            return;
        }

        try {
            if(await _module.InvokeAsync<bool>("isTypingTarget")) {
                return;
            }
        }
        catch(JSException) {
            return;
        }

        switch(args.Key) {
            case "/":
                await FocusSearchAsync();
                break;
            case "?":
                _helpVisible = !_helpVisible;
                StateHasChanged();
                break;
            case "c":
            case "C":
                await CopySnippetAsync();
                break;
            default:
                break;
        }
    }

    private async Task FocusSearchAsync() {
        try {
            await _module!.InvokeVoidAsync("focusSearch", "ctl-query");
        }
        catch(JSException) {
            // Focus movement is best-effort.
        }
    }

    private Task CopySnippetAsync() => _preview?.CopySnippetAsync() ?? Task.CompletedTask;

    /// <inheritdoc />
    protected override void OnParametersSet() {
        var route = ExplorerRoute.Parse(Navigation.ToAbsoluteUri(Navigation.Uri).Query);
        if(_initialized && RouteMatches(route)) {
            return;
        }

        _initialized = true;
        _state.Query = route.Query;
        _state.Style = route.Style;
        _state.Selected = _catalogue.Find(route.Icon);

        if(_state.Selected is { } selected && !selected.SupportsStyle(_state.Preview.Style)) {
            _state.Preview.Style = selected.PrimaryStyle;
        }

        RecomputeResults();
    }

    private bool RouteMatches(ExplorerRouteState route) =>
        string.Equals(route.Query, _state.Query, StringComparison.Ordinal)
        && route.Style == _state.Style
        && string.Equals(route.Icon, _state.Selected?.Name, StringComparison.Ordinal);

    private async Task OnQueryChangedAsync(string? query) {
        _state.Query = query?.Trim() ?? string.Empty;
        var version = ++_queryVersion;

        await Task.Delay(QUERY_DEBOUNCE_MS);
        if(version != _queryVersion) {
            return;
        }

        RecomputeResults();
        SyncUrl(replace: true);
    }

    private Task OnStyleChangedAsync(IconStyle? style) {
        _state.Style = style;
        RecomputeResults();
        SyncUrl(replace: false);
        return Task.CompletedTask;
    }

    private Task OnSelectAsync(ExplorerIconEntry entry) {
        var changed = !ReferenceEquals(_state.Selected, entry)
                      && !string.Equals(_state.Selected?.Name, entry.Name, StringComparison.Ordinal);

        _state.Selected = entry;

        // Selecting a different icon starts from the defaults so options configured for the previous
        // icon never carry over to the new one.
        if(changed) {
            _state.Preview.Reset();
        }

        // Seed the preview style to the icon's primary Free style when the current one is not
        // available for it, so the preview and its snippet stay meaningful across selections.
        if(!entry.SupportsStyle(_state.Preview.Style)) {
            _state.Preview.Style = entry.PrimaryStyle;
        }

        SyncUrl(replace: false);
        return Task.CompletedTask;
    }

    private void RecomputeResults() => _results = _catalogue.Search(_state.Query, _state.Style);

    private void SyncUrl(bool replace) {
        var target = ExplorerRoute.Build(Navigation, _state.ToRouteState());
        if(string.Equals(target, Navigation.Uri, StringComparison.OrdinalIgnoreCase)) {
            return;
        }

        Navigation.NavigateTo(target, forceLoad: false, replace: replace);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync() {
        _themeObserverRef?.Dispose();

        if(_module is not null) {
            try {
                await _module.DisposeAsync();
            }
            catch(JSException) {
                // The circuit may already be gone; disposal is best-effort.
            }
        }

        GC.SuppressFinalize(this);
    }
}
