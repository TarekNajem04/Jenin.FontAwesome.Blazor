//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.MudBlazor.Tests;

[TestClass]
public class MudIconMapperTests {
    [TestMethod]
    [TestCategory("Model")]
    [DataRow("fa-house", IconStyle.Solid, "fa-solid fa-house")]
    [DataRow("house", IconStyle.Solid, "fa-solid fa-house")]
    [DataRow("fa-house", IconStyle.Regular, "fa-regular fa-house")]
    [DataRow("fa-house", IconStyle.Light, "fa-light fa-house")]
    [DataRow("fa-house", IconStyle.Thin, "fa-thin fa-house")]
    [DataRow("fa-house", IconStyle.Duotone, "fa-duotone fa-house")]
    [DataRow("fa-github", IconStyle.Brands, "fa-brands fa-github")]
    [DataRow("fa-acme-widget", IconStyle.Solid, "fa-solid fa-acme-widget")]
    public void ToFontClass_MapsNameAndStyle(string iconName, IconStyle style, string expected) {
        Assert.AreEqual(expected, MudIconMapper.ToFontClass(iconName, style));
    }

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void ToFontClass_RejectsEmpty(string? iconName) {
        Assert.AreEqual(string.Empty, MudIconMapper.ToFontClass(iconName));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void ToMudSize_MapsOverlappingSteps() {
        Assert.AreEqual(global::MudBlazor.Size.Small, MudIconMapper.ToMudSize(IconSize.Small));
        Assert.AreEqual(global::MudBlazor.Size.Large, MudIconMapper.ToMudSize(IconSize.Large));
        Assert.AreEqual(global::MudBlazor.Size.Medium, MudIconMapper.ToMudSize(IconSize.NotSet) ?? global::MudBlazor.Size.Medium);
    }

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(IconSize.X02)]
    [DataRow(IconSize.X10)]
    [DataRow(IconSize.ExtraSmall)]
    [DataRow(null)]
    public void ToMudSize_ReturnsNullOutsideOverlap(IconSize? size) {
        Assert.IsNull(MudIconMapper.ToMudSize(size));
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task JeninMudIcon_RendersMappedMudIconAsync() {
        var markup = await TestHost.RenderMarkupAsync<JeninMudIcon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("IconStyle", IconStyle.Solid),
            ("Title", "Home")));

        Assert.Contains("mud-icon-root", markup);
        Assert.Contains("fa-solid fa-house", markup);
        Assert.Contains("title=\"Home\"", markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task JeninMudIcon_PassesThroughBrandsAsync() {
        var markup = await TestHost.RenderMarkupAsync<JeninMudIcon>(TestHost.Parameters(
            ("IconName", "github"),
            ("IconStyle", IconStyle.Brands)));

        Assert.Contains("fa-brands fa-github", markup);
    }
}
