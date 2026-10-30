//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Transformations;

/// <summary>
/// To rotate or flip icons use any combination of <see cref="Rotating"/>, <see cref="FlipVertical"/>, and <see cref="FlipHorizontal"/> with any arbitrary value.
/// <para>Any arbitrary value. Units are degrees with negative numbers allowed</para>
/// <para>Construct via <see cref="Rotate(float)"/>.</para>
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/power-transform#rotating-and-flipping">Rotating and Flipping</a>
/// </para>
/// </summary>
public class Rotating : Transformation {
    internal Rotating(float value) : base(value) { }

    /// <summary>
    /// Serializes the rotation token to its Font Awesome Power Transform value.
    /// <returns>The rotation token for the configured value.</returns>
    /// </summary>
    public override string GetTransformation() => $"rotate-{GetFormattedValue()}";

    /// <summary>
    /// Creates a <see cref="Rotating"/> that rotates an icon.
    /// </summary>
    /// <param name="value">The rotation in degrees (negative values allowed).</param>
    /// <returns>The new <see cref="Rotating"/>.</returns>
    public static Rotating Rotate(float value) => new(value);
}
