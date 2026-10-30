//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Animations;

[TestClass]
public class FlipTests {
    [TestMethod]
    [TestCategory("Model")]
    public void BuildClasses_ReturnsFaFlip() => Assert.AreEqual("fa-flip", new Flip().BuildClasses());

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithValues_EmitsVectorAndAngle() {
        var styles = new Flip(new FlipOptions { FlipX = 0.5f, FlipY = 1, FlipZ = 0, FlipAngle = 90 }).BuildStyles();

        Assert.AreEqual("0.50", styles["--fa-flip-x"]);
        Assert.AreEqual("1.00", styles["--fa-flip-y"]);
        Assert.AreEqual("0.00", styles["--fa-flip-z"]);
        Assert.AreEqual("90.00", styles["--fa-flip-angle"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void VectorComponents_AreClampedToUnitInterval() {
        var styles = new Flip(new FlipOptions { FlipX = 5, FlipY = -5 }).BuildStyles();

        Assert.AreEqual("1.00", styles["--fa-flip-x"]);
        Assert.AreEqual("0.00", styles["--fa-flip-y"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithOvershootAndAnticipation_EmitsDocumentedCustomProperties() {
        var styles = new Flip(new FlipOptions { Overshoot = 1.04f, AnticipationScale = 0.95f }).BuildStyles();

        Assert.AreEqual("1.04", styles["--fa-flip-overshoot"]);
        Assert.AreEqual("0.95", styles["--fa-flip-anticipation-scale"]);
        Assert.HasCount(2, styles);
    }
}
