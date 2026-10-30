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

        StringAssert.Contains(markup, "role=\"group\"");
        StringAssert.Contains(markup, "aria-label=\"Active users: 1,234\"");
        StringAssert.Contains(markup, "fa-chart-line");
        StringAssert.Contains(markup, "1,234");
        StringAssert.Contains(markup, "fa-arrow-trend-up");
        StringAssert.Contains(markup, "&#x2B;12%");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task StatCard_NoDelta_RendersNoDirectionAsync() {
        var markup = await TestHost.RenderMarkupAsync<StatCard>(TestHost.Parameters(
            ("Value", "7"),
            ("Label", "Drafts")));

        Assert.IsFalse(markup.Contains("fa-arrow-trend", StringComparison.Ordinal));
        Assert.IsFalse(markup.Contains("fa-minus", StringComparison.Ordinal));
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task EmptyState_RendersGlyphTitleDescriptionAndActionsAsync() {
        var markup = await TestHost.RenderMarkupAsync<EmptyState>(TestHost.Parameters(
            ("Title", "No results"),
            ("Description", "Try a different term."),
            ("ChildContent", (RenderFragment)(builder => builder.AddContent(0, "Retry")))));

        StringAssert.Contains(markup, "fa-inbox");
        StringAssert.Contains(markup, "No results");
        StringAssert.Contains(markup, "Try a different term.");
        StringAssert.Contains(markup, "Retry");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ActivityItem_RendersMarkerTitleBodyAndTimeAsync() {
        var markup = await TestHost.RenderMarkupAsync<ActivityItem>(TestHost.Parameters(
            ("Title", "Deploy finished"),
            ("Time", "2h ago"),
            ("ChildContent", (RenderFragment)(builder => builder.AddContent(0, "Production is live.")))));

        StringAssert.Contains(markup, "fa-bell");
        StringAssert.Contains(markup, "Deploy finished");
        StringAssert.Contains(markup, "Production is live.");
        StringAssert.Contains(markup, "2h ago");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ShareButtons_RendersDocumentedIntentsAsync() {
        var markup = await TestHost.RenderMarkupAsync<ShareButtons>(TestHost.Parameters(
            ("Url", "https://example.com/a b"),
            ("Text", "Hello & welcome")));

        StringAssert.Contains(markup, "role=\"group\"");
        StringAssert.Contains(markup, "aria-label=\"Share\"");
        StringAssert.Contains(markup, "https://twitter.com/intent/tweet?url=https%3A%2F%2Fexample.com%2Fa%20b&amp;text=Hello%20%26%20welcome");
        StringAssert.Contains(markup, "https://www.linkedin.com/sharing/share-offsite/?url=https%3A%2F%2Fexample.com%2Fa%20b");
        StringAssert.Contains(markup, "https://wa.me/?text=Hello%20%26%20welcome%20https%3A%2F%2Fexample.com%2Fa%20b");
        StringAssert.Contains(markup, "https://t.me/share/url?url=https%3A%2F%2Fexample.com%2Fa%20b&amp;text=Hello%20%26%20welcome");
        StringAssert.Contains(markup, "mailto:?subject=Hello%20%26%20welcome&amp;body=https%3A%2F%2Fexample.com%2Fa%20b");
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

        StringAssert.Contains(markup, "mailto:");
        Assert.IsFalse(markup.Contains("twitter.com", StringComparison.Ordinal));
        Assert.IsFalse(markup.Contains("linkedin.com", StringComparison.Ordinal));
    }
}
