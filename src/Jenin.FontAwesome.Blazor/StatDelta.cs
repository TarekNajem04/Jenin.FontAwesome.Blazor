//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// The direction of change shown beside a <see cref="Components.StatCard"/> value.
/// </summary>
public enum StatDelta {
    /// <summary>
    /// No direction indicator is rendered.
    /// </summary>
    None = 0,

    /// <summary>
    /// An upward trend indicator.
    /// </summary>
    Up = 1,

    /// <summary>
    /// A downward trend indicator.
    /// </summary>
    Down = 2,

    /// <summary>
    /// A flat (unchanged) indicator.
    /// </summary>
    Flat = 3,
}
