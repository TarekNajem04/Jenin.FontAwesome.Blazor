namespace Jenin.FontAwesome.Blazor.JsInterop;

internal static class FontAwesomeConfigUtilities {
    public static async ValueTask<string> SetAutoReplaceSvgAsync(this IJSRuntime jSRuntime, string value) {
        var old = await jSRuntime.InvokeAsync<string>("FontAwesomeConfigUtilities.getAutoReplaceSvg");
        await jSRuntime.InvokeVoidAsync("FontAwesomeConfigUtilities.setAutoReplaceSvg", value);

        return old;
    }

    public static ValueTask<string> GetAutoReplaceSvgAsync(this IJSRuntime jSRuntime)
        => jSRuntime.InvokeAsync<string>("FontAwesomeConfigUtilities.getAutoReplaceSvg");
}
