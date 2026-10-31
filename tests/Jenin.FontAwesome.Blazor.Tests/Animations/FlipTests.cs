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
    public void BuildClasses_ReturnsFaFlip() => new Flip().BuildClasses().Should().Be("fa-flip");

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithValues_EmitsVectorAndAngle() {
        var styles = new Flip(new FlipOptions { FlipX = 0.5f, FlipY = 1, FlipZ = 0, FlipAngle = 90 }).BuildStyles();

        styles["--fa-flip-x"].Should().Be("0.50");
        styles["--fa-flip-y"].Should().Be("1.00");
        styles["--fa-flip-z"].Should().Be("0.00");
        styles["--fa-flip-angle"].Should().Be("90.00");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void VectorComponents_AreClampedToUnitInterval() {
        var styles = new Flip(new FlipOptions { FlipX = 5, FlipY = -5 }).BuildStyles();

        styles["--fa-flip-x"].Should().Be("1.00");
        styles["--fa-flip-y"].Should().Be("0.00");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithOvershootAndAnticipation_EmitsDocumentedCustomProperties() {
        var styles = new Flip(new FlipOptions { Overshoot = 1.04f, AnticipationScale = 0.95f }).BuildStyles();

        styles["--fa-flip-overshoot"].Should().Be("1.04");
        styles["--fa-flip-anticipation-scale"].Should().Be("0.95");
        styles.Should().HaveCount(2);
    }
}
