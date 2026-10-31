//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Transformations;

[TestClass]
public class RotatingTests {
    [TestMethod]
    [TestCategory("Model")]
    public void Rotate_EmitsRotateToken() => Rotating.Rotate(90).GetTransformation().Should().Be("rotate-90.00");

    [TestMethod]
    [TestCategory("Model")]
    public void Rotate_NegativeValue_IsNotNormalized() => Rotating.Rotate(-45).GetTransformation().Should().Be("rotate--45.00");
}
