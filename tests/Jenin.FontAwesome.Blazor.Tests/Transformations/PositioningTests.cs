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
    public void Up_EmitsUpToken() => Positioning.Up(6).GetTransformation().Should().Be("up-6.00");

    [TestMethod]
    [TestCategory("Model")]
    public void Down_EmitsDownToken() => Positioning.Down(6).GetTransformation().Should().Be("down-6.00");

    [TestMethod]
    [TestCategory("Model")]
    public void Left_EmitsLeftToken() => Positioning.Left(6).GetTransformation().Should().Be("left-6.00");

    [TestMethod]
    [TestCategory("Model")]
    public void Right_EmitsRightToken() => Positioning.Right(6).GetTransformation().Should().Be("right-6.00");

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(0f, "up-1.00")]
    [DataRow(100f, "up-16.00")]
    public void Up_NormalizesToUnitToSixteen(float value, string expected) => Positioning.Up(value).GetTransformation().Should().Be(expected);
}
