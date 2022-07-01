using Jenin.FontAwesome.Blazor.Extensions;

namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to grab attention by visually bouncing an icon up and down.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#bounce">Font Awesome Bounce</a>.
/// </para>
/// </summary>
public class Bounce : Animation {
    public Bounce() : base("fa-bounce") { }

    /// <summary>
    /// Set the amount of rebound an icon has when landing after the jump.
    /// </summary>
    public float? Rebound { get; set; }

    /// <summary>
    /// Set the max height an icon will jump to when bouncing.
    /// </summary>
    public float? Height { get; set; }

    /// <summary>
    /// Set the icon's horizontal distortion ("squish") when starting to bounce.
    /// </summary>
    public float? StartScaleX { get; set; }

    /// <summary>
    /// Set the icon's vertical distortion ("squish") when starting to bounce.
    /// </summary>
    public float? StartScaleY { get; set; }

    /// <summary>
    /// Set the icon's horizontal distortion ("squish") at the top of the jump.
    /// </summary>
    public float? JumpScaleX { get; set; }

    /// <summary>
    /// Set the icon's vertical distortion ("squish") at the top of the jump.
    /// </summary>
    public float? JumpScaleY { get; set; }

    /// <summary>
    /// Set the icon's horizontal distortion ("squish") when landing after the jump.
    /// </summary>
    public float? LandScaleX { get; set; }

    /// <summary>
    /// Set the icon's vertical distortion ("squish") when landing after the jump.
    /// </summary>
    public float? LandScaleY { get; set; }

    public override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) => base.AddStyles(styles)
                                                                                                   .AddIfNotNull("--fa-bounce-rebound", Rebound)
                                                                                                   .AddIfNotNull("--fa-bounce-height", Height)
                                                                                                   .AddIfNotNull("--fa-bounce-start-scale-x", StartScaleX)
                                                                                                   .AddIfNotNull("--fa-bounce-start-scale-y", StartScaleY)
                                                                                                   .AddIfNotNull("--fa-bounce-jump-scale-x", JumpScaleX)
                                                                                                   .AddIfNotNull("--fa-bounce-jump-scale-y", JumpScaleY)
                                                                                                   .AddIfNotNull("--fa-bounce-land-scale-x", LandScaleX)
                                                                                                   .AddIfNotNull("--fa-bounce-land-scale-y", LandScaleY);
    public static Bounce Animate
        (
            float? rebound,
            float? height,
            float? startScaleX,
            float? startScaleY,
            float? jumpScaleX,
            float? jumpScaleY,
            float? landScaleX,
            float? landScaleY
        )
        => new() {
            Rebound = rebound,
            Height = height,
            StartScaleX = startScaleX,
            StartScaleY = startScaleY,
            JumpScaleX = jumpScaleX,
            JumpScaleY = jumpScaleY,
            LandScaleX = landScaleX,
            LandScaleY = landScaleY
        };
    public static Bounce Animate
        (
            float? rebound = null,
            float? height = null,
            float? startScaleX = null,
            float? startScaleY = null,
            float? jumpScaleX = null,
            float? jumpScaleY = null,
            float? landScaleX = null,
            float? landScaleY = null,
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
            Rebound = rebound,
            Height = height,
            StartScaleX = startScaleX,
            StartScaleY = startScaleY,
            JumpScaleX = jumpScaleX,
            JumpScaleY = jumpScaleY,
            LandScaleX = landScaleX,
            LandScaleY = landScaleY
        };
}
