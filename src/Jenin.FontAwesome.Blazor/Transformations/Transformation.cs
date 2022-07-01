namespace Jenin.FontAwesome.Blazor.Transformations;

public abstract class Transformation {
    protected Transformation(float value) => Value = value;

    public float Value { get; set; }

    public abstract string GetTransformation();

    protected string GetFormattedValue() => Value.ToString("F", CultureInfo.InvariantCulture);
}
