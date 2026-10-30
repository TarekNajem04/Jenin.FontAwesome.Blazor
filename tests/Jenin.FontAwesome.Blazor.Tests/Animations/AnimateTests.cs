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

        Assert.IsInstanceOfType<Beat>(animation);
        Assert.IsNull(animation.Options.Scale);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Beat_SetsScale() {
        var animation = Animate.Beat(new BeatOptions { Scale = 2 });

        Assert.AreEqual(2f, animation.Options.Scale);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Fade_SetsOpacity() {
        var animation = Animate.Fade(new FadeOptions { Opacity = 0.5f });

        Assert.IsInstanceOfType<Fade>(animation);
        Assert.AreEqual(0.5f, animation.Options.Opacity);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BeatFade_SetsOpacityAndScale() {
        var animation = Animate.BeatFade(new BeatFadeOptions { Opacity = 0.25f, Scale = 3 });

        Assert.AreEqual(0.25f, animation.Options.Opacity);
        Assert.AreEqual(3f, animation.Options.Scale);
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

        Assert.AreEqual(1f, animation.Options.Rebound);
        Assert.AreEqual(2f, animation.Options.Height);
        Assert.AreEqual(3f, animation.Options.StartScaleX);
        Assert.AreEqual(4f, animation.Options.StartScaleY);
        Assert.AreEqual(5f, animation.Options.JumpScaleX);
        Assert.AreEqual(6f, animation.Options.JumpScaleY);
        Assert.AreEqual(7f, animation.Options.LandScaleX);
        Assert.AreEqual(8f, animation.Options.LandScaleY);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Flip_SetsVectorAndAngle() {
        var animation = Animate.Flip(new FlipOptions { FlipX = 1, FlipY = 0, FlipZ = 1, FlipAngle = 90 });

        Assert.AreEqual(1f, animation.Options.FlipX);
        Assert.AreEqual(0f, animation.Options.FlipY);
        Assert.AreEqual(1f, animation.Options.FlipZ);
        Assert.AreEqual(90f, animation.Options.FlipAngle);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Shake_ReturnsShake() => Assert.IsInstanceOfType<Shake>(Animate.Shake());

    [TestMethod]
    [TestCategory("Model")]
    public void Spin_SetsPulseAndReverse() {
        var animation = Animate.Spin(new SpinOptions { Pulse = true, Reverse = true });

        Assert.IsTrue(animation.Options.Pulse);
        Assert.IsTrue(animation.Options.Reverse);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Factories_FlowOptionsToConcreteAnimation() {
        var options = new BeatOptions { Delay = TimeSpan.FromMilliseconds(50) };

        var animation = Animate.Beat(options);

        Assert.AreSame(options, animation.Options);
    }
}
