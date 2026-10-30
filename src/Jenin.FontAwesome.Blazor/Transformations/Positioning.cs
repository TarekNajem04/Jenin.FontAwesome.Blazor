//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Transformations;

/// <summary>
/// To move icons <see cref="PositioningKind.Up"/>, <see cref="PositioningKind.Down"/>, <see cref="PositioningKind.Left"/>, or <see cref="PositioningKind.Right"/>
/// <para>Units are 1/16em.</para>
/// <para>Construct via <see cref="Up(float)"/>, <see cref="Down(float)"/>, <see cref="Left(float)"/>, or <see cref="Right(float)"/>.
/// Values are normalized to [1,16] at construction.</para>
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/power-transform#positioning">Positioning</a>
/// </para>
/// </summary>
public class Positioning : Transformation {
    internal Positioning(float value, PositioningKind kind) : base(Normalize(value)) => Kind = kind;

    /// <summary>
    /// Serializes the positioning token to its Font Awesome Power Transform value.
    /// <returns>The positioning token for the configured kind and value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the positioning kind holds an unsupported value. Unreachable for instances created through the <see cref="Up(float)"/>, <see cref="Down(float)"/>, <see cref="Left(float)"/>, or <see cref="Right(float)"/> factory methods.</exception>
    /// </summary>
    public override string GetTransformation()
        => Kind switch {
            PositioningKind.Up => $"up-{GetFormattedValue()}",
            PositioningKind.Down => $"down-{GetFormattedValue()}",
            PositioningKind.Left => $"left-{GetFormattedValue()}",
            PositioningKind.Right => $"right-{GetFormattedValue()}",
            _ => throw new InvalidOperationException($"Unsupported {nameof(PositioningKind)} '{Kind}'.")
        };

    /// <summary>
    /// Creates a <see cref="Positioning"/> that moves an icon up.
    /// </summary>
    /// <param name="value">The offset in 1/16em units (normalized to [1,16] at construction).</param>
    /// <returns>The new <see cref="Positioning"/>.</returns>
    public static Positioning Up(float value) => new(value, PositioningKind.Up);

    /// <summary>
    /// Creates a <see cref="Positioning"/> that moves an icon down.
    /// </summary>
    /// <param name="value">The offset in 1/16em units (normalized to [1,16] at construction).</param>
    /// <returns>The new <see cref="Positioning"/>.</returns>
    public static Positioning Down(float value) => new(value, PositioningKind.Down);

    /// <summary>
    /// Creates a <see cref="Positioning"/> that moves an icon left.
    /// </summary>
    /// <param name="value">The offset in 1/16em units (normalized to [1,16] at construction).</param>
    /// <returns>The new <see cref="Positioning"/>.</returns>
    public static Positioning Left(float value) => new(value, PositioningKind.Left);

    /// <summary>
    /// Creates a <see cref="Positioning"/> that moves an icon right.
    /// </summary>
    /// <param name="value">The offset in 1/16em units (normalized to [1,16] at construction).</param>
    /// <returns>The new <see cref="Positioning"/>.</returns>
    public static Positioning Right(float value) => new(value, PositioningKind.Right);

    internal PositioningKind Kind { get; }

    private static float Normalize(float value) => MathF.Min(16, MathF.Max(1, value));
}
