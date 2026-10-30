//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer;

/// <summary>
/// The Font Awesome animation families selectable in the explorer preview.
/// </summary>
public enum PreviewAnimation {
    /// <summary>No animation.</summary>
    None = 0,

    /// <summary><c>fa-beat</c>.</summary>
    Beat,

    /// <summary><c>fa-fade</c>.</summary>
    Fade,

    /// <summary><c>fa-beat-fade</c>.</summary>
    BeatFade,

    /// <summary><c>fa-bounce</c>.</summary>
    Bounce,

    /// <summary><c>fa-flip</c>.</summary>
    Flip,

    /// <summary><c>fa-shake</c>.</summary>
    Shake,

    /// <summary><c>fa-spin</c>.</summary>
    Spin,

    /// <summary><c>fa-spin-pulse</c>.</summary>
    SpinSnap,

    /// <summary><c>fa-buzz</c>.</summary>
    Buzz,

    /// <summary><c>fa-float</c>.</summary>
    Float,

    /// <summary><c>fa-jello</c>.</summary>
    Jello,

    /// <summary><c>fa-flip-360</c>.</summary>
    Flip360,

    /// <summary><c>fa-swing</c>.</summary>
    Swing,

    /// <summary><c>fa-wag</c>.</summary>
    Wag,
}

/// <summary>
/// Mutable state for the live preview controls in the explorer. Every property maps to a
/// <see cref="Icon"/> parameter; when a value is left at its default the corresponding parameter is
/// emitted with its default (no class/attribute), matching how the library omits defaults.
/// </summary>
public sealed class ExplorerPreviewState {
    /// <summary>The default foreground color, used when the foreground is applied and as the value restored by Cancel.</summary>
    public const string DEFAULT_COLOR = "#ffffff";

    /// <summary>The default background color, used when the background is applied and as the value restored by Cancel.</summary>
    public const string DEFAULT_BACKGROUND_COLOR = "#6d0ca1";

    /// <summary>The default animation duration, in seconds.</summary>
    public const float DEFAULT_ANIMATION_DURATION = 1f;

    /// <summary>The default animation iteration count.</summary>
    public const float DEFAULT_ANIMATION_ITERATION_COUNT = 3f;

    /// <summary>The default power-transform scale, in 1/16em units.</summary>
    public const float DEFAULT_TRANSFORM_SCALE = 6f;

    /// <summary>The default power-transform rotation angle, in degrees.</summary>
    public const float DEFAULT_TRANSFORM_ROTATE = -15f;

    /// <summary>The default duotone primary opacity.</summary>
    public const float DEFAULT_DUOTONE_PRIMARY_OPACITY = 0.4f;

    /// <summary>The default duotone secondary opacity.</summary>
    public const float DEFAULT_DUOTONE_SECONDARY_OPACITY = 0.4f;

    /// <summary>Gets or sets the icon style. Maps to <see cref="Icon.IconStyle"/>.</summary>
    public IconStyle Style { get; set; } = IconStyle.Solid;

    /// <summary>Gets or sets the typeface. Maps to <see cref="Icon.Typeface"/>.</summary>
    public Typeface Typeface { get; set; } = Typeface.Classic;

    /// <summary>Gets or sets the size class. Maps to <see cref="Icon.IconSize"/>.</summary>
    public IconSize Size { get; set; } = IconSize.NotSet;

    /// <summary>Gets or sets the width mode. Maps to <see cref="Icon.WidthMode"/>.</summary>
    public IconWidth WidthMode { get; set; } = IconWidth.NotSet;

    /// <summary>Gets or sets the foreground color. Maps to <see cref="Icon.Color"/> when <see cref="UseColor"/> is set.</summary>
    public string Color { get; set; } = DEFAULT_COLOR;

    /// <summary>Gets or sets a value indicating whether the foreground color is applied.</summary>
    public bool UseColor { get; set; }

    /// <summary>Gets or sets the background color. Maps to <see cref="Icon.BackgroundColor"/> when <see cref="UseBackgroundColor"/> is set.</summary>
    public string BackgroundColor { get; set; } = DEFAULT_BACKGROUND_COLOR;

