namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to grab attention or note that something is not allowed by shaking an icon back and forth.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#shake">Font Awesome Shake</a>.
/// </para>
/// </summary>
public class Shake : Animation {
    public Shake() : base("fa-shake") { }
    public static Shake Animate() => new();
    public static Shake Animate
        (
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
            TimingFunction = timingFunction
        };
}