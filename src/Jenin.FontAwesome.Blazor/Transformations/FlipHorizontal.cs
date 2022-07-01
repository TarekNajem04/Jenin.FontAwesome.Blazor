namespace Jenin.FontAwesome.Blazor.Transformations;

/// <summary>
/// To rotate or flip icons use any combination of <see cref="Rotating"/>, <see cref="FlipVertical"/>, and <see cref="FlipHorizontal"/> with any arbitrary value.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/power-transform#rotating-and-flipping">Rotating and Flipping</a>
/// </para>
/// </summary>
public class FlipHorizontal : Transformation {
    public FlipHorizontal() : base(0) { }

    public override string GetTransformation() => "flip-h";
    public static FlipHorizontal Flip() => new();
}