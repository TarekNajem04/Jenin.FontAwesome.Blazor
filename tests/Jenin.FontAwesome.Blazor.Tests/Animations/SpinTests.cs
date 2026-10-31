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
    public void BuildClasses_DefaultsToFaSpin() => new Spin().BuildClasses().Should().Be("fa-spin");

    [TestMethod]
    [TestCategory("Model")]
    public void BuildClasses_WithPulse_UsesPulseClass() => new Spin(new SpinOptions { Pulse = true }).BuildClasses().Should().Be("fa-spin-pulse");

    [TestMethod]
    [TestCategory("Model")]
    public void BuildClasses_WithReverse_AppendsReverseClass() => new Spin(new SpinOptions { Reverse = true }).BuildClasses().Should().Be("fa-spin fa-spin-reverse");

    [TestMethod]
    [TestCategory("Model")]
    public void BuildClasses_WithPulseAndReverse_CombinesClasses() => new Spin(new SpinOptions { Pulse = true, Reverse = true }).BuildClasses().Should().Be("fa-spin-pulse fa-spin-reverse");

    [TestMethod]
    [TestCategory("Model")]
    public void Pulse_RoundTripsThroughSerialization() {
        var styles = new Spin(new SpinOptions { Pulse = true }).SetDelay(TimeSpan.FromMilliseconds(10)).BuildStyles();

        styles["--fa-animation-delay"].Should().Be("10ms");
    }
}
