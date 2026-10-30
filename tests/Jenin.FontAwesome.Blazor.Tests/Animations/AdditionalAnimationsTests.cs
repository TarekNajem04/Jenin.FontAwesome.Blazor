//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Animations;

[TestClass]
public class AdditionalAnimationsTests {
    [TestMethod]
    [TestCategory("Model")]
    public void NewAnimations_BuildExpectedFontAwesomeClasses() {
        Assert.AreEqual("fa-buzz", new Buzz().BuildClasses());
        Assert.AreEqual("fa-float", new Float().BuildClasses());
        Assert.AreEqual("fa-jello", new Jello().BuildClasses());
        Assert.AreEqual("fa-flip-360", new Flip360().BuildClasses());
        Assert.AreEqual("fa-swing", new Swing().BuildClasses());
        Assert.AreEqual("fa-wag", new Wag().BuildClasses());
        Assert.AreEqual("fa-spin-snap", new SpinSnap().BuildClasses());
        Assert.AreEqual("fa-spin-snap-4", new SpinSnap(new SpinSnapOptions { Size = SpinSnapSize.Four }).BuildClasses());
        Assert.AreEqual("fa-spin-snap-8", new SpinSnap(new SpinSnapOptions { Size = SpinSnapSize.Eight }).BuildClasses());
    }

    [TestMethod]
    [TestCategory("Model")]
    public void NewAnimations_ProduceNoStylesByDefault() {
        Assert.IsEmpty(new Buzz().BuildStyles());
        Assert.IsEmpty(new Float().BuildStyles());
        Assert.IsEmpty(new Jello().BuildStyles());
        Assert.IsEmpty(new Flip360().BuildStyles());
        Assert.IsEmpty(new Swing().BuildStyles());
        Assert.IsEmpty(new Wag().BuildStyles());
        Assert.IsEmpty(new SpinSnap().BuildStyles());
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Factories_ReturnExpectedConcreteTypes() {
        Assert.IsInstanceOfType<Buzz>(Animate.Buzz());
        Assert.IsInstanceOfType<Float>(Animate.Float());
        Assert.IsInstanceOfType<Jello>(Animate.Jello());
        Assert.IsInstanceOfType<Flip360>(Animate.Flip360());
        Assert.IsInstanceOfType<Swing>(Animate.Swing());
        Assert.IsInstanceOfType<Wag>(Animate.Wag());
        Assert.IsInstanceOfType<SpinSnap>(Animate.SpinSnap());
    }

    [TestMethod]
    [TestCategory("Model")]
    public void SpinSnapFactory_ForwardsSize() {
        var animation = Animate.SpinSnap(new SpinSnapOptions { Size = SpinSnapSize.Eight });

        Assert.AreEqual(SpinSnapSize.Eight, animation.Options.Size);
        Assert.AreEqual("fa-spin-snap-8", animation.BuildClasses());
    }

    [TestMethod]
    [TestCategory("Model")]
    public void FluentSetters_PreserveConcreteType() {
        var buzz = Animate.Buzz()
                          .SetDelay(TimeSpan.FromSeconds(1))
                          .SetDuration(TimeSpan.FromSeconds(2))
                          .SetIterationCount(3);

        Assert.AreEqual(typeof(Buzz), buzz.GetType());
        Assert.AreEqual(TimeSpan.FromSeconds(1), buzz.Options.Delay);
        Assert.AreEqual(TimeSpan.FromSeconds(2), buzz.Options.Duration);
        Assert.AreEqual(3f, buzz.Options.IterationCount!.Value);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void FloatOptions_EmitDocumentedCustomProperties() {
        var styles = new Float(new FloatOptions {
            Drift = "2px",
            Height = "8px",
            SquashX = 1.02f,
            SquashY = 0.98f,
            StretchX = 0.98f,
            StretchY = 1.03f,
            Tilt = 1.5f
        }).BuildStyles();

        Assert.AreEqual("2px", styles["--fa-float-drift"]);
        Assert.AreEqual("8px", styles["--fa-float-height"]);
        Assert.AreEqual("1.02", styles["--fa-float-squash-x"]);
        Assert.AreEqual("0.98", styles["--fa-float-squash-y"]);
        Assert.AreEqual("0.98", styles["--fa-float-stretch-x"]);
        Assert.AreEqual("1.03", styles["--fa-float-stretch-y"]);
        Assert.AreEqual("1.5deg", styles["--fa-float-tilt"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuzzOptions_EmitsDocumentedCustomProperty() {
        var styles = new Buzz(new BuzzOptions { Distance = "4px" }).BuildStyles();

        Assert.AreEqual("4px", styles["--fa-buzz-distance"]);
        Assert.HasCount(1, styles);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void JelloOptions_EmitDocumentedCustomProperties() {
        var styles = new Jello(new JelloOptions { ScaleX = 1.15f, ScaleY = 1.12f }).BuildStyles();

        Assert.AreEqual("1.15", styles["--fa-jello-scale-x"]);
        Assert.AreEqual("1.12", styles["--fa-jello-scale-y"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void SwingOptions_EmitsDocumentedCustomProperty() {
        var styles = new Swing(new SwingOptions { Angle = 22.5f }).BuildStyles();

        Assert.AreEqual("22.5deg", styles["--fa-swing-angle"]);
        Assert.HasCount(1, styles);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void WagOptions_EmitsDocumentedCustomProperty() {
        var styles = new Wag(new WagOptions { Angle = 12 }).BuildStyles();

        Assert.AreEqual("12deg", styles["--fa-wag-angle"]);
        Assert.HasCount(1, styles);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void NewAnimationOptions_ProduceNoStylesWhenUnset() {
        Assert.IsEmpty(new Float(new FloatOptions { Tilt = null }).BuildStyles());
        Assert.IsEmpty(new Buzz(new BuzzOptions()).BuildStyles());
        Assert.IsEmpty(new Jello(new JelloOptions()).BuildStyles());
        Assert.IsEmpty(new Swing(new SwingOptions()).BuildStyles());
        Assert.IsEmpty(new Wag(new WagOptions()).BuildStyles());
    }
}
