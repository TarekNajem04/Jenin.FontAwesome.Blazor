//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class IconForTests {
    private enum ShipmentStatus {
        [Icon("fa-clock")]
        Pending,

        [Icon("fa-truck", Style = IconStyle.Regular, Color = "blue")]
        Shipped,

        Cancelled,
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task DeclaredName_RendersGlyphAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconFor<ShipmentStatus>>(TestHost.Parameters(
            ("Value", ShipmentStatus.Pending)));

        markup.Should().Be(
            "<span><i class=\"fa-solid fa-clock\" aria-hidden=\"true\" role=\"presentation\"></i></span>"
        );
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task DeclaredStyleAndColor_FlowThroughAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconFor<ShipmentStatus>>(TestHost.Parameters(
            ("Value", ShipmentStatus.Shipped)));

        markup.Should().Be(
            "<span><i class=\"fa-regular fa-truck\" style=\"color:blue;\" aria-hidden=\"true\" role=\"presentation\"></i></span>"
        );
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task UnmappedValue_RendersFallbackAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconFor<ShipmentStatus>>(TestHost.Parameters(
            ("Value", ShipmentStatus.Cancelled)));

        markup.Should().Be(
            "<span><i class=\"fa-solid fa-circle-question\" aria-hidden=\"true\" role=\"presentation\"></i></span>"
        );
    }
}
