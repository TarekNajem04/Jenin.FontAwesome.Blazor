namespace Jenin.FontAwesome.Blazor.Transformations;

/// <summary>
/// Power Transform scaling affects icon size without changing or moving the container.
/// To scale icons up or down, use <see cref="ScalingKind.Grow"/> or <see cref="ScalingKind.Shrink"/> with any arbitrary value, including decimals.
/// <para>Units are 1/16em.</para>
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/power-transform#scaling">Scaling</a>
/// </para>
/// </summary>
public class Scaling : Transformation {
    public Scaling(float value, ScalingKind kind) : base(value) => Kind = kind;

    public ScalingKind Kind { get; set; }

    public override string GetTransformation() {
        Value = Convert.ToInt32(MathF.Min(16, Math.Max(1, Value)));

        return Kind switch {
            ScalingKind.Grow => $"grow-{GetFormattedValue()}",
            ScalingKind.Shrink => $"shrink-{GetFormattedValue()}",
            _ => null
        };
    }

    public static Scaling Grow(float value) => new(value, ScalingKind.Grow);
    public static Scaling Shrink(float value) => new(value, ScalingKind.Shrink);
}
