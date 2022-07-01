namespace Jenin.FontAwesome.Blazor;

public enum IconSize {
    /// <summary>
    /// The icon size will not be applied.
    /// </summary>
    [Value("")]
    NotSet,

    /// <summary>
    /// Inherit value from container.
    /// </summary>
    [Value("")]
    Inherits,

    /// <summary>
    /// Changes an icon's font-size to 1em
    /// </summary>
    [Value("fa-1x")]
    X01,

    /// <summary>
    /// Changes an icon's font-size to 2em
    /// </summary>
    [Value("fa-2x")]
    X02,

    /// <summary>
    /// Changes an icon's font-size to 3em
    /// </summary>
    [Value("fa-3x")]
    X03,

    /// <summary>
    /// Changes an icon's font-size to 4em
    /// </summary>
    [Value("fa-4x")]
    X04,

    /// <summary>
    /// Changes an icon's font-size to 5em
    /// </summary>
    [Value("fa-5x")]
    X05,

    /// <summary>
    /// Changes an icon's font-size to 6em
    /// </summary>
    [Value("fa-6x")]
    X06,

    /// <summary>
    /// Changes an icon's font-size to 7em
    /// </summary>
    [Value("fa-7x")]
    X07,

    /// <summary>
    /// Changes an icon's font-size to 8em
    /// </summary>
    [Value("fa-8x")]
    X08,

    /// <summary>
    /// Changes an icon's font-size to 9em
    /// </summary>
    [Value("fa-9x")]
    X09,

    /// <summary>
    /// Changes an icon's font-size to 10em
    /// </summary>
    [Value("fa-10x")]
    X10,

    /// <summary>
    /// Changes an icon's font-size to 0.625em (~10px) and also vertically aligns icon
    /// </summary>
    [Value("fa-2xs")]
    ExtraSmall_2,

    /// <summary>
    /// Changes an icon's font-size to .75em (~12px) and also vertically aligns icon
    /// </summary>
    [Value("fa-xs")]
    ExtraSmall,

    /// <summary>
    /// Changes an icon's font-size to 0.875em (~14px) and also vertically aligns icon
    /// </summary>
    [Value("fa-sm")]
    Small,

    /// <summary>
    /// Changes an icon's font-size to 1.25em (~120px) and also vertically aligns icon
    /// </summary>
    [Value("fa-lg")]
    Large,

    /// <summary>
    /// Changes an icon's font-size to 1.5em (~24px) and also vertically aligns icon
    /// </summary>
    [Value("fa-xl")]
    ExtraLarge,

    /// <summary>
    /// Changes an icon's font-size to 2em (~32px) and also vertically aligns icon
    /// </summary>
    [Value("fa-2xl")]
    ExtraLarge_2,
}
