namespace Jenin.FontAwesome.Blazor;

public enum ProcessingSvgMethod {
    /// <summary>
    /// The icon size will not be applied.
    /// </summary>
    [Value("")]
    NotSet = 0,

    /// <summary>
    /// When false this will cause Font Awesome to use webfont.
    /// </summary>
    [Value("false")]
    False = 1,

    /// <summary>
    /// When true (default) this will cause Font Awesome to search through the DOM for any <i> tags and automatically replace them with <svg> tags.
    /// </summary>
    [Value("true")]
    True = 2,

    /// <summary>
    /// By default Font Awesome will replace elements.
    /// However, setting this value to nest will cause Font Awesome to add a child element to the existing <i> tag.
    /// </summary>
    [Value("nest")]
    Nest = 3,
}
