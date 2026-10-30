//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Transformations;

/// <summary>
/// Flips an icon horizontally (the <c>flip-h</c> Power Transform token). To rotate or flip icons use any combination of <see cref="Rotating"/>, <see cref="FlipVertical"/>, and <see cref="FlipHorizontal"/> with any arbitrary value.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/power-transform#rotating-and-flipping">Rotating and Flipping</a>
/// </para>
/// </summary>
public class FlipHorizontal : Transformation {
    internal FlipHorizontal() : base(0) { }

    /// <summary>
    /// Serializes the horizontal flip token to its Font Awesome Power Transform value.
    /// <returns>The <c>flip-h</c> token.</returns>
    /// </summary>
    public override string GetTransformation() => "flip-h";

    /// <summary>
    /// Creates a <see cref="FlipHorizontal"/> that flips an icon horizontally.
    /// </summary>
    /// <returns>The new <see cref="FlipHorizontal"/>.</returns>
    public static FlipHorizontal Flip() => new();
}
