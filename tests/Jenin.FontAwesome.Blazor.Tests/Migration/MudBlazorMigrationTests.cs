//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Tests.Migration;

/// <summary>
/// Verifies the MudBlazor migration guide (<c>docs/migration/from-mudblazor.md</c>): every
/// mapping-table row's target side renders the documented Font Awesome output. Source excerpts
/// follow the MudBlazor docs and <c>MudIcon.razor</c> source as of 2026-10-26.
/// </summary>
[TestClass]
public class MudBlazorMigrationTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task FontClassString_MapsToSolidIconAsync() {
        // Before: <MudIcon Icon="fas fa-dragon" />
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-dragon"),
            ("IconStyle", IconStyle.Solid)));

        Assert.Contains("class=\"fa-solid fa-dragon\"", markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task BrandsFontClass_MapsToBrandsIconAsync() {
        // Before: <MudIcon FontClass="fab fa-github" />
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-github"),
            ("IconStyle", IconStyle.Brands)));

        Assert.Contains("class=\"fa-brands fa-github\"", markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task MudSizeLarge_MapsToLargeTokenAsync() {
        // Before: <MudIcon Icon="fas fa-dragon" Size="Size.Large" />
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-dragon"),
            ("IconSize", IconSize.Large)));

        Assert.Contains("fa-lg", markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task MudPaletteColor_MapsToCssColorAsync() {
        // Before: Color="Color.Success" (theme palette) -> CSS color, e.g. a MudBlazor variable.
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-dragon"),
            ("Color", "var(--mud-palette-success)")));

        Assert.Contains("color:var(--mud-palette-success)", markup.Replace(" ", string.Empty, StringComparison.Ordinal));
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task MudTitle_MapsToMeaningfulIconAsync() {
        // Before: <MudIcon Icon="fas fa-dragon" Title="Dragon" />
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-dragon"),
            ("Title", "Dragon")));

        Assert.Contains("title=\"Dragon\"", markup);
        Assert.Contains("role=\"img\"", markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task MudIconButton_MapsToIconButtonAsync() {
        // Before: <MudIconButton Icon="fas fa-bug" Color="Color.Error" />
        var markup = await TestHost.RenderMarkupAsync<IconButton>(TestHost.Parameters(
            ("IconName", "fa-bug"),
            ("IconStyle", IconStyle.Solid),
            ("Color", "red"),
            ("AriaLabel", "Report a bug")));

        Assert.Contains("class=\"fa-solid fa-bug\"", markup);
        Assert.Contains("aria-label=\"Report a bug\"", markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task BrandsPreset_CarriesBakedStyleAsync() {
        // The preset recommended by the guide keeps the brands style without parameters.
        var markup = await TestHost.RenderMarkupAsync<GithubIcon>();

        Assert.Contains("class=\"fa-brands fa-github\"", markup);
    }
}
