//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Animations;

[TestClass]
public class AnimationOptionsTests {
    [TestMethod]
    [TestCategory("Model")]
    public void Constructor_DefaultsToEmptyOptions() {
        var options = new BeatOptions();

        options.Delay.Should().BeNull();
        options.Duration.Should().BeNull();
        options.Direction.Should().Be(AnimationDirection.None);
        options.IterationCount.Should().BeNull();
        options.TimingFunction.Should().BeNull();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void WithExpression_PreservesUnspecifiedValues() {
        var options = new BeatOptions { Delay = TimeSpan.FromSeconds(1), TimingFunction = "linear" };

        var updated = options with { Duration = TimeSpan.FromSeconds(2) };

        updated.Delay.Should().Be(TimeSpan.FromSeconds(1));
        updated.Duration.Should().Be(TimeSpan.FromSeconds(2));
        updated.TimingFunction.Should().Be("linear");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void DerivedOptions_InheritSharedSettingsAndAddTheirOwn() {
        var options = new BounceOptions { Delay = TimeSpan.FromSeconds(1), Rebound = 0.5f };

        options.Should().BeAssignableTo<AnimationOptions>();
        options.Delay.Should().Be(TimeSpan.FromSeconds(1));
        options.Rebound.Should().Be(0.5f);
    }
}
