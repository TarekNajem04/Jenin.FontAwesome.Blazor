using Jenin.FontAwesome.Blazor.Extensions;

namespace Jenin.FontAwesome.Blazor.Animations;

public abstract class Animation {
    protected Animation(string cssClass) => CssClass = cssClass;

    protected string CssClass { get; }

    /// <summary>
    /// Specify the unit for <see cref="Delay"/>.
    /// </summary>
    public DurationUnit DelayUnit { get; set; }

    /// <summary>
    /// Set an initial delay for animation
    /// This may be specified in either <see cref="DurationUnit.Seconds"/> or <see cref="DurationUnit.Milliseconds"/>.
    /// The <see cref="DelayUnit"/> is required.
    /// </summary>
    public float? Delay { get; set; }

    /// <summary>
    /// Set direction for animation.
    /// </summary>
    public AnimationDirection Direction { get; set; }

    /// <summary>
    /// Specify the unit for <see cref="Duration"/>.
    /// </summary>
    public DurationUnit DurationUnit { get; set; }

    /// <summary>
    /// Set duration for animation
    /// This may be specified in either <see cref="DurationUnit.Seconds"/> or <see cref="DurationUnit.Milliseconds"/>.
    /// The <see cref="DurationUnit"/> is required.
    /// </summary>
    public float? Duration { get; set; }

    /// <summary>
    /// Sets the number of times an animation sequence should be played before stopping.
    /// </summary>
    public float? IterationCount { get; set; }

    /// <summary>
    /// Sets how an animation progresses through the duration of each cycle.
    /// <para>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/CSS/animation-timing-function">Any valid CSS animation-timing-function value</a>.
    /// </para>
    /// </summary>
    public string TimingFunction { get; set; }

    public virtual string BuildClasses() => CssClass;

    public string BuildStyles() {
        var styles = AddStyles(new Dictionary<string, string>());

        return styles.JoinAsStyles();
    }

    public virtual Dictionary<string, string> AddStyles(Dictionary<string, string> styles) {
        if (Delay.HasValue) {
            styles.Add("--fa-animation-delay", Delay.Value.ToString("F", CultureInfo.InvariantCulture) + DelayUnit.GetStringValue());
        }

        if (Direction is not AnimationDirection.None) {
            var directionStyle = string.Join(", ", Direction.GetAllFlags().Select(x => x.GetStringValue()));

            _ = styles.AddIfNotNull("--fa-animation-direction", directionStyle);
        }

        if (Duration.HasValue) {
            styles.Add("--fa-animation-duration", Duration.Value.ToString("F", CultureInfo.InvariantCulture) + DurationUnit.GetStringValue());
        }

        return styles.AddIfNotNull("--fa-animation-iteration-count", IterationCount)
                     .AddIfNotNull("--fa-animation-timing", TimingFunction);
    }

    public static Beat Beat
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

    public static Fade Fade
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

    public static BeatFade BeatFade
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

    public static Bounce Bounce
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

    public static Flip Flip
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

    public static Shake Shake
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

    public static Spin Spin
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