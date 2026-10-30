//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Explorer;

/// <summary>
/// Builds the library animation and power-transform values from an <see cref="ExplorerPreviewState"/>.
/// </summary>
/// <remarks>
/// Shared by the live preview and the snippet generator so that the rendered icon and the generated
/// code always describe the same configuration.
/// </remarks>
internal static class PreviewIconFactory {
    /// <summary>
    /// Builds the <see cref="Animation"/> for <paramref name="state"/>, or <c>null</c> when no animation is selected.
    /// </summary>
    /// <param name="state">The preview state.</param>
    /// <returns>The animation, or <c>null</c>.</returns>
    public static Animation? BuildAnimation(ExplorerPreviewState state) {
        ArgumentNullException.ThrowIfNull(state);

        return state.Animation switch {
            PreviewAnimation.Beat => Animate.Beat(Options<BeatOptions>(state)),
            PreviewAnimation.Fade => Animate.Fade(Options<FadeOptions>(state)),
            PreviewAnimation.BeatFade => Animate.BeatFade(Options<BeatFadeOptions>(state)),
            PreviewAnimation.Bounce => Animate.Bounce(Options<BounceOptions>(state)),
            PreviewAnimation.Flip => Animate.Flip(Options<FlipOptions>(state)),
            PreviewAnimation.Shake => Animate.Shake(Options<ShakeOptions>(state)),
            PreviewAnimation.Spin => Animate.Spin(Options<SpinOptions>(state)),
            PreviewAnimation.SpinSnap => Animate.SpinSnap(Options<SpinSnapOptions>(state)),
            PreviewAnimation.Buzz => Animate.Buzz(Options<BuzzOptions>(state)),
            PreviewAnimation.Float => Animate.Float(Options<FloatOptions>(state)),
            PreviewAnimation.Jello => Animate.Jello(Options<JelloOptions>(state)),
            PreviewAnimation.Flip360 => Animate.Flip360(Options<Flip360Options>(state)),
            PreviewAnimation.Swing => Animate.Swing(Options<SwingOptions>(state)),
            PreviewAnimation.Wag => Animate.Wag(Options<WagOptions>(state)),
            _ => null,
        };
    }

    /// <summary>
    /// Builds the <see cref="Transform"/> for <paramref name="state"/>, or <c>null</c> when the power
    /// transform is disabled.
    /// </summary>
    /// <param name="state">The preview state.</param>
    /// <returns>The transform, or <c>null</c>.</returns>
    public static Transform? BuildTransform(ExplorerPreviewState state) {
        ArgumentNullException.ThrowIfNull(state);

        if(!state.UseTransform) {
            return null;
        }

        var transform = new Transform();
        transform = state.TransformGrow ? transform.Grow(state.TransformScale) : transform.Shrink(state.TransformScale);

        if(state.TransformX > 0) {
            transform = transform.Right(state.TransformX);
        } else if(state.TransformX < 0) {
            transform = transform.Left(-state.TransformX);
        }

        if(state.TransformY > 0) {
            transform = transform.Down(state.TransformY);
        } else if(state.TransformY < 0) {
            transform = transform.Up(-state.TransformY);
        }

        if(state.TransformRotate != 0) {
            transform = transform.Rotate(state.TransformRotate);
        }

        if(state.TransformFlipHorizontal && state.TransformFlipVertical) {
            return transform.FlipBoth();
        } else if(state.TransformFlipHorizontal) {
            return transform.FlipH();
        } else if(state.TransformFlipVertical) {
            return transform.FlipV();
        }

        return transform;
    }

    private static T Options<T>(ExplorerPreviewState state) where T : AnimationOptions, new() => new() {
        Duration = state.UseAnimationDuration ? TimeSpan.FromSeconds(state.AnimationDuration) : null,
        IterationCount = state.UseAnimationIterationCount ? state.AnimationIterationCount : null,
        Direction = state.AnimationDirection,
        TimingFunction = string.IsNullOrWhiteSpace(state.AnimationTimingFunction) ? null : state.AnimationTimingFunction,
    };
}
