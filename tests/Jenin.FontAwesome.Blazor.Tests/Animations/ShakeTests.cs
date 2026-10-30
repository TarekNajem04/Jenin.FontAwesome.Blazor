//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Animations;

[TestClass]
public class ShakeTests {
    [TestMethod]
    [TestCategory("Model")]
    public void BuildClasses_ReturnsFaShake() => Assert.AreEqual("fa-shake", new Shake().BuildClasses());

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_IsEmpty() => Assert.IsEmpty(new Shake().BuildStyles());
}
