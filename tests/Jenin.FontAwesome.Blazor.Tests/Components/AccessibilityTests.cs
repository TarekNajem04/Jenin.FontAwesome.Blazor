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

        markup.Should().Be(
            "<span><i class=\"fa-solid fa-house\" aria-hidden=\"true\" role=\"presentation\"></i></span>"
        );
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Icon_WithTitle_IsMeaningfulAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Title", "Home")));

        markup.Should().Be(
            "<span><i class=\"fa-solid fa-house\" title=\"Home\" role=\"img\" aria-hidden=\"false\"></i></span>"
        );
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Icon_WithAriaLabel_IsMeaningfulAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("AriaLabel", "Home")));

        markup.Should().Be(
            "<span><i class=\"fa-solid fa-house\" aria-label=\"Home\" role=\"img\" aria-hidden=\"false\"></i></span>"
        );
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Icon_WithTitleAndAriaLabel_EmitsBothAccessibleNamesAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Title", "Home"),
            ("AriaLabel", "Home page")));

        markup.Should().Contain("title=\"Home\"");
        markup.Should().Contain("aria-label=\"Home page\"");
        markup.Should().Contain("aria-hidden=\"false\"");
        markup.Should().Contain("role=\"img\"");
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Icon_ExplicitMeaningful_WithoutAccessibleName_EmitsImageRoleAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Decorative", false)));

        markup.Should().Be(
            "<span><i class=\"fa-solid fa-house\" role=\"img\" aria-hidden=\"false\"></i></span>"
        );
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Icon_ExplicitDecorative_WinsOverAccessibleNameAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Title", "Home"),
            ("Decorative", true)));

        markup.Should().Be(
            "<span><i class=\"fa-solid fa-house\" title=\"Home\" aria-hidden=\"true\" role=\"presentation\"></i></span>"
        );
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Icon_AccessibleNamesAreTrimmedAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Title", "  Home  "),
            ("AriaLabel", "  Home page  ")));

        markup.Should().Contain("title=\"Home\"");
        markup.Should().Contain("aria-label=\"Home page\"");
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Container_WithoutAccessibleName_HasNoAccessibilityAttributesAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconContainer>();

        markup.Should().Be("<div></div>");
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Container_WithAriaLabel_EmitsAccessibleNameAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconContainer>(TestHost.Parameters(("AriaLabel", "Icons")));

        markup.Should().Be("<div aria-label=\"Icons\"></div>");
    }

    [TestMethod]
    [TestCategory("Accessibility")]
    public async Task Container_ExplicitDecorative_EmitsAriaHiddenAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconContainer>(TestHost.Parameters(("Decorative", true)));

        markup.Should().Be("<div aria-hidden=\"true\"></div>");
    }
}
