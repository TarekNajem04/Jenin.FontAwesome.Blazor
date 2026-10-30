//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Options for the <see cref="SpinSnap"/> animation.
/// </summary>
public sealed record SpinSnapOptions : AnimationOptions {
    /// <summary>
    /// The number of discrete steps used by the snap rotation.
    /// </summary>
    public SpinSnapSize Size { get; init; } = SpinSnapSize.Default;
}
