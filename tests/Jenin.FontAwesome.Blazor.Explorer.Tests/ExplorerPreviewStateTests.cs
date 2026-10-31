//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Tests;

[TestClass]
public sealed class ExplorerPreviewStateTests {
    [TestMethod]
    public void ResetAnimationOptions_Restores_Defaults_And_Keeps_Animation() {
        var state = new ExplorerPreviewState {
            Animation = PreviewAnimation.Beat,
            UseAnimationDuration = true,
            AnimationDuration = 4f,
            UseAnimationIterationCount = true,
            AnimationIterationCount = 9f,
            AnimationDirection = AnimationDirection.AlternateReverse,
            AnimationTimingFunction = "linear",
        };

        state.ResetAnimationOptions();

        state.Animation.Should().Be(PreviewAnimation.Beat, "The selected animation must be kept.");
        state.UseAnimationDuration.Should().BeFalse();
        state.AnimationDuration.Should().Be(ExplorerPreviewState.DEFAULT_ANIMATION_DURATION);
        state.UseAnimationIterationCount.Should().BeFalse();
        state.AnimationIterationCount.Should().Be(ExplorerPreviewState.DEFAULT_ANIMATION_ITERATION_COUNT);
        state.AnimationDirection.Should().Be(AnimationDirection.None);
        state.AnimationTimingFunction.Should().BeNull();
    }

    [TestMethod]
    public void Reset_Restores_Every_Option_To_Default() {
        var state = new ExplorerPreviewState {
            Style = IconStyle.Brands,
            Typeface = Typeface.Sharp,
            Size = IconSize.ExtraLarge,
            WidthMode = IconWidth.Fixed,
            FixedWidth = true,
            UseColor = true,
            Color = "#ff0000",
            UseBackgroundColor = true,
            BackgroundColor = "#00ff00",
            Rotate = IconRotate.RotateBy,
            RotateAngle = 90f,
            PullPosition = PullPosition.Start,
            Bordered = true,
            Inverse = true,
            SwapOpacity = true,
            UseDuotone = true,
            UseDuotoneOpacity = true,
            DuotonePrimaryOpacity = 0.9f,
            DuotoneSecondaryOpacity = 0.1f,
            Animation = PreviewAnimation.Spin,
            UseAnimationDuration = true,
            AnimationDuration = 3f,
            UseTransform = true,
            TransformScale = 16f,
            TransformGrow = false,
            TransformRotate = 45f,
            TransformX = 4f,
            TransformY = 5f,
            TransformFlipHorizontal = true,
            TransformFlipVertical = true,
            UseMask = true,
            MaskIconName = "fa-star",
            MaskIconStyle = IconStyle.Regular,
        };

        state.Reset();

        state.Style.Should().Be(IconStyle.Brands, "Style is intentionally kept.");
        state.Typeface.Should().Be(Typeface.Sharp, "Typeface is intentionally kept.");
        state.Size.Should().Be(IconSize.NotSet);
        state.WidthMode.Should().Be(IconWidth.NotSet);
        state.FixedWidth.Should().BeFalse();
        state.UseColor.Should().BeFalse();
        state.Color.Should().Be(ExplorerPreviewState.DEFAULT_COLOR);
        state.UseBackgroundColor.Should().BeFalse();
        state.BackgroundColor.Should().Be(ExplorerPreviewState.DEFAULT_BACKGROUND_COLOR);
        state.Rotate.Should().Be(IconRotate.NotSet);
        state.PullPosition.Should().Be(PullPosition.None);
        state.Bordered.Should().BeFalse();
        state.Inverse.Should().BeFalse();
        state.SwapOpacity.Should().BeFalse();
        state.UseDuotone.Should().BeFalse();
        state.UseDuotoneOpacity.Should().BeFalse();
        state.DuotonePrimaryOpacity.Should().Be(ExplorerPreviewState.DEFAULT_DUOTONE_PRIMARY_OPACITY);
        state.DuotoneSecondaryOpacity.Should().Be(ExplorerPreviewState.DEFAULT_DUOTONE_SECONDARY_OPACITY);
        state.Animation.Should().Be(PreviewAnimation.None);
        state.UseAnimationDuration.Should().BeFalse();
        state.UseTransform.Should().BeFalse();
        state.TransformScale.Should().Be(ExplorerPreviewState.DEFAULT_TRANSFORM_SCALE);
        state.TransformGrow.Should().BeTrue();
        state.TransformRotate.Should().Be(ExplorerPreviewState.DEFAULT_TRANSFORM_ROTATE);
        state.TransformX.Should().Be(0f);
        state.TransformY.Should().Be(0f);
        state.TransformFlipHorizontal.Should().BeFalse();
        state.TransformFlipVertical.Should().BeFalse();
        state.UseMask.Should().BeFalse();
    }
}
