//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Animations;

[TestClass]
public class SpinTests {
    [TestMethod]
    [TestCategory("Model")]
    public void BuildClasses_DefaultsToFaSpin() => Assert.AreEqual("fa-spin", new Spin().BuildClasses());

    [TestMethod]
    [TestCategory("Model")]
    public void BuildClasses_WithPulse_UsesPulseClass() => Assert.AreEqual("fa-spin-pulse", new Spin(new SpinOptions { Pulse = true }).BuildClasses());

    [TestMethod]
    [TestCategory("Model")]
    public void BuildClasses_WithReverse_AppendsReverseClass() => Assert.AreEqual("fa-spin fa-spin-reverse", new Spin(new SpinOptions { Reverse = true }).BuildClasses());

    [TestMethod]
    [TestCategory("Model")]
    public void BuildClasses_WithPulseAndReverse_CombinesClasses() => Assert.AreEqual("fa-spin-pulse fa-spin-reverse", new Spin(new SpinOptions { Pulse = true, Reverse = true }).BuildClasses());

    [TestMethod]
    [TestCategory("Model")]
    public void Pulse_RoundTripsThroughSerialization() {
        var styles = new Spin(new SpinOptions { Pulse = true }).SetDelay(TimeSpan.FromMilliseconds(10)).BuildStyles();

        Assert.AreEqual("10ms", styles["--fa-animation-delay"]);
    }
}
