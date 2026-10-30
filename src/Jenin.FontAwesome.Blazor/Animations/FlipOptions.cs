//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Options for the <see cref="Flip"/> animation.
/// </summary>
public sealed record FlipOptions : AnimationOptions {
    /// <summary>
    /// The x-coordinate of the vector denoting the axis of rotation (normalized to the range 0 to 1).
    /// </summary>
    public float? FlipX {
        get;
        init => field = NormalizeUnitInterval(value);
    }

    /// <summary>
    /// The y-coordinate of the vector denoting the axis of rotation (normalized to the range 0 to 1).
    /// </summary>
    public float? FlipY {
        get;
        init => field = NormalizeUnitInterval(value);
    }

    /// <summary>
    /// The z-coordinate of the vector denoting the axis of rotation (normalized to the range 0 to 1).
    /// </summary>
    public float? FlipZ {
        get;
        init => field = NormalizeUnitInterval(value);
    }

    /// <summary>
    /// The rotation angle of the flip. A positive angle denotes a clockwise rotation, a negative angle a counter-clockwise one.
    /// </summary>
    public float? FlipAngle { get; init; }

    /// <summary>
    /// The overshoot scale factor reached at the apex of the flip.
    /// </summary>
    public float? Overshoot { get; init; }

    /// <summary>
    /// The anticipation scale factor applied before the flip starts.
    /// </summary>
    public float? AnticipationScale { get; init; }
}
