//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// The Font Awesome width behavior. Font Awesome 7 renders icons at a fixed width that fills the
/// Icon Canvas by default; this selects between the automatic width opt-out and the legacy fixed
/// width while a value of <see cref="NotSet"/> emits nothing.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/icon-canvas">The Icon Canvas</a>
/// </para>
/// </summary>
public enum IconWidth {
    /// <summary>
    /// No width class is applied; Font Awesome's built-in default (fixed width) applies.
    /// </summary>
    [Value("")]
    NotSet = 0,

    /// <summary>
    /// Opts the icon out of the fixed canvas width (<c>fa-width-auto</c>).
    /// </summary>
    [Value("fa-width-auto")]
    Auto = 1,

    /// <summary>
    /// Re-applies the legacy fixed width (<c>fa-width-fixed</c>), the replacement of the
    /// deprecated <c>fa-fw</c> class.
    /// </summary>
    [Value("fa-width-fixed")]
    Fixed = 2,
}
