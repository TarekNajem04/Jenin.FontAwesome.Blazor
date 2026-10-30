//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.IconPicker.Tests;

[TestClass]
public sealed class IconPickerFieldTests {
    private sealed class FormModel {
        public string? Icon { get; set; }
    }

    private static IDictionary<string, object?> BoundParameters(FormModel model) =>
        TestHost.Parameters(
            ("Value", model.Icon),
            ("ValueChanged", EventCallback.Factory.Create<string?>(new object(), (string? value) => model.Icon = value)),
            ("ValueExpression", (Expression<Func<string?>>)(() => model.Icon)));

    [TestMethod]
    public async Task RendersComboboxWithoutOpeningAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPickerField>(BoundParameters(new FormModel()));

        Assert.Contains("role=\"combobox\"", markup);
        Assert.Contains("Search icons...", markup);
        Assert.IsFalse(markup.Contains("listbox", StringComparison.Ordinal));
    }

    [TestMethod]
    public async Task RendersPreviewAndClearForInitialValueAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPickerField>(BoundParameters(new FormModel { Icon = "fa-house" }));

        Assert.Contains("value=\"fa-house\"", markup);
        Assert.Contains("fa-house", markup);
        Assert.Contains("Clear selection", markup);
    }

    [TestMethod]
    public async Task EmptyValue_RendersNoPreviewOrClearAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPickerField>(BoundParameters(new FormModel()));

        Assert.IsFalse(markup.Contains("fa-icon-picker-field__preview", StringComparison.Ordinal));
        Assert.IsFalse(markup.Contains("fa-icon-picker-field__clear", StringComparison.Ordinal));
    }
}
