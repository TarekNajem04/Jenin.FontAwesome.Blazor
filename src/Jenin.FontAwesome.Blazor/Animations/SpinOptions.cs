//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Options for the <see cref="Spin"/> animation.
/// </summary>
public sealed record SpinOptions : AnimationOptions {
    /// <summary>
    /// Makes an icon spin 360° clockwise in 8 incremental steps.
    /// </summary>
    public bool? Pulse { get; init; }

    /// <summary>
    /// When used in conjunction with spin, makes an icon spin counter-clockwise.
    /// </summary>
    public bool? Reverse { get; init; }
}
