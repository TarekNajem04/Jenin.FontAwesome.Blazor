//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// A named duotone color scheme with light- and dark-mode layer colors, consumed by
/// <see cref="Components.IconTheme"/>. Colors are starting points; every value is overridable.
/// </summary>
/// <param name="Name">The palette name (for example <c>Info</c>).</param>
/// <param name="LightPrimary">The primary layer color on light surfaces.</param>
/// <param name="LightSecondary">The secondary layer color on light surfaces.</param>
/// <param name="DarkPrimary">The primary layer color on dark surfaces.</param>
/// <param name="DarkSecondary">The secondary layer color on dark surfaces.</param>
public sealed record DuotonePalette(string Name, string LightPrimary, string LightSecondary, string DarkPrimary, string DarkSecondary);
