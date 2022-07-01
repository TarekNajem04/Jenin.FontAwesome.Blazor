namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// sets the line style for all four sides of an element's border.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/pull">Bordered and Pulled Classes</a>
/// </para>
/// </summary>
public enum BorderStyle {
    [Value("none")]
    None,

    [Value("hidden")]
    Hidden,

    [Value("dotted")]
    Dotted,

    [Value("dashed")]
    Dashed,

    [Value("solid")]
    Solid,

    [Value("double")]
    Double,

    [Value("groove")]
    Groove,

    [Value("ridge")]
    Ridge,

    [Value("inset")]
    Inset,

    [Value("outset")]
    Outset
}
/// <summary>
/// sets the line style for all four sides of an element's border.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/pull">Bordered and Pulled Classes</a>
/// </para>
/// </summary>
public enum PullPosition {
    [Value("")]
    None,

    /// <summary>
    /// Pulls an icon by floating it left and applying a margin-right
    /// </summary>
    [Value("fa-pull-left")]
    Left,

    /// <summary>
    /// Pulls an icon by floating it right and applying a margin-left
    /// </summary>
    [Value("fa-pull-right")]
    Right,
}
