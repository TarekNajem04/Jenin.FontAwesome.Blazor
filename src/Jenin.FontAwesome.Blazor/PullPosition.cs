//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Floats an icon to the left or right of the surrounding text.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/pull">Bordered and Pulled Classes</a>
/// </para>
/// </summary>
public enum PullPosition {
    /// <summary>
    /// The icon is not pulled.
    /// </summary>
    [Value("")]
    None = 0,

    /// <summary>
    /// Pulls an icon by floating it left and applying a margin-right
    /// </summary>
    [Value("fa-pull-left")]
    Left = 1,

    /// <summary>
    /// Pulls an icon by floating it right and applying a margin-left
    /// </summary>
    [Value("fa-pull-right")]
    Right = 2,

    /// <summary>
    /// Pulls an icon by floating it toward the inline start (left in left-to-right writing modes)
    /// and applying an inline end margin. Uses CSS logical properties (Font Awesome 7).
    /// </summary>
    [Value("fa-pull-start")]
    Start = 3,

    /// <summary>
    /// Pulls an icon by floating it toward the inline end (right in left-to-right writing modes)
    /// and applying an inline start margin. Uses CSS logical properties (Font Awesome 7).
    /// </summary>
    [Value("fa-pull-end")]
    End = 4,
}
