//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class IconRatingTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Readonly_RendersFractionalOverlayAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconRating>(TestHost.Parameters(
            ("Value", 3.5f),
            ("Interactive", false),
            ("AriaLabel", "Product rating")));

        StringAssert.Contains(markup, "role=\"img\"");
        StringAssert.Contains(markup, "aria-label=\"3.5 out of 5\"");
        StringAssert.Contains(markup, "width:70.00%;");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Readonly_WholeValue_OmitsDecimalsAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconRating>(TestHost.Parameters(
            ("Value", 4f),
            ("Interactive", false)));

        StringAssert.Contains(markup, "aria-label=\"4 out of 5\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Interactive_RendersNativeRadiosAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconRating>(TestHost.Parameters(
            ("Value", 3f),
            ("RatingLabel", "Service rating")));

        StringAssert.Contains(markup, "role=\"radiogroup\"");
        StringAssert.Contains(markup, "aria-label=\"Service rating\"");
        Assert.AreEqual(5, CountOccurrences(markup, "type=\"radio\""));
        Assert.AreEqual(1, CountOccurrences(markup, "checked"));
        Assert.AreEqual(1, CountOccurrences(markup, "tabindex=\"0\""));
        Assert.AreEqual(4, CountOccurrences(markup, "tabindex=\"-1\""));
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Interactive_ZeroValue_ChecksNothingAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconRating>(TestHost.Parameters(
            ("Value", 0f)));

        Assert.AreEqual(0, CountOccurrences(markup, "checked"));
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Disabled_SuppressesClearAndDisablesRadiosAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconRating>(TestHost.Parameters(
            ("Value", 3f),
            ("Disabled", true)));

        Assert.AreEqual(5, CountOccurrences(markup, "disabled"));
        Assert.IsFalse(markup.Contains("Clear rating", StringComparison.Ordinal));
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ClearOfferedWhenDeselectAllowedAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconRating>(TestHost.Parameters(
            ("Value", 3f)));

        StringAssert.Contains(markup, "aria-label=\"Clear rating\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ClearHiddenWhenDeselectDisallowedAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconRating>(TestHost.Parameters(
            ("Value", 3f),
            ("AllowDeselect", false)));

        Assert.IsFalse(markup.Contains("Clear rating", StringComparison.Ordinal));
    }

    private static int CountOccurrences(string source, string token) {
        var count = 0;
        var index = 0;

        while((index = source.IndexOf(token, index, StringComparison.Ordinal)) >= 0) {
            count++;
            index += token.Length;
        }

        return count;
    }
}
