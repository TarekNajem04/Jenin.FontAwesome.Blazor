//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Animations;

[TestClass]
public class BeatFadeTests {
    [TestMethod]
    [TestCategory("Model")]
    public void BuildClasses_ReturnsFaBeatFade() => new BeatFade().BuildClasses().Should().Be("fa-beat-fade");

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_EmitsOpacityAndScale() {
        var styles = new BeatFade(new BeatFadeOptions { Opacity = 0.25f, Scale = 2 }).BuildStyles();

        styles["--fa-beat-fade-opacity"].Should().Be("0.25");
        styles["--fa-beat-fade-scale"].Should().Be("2.00");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Opacity_IsClampedToUnitInterval() {
        var styles = new BeatFade(new BeatFadeOptions { Opacity = 5 }).BuildStyles();

        styles["--fa-beat-fade-opacity"].Should().Be("1.00");
    }
}
