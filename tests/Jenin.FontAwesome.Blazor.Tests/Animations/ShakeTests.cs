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
    public void BuildClasses_ReturnsFaShake() => new Shake().BuildClasses().Should().Be("fa-shake");

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_IsEmpty() => new Shake().BuildStyles().Should().BeEmpty();
}
