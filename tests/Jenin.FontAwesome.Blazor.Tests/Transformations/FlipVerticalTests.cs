//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Transformations;

[TestClass]
public class FlipVerticalTests {
    [TestMethod]
    [TestCategory("Model")]
    public void Flip_EmitsFlipVToken() => Assert.AreEqual("flip-v", FlipVertical.Flip().GetTransformation());
}
