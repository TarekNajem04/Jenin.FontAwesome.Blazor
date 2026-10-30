//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Sample.Shared.Pages.Inheritance;

public class HouseIcon : Icon {
    public HouseIcon() {
        IconStyle = IconStyle.Solid;
        IconName = "fa-house";
        IconSize = IconSize.X01;
        Color = "#553592";
        Bordered = true;
        BorderColor = "#FF0090";
    }
}
