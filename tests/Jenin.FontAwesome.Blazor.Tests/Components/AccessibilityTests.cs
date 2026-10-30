//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class AccessibilityTests {
    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Icon_WithoutAccessibleName_IsDecorativeAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(("IconName", "fa-house")));

        Assert.AreEqual(
            "<span><i class=\"fa-solid fa-house\" aria-hidden=\"true\" role=\"presentation\"></i></span>",
            markup);
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Icon_WithTitle_IsMeaningfulAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Title", "Home")));

        Assert.AreEqual(
            "<span><i class=\"fa-solid fa-house\" title=\"Home\" role=\"img\" aria-hidden=\"false\"></i></span>",
            markup);
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Icon_WithAriaLabel_IsMeaningfulAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("AriaLabel", "Home")));

        Assert.AreEqual(
            "<span><i class=\"fa-solid fa-house\" aria-label=\"Home\" role=\"img\" aria-hidden=\"false\"></i></span>",
            markup);
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Icon_WithTitleAndAriaLabel_EmitsBothAccessibleNamesAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Title", "Home"),
            ("AriaLabel", "Home page")));

        Assert.Contains("title=\"Home\"", markup);
        Assert.Contains("aria-label=\"Home page\"", markup);
        Assert.Contains("aria-hidden=\"false\"", markup);
        Assert.Contains("role=\"img\"", markup);
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Icon_ExplicitMeaningful_WithoutAccessibleName_EmitsImageRoleAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Decorative", false)));

        Assert.AreEqual(
            "<span><i class=\"fa-solid fa-house\" role=\"img\" aria-hidden=\"false\"></i></span>",
            markup);
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Icon_ExplicitDecorative_WinsOverAccessibleNameAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Title", "Home"),
            ("Decorative", true)));

        Assert.AreEqual(
            "<span><i class=\"fa-solid fa-house\" title=\"Home\" aria-hidden=\"true\" role=\"presentation\"></i></span>",
            markup);
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Icon_AccessibleNamesAreTrimmedAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Title", "  Home  "),
            ("AriaLabel", "  Home page  ")));

        Assert.Contains("title=\"Home\"", markup);
        Assert.Contains("aria-label=\"Home page\"", markup);
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Container_WithoutAccessibleName_HasNoAccessibilityAttributesAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconContainer>();

        Assert.AreEqual("<div></div>", markup);
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Container_WithAriaLabel_EmitsAccessibleNameAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconContainer>(TestHost.Parameters(("AriaLabel", "Icons")));

        Assert.AreEqual("<div aria-label=\"Icons\"></div>", markup);
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Container_ExplicitDecorative_EmitsAriaHiddenAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconContainer>(TestHost.Parameters(("Decorative", true)));

        Assert.AreEqual("<div aria-hidden=\"true\"></div>", markup);
    }
}
