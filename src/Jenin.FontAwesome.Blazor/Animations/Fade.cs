using Jenin.FontAwesome.Blazor.Extensions;

namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// To fade an icon in and out visually to grab attention in a subtle (or not so subtle) way.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#fade">Font Awesome Fade</a>.
/// </para>
/// </summary>
public class Fade : Animation {
    public Fade() : base("fa-fade") { }

    /// <summary>
    /// Set lowest opacity value an icon will fade to and from
    /// </summary>
    public float? Opacity { get; set; }

    public override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) => base.AddStyles(styles)
                                                                                                   .AddIfNotNull("--fa-fade-opacity", Opacity);
    public static Fade Animate(float? opacity) => new() { Opacity = opacity };
    public static Fade Animate
        (
            float? opacity = null,
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
            Opacity = opacity
        };
}
