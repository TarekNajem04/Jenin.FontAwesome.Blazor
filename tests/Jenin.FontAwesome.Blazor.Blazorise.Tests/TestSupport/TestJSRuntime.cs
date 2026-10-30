//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Blazorise.Tests.TestSupport;

/// <summary>
/// No-op <see cref="IJSRuntime"/> so Blazorise components render statically without a JS
/// runtime. Any invoked call returns the default value.
/// </summary>
internal sealed class TestJSRuntime : IJSRuntime {
    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => new(default(TValue)!);

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => new(default(TValue)!);
}
