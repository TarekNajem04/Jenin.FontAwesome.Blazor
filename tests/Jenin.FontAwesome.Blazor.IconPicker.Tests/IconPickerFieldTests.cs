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
            ("ValueChanged", EventCallback.Factory.Create<string?>(new object(), value => model.Icon = value)),
            ("ValueExpression", (Expression<Func<string?>>)(() => model.Icon)));

    [TestMethod]
    public async Task RendersComboboxWithoutOpeningAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPickerField>(BoundParameters(new FormModel()));

        markup.Should().Contain("role=\"combobox\"");
        markup.Should().Contain("Search icons...");
        markup.Contains("listbox", StringComparison.Ordinal).Should().BeFalse();
    }

    [TestMethod]
    public async Task RendersPreviewAndClearForInitialValueAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPickerField>(BoundParameters(new FormModel { Icon = "fa-house" }));

        markup.Should().Contain("value=\"fa-house\"");
        markup.Should().Contain("fa-house");
        markup.Should().Contain("Clear selection");
    }

    [TestMethod]
    public async Task EmptyValue_RendersNoPreviewOrClearAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPickerField>(BoundParameters(new FormModel()));

        markup.Contains("fa-icon-picker-field__preview", StringComparison.Ordinal).Should().BeFalse();
        markup.Contains("fa-icon-picker-field__clear", StringComparison.Ordinal).Should().BeFalse();
    }
}
