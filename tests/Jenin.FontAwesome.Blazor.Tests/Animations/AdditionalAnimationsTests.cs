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
        new Buzz().BuildClasses().Should().Be("fa-buzz");
        new Float().BuildClasses().Should().Be("fa-float");
        new Jello().BuildClasses().Should().Be("fa-jello");
        new Flip360().BuildClasses().Should().Be("fa-flip-360");
        new Swing().BuildClasses().Should().Be("fa-swing");
        new Wag().BuildClasses().Should().Be("fa-wag");
        new SpinSnap().BuildClasses().Should().Be("fa-spin-snap");
        new SpinSnap(new SpinSnapOptions { Size = SpinSnapSize.Four }).BuildClasses().Should().Be("fa-spin-snap-4");
        new SpinSnap(new SpinSnapOptions { Size = SpinSnapSize.Eight }).BuildClasses().Should().Be("fa-spin-snap-8");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void NewAnimations_ProduceNoStylesByDefault() {
        new Buzz().BuildStyles().Should().BeEmpty();
        new Float().BuildStyles().Should().BeEmpty();
        new Jello().BuildStyles().Should().BeEmpty();
        new Flip360().BuildStyles().Should().BeEmpty();
        new Swing().BuildStyles().Should().BeEmpty();
        new Wag().BuildStyles().Should().BeEmpty();
        new SpinSnap().BuildStyles().Should().BeEmpty();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Factories_ReturnExpectedConcreteTypes() {
        Animate.Buzz().Should().BeOfType<Buzz>();
        Animate.Float().Should().BeOfType<Float>();
        Animate.Jello().Should().BeOfType<Jello>();
        Animate.Flip360().Should().BeOfType<Flip360>();
        Animate.Swing().Should().BeOfType<Swing>();
        Animate.Wag().Should().BeOfType<Wag>();
        Animate.SpinSnap().Should().BeOfType<SpinSnap>();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void SpinSnapFactory_ForwardsSize() {
        var animation = Animate.SpinSnap(new SpinSnapOptions { Size = SpinSnapSize.Eight });

        animation.Options.Size.Should().Be(SpinSnapSize.Eight);
        animation.BuildClasses().Should().Be("fa-spin-snap-8");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void FluentSetters_PreserveConcreteType() {
        var buzz = Animate.Buzz()
                          .SetDelay(TimeSpan.FromSeconds(1))
                          .SetDuration(TimeSpan.FromSeconds(2))
                          .SetIterationCount(3);

        buzz.GetType().Should().Be(typeof(Buzz));
        buzz.Options.Delay.Should().Be(TimeSpan.FromSeconds(1));
        buzz.Options.Duration.Should().Be(TimeSpan.FromSeconds(2));
        buzz.Options.IterationCount!.Value.Should().Be(3f);
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

        styles["--fa-float-drift"].Should().Be("2px");
        styles["--fa-float-height"].Should().Be("8px");
        styles["--fa-float-squash-x"].Should().Be("1.02");
        styles["--fa-float-squash-y"].Should().Be("0.98");
        styles["--fa-float-stretch-x"].Should().Be("0.98");
        styles["--fa-float-stretch-y"].Should().Be("1.03");
        styles["--fa-float-tilt"].Should().Be("1.5deg");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuzzOptions_EmitsDocumentedCustomProperty() {
        var styles = new Buzz(new BuzzOptions { Distance = "4px" }).BuildStyles();

        styles["--fa-buzz-distance"].Should().Be("4px");
        styles.Should().HaveCount(1);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void JelloOptions_EmitDocumentedCustomProperties() {
        var styles = new Jello(new JelloOptions { ScaleX = 1.15f, ScaleY = 1.12f }).BuildStyles();

        styles["--fa-jello-scale-x"].Should().Be("1.15");
        styles["--fa-jello-scale-y"].Should().Be("1.12");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void SwingOptions_EmitsDocumentedCustomProperty() {
        var styles = new Swing(new SwingOptions { Angle = 22.5f }).BuildStyles();

        styles["--fa-swing-angle"].Should().Be("22.5deg");
        styles.Should().HaveCount(1);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void WagOptions_EmitsDocumentedCustomProperty() {
        var styles = new Wag(new WagOptions { Angle = 12 }).BuildStyles();

        styles["--fa-wag-angle"].Should().Be("12deg");
        styles.Should().HaveCount(1);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void NewAnimationOptions_ProduceNoStylesWhenUnset() {
        new Float(new FloatOptions { Tilt = null }).BuildStyles().Should().BeEmpty();
        new Buzz(new BuzzOptions()).BuildStyles().Should().BeEmpty();
        new Jello(new JelloOptions()).BuildStyles().Should().BeEmpty();
        new Swing(new SwingOptions()).BuildStyles().Should().BeEmpty();
        new Wag(new WagOptions()).BuildStyles().Should().BeEmpty();
    }
}
