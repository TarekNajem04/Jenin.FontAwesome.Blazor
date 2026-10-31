//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class ContentBlocksTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task StatCard_RendersValueLabelAndDeltaAsync() {
        var markup = await TestHost.RenderMarkupAsync<StatCard>(TestHost.Parameters(
            ("Value", "1,234"),
            ("Label", "Active users"),
            ("Delta", StatDelta.Up),
            ("DeltaText", "+12%")));

        markup.Should().Contain("role=\"group\"");
        markup.Should().Contain("aria-label=\"Active users: 1,234\"");
        markup.Should().Contain("fa-chart-line");
        markup.Should().Contain("1,234");
        markup.Should().Contain("fa-arrow-trend-up");
        markup.Should().Contain("&#x2B;12%");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task StatCard_NoDelta_RendersNoDirectionAsync() {
        var markup = await TestHost.RenderMarkupAsync<StatCard>(TestHost.Parameters(
            ("Value", "7"),
            ("Label", "Drafts")));

        markup.Contains("fa-arrow-trend", StringComparison.Ordinal).Should().BeFalse();
        markup.Contains("fa-minus", StringComparison.Ordinal).Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task EmptyState_RendersGlyphTitleDescriptionAndActionsAsync() {
        var markup = await TestHost.RenderMarkupAsync<EmptyState>(TestHost.Parameters(
            ("Title", "No results"),
            ("Description", "Try a different term."),
            ("ChildContent", (RenderFragment)(builder => builder.AddContent(0, "Retry")))));

        markup.Should().Contain("fa-inbox");
        markup.Should().Contain("No results");
        markup.Should().Contain("Try a different term.");
        markup.Should().Contain("Retry");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ActivityItem_RendersMarkerTitleBodyAndTimeAsync() {
        var markup = await TestHost.RenderMarkupAsync<ActivityItem>(TestHost.Parameters(
            ("Title", "Deploy finished"),
            ("Time", "2h ago"),
            ("ChildContent", (RenderFragment)(builder => builder.AddContent(0, "Production is live.")))));

        markup.Should().Contain("fa-bell");
        markup.Should().Contain("Deploy finished");
        markup.Should().Contain("Production is live.");
        markup.Should().Contain("2h ago");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ShareButtons_RendersDocumentedIntentsAsync() {
        var markup = await TestHost.RenderMarkupAsync<ShareButtons>(TestHost.Parameters(
            ("Url", "https://example.com/a b"),
            ("Text", "Hello & welcome")));

        markup.Should().Contain("role=\"group\"");
        markup.Should().Contain("aria-label=\"Share\"");
        markup.Should().Contain("https://twitter.com/intent/tweet?url=https%3A%2F%2Fexample.com%2Fa%20b&amp;text=Hello%20%26%20welcome");
        markup.Should().Contain("https://www.linkedin.com/sharing/share-offsite/?url=https%3A%2F%2Fexample.com%2Fa%20b");
        markup.Should().Contain("https://wa.me/?text=Hello%20%26%20welcome%20https%3A%2F%2Fexample.com%2Fa%20b");
        markup.Should().Contain("https://t.me/share/url?url=https%3A%2F%2Fexample.com%2Fa%20b&amp;text=Hello%20%26%20welcome");
        markup.Should().Contain("mailto:?subject=Hello%20%26%20welcome&amp;body=https%3A%2F%2Fexample.com%2Fa%20b");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ShareButtons_RespectsChannelTogglesAsync() {
        var markup = await TestHost.RenderMarkupAsync<ShareButtons>(TestHost.Parameters(
            ("Url", "https://example.com"),
            ("IncludeX", false),
            ("IncludeLinkedIn", false),
            ("IncludeWhatsApp", false),
            ("IncludeTelegram", false),
            ("IncludeMail", true)));

        markup.Should().Contain("mailto:");
        markup.Contains("twitter.com", StringComparison.Ordinal).Should().BeFalse();
        markup.Contains("linkedin.com", StringComparison.Ordinal).Should().BeFalse();
    }
}
