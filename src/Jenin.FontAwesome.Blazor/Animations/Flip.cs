using Jenin.FontAwesome.Blazor.Extensions;

namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to rotate an icon in 3D space.
/// By default, flip rotates an icon about the Y axis 180 degrees.
/// Flipping is helpful for transitions, processing states,
/// or for using physical objects that one flips in the real world.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#beat-fade">Font Awesome Beat Fade</a>.
/// </para>
/// </summary>
public class Flip : Animation {
    public Flip() : base("fa-flip") { }

    /// <summary>
    /// Set x-coordinate of the vector denoting the axis of rotation (between 0 and 1).
    /// </summary>
    public float? FlipX { get; set; }

    /// <summary>
    /// Set y-coordinate of the vector denoting the axis of rotation (between 0 and 1).
    /// </summary>
    public float? FlipY { get; set; }

    /// <summary>
    /// Set z-coordinate of the vector denoting the axis of rotation (between 0 and 1).
    /// </summary>
    public float? FlipZ { get; set; }

    /// <summary>
    /// Set rotation angle of flip. A positive angle denotes a clockwise rotation, a negative angle a counter-clockwise one.
    /// </summary>
    public float? FlipAngle { get; set; }

    public override Dictionary<string, string> AddStyles(Dictionary<string, string> styles)
        => base.AddStyles(styles)
               .AddIfNotNull("--fa-flip-x", FlipX = FlipX.HasValue ? MathF.Min(1, Math.Max(0, FlipX.Value)) : FlipX)
               .AddIfNotNull("--fa-flip-y", FlipY = FlipY.HasValue ? MathF.Min(1, Math.Max(0, FlipY.Value)) : FlipY)
               .AddIfNotNull("--fa-flip-z", FlipZ = FlipZ.HasValue ? MathF.Min(1, Math.Max(0, FlipZ.Value)) : FlipZ)
               .AddIfNotNull("--fa-flip-angle", FlipAngle);

    public static Flip Animate(float? flipX, float? flipY, float? flipZ) => new() { FlipX = flipX, FlipY = flipY, FlipZ = flipZ };
    public static Flip Animate
        (
            float? flipX = null,
            float? flipY = null,
            float? flipZ = null,
            DurationUnit delayUnit = DurationUnit.Seconds,
            float? delay = null,
            AnimationDirection direction = AnimationDirection.None,
            DurationUnit durationUnit = DurationUnit.Seconds,
            float? duration = null,
            float? iterationCount = null,
            string timingFunction = null
        )
        => new() {
            DelayUnit = delayUnit,
            Delay = delay,
            Direction = direction,
            DurationUnit = durationUnit,
            Duration = duration,
            IterationCount = iterationCount,
            TimingFunction = timingFunction,
            FlipX = flipX,
            FlipY = flipY,
            FlipZ = flipZ
        };
}