    /// <summary>Gets or sets a value indicating whether the background color is applied.</summary>
    public bool UseBackgroundColor { get; set; }

    /// <summary>Gets or sets the rotation/flip token. Maps to <see cref="Icon.Rotate"/>.</summary>
    public IconRotate Rotate { get; set; } = IconRotate.NotSet;

    /// <summary>Gets or sets the arbitrary rotation angle used when <see cref="Rotate"/> is <see cref="IconRotate.RotateBy"/>.</summary>
    public float RotateAngle { get; set; } = -30f;

    /// <summary>Gets or sets the combined rotation token. Maps to <see cref="Icon.CombiningRotate"/>.</summary>
    public IconRotate CombiningRotate { get; set; } = IconRotate.NotSet;

    /// <summary>Gets or sets the pull position. Maps to <see cref="Icon.PullPosition"/>.</summary>
    public PullPosition PullPosition { get; set; } = PullPosition.None;

    /// <summary>Gets or sets a value indicating whether the icon is bordered. Maps to <see cref="Icon.Bordered"/>.</summary>
    public bool Bordered { get; set; }

    /// <summary>Gets or sets a value indicating whether the icon is inverted. Maps to <see cref="Icon.Inverse"/>.</summary>
    public bool Inverse { get; set; }

    /// <summary>Gets or sets a value indicating whether Fixed Width is applied. Maps to <see cref="Icon.FixedWidth"/>.</summary>
    public bool FixedWidth { get; set; }

    /// <summary>Gets or sets a value indicating whether duotone layer colors are applied.</summary>
    public bool UseDuotone { get; set; }

    /// <summary>Gets or sets the duotone primary layer color. Maps to <see cref="Icon.PrimaryColor"/>.</summary>
    public string DuotonePrimary { get; set; } = "#0d6efd";

    /// <summary>Gets or sets the duotone secondary layer color. Maps to <see cref="Icon.SecondaryColor"/>.</summary>
    public string DuotoneSecondary { get; set; } = "#fd7e14";

    /// <summary>Gets or sets a value indicating whether the duotone opacities are applied.</summary>
    public bool UseDuotoneOpacity { get; set; }

    /// <summary>Gets or sets the duotone primary opacity. Maps to <see cref="Icon.PrimaryOpacity"/>.</summary>
    public float DuotonePrimaryOpacity { get; set; } = DEFAULT_DUOTONE_PRIMARY_OPACITY;

    /// <summary>Gets or sets the duotone secondary opacity. Maps to <see cref="Icon.SecondaryOpacity"/>.</summary>
    public float DuotoneSecondaryOpacity { get; set; } = DEFAULT_DUOTONE_SECONDARY_OPACITY;

    /// <summary>Gets or sets a value indicating whether the duotone layer opacity is swapped. Maps to <see cref="Icon.SwapOpacity"/>.</summary>
    public bool SwapOpacity { get; set; }

    /// <summary>Gets or sets the animation family. Maps to <see cref="Icon.Animation"/>.</summary>
    public PreviewAnimation Animation { get; set; } = PreviewAnimation.None;

    /// <summary>Gets or sets a value indicating whether the animation duration is applied.</summary>
    public bool UseAnimationDuration { get; set; }

    /// <summary>Gets or sets the animation duration, in seconds.</summary>
    public float AnimationDuration { get; set; } = DEFAULT_ANIMATION_DURATION;

    /// <summary>Gets or sets a value indicating whether the animation iteration count is applied.</summary>
    public bool UseAnimationIterationCount { get; set; }

    /// <summary>Gets or sets the animation iteration count.</summary>
    public float AnimationIterationCount { get; set; } = DEFAULT_ANIMATION_ITERATION_COUNT;

    /// <summary>Gets or sets the animation direction.</summary>
    public AnimationDirection AnimationDirection { get; set; } = AnimationDirection.None;

    /// <summary>Gets or sets the animation timing function (any valid CSS <c>animation-timing-function</c>).</summary>
    public string? AnimationTimingFunction { get; set; }

