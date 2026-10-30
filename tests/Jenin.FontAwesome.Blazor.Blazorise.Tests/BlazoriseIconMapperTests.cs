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
    public void ToIconName_NormalizesToFaForm(string iconName, string expected) {
        Assert.AreEqual(expected, BlazoriseIconMapper.ToIconName(iconName));
    }

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(null)]
    [DataRow("")]
    public void ToIconName_RejectsEmpty(string? iconName) {
        Assert.AreEqual(string.Empty, BlazoriseIconMapper.ToIconName(iconName));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void ToBlazoriseStyle_MapsSharedStyles() {
        Assert.AreEqual(global::Blazorise.IconStyle.Solid, BlazoriseIconMapper.ToBlazoriseStyle(Jenin.FontAwesome.Blazor.IconStyle.Solid));
        Assert.AreEqual(global::Blazorise.IconStyle.Regular, BlazoriseIconMapper.ToBlazoriseStyle(Jenin.FontAwesome.Blazor.IconStyle.Regular));
        Assert.AreEqual(global::Blazorise.IconStyle.Light, BlazoriseIconMapper.ToBlazoriseStyle(Jenin.FontAwesome.Blazor.IconStyle.Light));
        Assert.AreEqual(global::Blazorise.IconStyle.DuoTone, BlazoriseIconMapper.ToBlazoriseStyle(Jenin.FontAwesome.Blazor.IconStyle.Duotone));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void ToBlazoriseStyle_ReturnsNullWithoutCounterpart() {
        Assert.IsNull(BlazoriseIconMapper.ToBlazoriseStyle(Jenin.FontAwesome.Blazor.IconStyle.Thin));
        Assert.IsNull(BlazoriseIconMapper.ToBlazoriseStyle(Jenin.FontAwesome.Blazor.IconStyle.Brands));
        Assert.IsNull(BlazoriseIconMapper.ToBlazoriseStyle(null));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void ToBlazoriseSize_MapsSharedSteps() {
        Assert.AreEqual(global::Blazorise.IconSize.ExtraSmall, BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.ExtraSmall));
        Assert.AreEqual(global::Blazorise.IconSize.Small, BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.Small));
        Assert.AreEqual(global::Blazorise.IconSize.Large, BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.Large));
        Assert.AreEqual(global::Blazorise.IconSize.x2, BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.X02));
        Assert.AreEqual(global::Blazorise.IconSize.x10, BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.X10));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void ToBlazoriseSize_ReturnsNullOutsideOverlap() {
        Assert.IsNull(BlazoriseIconMapper.ToBlazoriseSize(null));
        Assert.IsNull(BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.NotSet));
        Assert.IsNull(BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.X01));
        Assert.IsNull(BlazoriseIconMapper.ToBlazoriseSize(Jenin.FontAwesome.Blazor.IconSize.ExtraLarge));
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

        Assert.Contains("fas", markup);
        Assert.Contains("fa-phone", markup);
        Assert.Contains("fa-lg", markup);
    }
}
