//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Transformations;

/// <summary>
/// Flips an icon vertically (the <c>flip-v</c> Power Transform token). To rotate or flip icons use any combination of <see cref="Rotating"/>, <see cref="FlipVertical"/>, and <see cref="FlipHorizontal"/> with any arbitrary value.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/power-transform#rotating-and-flipping">Rotating and Flipping</a>
/// </para>
/// </summary>
public class FlipVertical : Transformation {
    internal FlipVertical() : base(0) { }

    /// <summary>
    /// Serializes the vertical flip token to its Font Awesome Power Transform value.
    /// <returns>The <c>flip-v</c> token.</returns>
    /// </summary>
    public override string GetTransformation() => "flip-v";

    /// <summary>
    /// Creates a <see cref="FlipVertical"/> that flips an icon vertically.
    /// </summary>
    /// <returns>The new <see cref="FlipVertical"/>.</returns>
    public static FlipVertical Flip() => new();
}
