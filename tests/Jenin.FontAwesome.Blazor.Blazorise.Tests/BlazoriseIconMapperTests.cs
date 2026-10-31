//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Blazorise.Tests;

[TestClass]
public class BlazoriseIconMapperTests {
    [TestMethod]
    [TestCategory("Model")]
    [DataRow("fa-phone", "fa-phone")]
    [DataRow("phone", "fa-phone")]
    [DataRow("fa-acme-widget", "fa-acme-widget")]
    public void ToIconName_NormalizesToFaForm(string iconName, string expected) => BlazoriseIconMapper.ToIconName(iconName).Should().Be(expected);

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(null)]
    [DataRow("")]
    public void ToIconName_RejectsEmpty(string? iconName) => BlazoriseIconMapper.ToIconName(iconName).Should().Be(string.Empty);

    [TestMethod]
    [TestCategory("Model")]
    public void ToBlazoriseStyle_MapsSharedStyles() {
        BlazoriseIconMapper.ToBlazoriseStyle(Jenin.FontAwesome.Blazor.IconStyle.Solid).Should().Be(global::Blazorise.IconStyle.Solid);
        BlazoriseIconMapper.ToBlazoriseStyle(Jenin.FontAwesome.Blazor.IconStyle.Regular).Should().Be(global::Blazorise.IconStyle.Regular);
        BlazoriseIconMapper.ToBlazoriseStyle(Jenin.FontAwesome.Blazor.IconStyle.Light).Should().Be(global::Blazorise.IconStyle.Light);
        BlazoriseIconMapper.ToBlazoriseStyle(Jenin.FontAwesome.Blazor.IconStyle.Duotone).Should().Be(global::Blazorise.IconStyle.DuoTone);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void ToBlazoriseStyle_ReturnsNullWithoutCounterpart() {
        BlazoriseIconMapper.ToBlazoriseStyle(Jenin.FontAwesome.Blazor.IconStyle.Thin).Should().BeNull();
        BlazoriseIconMapper.ToBlazoriseStyle(Jenin.FontAwesome.Blazor.IconStyle.Brands).Should().BeNull();
        BlazoriseIconMapper.ToBlazoriseStyle(null).Should().BeNull();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void ToBlazoriseSize_MapsSharedSteps() {
        BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.ExtraSmall).Should().Be(global::Blazorise.IconSize.ExtraSmall);
        BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.Small).Should().Be(global::Blazorise.IconSize.Small);
        BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.Large).Should().Be(global::Blazorise.IconSize.Large);
        BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.X02).Should().Be(global::Blazorise.IconSize.x2);
        BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.X10).Should().Be(global::Blazorise.IconSize.x10);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void ToBlazoriseSize_ReturnsNullOutsideOverlap() {
        BlazoriseIconMapper.ToBlazoriseSize(null).Should().BeNull();
        BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.NotSet).Should().BeNull();
        BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.X01).Should().BeNull();
        BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.ExtraLarge).Should().BeNull();
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task JeninBlazoriseIcon_RendersMappedIconAsync() {
        // The Blazorise FA provider normalizes the style to the classic fas/far/fab tokens
        // (FA CSS accepts both forms).
        var markup = await TestHost.RenderMarkupAsync<JeninBlazoriseIcon>(
            TestHost.Parameters(
                ("IconName", "fa-phone"),
                ("IconStyle", Jenin.FontAwesome.Blazor.IconStyle.Solid),
                ("IconSize", Jenin.FontAwesome.Blazor.IconSize.Large)),
            services => {
                services.AddSingleton<Microsoft.JSInterop.IJSRuntime, TestSupport.TestJSRuntime>();
                services.AddBlazorise();
                services.AddBootstrap5Providers();
                services.AddFontAwesomeIcons();
            });

        markup.Should().Contain("fas");
        markup.Should().Contain("fa-phone");
        markup.Should().Contain("fa-lg");
    }
}
