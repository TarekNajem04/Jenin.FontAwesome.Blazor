//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Animations;

[TestClass]
public class BounceTests {
    [TestMethod]
    [TestCategory("Model")]
    public void BuildClasses_ReturnsFaBounce() => Assert.AreEqual("fa-bounce", new Bounce().BuildClasses());

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithoutValues_IsEmpty() => Assert.IsEmpty(new Bounce().BuildStyles());

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithValues_EmitsDocumentedCustomProperties() {
        var styles = new Bounce(new BounceOptions {
            Rebound = 1,
            Height = 2,
            StartScaleX = 3,
            StartScaleY = 4,
            JumpScaleX = 5,
            JumpScaleY = 6,
            LandScaleX = 7,
            LandScaleY = 8
        }).BuildStyles();

        Assert.AreEqual("1.00", styles["--fa-bounce-rebound"]);
        Assert.AreEqual("2.00", styles["--fa-bounce-height"]);
        Assert.AreEqual("3.00", styles["--fa-bounce-start-scale-x"]);
        Assert.AreEqual("4.00", styles["--fa-bounce-start-scale-y"]);
        Assert.AreEqual("5.00", styles["--fa-bounce-jump-scale-x"]);
        Assert.AreEqual("6.00", styles["--fa-bounce-jump-scale-y"]);
        Assert.AreEqual("7.00", styles["--fa-bounce-land-scale-x"]);
        Assert.AreEqual("8.00", styles["--fa-bounce-land-scale-y"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithAnticipation_EmitsDocumentedCustomProperty() {
        var styles = new Bounce(new BounceOptions { Anticipation = "3px" }).BuildStyles();

        Assert.AreEqual("3px", styles["--fa-bounce-anticipation"]);
        Assert.HasCount(1, styles);
    }
}
