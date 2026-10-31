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

        animation.BuildClasses().Should().Be("fa-beat");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_WithoutOptions_IsEmpty() {
        var styles = new Beat().BuildStyles();

        styles.Should().BeEmpty();
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

        styles["--fa-animation-delay"].Should().Be("500ms");
        styles["--fa-animation-duration"].Should().Be("2000ms");
        styles["--fa-animation-direction"].Should().Be("reverse");
        styles["--fa-animation-iteration-count"].Should().Be("3.00");
        styles["--fa-animation-timing"].Should().Be("ease-in");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_DirectionNone_OmitsDirection() {
        var styles = new Beat(new BeatOptions { Direction = AnimationDirection.None }).BuildStyles();

        styles.ContainsKey("--fa-animation-direction").Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_PartialMilliseconds_FormatsInvariantly() {
        var styles = new Beat(new BeatOptions { Delay = TimeSpan.FromMilliseconds(250.5) }).BuildStyles();

        styles["--fa-animation-delay"].Should().Be("250.5ms");
    }

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(AnimationDirection.Normal, "normal")]
    [DataRow(AnimationDirection.Reverse, "reverse")]
    [DataRow(AnimationDirection.Alternate, "alternate")]
    [DataRow(AnimationDirection.AlternateReverse, "alternate-reverse")]
    public void BuildStyles_Direction_EmitsDirectionToken(AnimationDirection direction, string expected) {
        var styles = new Beat(new BeatOptions { Direction = direction }).BuildStyles();

        styles["--fa-animation-direction"].Should().Be(expected);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AnimationOptions_NegativeIterationCount_IsClampedToZero() {
        var options = new BeatOptions { IterationCount = -3 };

        options.IterationCount!.Value.Should().Be(0f);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void SetDelay_ReturnsNewInstancePreservingRuntimeType() {
        var original = new Beat();

        var updated = original.SetDelay(TimeSpan.FromSeconds(1));

        updated.Should().BeOfType<Beat>();
        ReferenceEquals(original, updated).Should().BeFalse();
        original.Options.Delay.Should().BeNull();
        updated.Options.Delay.Should().Be(TimeSpan.FromSeconds(1));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void SetDuration_ReturnsNewInstancePreservingRuntimeType() {
        var original = new Fade();

        var updated = original.SetDuration(TimeSpan.FromSeconds(2));

        updated.Should().BeOfType<Fade>();
        original.Options.Duration.Should().BeNull();
        updated.Options.Duration.Should().Be(TimeSpan.FromSeconds(2));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void SetDirection_ReturnsNewInstancePreservingRuntimeType() {
        var original = new Shake();

        var updated = original.SetDirection(AnimationDirection.Alternate);

        updated.Should().BeOfType<Shake>();
        original.Options.Direction.Should().Be(AnimationDirection.None);
        updated.Options.Direction.Should().Be(AnimationDirection.Alternate);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void SetIterationCount_ReturnsNewInstancePreservingRuntimeType() {
        var original = new Bounce();

        var updated = original.SetIterationCount(4);

        updated.Should().BeOfType<Bounce>();
        original.Options.IterationCount.Should().BeNull();
        updated.Options.IterationCount!.Value.Should().Be(4f);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void SetTimingFunction_ReturnsNewInstancePreservingRuntimeType() {
        var original = new Shake();

        var updated = original.SetTimingFunction("linear");

        updated.Should().BeOfType<Shake>();
        original.Options.TimingFunction.Should().BeNull();
        updated.Options.TimingFunction.Should().Be("linear");
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

        animation.Should().BeOfType<Beat>();
        animation.Options.Delay.Should().Be(TimeSpan.FromMilliseconds(100));
        animation.Options.Duration.Should().Be(TimeSpan.FromMilliseconds(200));
        animation.Options.Direction.Should().Be(AnimationDirection.Reverse);
        animation.Options.IterationCount!.Value.Should().Be(2f);
        animation.Options.TimingFunction.Should().Be("ease-out");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void BuildStyles_IsPureAndDeterministic() {
        var animation = new Beat(new BeatOptions { IterationCount = 5, Scale = 2 });

        var first = animation.BuildStyles();
        var second = animation.BuildStyles();

        second.Should().HaveCount(first.Count);

        foreach(var pair in first) {
            second[pair.Key].Should().Be(pair.Value);
        }

        animation.Options.Scale.Should().Be(2f);
    }
}
