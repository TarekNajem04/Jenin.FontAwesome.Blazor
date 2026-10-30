//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Selects the relative size of an icon within a stack (<c>fa-stack-1x</c> or <c>fa-stack-2x</c>).
/// </summary>
public enum IconStackSize {
    /// <summary>
    /// The icon stack size will not be applied.
    /// </summary>
    [Value("")]
    NotSet = 0,

    /// <summary>
    /// Used on the icon which should be displayed at base size when stacked.
    /// </summary>
    [Value("fa-stack-1x")]
    Same = 1,

    /// <summary>
    /// Used on the icon which should be displayed larger when stacked.
    /// </summary>
    [Value("fa-stack-2x")]
    Larger = 2,
}
