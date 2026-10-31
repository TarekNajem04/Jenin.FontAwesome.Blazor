//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class IconContainerTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task EmptyContainer_RendersDivRootAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconContainer>();

        markup.Should().Be("<div></div>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Container_RendersCascadedChildIconsAsync() {
        RenderFragment icons = builder => {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, "IconName", "fa-star");
            builder.CloseComponent();
        };

        var markup = await TestHost.RenderMarkupAsync<IconContainer>(TestHost.Parameters(("Icons", icons)));

        markup.Should().Be("<div><span><i class=\"fa-solid fa-star\" aria-hidden=\"true\" role=\"presentation\"></i></span></div>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Container_AppliesCssClassToRootAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconContainer>(TestHost.Parameters(("CssClass", "wrap")));

        markup.Should().Be("<div class=\"wrap\"></div>");
    }
}
