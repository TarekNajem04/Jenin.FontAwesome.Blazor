//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Transformations;

[TestClass]
public class ScalingTests {
    [TestMethod]
    [TestCategory("Model")]
    public void Grow_EmitsGrowToken() => Scaling.Grow(6).GetTransformation().Should().Be("grow-6.00");

    [TestMethod]
    [TestCategory("Model")]
    public void Shrink_EmitsShrinkToken() => Scaling.Shrink(3).GetTransformation().Should().Be("shrink-3.00");

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(0f, "grow-1.00")]
    [DataRow(-5f, "grow-1.00")]
    [DataRow(100f, "grow-16.00")]
    [DataRow(1.5f, "grow-1.50")]
    public void Grow_NormalizesToUnitToSixteen(float value, string expected) => Scaling.Grow(value).GetTransformation().Should().Be(expected);
}
