//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Sample.Shared.JsInterop;

public static class PrismUtilities {
    public static ValueTask HighlightAllAsync(this IJSRuntime jSRuntime, bool runAsync)
        => jSRuntime.InvokeVoidAsync("PrismUtilities.highlightAll", runAsync, null);

    public static ValueTask HighlightAllUnderAsync(this IJSRuntime jSRuntime, string containerId, bool runAsync)
        => jSRuntime.InvokeVoidAsync("PrismUtilities.highlightAllUnderById", containerId, runAsync, null);

    public static ValueTask HighlightElementAsync(this IJSRuntime jSRuntime, string elementId, bool runAsync)
        => jSRuntime.InvokeVoidAsync("PrismUtilities.highlightElementById", elementId, runAsync, null);
}
