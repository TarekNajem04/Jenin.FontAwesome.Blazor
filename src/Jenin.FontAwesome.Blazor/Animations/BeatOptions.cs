//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Options for the <see cref="Beat"/> animation.
/// </summary>
public sealed record BeatOptions : AnimationOptions {
    /// <summary>
    /// Max value that an icon will scale to in the Beat animation.
    /// </summary>
    public float? Scale { get; init; }
}
