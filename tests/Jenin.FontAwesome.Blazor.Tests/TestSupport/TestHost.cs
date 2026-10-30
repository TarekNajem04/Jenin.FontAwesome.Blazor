//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.TestSupport;

/// <summary>
/// First-party rendering helper. Uses the built-in <see cref="HtmlRenderer"/> (static server-side
/// rendering) so markup, classes, styles, attributes, and structure can be asserted without a
/// browser, a JavaScript runtime, or any third-party test host.
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

    /// <summary>
    /// Renders the component, applies each successive parameter set in order, and returns the markup
    /// after the final set. Use this to assert that a component updates correctly across re-renders
    /// (for example that a style or class removed on a later render is no longer emitted) rather than
    /// only on its first render.
    /// </summary>
    /// <typeparam name="TComponent">The component type to render.</typeparam>
    /// <param name="parameterSets">The parameter sets applied in order; the last one is asserted.</param>
    /// <returns>The markup after the final parameter set has been applied.</returns>
    public static async Task<string> RenderMarkupSequenceAsync<TComponent>(params IDictionary<string, object?>[] parameterSets)
        where TComponent : IComponent {
        ArgumentNullException.ThrowIfNull(parameterSets);

        if(parameterSets.Length == 0) {
            throw new ArgumentException("At least one parameter set is required.", nameof(parameterSets));
        }

        var services = new ServiceCollection();
        await using var provider = services.BuildServiceProvider();
        await using var renderer = new HtmlRenderer(provider, NullLoggerFactory.Instance);

        return await renderer.Dispatcher.InvokeAsync(async () => {
            ReRenderHost<TComponent>? host = null;
            var first = ParameterView.FromDictionary(new Dictionary<string, object?>(StringComparer.Ordinal) {
                ["InnerParameters"] = Copy(parameterSets[0]),
                ["Register"] = (Action<ReRenderHost<TComponent>>)(instance => host = instance),
            });

            var output = await renderer.RenderComponentAsync<ReRenderHost<TComponent>>(first);

            for(var index = 1; index < parameterSets.Length; index++) {
                await host!.UpdateAsync(Copy(parameterSets[index]));
            }

            return output.ToHtmlString();
        });

        static Dictionary<string, object?> Copy(IDictionary<string, object?> source) {
            var copy = new Dictionary<string, object?>(source.Count, StringComparer.Ordinal);

            foreach(var (key, value) in source) {
                copy[key] = value;
            }

            return copy;
        }
    }
}
