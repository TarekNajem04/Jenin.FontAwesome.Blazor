//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class IconButtonTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task RendersButtonWithIconAndLabelWithoutRedundantNameAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconButton>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Label", "Home")));

        // The visible label already names the button, so no aria-label is emitted.
        markup.Should().Be(
            "<button type=\"button\">" +
            "<span><i class=\"fa-solid fa-house\" aria-hidden=\"true\" role=\"presentation\"></i></span> Home</button>"
        );
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task IconOnlyButton_UsesTitleAsAccessibleNameAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconButton>(TestHost.Parameters(
            ("IconName", "fa-bell"),
            ("Title", "Notifications")));

        markup.Should().Be(
            "<button type=\"button\" aria-label=\"Notifications\" title=\"Notifications\">" +
            "<span><i class=\"fa-solid fa-bell\" aria-hidden=\"true\" role=\"presentation\"></i></span></button>"
        );
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ConsumerStyleFlowsToGlyphAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconButton>(TestHost.Parameters(
            ("IconName", "fa-github"),
            ("IconStyle", IconStyle.Brands),
            ("AriaLabel", "GitHub")));

        markup.Should().Contain("class=\"fa-brands fa-github\"");
        markup.Should().Contain("aria-label=\"GitHub\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Busy_SwapsGlyphDisablesAndMarksBusyAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconButton>(TestHost.Parameters(
            ("IconName", "fa-floppy-disk"),
            ("Label", "Save"),
            ("Busy", true)));

        markup.Should().Be(
            "<button type=\"button\" disabled aria-busy=\"true\">" +
            "<span><i class=\"fa-solid fa-spinner fa-spin\" aria-hidden=\"true\" role=\"presentation\"></i></span> Save</button>"
        );
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Toggle_ReRender_AppliesPressedStateAsync() {
        var markup = await TestHost.RenderMarkupSequenceAsync<IconToggleButton>(
            TestHost.Parameters(
                ("IconName", "fa-heart"),
                ("IconStyle", IconStyle.Regular),
                ("PressedIconStyle", IconStyle.Solid),
                ("PressedColor", "red"),
                ("PressedAnimation", null),
                ("Pressed", false)),
            TestHost.Parameters(
                ("IconName", "fa-heart"),
                ("IconStyle", IconStyle.Regular),
                ("PressedIconStyle", IconStyle.Solid),
                ("PressedColor", "red"),
                ("PressedAnimation", null),
                ("Pressed", true)));

        markup.Should().Be(
            "<button type=\"button\" aria-pressed=\"true\">" +
            "<span><i class=\"fa-solid fa-heart\" aria-hidden=\"true\" role=\"presentation\" style=\"color:red;\"></i></span></button>"
        );
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task FavoriteButton_RendersBakedDefaultsAsync() {
        var markup = await TestHost.RenderMarkupAsync<FavoriteButton>();

        markup.Should().Be(
            "<button type=\"button\" aria-label=\"Add to favorites\" title=\"Add to favorites\" aria-pressed=\"false\">" +
            "<span><i class=\"fa-regular fa-heart\" aria-hidden=\"true\" role=\"presentation\"></i></span></button>"
        );
    }
}
