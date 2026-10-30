//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Transformations;

/// <summary>
/// Power Transform scaling affects icon size without changing or moving the container.
/// To scale icons up or down, use <see cref="ScalingKind.Grow"/> or <see cref="ScalingKind.Shrink"/> with any arbitrary value, including decimals.
/// <para>Units are 1/16em.</para>
/// <para>Construct via <see cref="Grow(float)"/> or <see cref="Shrink(float)"/>.
/// Values are normalized to [1,16] at construction without rounding.</para>
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/power-transform#scaling">Scaling</a>
/// </para>
/// </summary>
public class Scaling : Transformation {
    internal Scaling(float value, ScalingKind kind) : base(Normalize(value)) => Kind = kind;

    /// <summary>
    /// Serializes the scaling token to its Font Awesome Power Transform value.
    /// <returns>The scaling token for the configured kind and value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the scaling kind holds an unsupported value. Unreachable for instances created through the <see cref="Grow(float)"/> or <see cref="Shrink(float)"/> factory methods.</exception>
    /// </summary>
    public override string GetTransformation()
        => Kind switch {
            ScalingKind.Grow => $"grow-{GetFormattedValue()}",
            ScalingKind.Shrink => $"shrink-{GetFormattedValue()}",
            _ => throw new InvalidOperationException($"Unsupported {nameof(ScalingKind)} '{Kind}'.")
        };

    /// <summary>
    /// Creates a <see cref="Scaling"/> that grows an icon.
    /// </summary>
    /// <param name="value">The scaling amount in 1/16em units (normalized to [1,16] at construction without rounding).</param>
    /// <returns>The new <see cref="Scaling"/>.</returns>
    public static Scaling Grow(float value) => new(value, ScalingKind.Grow);
    /// <summary>
    /// Creates a <see cref="Scaling"/> that shrinks an icon.
    /// </summary>
    /// <param name="value">The scaling amount in 1/16em units (normalized to [1,16] at construction without rounding).</param>
    /// <returns>The new <see cref="Scaling"/>.</returns>
    public static Scaling Shrink(float value) => new(value, ScalingKind.Shrink);

    internal ScalingKind Kind { get; }

    private static float Normalize(float value) => MathF.Min(16, MathF.Max(1, value));
}