    /// <summary>Gets or sets a value indicating whether the Power Transform is applied.</summary>
    public bool UseTransform { get; set; }

    /// <summary>Gets or sets the transform scaling amount in 1/16em units (values from 1 to 16).</summary>
    public float TransformScale { get; set; } = DEFAULT_TRANSFORM_SCALE;

    /// <summary>Gets or sets a value indicating whether the transform grows (otherwise it shrinks).</summary>
    public bool TransformGrow { get; set; } = true;

    /// <summary>Gets or sets the transform rotation angle in degrees.</summary>
    public float TransformRotate { get; set; } = DEFAULT_TRANSFORM_ROTATE;

    /// <summary>Gets or sets the horizontal positioning offset in 1/16em units.</summary>
    public float TransformX { get; set; }

    /// <summary>Gets or sets the vertical positioning offset in 1/16em units.</summary>
    public float TransformY { get; set; }

    /// <summary>Gets or sets a value indicating whether the transform flips horizontally.</summary>
    public bool TransformFlipHorizontal { get; set; }

    /// <summary>Gets or sets a value indicating whether the transform flips vertically.</summary>
    public bool TransformFlipVertical { get; set; }

    /// <summary>Gets or sets a value indicating whether the mask example is applied.</summary>
    public bool UseMask { get; set; }

    /// <summary>Gets or sets the mask icon name. Maps to <see cref="Icon.MaskIconName"/>.</summary>
    public string MaskIconName { get; set; } = "fa-circle";

    /// <summary>Gets or sets the mask icon style. Maps to <see cref="Icon.MaskIconStyle"/>.</summary>
    public IconStyle MaskIconStyle { get; set; } = IconStyle.Solid;

    /// <summary>
    /// Restores the animation-specific options (duration, iterations, direction, timing) to their
    /// defaults while keeping the selected animation. Called whenever the animation changes so options
    /// configured for one animation never silently carry over to the next.
    /// </summary>
    public void ResetAnimationOptions() {
        UseAnimationDuration = false;
        AnimationDuration = DEFAULT_ANIMATION_DURATION;
        UseAnimationIterationCount = false;
        AnimationIterationCount = DEFAULT_ANIMATION_ITERATION_COUNT;
        AnimationDirection = AnimationDirection.None;
        AnimationTimingFunction = null;
    }

    /// <summary>
    /// Restores every preview option to its default, keeping only the current style and typeface. Called
    /// when the selected icon changes so options configured for one icon do not carry over to the next.
    /// </summary>
    public void Reset() {
        Size = IconSize.NotSet;
        WidthMode = IconWidth.NotSet;
        FixedWidth = false;

        UseColor = false;
        Color = DEFAULT_COLOR;
        UseBackgroundColor = false;
        BackgroundColor = DEFAULT_BACKGROUND_COLOR;

        Rotate = IconRotate.NotSet;
        RotateAngle = -30f;
        CombiningRotate = IconRotate.NotSet;
        PullPosition = PullPosition.None;

        Bordered = false;
        Inverse = false;
        SwapOpacity = false;

        UseDuotone = false;
        DuotonePrimary = "#0d6efd";
        DuotoneSecondary = "#fd7e14";
        UseDuotoneOpacity = false;
        DuotonePrimaryOpacity = DEFAULT_DUOTONE_PRIMARY_OPACITY;
        DuotoneSecondaryOpacity = DEFAULT_DUOTONE_SECONDARY_OPACITY;

        Animation = PreviewAnimation.None;
        ResetAnimationOptions();

        UseTransform = false;
        TransformScale = DEFAULT_TRANSFORM_SCALE;
        TransformGrow = true;
        TransformRotate = DEFAULT_TRANSFORM_ROTATE;
        TransformX = 0f;
        TransformY = 0f;
        TransformFlipHorizontal = false;
        TransformFlipVertical = false;

        UseMask = false;
        MaskIconName = "fa-circle";
        MaskIconStyle = IconStyle.Solid;
    }
}
