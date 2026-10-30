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
        Assert.AreEqual(
            "<button type=\"button\">" +
            "<span><i class=\"fa-solid fa-house\" aria-hidden=\"true\" role=\"presentation\"></i></span> Home</button>",
            markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task IconOnlyButton_UsesTitleAsAccessibleNameAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconButton>(TestHost.Parameters(
            ("IconName", "fa-bell"),
            ("Title", "Notifications")));

        Assert.AreEqual(
            "<button type=\"button\" aria-label=\"Notifications\" title=\"Notifications\">" +
            "<span><i class=\"fa-solid fa-bell\" aria-hidden=\"true\" role=\"presentation\"></i></span></button>",
            markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ConsumerStyleFlowsToGlyphAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconButton>(TestHost.Parameters(
            ("IconName", "fa-github"),
            ("IconStyle", IconStyle.Brands),
            ("AriaLabel", "GitHub")));

        Assert.Contains("class=\"fa-brands fa-github\"", markup);
        Assert.Contains("aria-label=\"GitHub\"", markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Busy_SwapsGlyphDisablesAndMarksBusyAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconButton>(TestHost.Parameters(
            ("IconName", "fa-floppy-disk"),
            ("Label", "Save"),
            ("Busy", true)));

        Assert.AreEqual(
            "<button type=\"button\" disabled aria-busy=\"true\">" +
            "<span><i class=\"fa-solid fa-spinner fa-spin\" aria-hidden=\"true\" role=\"presentation\"></i></span> Save</button>",
            markup);
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

        Assert.AreEqual(
            "<button type=\"button\" aria-pressed=\"true\">" +
            "<span><i class=\"fa-solid fa-heart\" aria-hidden=\"true\" role=\"presentation\" style=\"color:red;\"></i></span></button>",
            markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task FavoriteButton_RendersBakedDefaultsAsync() {
        var markup = await TestHost.RenderMarkupAsync<FavoriteButton>();

        Assert.AreEqual(
            "<button type=\"button\" aria-label=\"Add to favorites\" title=\"Add to favorites\" aria-pressed=\"false\">" +
            "<span><i class=\"fa-regular fa-heart\" aria-hidden=\"true\" role=\"presentation\"></i></span></button>",
            markup);
    }
}
