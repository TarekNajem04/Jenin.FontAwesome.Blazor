namespace Jenin.FontAwesome.Blazor.Transformations;

/// <summary>
/// To rotate or flip icons use any combination of <see cref="Rotating"/>, <see cref="FlipVertical"/>, and <see cref="FlipHorizontal"/> with any arbitrary value.
/// <para>Any arbitrary value. Units are degrees with negative numbers allowed</para>
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/power-transform#rotating-and-flipping">Rotating and Flipping</a>
/// </para>
/// </summary>
public class Rotating : Transformation {
    public Rotating(float value) : base(value) { }

    public override string GetTransformation() => $"rotate-{GetFormattedValue()}";
    public static Rotating Rotate(float value) => new(value);
}
