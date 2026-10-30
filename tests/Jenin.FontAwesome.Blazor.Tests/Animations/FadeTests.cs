//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Animations;

[TestClass]
public class FadeTests {
    [TestMethod]
    [TestCategory("Model")]
    public void BuildClasses_ReturnsFaFade() => Assert.AreEqual("fa-fade", new Fade().BuildClasses());

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithOpacity_EmitsValue() => Assert.AreEqual("0.50", new Fade(new FadeOptions { Opacity = 0.5f }).BuildStyles()["--fa-fade-opacity"]);

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(2f, "1.00")]
    [DataRow(-1f, "0.00")]
    [DataRow(0f, "0.00")]
    [DataRow(1f, "1.00")]
    public void BuildStyles_Opacity_IsClampedToUnitInterval(float opacity, string expected) {
        var styles = new Fade(new FadeOptions { Opacity = opacity }).BuildStyles();

        Assert.AreEqual(expected, styles["--fa-fade-opacity"]);
    }
}
