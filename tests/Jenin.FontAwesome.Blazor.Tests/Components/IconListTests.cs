//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class IconListTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task List_RendersUnorderedListWithFa_ulClassAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconList>();

        markup.Should().Be("<ul class=\"fa-ul\"></ul>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task OrderedList_RendersOrderedListAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconList>(TestHost.Parameters(("OrderedList", true)));

        markup.Should().Be("<ol class=\"fa-ul\"></ol>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ListItemMarginAndWidth_EmitCustomPropertiesAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconList>(TestHost.Parameters(
            ("ListItemMargin", "0.5em"),
            ("ListItemWidth", "1.25em")));

        markup.Should().Be("<ul class=\"fa-ul\" style=\"--fa-li-margin:0.5em;--fa-li-width:1.25em;\"></ul>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task List_RendersItemsAsync() {
        RenderFragment items = builder => {
            builder.OpenComponent<IconListItem>(0);
            builder.AddAttribute(1, "ItemIcon", (RenderFragment)(b => {
                b.OpenComponent<Icon>(0);
                b.AddAttribute(1, "IconName", "fa-check");
                b.CloseComponent();
            }));
            builder.AddAttribute(2, "ItemContent", (RenderFragment)(b => b.AddContent(0, "Item")));
            builder.CloseComponent();
        };

        var markup = await TestHost.RenderMarkupAsync<IconList>(TestHost.Parameters(("Items", items)));

        markup.Should().Be(
            "<ul class=\"fa-ul\"><li><span class=\"fa-li\"><span><i class=\"fa-solid fa-check\" aria-hidden=\"true\" role=\"presentation\"></i></span></span>Item</li></ul>"
        );
    }
}
