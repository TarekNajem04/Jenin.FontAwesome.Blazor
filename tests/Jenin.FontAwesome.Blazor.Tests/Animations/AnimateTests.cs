//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Animations;

[TestClass]
public class AnimateTests {
    [TestMethod]
    [TestCategory("Model")]
    public void Beat_ReturnsBeat() {
        var animation = Animate.Beat();

        animation.Should().BeOfType<Beat>();
        animation.Options.Scale.Should().BeNull();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Beat_SetsScale() {
        var animation = Animate.Beat(new BeatOptions { Scale = 2 });

        animation.Options.Scale.Should().Be(2f);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Fade_SetsOpacity() {
        var animation = Animate.Fade(new FadeOptions { Opacity = 0.5f });

        animation.Should().BeOfType<Fade>();
        animation.Options.Opacity.Should().Be(0.5f);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BeatFade_SetsOpacityAndScale() {
        var animation = Animate.BeatFade(new BeatFadeOptions { Opacity = 0.25f, Scale = 3 });

        animation.Options.Opacity.Should().Be(0.25f);
        animation.Options.Scale.Should().Be(3f);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Bounce_SetsAllDistortionValues() {
        var animation = Animate.Bounce(new BounceOptions {
            Rebound = 1,
            Height = 2,
            StartScaleX = 3,
            StartScaleY = 4,
            JumpScaleX = 5,
            JumpScaleY = 6,
            LandScaleX = 7,
            LandScaleY = 8
        });

        animation.Options.Rebound.Should().Be(1f);
        animation.Options.Height.Should().Be(2f);
        animation.Options.StartScaleX.Should().Be(3f);
        animation.Options.StartScaleY.Should().Be(4f);
        animation.Options.JumpScaleX.Should().Be(5f);
        animation.Options.JumpScaleY.Should().Be(6f);
        animation.Options.LandScaleX.Should().Be(7f);
        animation.Options.LandScaleY.Should().Be(8f);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Flip_SetsVectorAndAngle() {
        var animation = Animate.Flip(new FlipOptions { FlipX = 1, FlipY = 0, FlipZ = 1, FlipAngle = 90 });

        animation.Options.FlipX.Should().Be(1f);
        animation.Options.FlipY.Should().Be(0f);
        animation.Options.FlipZ.Should().Be(1f);
        animation.Options.FlipAngle.Should().Be(90f);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Shake_ReturnsShake() => Animate.Shake().Should().BeOfType<Shake>();

    [TestMethod]
    [TestCategory("Model")]
    public void Spin_SetsPulseAndReverse() {
        var animation = Animate.Spin(new SpinOptions { Pulse = true, Reverse = true });

        animation.Options.Pulse.Should().BeTrue();
        animation.Options.Reverse.Should().BeTrue();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Factories_FlowOptionsToConcreteAnimation() {
        var options = new BeatOptions { Delay = TimeSpan.FromMilliseconds(50) };

        var animation = Animate.Beat(options);

        animation.Options.Should().BeSameAs(options);
    }
}
