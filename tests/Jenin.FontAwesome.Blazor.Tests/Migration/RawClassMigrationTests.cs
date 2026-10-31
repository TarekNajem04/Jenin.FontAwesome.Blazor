//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Tests.Migration;

/// <summary>
/// Verifies the raw-classes migration guide (<c>docs/migration/from-raw-fontawesome.md</c>):
/// every mapping-table row's target side renders the documented Font Awesome output. Source
/// anchors follow Font Awesome v6/v7 class syntax (v6 names render unchanged in v7 per the
/// official upgrade guide).
/// </summary>
[TestClass]
public class RawClassMigrationTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task StylePrefixes_MapToIconStyleAsync() {
        // Before: <i class="fa-solid fa-house"> / fa-regular / fa-brands
        var solid = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("IconStyle", IconStyle.Solid)));
        var regular = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-star"),
            ("IconStyle", IconStyle.Regular)));
        var brands = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-github"),
            ("IconStyle", IconStyle.Brands)));

        solid.Should().Contain("class=\"fa-solid fa-house\"");
        regular.Should().Contain("class=\"fa-regular fa-star\"");
        brands.Should().Contain("class=\"fa-brands fa-github\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task SizeClasses_MapToIconSizeAsync() {
        // Before: fa-2x, fa-lg, fa-sm
        var x2 = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("IconSize", IconSize.X02)));
        var large = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("IconSize", IconSize.Large)));
        var small = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("IconSize", IconSize.Small)));

        x2.Should().Contain("fa-2x");
        large.Should().Contain("fa-lg");
        small.Should().Contain("fa-sm");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task SpinClass_MapsToAnimationFactoryAsync() {
        // Before: <i class="fa-solid fa-spinner fa-spin">
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-spinner"),
            ("Animation", Animate.Spin())));

        markup.Should().Contain("fa-spin");
        markup.Should().Contain("fa-solid fa-spinner");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task TitleAttribute_MapsToMeaningfulIconAsync() {
        // Before: <i class="fa-solid fa-house fa-2x" title="Home">
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("IconStyle", IconStyle.Solid),
            ("IconSize", IconSize.X02),
            ("Title", "Home")));

        markup.Should().Be(
            "<span><i class=\"fa-2x fa-solid fa-house\" title=\"Home\" role=\"img\" aria-hidden=\"false\"></i></span>"
        );
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task RepeatedIcon_MapsToPresetOrConstantAsync() {
        // Before: "fa-house" pasted in 12 places — one preset / one constant instead.
        var preset = await TestHost.RenderMarkupAsync<HouseIcon>();
        var constant = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", IconNames.Solid.House)));

        preset.Should().Contain("class=\"fa-solid fa-house\"");
        constant.Should().Contain("class=\"fa-solid fa-house\"");
    }
}
