//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Specifies the direction in which an animation is played.
/// <para>
/// <a href="https://developer.mozilla.org/en-US/docs/Web/CSS/animation-direction">CSS animation-direction</a>.
/// </para>
/// </summary>
public enum AnimationDirection {
    /// <summary>
    /// No direction is specified and no CSS <c>animation-direction</c> value is emitted, leaving the browser default.
    /// </summary>
    [Value("")]
    None = 0,

    /// <summary>
    /// Plays the animation forwards each cycle, as in CSS <c>animation-direction: normal</c>.
    /// </summary>
    [Value("normal")]
    Normal = 1,

    /// <summary>
    /// Plays the animation backwards each cycle, as in CSS <c>animation-direction: reverse</c>.
    /// </summary>
    [Value("reverse")]
    Reverse = 2,

    /// <summary>
    /// Alternates forwards and backwards on successive cycles, as in CSS <c>animation-direction: alternate</c>.
    /// </summary>
    [Value("alternate")]
    Alternate = 3,

    /// <summary>
    /// Alternates backwards and forwards on successive cycles, as in CSS <c>animation-direction: alternate-reverse</c>.
    /// </summary>
    [Value("alternate-reverse")]
    AlternateReverse = 4,
}
