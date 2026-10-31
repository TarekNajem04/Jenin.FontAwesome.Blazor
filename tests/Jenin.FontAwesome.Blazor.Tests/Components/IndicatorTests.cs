//

// Copyright (c) Jeninnet.

// Part of the Jeninnet Platform.

// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/

// Licensed under the MIT License.

//

namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]

public class IndicatorTests {

    private static RenderFragment Text(string value) => builder => builder.AddContent(0, value);

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task Badge_RendersChildAndCountAsync() {

        var markup = await TestHost.RenderMarkupAsync<NotificationBadge>(TestHost.Parameters(

            ("ChildContent", Text("Inbox")),

            ("Count", 3),

            ("Unit", "unread messages")));

        markup.Should().Be(
            "<span style=\"position:relative;display:inline-flex;\">Inbox" +

            "<span role=\"status\" class=\"fa-beat\" style=\"--fa-animation-iteration-count:1;position:absolute;inset-block-start:-0.35em;inset-inline-end:-0.35em;display:inline-flex;align-items:center;justify-content:center;min-width:1.25em;height:1.25em;padding:0 0.3em;border-radius:9999px;font-size:0.65em;font-weight:700;line-height:1;background-color:#dc2626;color:#ffffff;\" aria-label=\"3 unread messages\">3</span></span>"

        );

    }

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task Badge_HidesAtZeroAsync() {

        var markup = await TestHost.RenderMarkupAsync<NotificationBadge>(TestHost.Parameters(

            ("ChildContent", Text("Inbox")),

            ("Count", 0)));

        markup.Contains("role=\"status\"", StringComparison.Ordinal).Should().BeFalse();

        markup.Should().Contain("Inbox");
    }

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task Badge_CapsAtMaxCountAsync() {

        var markup = await TestHost.RenderMarkupAsync<NotificationBadge>(TestHost.Parameters(

            ("ChildContent", Text("Inbox")),

            ("Count", 150)));

        markup.Should().Contain(">99&#x2B;</span>");
    }

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task Badge_ReRender_UpdatesCountAsync() {

        var markup = await TestHost.RenderMarkupSequenceAsync<NotificationBadge>(

            TestHost.Parameters(("ChildContent", Text("Inbox")), ("Count", 2)),

            TestHost.Parameters(("ChildContent", Text("Inbox")), ("Count", 5)));

        markup.Should().Contain(">5</span>");
        markup.Contains(">2</span>", StringComparison.Ordinal).Should().BeFalse();

    }

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task StatusDot_RendersColorAndSizeAsync() {

        var markup = await TestHost.RenderMarkupAsync<StatusDot>(TestHost.Parameters(

            ("BackgroundColor", "#ef4444"),

            ("Diameter", "12px"),

            ("Title", "Busy")));

        markup.Should().Be(
            "<span style=\"background-color:#ef4444;display:inline-block;border-radius:50%;width:12px;height:12px;\" role=\"status\" aria-label=\"Busy\" title=\"Busy\"></span>"

        );

    }

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task StatusDot_Pulse_AddsBeatClassAsync() {

        var markup = await TestHost.RenderMarkupAsync<StatusDot>(TestHost.Parameters(

            ("Pulse", true)));

        markup.Should().Contain("class=\"fa-beat\"");
    }

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task IconProgress_RendersRingAndValueAsync() {

        var markup = await TestHost.RenderMarkupAsync<IconProgress>(TestHost.Parameters(

            ("Progress", 0.25f),

            ("AriaLabel", "Upload progress"),

            ("ChildContent", Text("25"))));

        markup.Should().Contain("role=\"progressbar\"");
        markup.Should().Contain("aria-valuenow=\"25\"");
        markup.Should().Contain("stroke-dasharray=\"138.23\"");
        markup.Should().Contain("stroke-dashoffset=\"103.67\"");
        markup.Should().Contain("font-size:19.20px;");
    }

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task AvatarIcon_RendersInitialsAndPresenceAsync() {

        var markup = await TestHost.RenderMarkupAsync<AvatarIcon>(TestHost.Parameters(

            ("Initials", "TK"),

            ("Presence", AvatarPresence.Online)));

        markup.Should().Contain("role=\"img\"");
        markup.Should().Contain("aria-label=\"TK\"");
        markup.Should().Contain(">TK<");
        markup.Should().Contain("background-color:#22c55e");
    }

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task AvatarIcon_DefaultsToUserGlyphAsync() {

        var markup = await TestHost.RenderMarkupAsync<AvatarIcon>();

        markup.Should().Contain("class=\"fa-solid fa-user\"");
    }
}

