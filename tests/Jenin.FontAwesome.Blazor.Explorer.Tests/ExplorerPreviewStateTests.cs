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

        Assert.AreEqual(PreviewAnimation.Beat, state.Animation, "The selected animation must be kept.");
        Assert.IsFalse(state.UseAnimationDuration);
        Assert.AreEqual(ExplorerPreviewState.DEFAULT_ANIMATION_DURATION, state.AnimationDuration);
        Assert.IsFalse(state.UseAnimationIterationCount);
        Assert.AreEqual(ExplorerPreviewState.DEFAULT_ANIMATION_ITERATION_COUNT, state.AnimationIterationCount);
        Assert.AreEqual(AnimationDirection.None, state.AnimationDirection);
        Assert.IsNull(state.AnimationTimingFunction);
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

        Assert.AreEqual(IconStyle.Brands, state.Style, "Style is intentionally kept.");
        Assert.AreEqual(Typeface.Sharp, state.Typeface, "Typeface is intentionally kept.");
        Assert.AreEqual(IconSize.NotSet, state.Size);
        Assert.AreEqual(IconWidth.NotSet, state.WidthMode);
        Assert.IsFalse(state.FixedWidth);
        Assert.IsFalse(state.UseColor);
        Assert.AreEqual(ExplorerPreviewState.DEFAULT_COLOR, state.Color);
        Assert.IsFalse(state.UseBackgroundColor);
        Assert.AreEqual(ExplorerPreviewState.DEFAULT_BACKGROUND_COLOR, state.BackgroundColor);
        Assert.AreEqual(IconRotate.NotSet, state.Rotate);
        Assert.AreEqual(PullPosition.None, state.PullPosition);
        Assert.IsFalse(state.Bordered);
        Assert.IsFalse(state.Inverse);
        Assert.IsFalse(state.SwapOpacity);
        Assert.IsFalse(state.UseDuotone);
        Assert.IsFalse(state.UseDuotoneOpacity);
        Assert.AreEqual(ExplorerPreviewState.DEFAULT_DUOTONE_PRIMARY_OPACITY, state.DuotonePrimaryOpacity);
        Assert.AreEqual(ExplorerPreviewState.DEFAULT_DUOTONE_SECONDARY_OPACITY, state.DuotoneSecondaryOpacity);
        Assert.AreEqual(PreviewAnimation.None, state.Animation);
        Assert.IsFalse(state.UseAnimationDuration);
        Assert.IsFalse(state.UseTransform);
        Assert.AreEqual(ExplorerPreviewState.DEFAULT_TRANSFORM_SCALE, state.TransformScale);
        Assert.IsTrue(state.TransformGrow);
        Assert.AreEqual(ExplorerPreviewState.DEFAULT_TRANSFORM_ROTATE, state.TransformRotate);
        Assert.AreEqual(0f, state.TransformX);
        Assert.AreEqual(0f, state.TransformY);
        Assert.IsFalse(state.TransformFlipHorizontal);
        Assert.IsFalse(state.TransformFlipVertical);
        Assert.IsFalse(state.UseMask);
    }
}
