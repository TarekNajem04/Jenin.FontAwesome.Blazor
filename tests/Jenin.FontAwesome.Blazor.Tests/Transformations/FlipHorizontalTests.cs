//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Transformations;

[TestClass]
public class FlipHorizontalTests {
    [TestMethod]
    [TestCategory("Model")]
    public void Flip_EmitsFlipHToken() => FlipHorizontal.Flip().GetTransformation().Should().Be("flip-h");
}
