//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class TextLayerTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task RendersBoundarySpanWrappingLayeredTextAsync() {
        var markup = await TestHost.RenderMarkupAsync<TextLayer>(TestHost.Parameters(("Text", "hi")));

        markup.Should().Be("<span><span class=\"fa-layers-text\">hi</span></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Text_IsTrimmedAsync() {
        var markup = await TestHost.RenderMarkupAsync<TextLayer>(TestHost.Parameters(("Text", "  hi  ")));

        markup.Should().Be("<span><span class=\"fa-layers-text\">hi</span></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task WithoutText_RendersEmptyLayeredTextElementAsync() {
        var markup = await TestHost.RenderMarkupAsync<TextLayer>();

        markup.Should().Be("<span><span class=\"fa-layers-text\"></span></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Transform_EmitsDataAttributeAsync() {
        var markup = await TestHost.RenderMarkupAsync<TextLayer>(TestHost.Parameters(
            ("Text", "hi"),
            ("Transform", new Transform().Grow(2))));

        markup.Should().Contain("data-fa-transform=\"grow-2.00\"");
    }
}
