namespace Jenin.FontAwesome.Blazor.Transformations;

/// <summary>
/// To move icons <see cref="PositioningKind.Up"/>, <see cref="PositioningKind.Down"/>, <see cref="PositioningKind.Left"/>, or <see cref="PositioningKind.Right"/>
/// <para>Units are 1/16em.</para>
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/power-transform#positioning">Positioning</a>
/// </para>
/// </summary>
public class Positioning : Transformation {
    public Positioning(float value, PositioningKind kind) : base(value) => Kind = kind;

    public PositioningKind Kind { get; set; }

    public override string GetTransformation() {
        Value = MathF.Min(16, Math.Max(1, Value));

        return Kind switch {
            PositioningKind.Up => $"up-{GetFormattedValue()}",
            PositioningKind.Down => $"down-{GetFormattedValue()}",
            PositioningKind.Left => $"left-{GetFormattedValue()}",
            PositioningKind.Right => $"right-{GetFormattedValue()}",
            _ => null
        };
    }

    public static Positioning Up(float value) => new(value, PositioningKind.Up);
    public static Positioning Down(float value) => new(value, PositioningKind.Down);
    public static Positioning Left(float value) => new(value, PositioningKind.Left);
    public static Positioning Right(float value) => new(value, PositioningKind.Right);
}
