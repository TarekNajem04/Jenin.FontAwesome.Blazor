using Jenin.FontAwesome.Blazor.Extensions;

namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to grab attention by visually scaling and pulsing an icon in and out.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#beat-fade">Font Awesome Beat Fade</a>.
/// </para>
/// </summary>
public class BeatFade : Animation {
    public BeatFade() : base("fa-beat-fade") { }

    /// <summary>
    /// Set lowest opacity value an icon will fade to and from
    /// </summary>
    public float? Opacity { get; set; }

    /// <summary>
    /// Set max value that an icon will scale.
    /// </summary>
    public float? Scale { get; set; }

    public override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) => base.AddStyles(styles)
                                                                                                   .AddIfNotNull("--fa-beat-fade-opacity", Opacity)
                                                                                                   .AddIfNotNull("--fa-beat-fade-scale", Scale);

    public static BeatFade Animate(float? opacity, float? scale) => new() { Opacity = opacity, Scale = scale };
    public static BeatFade Animate
        (
            float? opacity = null,
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
            Opacity = opacity,
            Scale = scale
        };
}
