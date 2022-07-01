using Microsoft.JSInterop;

namespace Jenin.FontAwesome.Blazor.Sample.JsInterop;

public static class PrismUtilities {
    public static ValueTask HighlightAllAsync(this IJSRuntime jSRuntime, bool runAsync)
        => jSRuntime.InvokeVoidAsync("PrismUtilities.highlightAll", runAsync, null);

    public static ValueTask HighlightAllUnderAsync(this IJSRuntime jSRuntime, string containerId, bool runAsync)
        => jSRuntime.InvokeVoidAsync("PrismUtilities.highlightAllUnderById", containerId, runAsync, null);

    public static ValueTask HighlightElementAsync(this IJSRuntime jSRuntime, string elementId, bool runAsync)
        => jSRuntime.InvokeVoidAsync("PrismUtilities.highlightElementById", elementId, runAsync, null);
}
