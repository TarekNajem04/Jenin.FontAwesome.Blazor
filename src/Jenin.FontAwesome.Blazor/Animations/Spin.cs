namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to get any icon to rotate, and use <see cref="Pulse"/> to have it rotate with eight steps.
/// This works especially well with <see cref="Spinner"/> and everything in the spinner icons category.
/// If you would like an icon to spin in reverse (e.g., counter-clockwise), set the <see cref="Reverse"/> property.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#spin">Font Awesome Spin</a>.
/// </para>
/// </summary>
public class Spin : Animation {
    public Spin() : base("fa-spin") { }

    /// <summary>
    /// Makes an icon spin 360° clockwise in 8 incremental steps.
    /// </summary>
    public bool? Pulse { get; set; }

    /// <summary>
    /// When used in conjunction with <see cref="Spin"/> or <see cref="Pulse"/>, makes an icon spin counter-clockwise.
    /// </summary>
    public bool? Reverse { get; set; }

    public override string BuildClasses()
        => string.Join
                  (
                    ' ',
                    new[] {
                        Pulse.HasValue ? "fa-spin-pulse" : base.BuildClasses(),
                        Reverse.HasValue ? "fa-spin-reverse" : null
                    }
                    .Where(x => !string.IsNullOrEmpty(x))
                  )
                 .Trim();
    public static Spin Animate(bool? pulse, bool? reverse) => new() { Pulse = pulse, Reverse = reverse };
    public static Spin Animate
        (
            bool? pulse = null,
            bool? reverse = null,
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
            Pulse = pulse,
            Reverse = reverse
        };
}
