namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Represents a different look of the same icons.
/// <para>
/// <a href="https://fontawesome.com/v6/docs/apis/javascript/import-icons#referencing-icons">Referencing Icons [Style]</a>
/// </para>
/// </summary>
public enum IconStackSize {
    /// <summary>
    /// The icon stack size will not be applied.
    /// </summary>
    [Value("")]
    NotSet,

    /// <summary>
    /// Used on the icon which should be displayed at base size when stacked.
    /// </summary>
    [Value("fa-stack-1x")]
    Same,

    /// <summary>
    /// Used on the icon which should be displayed larger when stackedr.
    /// </summary>
    [Value("fa-stack-2x")]
    Larger,
}
