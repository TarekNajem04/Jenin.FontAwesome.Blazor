//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Presence states for <see cref="Components.AvatarIcon"/>, each mapped to a conventional color.
/// </summary>
public enum AvatarPresence {
    /// <summary>
    /// No presence indicator is rendered.
    /// </summary>
    None = 0,

    /// <summary>
    /// Online (<c>#22c55e</c>).
    /// </summary>
    Online = 1,

    /// <summary>
    /// Busy (<c>#ef4444</c>).
    /// </summary>
    Busy = 2,

    /// <summary>
    /// Away (<c>#f59e0b</c>).
    /// </summary>
    Away = 3,

    /// <summary>
    /// Offline (<c>#9ca3af</c>).
    /// </summary>
    Offline = 4,
}
