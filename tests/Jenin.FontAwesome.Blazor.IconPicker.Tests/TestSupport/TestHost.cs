//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.IconPicker.Tests.TestSupport;

/// <summary>
/// First-party rendering helper for the IconPicker tests. Uses the built-in <see cref="HtmlRenderer"/>
/// (static server-side rendering) so markup, classes, and structure can be asserted without a browser,
/// a JavaScript runtime, or any third-party test host.
/// </summary>
internal static class TestHost {
    public static Task<string> RenderMarkupAsync<TComponent>(IDictionary<string, object?>? parameters = null)
        where TComponent : IComponent
        => RenderMarkupAsync<TComponent>(parameters, configureServices: null);

    public static async Task<string> RenderMarkupAsync<TComponent>(IDictionary<string, object?>? parameters, Action<IServiceCollection>? configureServices)
        where TComponent : IComponent {
        var services = new ServiceCollection();
        configureServices?.Invoke(services);
        await using var provider = services.BuildServiceProvider();
        await using var renderer = new HtmlRenderer(provider, NullLoggerFactory.Instance);

        return await renderer.Dispatcher.InvokeAsync(async () => {
            var view = ParameterView.FromDictionary(parameters ?? new Dictionary<string, object?>());
            var output = await renderer.RenderComponentAsync<TComponent>(view);

            return output.ToHtmlString();
        });
    }

    public static IDictionary<string, object?> Parameters(params (string Key, object? Value)[] values) {
        var parameters = new Dictionary<string, object?>();

        foreach(var (key, value) in values) {
            parameters[key] = value;
        }

        return parameters;
    }
}
