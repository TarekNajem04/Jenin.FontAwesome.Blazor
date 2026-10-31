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
    public void ToFontClass_MapsNameAndStyle(string iconName, IconStyle style, string expected) => MudIconMapper.ToFontClass(iconName, style).Should().Be(expected);

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void ToFontClass_RejectsEmpty(string? iconName) => MudIconMapper.ToFontClass(iconName).Should().Be(string.Empty);

    [TestMethod]
    [TestCategory("Model")]
    public void ToMudSize_MapsOverlappingSteps() {
        MudIconMapper.ToMudSize(IconSize.Small).Should().Be(global::MudBlazor.Size.Small);
        MudIconMapper.ToMudSize(IconSize.Large).Should().Be(global::MudBlazor.Size.Large);
        (MudIconMapper.ToMudSize(IconSize.NotSet) ?? global::MudBlazor.Size.Medium).Should().Be(global::MudBlazor.Size.Medium);
    }

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(IconSize.X02)]
    [DataRow(IconSize.X10)]
    [DataRow(IconSize.ExtraSmall)]
    [DataRow(null)]
    public void ToMudSize_ReturnsNullOutsideOverlap(IconSize? size) => MudIconMapper.ToMudSize(size).Should().BeNull();

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task JeninMudIcon_RendersMappedMudIconAsync() {
        var markup = await TestHost.RenderMarkupAsync<JeninMudIcon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("IconStyle", IconStyle.Solid),
            ("Title", "Home")));

        markup.Should().Contain("mud-icon-root");
        markup.Should().Contain("fa-solid fa-house");
        markup.Should().Contain("title=\"Home\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task JeninMudIcon_PassesThroughBrandsAsync() {
        var markup = await TestHost.RenderMarkupAsync<JeninMudIcon>(TestHost.Parameters(
            ("IconName", "github"),
            ("IconStyle", IconStyle.Brands)));

        markup.Should().Contain("fa-brands fa-github");
    }
}
