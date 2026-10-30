//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Transformations;

[TestClass]
public class PositioningTests {
    [TestMethod]
    [TestCategory("Model")]
    public void Up_EmitsUpToken() => Assert.AreEqual("up-6.00", Positioning.Up(6).GetTransformation());

    [TestMethod]
    [TestCategory("Model")]
    public void Down_EmitsDownToken() => Assert.AreEqual("down-6.00", Positioning.Down(6).GetTransformation());

    [TestMethod]
    [TestCategory("Model")]
    public void Left_EmitsLeftToken() => Assert.AreEqual("left-6.00", Positioning.Left(6).GetTransformation());

    [TestMethod]
    [TestCategory("Model")]
    public void Right_EmitsRightToken() => Assert.AreEqual("right-6.00", Positioning.Right(6).GetTransformation());

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(0f, "up-1.00")]
    [DataRow(100f, "up-16.00")]
    public void Up_NormalizesToUnitToSixteen(float value, string expected) => Assert.AreEqual(expected, Positioning.Up(value).GetTransformation());
}
