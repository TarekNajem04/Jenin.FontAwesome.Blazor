namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Sharp is built on a 16 pixel grid but was intentionally cut out as much rounding as reasonably possible.
/// <para>
/// <a href="https://blog.fontawesome.com/introducing-font-awesome-sharp/">Introducing Font Awesome Sharp</a>
/// </para>
/// </summary>
public enum Typefaces {
    /// <summary>
    /// The icon will be drawn with a rounded edge.
    /// </summary>
    [Value("")]
    Classic,

    /// <summary>
    /// The icon will be drawn with a sharp edge.
    /// </summary>
    [Value("fa-sharp")]
    Sharp,
}
