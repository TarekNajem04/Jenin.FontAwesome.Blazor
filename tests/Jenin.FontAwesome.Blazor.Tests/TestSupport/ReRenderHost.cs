//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Tests.TestSupport;

/// <summary>
/// A minimal test-only host that renders an inner component from a parameter dictionary and can be
/// asked to re-render it with a new dictionary. The built-in <see cref="Microsoft.AspNetCore.Components.Web.HtmlRenderer"/>
/// returns a root component whose parameters cannot be re-applied, so this wrapper captures the inner
/// component instance and drives it through <see cref="IComponent.SetParametersAsync(ParameterView)"/>
/// directly. That path (unlike a null render-tree attribute) applies a parameter that is explicitly set
/// to <c>null</c>, which the tests need to assert that a cleared value is no longer emitted.
/// </summary>
/// <typeparam name="TComponent">The component type to render.</typeparam>
internal sealed class ReRenderHost<TComponent> : ComponentBase where TComponent : IComponent {
    private TComponent? _inner;

    [Parameter]
    public Dictionary<string, object?> InnerParameters { get; set; } = [];

    [Parameter]
    public Action<ReRenderHost<TComponent>>? Register { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized() => Register?.Invoke(this);

    /// <summary>
    /// Applies a new parameter set to the inner component and re-renders.
    /// </summary>
    /// <param name="parameters">The parameters to apply.</param>
    /// <returns>A task that completes when the re-render has been processed.</returns>
    public async Task UpdateAsync(Dictionary<string, object?> parameters) {
        InnerParameters = parameters;

        if(_inner is not null) {
            await _inner.SetParametersAsync(ParameterView.FromDictionary(parameters));
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenComponent<TComponent>(0);
        builder.AddMultipleAttributes(1, InnerParameters!);
        builder.AddComponentReferenceCapture(2, instance => _inner = (TComponent)instance);
        builder.CloseComponent();
    }
}
