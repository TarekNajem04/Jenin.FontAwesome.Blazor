//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class IconStackContainerTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task EmptyContainer_RendersSpanWithFa_stackClassAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconStackContainer>();

        markup.Should().Be("<span class=\"fa-stack\"></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task VerticalAlign_EmitsStyleAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconStackContainer>(TestHost.Parameters(("VerticalAlign", "middle")));

        markup.Should().Be("<span class=\"fa-stack\" style=\"vertical-align:middle;\"></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Container_RendersStackedChildrenAsync() {
        RenderFragment icons = builder => {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, "IconName", "fa-circle");
            builder.AddAttribute(2, "StackSize", IconStackSize.Larger);
            builder.CloseComponent();
        };

        var markup = await TestHost.RenderMarkupAsync<IconStackContainer>(TestHost.Parameters(("Icons", icons)));

        markup.Should().Be(
            "<span class=\"fa-stack\"><span><i class=\"fa-solid fa-circle fa-stack-2x\" aria-hidden=\"true\" role=\"presentation\"></i></span></span>"
        );
    }
}
