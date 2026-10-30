//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// The transition used by <see cref="Components.IconTransition"/> when swapping between its two
/// states. All kinds are pure CSS transitions on stacked layers (see
/// <c>animation-triggers.css</c>); they replay on every toggle and collapse to an instant swap
/// under <c>prefers-reduced-motion</c>.
/// </summary>
public enum IconTransitionKind {
    /// <summary>
    /// The incoming layer fades in over the outgoing layer.
    /// </summary>
    Crossfade = 0,

    /// <summary>
    /// The outgoing layer shrinks away while the incoming layer grows in.
    /// </summary>
    Scale = 1,

    /// <summary>
    /// The layers flip around the vertical axis like a card.
    /// </summary>
    Flip = 2,

    /// <summary>
    /// The outgoing layer slides out while the incoming layer slides in.
    /// </summary>
    Slide = 3,
}
