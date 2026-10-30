//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class IconListItemTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task RendersListItemWithFa_liSpanAndContentAsync() {
        RenderFragment icon = builder => {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, "IconName", "fa-check");
            builder.CloseComponent();
        };

        var markup = await TestHost.RenderMarkupAsync<IconListItem>(TestHost.Parameters(
            ("ItemIcon", icon),
            ("ItemContent", (RenderFragment)(builder => builder.AddContent(0, "Content")))));

        Assert.AreEqual(
            "<li><span class=\"fa-li\"><span><i class=\"fa-solid fa-check\" aria-hidden=\"true\" role=\"presentation\"></i></span></span>Content</li>",
            markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ItemMarginAndWidth_EmitCustomPropertiesAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconListItem>(TestHost.Parameters(
            ("ItemMargin", "1em"),
            ("ItemWidth", "1em")));

        Assert.AreEqual(
            "<li><span class=\"fa-li\" style=\"--fa-li-margin:1em;--fa-li-width:1em;\"></span></li>",
            markup);
    }
}
