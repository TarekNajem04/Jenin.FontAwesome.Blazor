//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Controls when an <see cref="Components.Icon"/>'s animation runs. The default
/// <see cref="None"/> plays immediately; the other values pause the keyframes until the named
/// interaction occurs. Requires the shipped <c>animation-triggers.css</c> stylesheet and, for
/// <see cref="ScrollReveal"/>, the <c>animation-triggers.js</c> helper.
/// </summary>
public enum AnimationTrigger {
    /// <summary>
    /// The animation plays immediately (Font Awesome default).
    /// </summary>
    None = 0,

    /// <summary>
    /// The animation is paused until the icon itself is hovered. Pairs with single-iteration
    /// animations for a one-shot effect (re-hovering does not replay without a re-render).
    /// </summary>
    Hover = 1,

    /// <summary>
    /// The animation is paused until an ancestor carrying the <c>jfa-anim-hover-within</c> class
    /// is hovered (for example a card wrapping the icon).
    /// </summary>
    ParentHover = 2,

    /// <summary>
    /// The animation is paused until the icon receives keyboard focus.
    /// </summary>
    Focus = 3,

    /// <summary>
    /// The icon stays hidden until it scrolls into view, then animates once. Pair with a finite
    /// animation (for example <c>Animate.Beat().SetIterationCount(1)</c>). Content stays visible
    /// when JavaScript is unavailable or <c>prefers-reduced-motion</c> is set.
    /// </summary>
    ScrollReveal = 4,
}
