//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class CounterLayerTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task RendersLayeredCounterSpanAsync() {
        var markup = await TestHost.RenderMarkupAsync<CounterLayer>(TestHost.Parameters(("Text", "42")));

        Assert.AreEqual("<span class=\"fa-layers-counter\">42</span>", markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Text_IsTrimmedAsync() {
        var markup = await TestHost.RenderMarkupAsync<CounterLayer>(TestHost.Parameters(("Text", "  7  ")));

        Assert.AreEqual("<span class=\"fa-layers-counter\">7</span>", markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Offsets_AndDimensions_EmitCustomPropertiesAsync() {
        var markup = await TestHost.RenderMarkupAsync<CounterLayer>(TestHost.Parameters(
            ("Text", "1"),
            ("LineHeight", "1"),
            ("MaxWidth", "4em"),
            ("MinWidth", "1em"),
            ("Scale", "2"),
            ("Top", "0"),
            ("Right", "0"),
            ("Bottom", "0"),
            ("Left", "0")));

        Assert.AreEqual(
            "<span class=\"fa-layers-counter\" style=\"--fa-counter-line-height:1;--fa-counter-max-width:4em;--fa-counter-min-width:1em;--fa-counter-scale:2;--fa-top:0;--fa-right:0;--fa-bottom:0;--fa-left:0;\">1</span>",
            markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task DocumentedTunables_EmitCustomPropertiesAsync() {
        var markup = await TestHost.RenderMarkupAsync<CounterLayer>(TestHost.Parameters(
            ("Text", "1"),
            ("CounterBackgroundColor", "Tomato"),
            ("CounterBorderRadius", "0.25em"),
            ("CounterPadding", "0.2em")));

        Assert.AreEqual(
            "<span class=\"fa-layers-counter\" style=\"--fa-counter-background-color:Tomato;--fa-counter-border-radius:0.25em;--fa-counter-padding:0.2em;\">1</span>",
            markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task WithoutText_RendersEmptyCounterAsync() {
        var markup = await TestHost.RenderMarkupAsync<CounterLayer>();

        Assert.AreEqual("<span class=\"fa-layers-counter\"></span>", markup);
    }
}
