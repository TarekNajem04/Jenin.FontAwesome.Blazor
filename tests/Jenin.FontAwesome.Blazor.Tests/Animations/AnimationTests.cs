//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Animations;

[TestClass]
public class AnimationTests {
    [TestMethod]
    [TestCategory("Model")]
    public void BuildClasses_ReturnsBaseFontAwesomeClass() {
        Animation animation = new Beat();

        Assert.AreEqual("fa-beat", animation.BuildClasses());
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithoutOptions_IsEmpty() {
        var styles = new Beat().BuildStyles();

        Assert.IsEmpty(styles);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithOptions_EmitsDocumentedCustomProperties() {
        var options = new BeatOptions {
            Delay = TimeSpan.FromMilliseconds(500),
            Duration = TimeSpan.FromSeconds(2),
            Direction = AnimationDirection.Reverse,
            IterationCount = 3,
            TimingFunction = "ease-in"
        };

        var styles = new Beat(options).BuildStyles();

        Assert.AreEqual("500ms", styles["--fa-animation-delay"]);
        Assert.AreEqual("2000ms", styles["--fa-animation-duration"]);
        Assert.AreEqual("reverse", styles["--fa-animation-direction"]);
        Assert.AreEqual("3.00", styles["--fa-animation-iteration-count"]);
        Assert.AreEqual("ease-in", styles["--fa-animation-timing"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_DirectionNone_OmitsDirection() {
        var styles = new Beat(new BeatOptions { Direction = AnimationDirection.None }).BuildStyles();

        Assert.IsFalse(styles.ContainsKey("--fa-animation-direction"));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_PartialMilliseconds_FormatsInvariantly() {
        var styles = new Beat(new BeatOptions { Delay = TimeSpan.FromMilliseconds(250.5) }).BuildStyles();

        Assert.AreEqual("250.5ms", styles["--fa-animation-delay"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(AnimationDirection.Normal, "normal")]
    [DataRow(AnimationDirection.Reverse, "reverse")]
    [DataRow(AnimationDirection.Alternate, "alternate")]
    [DataRow(AnimationDirection.AlternateReverse, "alternate-reverse")]
    public void BuildStyles_Direction_EmitsDirectionToken(AnimationDirection direction, string expected) {
        var styles = new Beat(new BeatOptions { Direction = direction }).BuildStyles();

        Assert.AreEqual(expected, styles["--fa-animation-direction"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AnimationOptions_NegativeIterationCount_IsClampedToZero() {
        var options = new BeatOptions { IterationCount = -3 };

        Assert.AreEqual(0f, options.IterationCount!.Value);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void SetDelay_ReturnsNewInstancePreservingRuntimeType() {
        var original = new Beat();

        var updated = original.SetDelay(TimeSpan.FromSeconds(1));

        Assert.IsInstanceOfType<Beat>(updated);
        Assert.AreNotSame(original, updated);
        Assert.IsNull(original.Options.Delay);
        Assert.AreEqual(TimeSpan.FromSeconds(1), updated.Options.Delay);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void SetDuration_ReturnsNewInstancePreservingRuntimeType() {
        var original = new Fade();

        var updated = original.SetDuration(TimeSpan.FromSeconds(2));

        Assert.IsInstanceOfType<Fade>(updated);
        Assert.IsNull(original.Options.Duration);
        Assert.AreEqual(TimeSpan.FromSeconds(2), updated.Options.Duration);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void SetDirection_ReturnsNewInstancePreservingRuntimeType() {
        var original = new Shake();

        var updated = original.SetDirection(AnimationDirection.Alternate);

        Assert.IsInstanceOfType<Shake>(updated);
        Assert.AreEqual(AnimationDirection.None, original.Options.Direction);
        Assert.AreEqual(AnimationDirection.Alternate, updated.Options.Direction);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void SetIterationCount_ReturnsNewInstancePreservingRuntimeType() {
        var original = new Bounce();

        var updated = original.SetIterationCount(4);

        Assert.IsInstanceOfType<Bounce>(updated);
        Assert.IsNull(original.Options.IterationCount);
        Assert.AreEqual(4f, updated.Options.IterationCount!.Value);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void SetTimingFunction_ReturnsNewInstancePreservingRuntimeType() {
        var original = new Shake();

        var updated = original.SetTimingFunction("linear");

        Assert.IsInstanceOfType<Shake>(updated);
        Assert.IsNull(original.Options.TimingFunction);
        Assert.AreEqual("linear", updated.Options.TimingFunction);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void FluentSetters_AreChainableAndPreserveRuntimeType() {
        var animation = new Beat()
            .SetDelay(TimeSpan.FromMilliseconds(100))
            .SetDuration(TimeSpan.FromMilliseconds(200))
            .SetDirection(AnimationDirection.Reverse)
            .SetIterationCount(2)
            .SetTimingFunction("ease-out");

        Assert.IsInstanceOfType<Beat>(animation);
        Assert.AreEqual(TimeSpan.FromMilliseconds(100), animation.Options.Delay);
        Assert.AreEqual(TimeSpan.FromMilliseconds(200), animation.Options.Duration);
        Assert.AreEqual(AnimationDirection.Reverse, animation.Options.Direction);
        Assert.AreEqual(2f, animation.Options.IterationCount!.Value);
        Assert.AreEqual("ease-out", animation.Options.TimingFunction);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_IsPureAndDeterministic() {
        var animation = new Beat(new BeatOptions { IterationCount = 5, Scale = 2 });

        var first = animation.BuildStyles();
        var second = animation.BuildStyles();

        Assert.HasCount(first.Count, second);

        foreach(var pair in first) {
            Assert.AreEqual(pair.Value, second[pair.Key]);
        }

        Assert.AreEqual(2f, animation.Options.Scale);
    }
}
