//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// A ready-made favorite toggle: an outlined heart that fills red when pressed, with
/// accessible pressed-state names out of the box. Every default remains overridable.
/// </summary>
public class FavoriteButton : IconToggleButton {
    /// <summary>
    /// Creates a favorite toggle with heart glyph, red pressed color, and pressed-state names.
    /// </summary>
    public FavoriteButton() {
        IconName = "fa-heart";
        IconStyle = IconStyle.Regular;
        PressedIconName = "fa-heart";
        PressedIconStyle = IconStyle.Solid;
        PressedColor = "red";
        Title = "Add to favorites";
        PressedTitle = "Remove from favorites";
    }
}
