namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Represents a different look of the same icons.
/// <para>
/// <a href="https://fontawesome.com/v6/docs/apis/javascript/import-icons#referencing-icons">Referencing Icons [Style]</a>
/// </para>
/// </summary>
public enum IconStyle {
    /// <summary>
    /// Icon will be filled with single-color.
    /// </summary>
    [Value("fa-solid")]
    Solid,

    /// <summary>
    /// Icon will be outlined with single-color.
    /// </summary>
    [Value("fa-regular")]
    Regular,

    /// <summary>
    /// Icon will be slightly lighter.
    /// </summary>
    [Value("fa-light")]
    Light,

    /// <summary>
    /// Icon will be thinner.
    /// </summary>
    [Value("fa-thin")]
    Thin,

    /// <summary>
    /// Icon will be shown in two-color tones.
    /// </summary>
    [Value("fa-duotone")]
    DuoTone,

    /// <summary>
    /// It will use the symbols of well-known brands
    /// </summary>
    [Value("fa-brands")]
    Brands,
}
