namespace Jenin.FontAwesome.Blazor.Transformations;

public static class TransformExtensions {
    public static Transform Grow(this Transform transform, float value) => transform.Add(new Scaling(value, ScalingKind.Grow));
    public static Transform Shrink(this Transform transform, float value) => transform.Add(new Scaling(value, ScalingKind.Shrink));

    public static Transform Up(this Transform transform, float value) => transform.Add(new Positioning(value, PositioningKind.Up));
    public static Transform Down(this Transform transform, float value) => transform.Add(new Positioning(value, PositioningKind.Down));
    public static Transform Left(this Transform transform, float value) => transform.Add(new Positioning(value, PositioningKind.Left));
    public static Transform Right(this Transform transform, float value) => transform.Add(new Positioning(value, PositioningKind.Right));

    public static Transform Rotate(this Transform transform, float value) => transform.Add(new Rotating(value));

    public static Transform FlipV(this Transform transform) => transform.Add(new FlipHorizontal());
    public static Transform FlipH(this Transform transform) => transform.Add(new FlipHorizontal());
}
