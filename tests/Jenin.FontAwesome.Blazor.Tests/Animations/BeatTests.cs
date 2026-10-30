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
    public void BuildClasses_ReturnsFaBeat() => Assert.AreEqual("fa-beat", new Beat().BuildClasses());

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithoutScale_OmitsScale() => Assert.IsFalse(new Beat().BuildStyles().ContainsKey("--fa-beat-scale"));

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithScale_EmitsInvariantTwoDecimalValue() {
        var styles = new Beat(new BeatOptions { Scale = 2 }).BuildStyles();

        Assert.AreEqual("2.00", styles["--fa-beat-scale"]);
    }
}
