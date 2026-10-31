//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Tests.Migration;

/// <summary>
/// Verifies the Blazicons migration guide (<c>docs/migration/from-blazicons.md</c>): every
/// mapping-table row's target side renders the documented output. Source excerpts follow the
/// Blazicons / Blazicons.FontAwesome readmes as of 2026-10-26.
/// </summary>
[TestClass]
public class BlaziconsMigrationTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task SolidValue_MapsToSolidComponentAsync() {
        // Before: <Blazicon Svg="FontAwesomeSolidIcon.Baseball" />
        var markup = await TestHost.RenderMarkupAsync<BaseballSolidIcon>();

        markup.Should().Contain("<svg");
        markup.Should().Contain("viewBox=\"0 0 512 512\"");
        markup.Should().Contain("<path");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task RegularValue_MapsToRegularComponentAsync() {
        // Before: <Blazicon Svg="FontAwesomeRegularIcon.Calendar" />
        var markup = await TestHost.RenderMarkupAsync<CalendarRegularIcon>();

        markup.Should().Contain("<svg");
        markup.Should().Contain("<path");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task FluentSizing_MapsToStylePassthroughAsync() {
        // Before: Svg.WithColor("#f00").WithSize("200%") — style passthrough instead.
        var markup = await TestHost.RenderMarkupAsync<HouseSolidIcon>(TestHost.Parameters(
            ("style", "color: #f00; font-size: 200%;")));

        markup.Should().Contain("color: #f00; font-size: 200%;");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Sizing_InheritsWithExplicitOverrideAsync() {
        // Default 1em height like Blazicon; explicit width/height win when passed.
        var markup = await TestHost.RenderMarkupAsync<HouseSolidIcon>();
        var sized = await TestHost.RenderMarkupAsync<HouseSolidIcon>(TestHost.Parameters(
            ("width", "64"),
            ("height", "64")));

        markup.Should().Contain("height=\"1em\"");
        markup.Should().NotContain("width=");
        sized.Should().Contain("width=\"64\"");
        sized.Should().Contain("height=\"64\"");
    }
}
