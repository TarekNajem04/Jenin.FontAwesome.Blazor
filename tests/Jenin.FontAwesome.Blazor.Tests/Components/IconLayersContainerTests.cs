//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class IconLayersContainerTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task EmptyContainer_RendersSpanWithFa_layersClassAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconLayersContainer>();

        Assert.AreEqual("<span class=\"fa-layers\"></span>", markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Container_RendersLayeredChildrenAsync() {
        RenderFragment icons = builder => {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, "IconName", "fa-circle");
            builder.CloseComponent();
        };

        var markup = await TestHost.RenderMarkupAsync<IconLayersContainer>(TestHost.Parameters(("Icons", icons)));

        Assert.AreEqual("<span class=\"fa-layers\"><span><i class=\"fa-solid fa-circle\" aria-hidden=\"true\" role=\"presentation\"></i></span></span>", markup);
    }
}
