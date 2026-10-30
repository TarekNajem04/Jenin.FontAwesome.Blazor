//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Layers are the new way to place icons and text visually on top of each other,
/// replacing Font Awesome classic icons stacks.
/// With this approach, you can use more than two icons.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/layer">Layering Text &amp; Counters</a>
/// </para>
/// </summary>
public class IconLayersContainer : IconContainer {
    private protected override string GetRootTag() => "span";

    private protected override List<string?> AddClasses(List<string?> classes) {
        classes = base.AddClasses(classes);
        classes.Add("fa-layers");

        return classes;
    }
}
