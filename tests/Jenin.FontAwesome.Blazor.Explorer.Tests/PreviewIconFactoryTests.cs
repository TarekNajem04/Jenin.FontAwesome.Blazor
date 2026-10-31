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

        PreviewIconFactory.BuildAnimation(state).Should().BeNull();
    }

    [TestMethod]
    public void BuildAnimation_Covers_Every_Non_None_Value() {
        foreach(var value in Enum.GetValues<PreviewAnimation>()) {
            if(value == PreviewAnimation.None) {
                continue;
            }

            var animation = PreviewIconFactory.BuildAnimation(new ExplorerPreviewState { Animation = value });

            animation.Should().NotBeNull(value.ToString());
            string.IsNullOrEmpty(animation!.BuildClasses()).Should().BeFalse(value.ToString());
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

        animation.Should().NotBeNull();
        animation!.Options.Duration.Should().Be(TimeSpan.FromSeconds(2));
        animation.Options.IterationCount.Should().Be(5f);
        animation.Options.Direction.Should().Be(AnimationDirection.Reverse);
        animation.Options.TimingFunction.Should().Be("linear");
    }

    [TestMethod]
    public void BuildAnimation_Leaves_Unset_Options_Null() {
        var state = new ExplorerPreviewState { Animation = PreviewAnimation.Beat };

        var animation = PreviewIconFactory.BuildAnimation(state);

        animation.Should().NotBeNull();
        animation!.Options.Duration.Should().BeNull();
        animation.Options.IterationCount.Should().BeNull();
        animation.Options.Direction.Should().Be(AnimationDirection.None);
        animation.Options.TimingFunction.Should().BeNull();
    }

    [TestMethod]
    public void BuildTransform_Returns_Null_When_Disabled() {
        var state = new ExplorerPreviewState { UseTransform = false, TransformGrow = true };

        PreviewIconFactory.BuildTransform(state).Should().BeNull();
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

        transform.Should().NotBeNull();
        var value = transform!.GetTransformation();
        value.Should().Contain("grow-8");
        value.Should().Contain("right-4");
        value.Should().Contain("up-2");
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

        transform.Should().NotBeNull();
        var value = transform!.GetTransformation();
        value.Should().Contain("shrink-6");
        value.Should().Contain("rotate--15");
        value.Should().Contain("flip-h");
        value.Should().Contain("flip-v");
    }
}
