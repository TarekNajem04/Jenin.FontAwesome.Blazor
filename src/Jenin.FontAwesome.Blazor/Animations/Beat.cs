using Jenin.FontAwesome.Blazor.Extensions;

namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to scale an icon up or down. This is useful for grabbing attention or for use with health/<c>heart-centric</c> icons.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#beat">Font Awesome Beat</a>.
/// </para>
/// </summary>
public class Beat : Animation {
    public Beat() : base("fa-beat") { }

    /// <summary>
    /// Set max value that an icon will scale.
    /// </summary>
    public float? Scale { get; set; }

    public override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) => base.AddStyles(styles)
                                                                                                   .AddIfNotNull("--fa-beat-scale", Scale);

    public static Beat Animate(float? scale) => new() { Scale = scale };
    public static Beat Animate
        (
            float? scale = null,
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
            Scale = scale
        };
}
