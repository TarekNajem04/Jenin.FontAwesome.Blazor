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
    public void BuildClasses_ReturnsFaBeatFade() => Assert.AreEqual("fa-beat-fade", new BeatFade().BuildClasses());

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_EmitsOpacityAndScale() {
        var styles = new BeatFade(new BeatFadeOptions { Opacity = 0.25f, Scale = 2 }).BuildStyles();

        Assert.AreEqual("0.25", styles["--fa-beat-fade-opacity"]);
        Assert.AreEqual("2.00", styles["--fa-beat-fade-scale"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Opacity_IsClampedToUnitInterval() {
        var styles = new BeatFade(new BeatFadeOptions { Opacity = 5 }).BuildStyles();

        Assert.AreEqual("1.00", styles["--fa-beat-fade-opacity"]);
    }
}
