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
    public void BuildClasses_ReturnsFaBounce() => new Bounce().BuildClasses().Should().Be("fa-bounce");

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithoutValues_IsEmpty() => new Bounce().BuildStyles().Should().BeEmpty();

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

        styles["--fa-bounce-rebound"].Should().Be("1.00");
        styles["--fa-bounce-height"].Should().Be("2.00");
        styles["--fa-bounce-start-scale-x"].Should().Be("3.00");
        styles["--fa-bounce-start-scale-y"].Should().Be("4.00");
        styles["--fa-bounce-jump-scale-x"].Should().Be("5.00");
        styles["--fa-bounce-jump-scale-y"].Should().Be("6.00");
        styles["--fa-bounce-land-scale-x"].Should().Be("7.00");
        styles["--fa-bounce-land-scale-y"].Should().Be("8.00");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithAnticipation_EmitsDocumentedCustomProperty() {
        var styles = new Bounce(new BounceOptions { Anticipation = "3px" }).BuildStyles();

        styles["--fa-bounce-anticipation"].Should().Be("3px");
        styles.Should().HaveCount(1);
    }
}
