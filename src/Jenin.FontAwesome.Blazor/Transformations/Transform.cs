namespace Jenin.FontAwesome.Blazor.Transformations;

public class Transform {
    private readonly List<Transformation> _transformations = new();

    public Transform Add(Transformation transformation) {
        _transformations.Add(transformation);

        return this;
    }

    public Transform Add(params Transformation[] transformations) {
        _transformations.AddRange(transformations);

        return this;
    }

    public static Transform Apply(params Transformation[] transformations) {
        var transform = new Transform();
        transform._transformations.AddRange(transformations);

        return transform;
    }

    public static Transform Grow(float value) => new Transform().Add(new Scaling(value, ScalingKind.Grow));
    public static Transform Shrink(float value) => new Transform().Add(new Scaling(value, ScalingKind.Shrink));

    public static Transform Up(float value) => new Transform().Add(new Positioning(value, PositioningKind.Up));
    public static Transform Down(float value) => new Transform().Add(new Positioning(value, PositioningKind.Down));
    public static Transform Left(float value) => new Transform().Add(new Positioning(value, PositioningKind.Left));
    public static Transform Right(float value) => new Transform().Add(new Positioning(value, PositioningKind.Right));

    public static Transform Rotate(float value) => new Transform().Add(new Rotating(value));

    public static Transform FlipV() => new Transform().Add(new FlipVertical());
    public static Transform FlipH() => new Transform().Add(new FlipHorizontal());
    public static Transform FlipBoth() => new Transform().Add(new FlipVertical(), new FlipHorizontal());

    public string GetTransformation() => string.Join(' ', _transformations.Select(x => x.GetTransformation()));
}
