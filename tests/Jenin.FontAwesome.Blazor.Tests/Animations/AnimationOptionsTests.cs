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

        Assert.IsNull(options.Delay);
        Assert.IsNull(options.Duration);
        Assert.AreEqual(AnimationDirection.None, options.Direction);
        Assert.IsNull(options.IterationCount);
        Assert.IsNull(options.TimingFunction);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void WithExpression_PreservesUnspecifiedValues() {
        var options = new BeatOptions { Delay = TimeSpan.FromSeconds(1), TimingFunction = "linear" };

        var updated = options with { Duration = TimeSpan.FromSeconds(2) };

        Assert.AreEqual(TimeSpan.FromSeconds(1), updated.Delay);
        Assert.AreEqual(TimeSpan.FromSeconds(2), updated.Duration);
        Assert.AreEqual("linear", updated.TimingFunction);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void DerivedOptions_InheritSharedSettingsAndAddTheirOwn() {
        var options = new BounceOptions { Delay = TimeSpan.FromSeconds(1), Rebound = 0.5f };

        Assert.IsInstanceOfType<AnimationOptions>(options);
        Assert.AreEqual(TimeSpan.FromSeconds(1), options.Delay);
        Assert.AreEqual(0.5f, options.Rebound);
    }
}
