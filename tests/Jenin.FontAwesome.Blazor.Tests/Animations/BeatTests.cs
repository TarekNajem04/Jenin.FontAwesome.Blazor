//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Animations;

[TestClass]
public class BeatTests {
    [TestMethod]
    [TestCategory("Model")]
    public void BuildClasses_ReturnsFaBeat() => new Beat().BuildClasses().Should().Be("fa-beat");

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithoutScale_OmitsScale() => new Beat().BuildStyles().ContainsKey("--fa-beat-scale").Should().BeFalse();

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithScale_EmitsInvariantTwoDecimalValue() {
        var styles = new Beat(new BeatOptions { Scale = 2 }).BuildStyles();

        styles["--fa-beat-scale"].Should().Be("2.00");
    }
}
