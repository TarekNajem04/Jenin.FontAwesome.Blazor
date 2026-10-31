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

        markup.Should().Contain("role=\"img\"");
        markup.Should().Contain("aria-label=\"3.5 out of 5\"");
        markup.Should().Contain("width:70.00%;");
    }

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task Readonly_WholeValue_OmitsDecimalsAsync() {

        var markup = await TestHost.RenderMarkupAsync<IconRating>(TestHost.Parameters(

            ("Value", 4f),

            ("Interactive", false)));

        markup.Should().Contain("aria-label=\"4 out of 5\"");
    }

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task Interactive_RendersNativeRadiosAsync() {

        var markup = await TestHost.RenderMarkupAsync<IconRating>(TestHost.Parameters(

            ("Value", 3f),

            ("RatingLabel", "Service rating")));

        markup.Should().Contain("role=\"radiogroup\"");
        markup.Should().Contain("aria-label=\"Service rating\"");
        CountOccurrences(markup, "type=\"radio\"").Should().Be(5);

        CountOccurrences(markup, "checked").Should().Be(1);

        CountOccurrences(markup, "tabindex=\"0\"").Should().Be(1);

        CountOccurrences(markup, "tabindex=\"-1\"").Should().Be(4);

    }

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task Interactive_ZeroValue_ChecksNothingAsync() {

        var markup = await TestHost.RenderMarkupAsync<IconRating>(TestHost.Parameters(

            ("Value", 0f)));

        CountOccurrences(markup, "checked").Should().Be(0);

    }

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task Disabled_SuppressesClearAndDisablesRadiosAsync() {

        var markup = await TestHost.RenderMarkupAsync<IconRating>(TestHost.Parameters(

            ("Value", 3f),

            ("Disabled", true)));

        CountOccurrences(markup, "disabled").Should().Be(5);

        markup.Contains("Clear rating", StringComparison.Ordinal).Should().BeFalse();

    }

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task ClearOfferedWhenDeselectAllowedAsync() {

        var markup = await TestHost.RenderMarkupAsync<IconRating>(TestHost.Parameters(

            ("Value", 3f)));

        markup.Should().Contain("aria-label=\"Clear rating\"");
    }

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task ClearHiddenWhenDeselectDisallowedAsync() {

        var markup = await TestHost.RenderMarkupAsync<IconRating>(TestHost.Parameters(

            ("Value", 3f),

            ("AllowDeselect", false)));

        markup.Contains("Clear rating", StringComparison.Ordinal).Should().BeFalse();

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

