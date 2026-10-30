//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Options for the <see cref="Buzz"/> animation.
/// </summary>
public sealed record BuzzOptions : AnimationOptions {
    /// <summary>
    /// The horizontal vibration distance applied while buzzing (any valid CSS length, for example <c>4px</c>).
    /// </summary>
    public string? Distance { get; init; }
}
