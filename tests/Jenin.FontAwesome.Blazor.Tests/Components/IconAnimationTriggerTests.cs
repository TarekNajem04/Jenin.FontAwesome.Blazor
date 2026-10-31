//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class IconAnimationTriggerTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task NoneTrigger_EmitsNoTriggerClassAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Animation", Animate.Spin())));

        markup.Contains("jfa-anim", StringComparison.Ordinal).Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Rendering")]
    [DataRow(AnimationTrigger.Hover, "jfa-anim-hover")]
    [DataRow(AnimationTrigger.ParentHover, "jfa-anim-parent-hover")]
    [DataRow(AnimationTrigger.Focus, "jfa-anim-focus")]
    [DataRow(AnimationTrigger.ScrollReveal, "jfa-anim-reveal-target")]
    public async Task Trigger_EmitsTriggerClassAsync(AnimationTrigger trigger, string cssClass) {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Animation", Animate.Spin()),
            ("Trigger", trigger)));

        markup.Should().Contain(cssClass);
        markup.Should().Contain("fa-spin");
    }
}
