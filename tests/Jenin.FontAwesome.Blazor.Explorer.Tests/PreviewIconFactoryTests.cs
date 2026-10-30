//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Tests;

[TestClass]
public sealed class PreviewIconFactoryTests {
    [TestMethod]
    public void BuildAnimation_Returns_Null_For_None() {
        var state = new ExplorerPreviewState { Animation = PreviewAnimation.None };

        Assert.IsNull(PreviewIconFactory.BuildAnimation(state));
    }

    [TestMethod]
    public void BuildAnimation_Covers_Every_Non_None_Value() {
        foreach(var value in Enum.GetValues<PreviewAnimation>()) {
            if(value == PreviewAnimation.None) {
                continue;
            }

            var animation = PreviewIconFactory.BuildAnimation(new ExplorerPreviewState { Animation = value });

            Assert.IsNotNull(animation, value.ToString());
            Assert.IsFalse(string.IsNullOrEmpty(animation.BuildClasses()), value.ToString());
        }
    }

    [TestMethod]
    public void BuildAnimation_Applies_Selected_Options() {
        var state = new ExplorerPreviewState {
            Animation = PreviewAnimation.Spin,
            UseAnimationDuration = true,
            AnimationDuration = 2f,
            UseAnimationIterationCount = true,
            AnimationIterationCount = 5f,
            AnimationDirection = AnimationDirection.Reverse,
            AnimationTimingFunction = "linear",
        };

        var animation = PreviewIconFactory.BuildAnimation(state);

        Assert.IsNotNull(animation);
        Assert.AreEqual(TimeSpan.FromSeconds(2), animation.Options.Duration);
        Assert.AreEqual(5f, animation.Options.IterationCount);
        Assert.AreEqual(AnimationDirection.Reverse, animation.Options.Direction);
        Assert.AreEqual("linear", animation.Options.TimingFunction);
    }

    [TestMethod]
    public void BuildAnimation_Leaves_Unset_Options_Null() {
        var state = new ExplorerPreviewState { Animation = PreviewAnimation.Beat };

        var animation = PreviewIconFactory.BuildAnimation(state);

        Assert.IsNotNull(animation);
        Assert.IsNull(animation.Options.Duration);
        Assert.IsNull(animation.Options.IterationCount);
        Assert.AreEqual(AnimationDirection.None, animation.Options.Direction);
        Assert.IsNull(animation.Options.TimingFunction);
    }

    [TestMethod]
    public void BuildTransform_Returns_Null_When_Disabled() {
        var state = new ExplorerPreviewState { UseTransform = false, TransformGrow = true };

        Assert.IsNull(PreviewIconFactory.BuildTransform(state));
    }

    [TestMethod]
    public void BuildTransform_Emits_Grow_And_Move() {
        var state = new ExplorerPreviewState {
            UseTransform = true,
            TransformGrow = true,
            TransformScale = 8f,
            TransformX = 4f,
            TransformY = -2f,
        };

        var transform = PreviewIconFactory.BuildTransform(state);

        Assert.IsNotNull(transform);
        var value = transform.GetTransformation();
        Assert.Contains("grow-8", value);
        Assert.Contains("right-4", value);
        Assert.Contains("up-2", value);
    }

    [TestMethod]
    public void BuildTransform_Emits_Shrink_Rotate_And_Flips() {
        var state = new ExplorerPreviewState {
            UseTransform = true,
            TransformGrow = false,
            TransformScale = 6f,
            TransformRotate = -15f,
            TransformFlipHorizontal = true,
            TransformFlipVertical = true,
        };

        var transform = PreviewIconFactory.BuildTransform(state);

        Assert.IsNotNull(transform);
        var value = transform.GetTransformation();
        Assert.Contains("shrink-6", value);
        Assert.Contains("rotate--15", value);
        Assert.Contains("flip-h", value);
        Assert.Contains("flip-v", value);
    }
}
