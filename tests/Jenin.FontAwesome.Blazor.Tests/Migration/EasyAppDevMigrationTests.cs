//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Tests.Migration;

/// <summary>
/// Verifies the EasyAppDev migration guide (<c>docs/migration/from-easyappdev.md</c>): every
/// mapping-table row's target side renders the documented output. Source excerpts follow the
/// EasyAppDev.Blazor.Icons.FontAwesome6 2.0.4 package docs (FA6 Free 6.7.2) as of 2026-10-26.
/// </summary>
[TestClass]
public class EasyAppDevMigrationTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task SolidComponent_MapsToSolidPresetAsync() {
        // Before: <Fa6SolidHome />
        var markup = await TestHost.RenderMarkupAsync<HouseIcon>();

        markup.Should().Contain("class=\"fa-solid fa-house\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task RegularComponent_MapsToPresetWithStyleAsync() {
        // Before: <Fa6RegularStar />
        var markup = await TestHost.RenderMarkupAsync<StarIcon>(TestHost.Parameters(
            ("IconStyle", IconStyle.Regular)));

        markup.Should().Contain("class=\"fa-regular fa-star\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task BrandsComponent_MapsToBrandsPresetAsync() {
        // Before: <Fa6BrandsDiscord />
        var markup = await TestHost.RenderMarkupAsync<DiscordIcon>();

        markup.Should().Contain("class=\"fa-brands fa-discord\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Fa5Component_MapsToSamePresetAsync() {
        // Before: <Fa5SolidHome /> — FA5 names carry over (v6 classes work unchanged in v7).
        var markup = await TestHost.RenderMarkupAsync<HouseIcon>();

        markup.Should().Contain("fa-house");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task SolidComponent_MapsToInlineSvgAsync() {
        // After (zero-asset shape): <HouseSolidIcon />
        var markup = await TestHost.RenderMarkupAsync<HouseSolidIcon>();

        markup.Should().Contain("<svg");
        markup.Should().Contain("viewBox=\"0 0 512 512\"");
        markup.Should().Contain("<path");
        markup.Should().NotContain("data-fa-i2svg");
        markup.Should().NotContain("<use");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task BrandsComponent_MapsToInlineSvgAsync() {
        // After (zero-asset shape): <GithubBrandsIcon /> for <Fa6BrandsGithub />.
        var markup = await TestHost.RenderMarkupAsync<GithubBrandsIcon>();

        markup.Should().Contain("<svg");
        markup.Should().Contain("<path");
    }
}
