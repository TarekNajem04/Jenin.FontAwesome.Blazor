namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Layers are the new way to place icons and text visually on top of each other,
/// replacing Font Awesome classic icons stacks.
/// With this approach, you can use more than two icons.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/layer">Layering Text & Counters</a>
/// </para>
/// </summary>
public class IconLayersContainer : IconContainer {
    /*
        <span @attributes=AdditionalAttributes
           @onclick=@OnClickHandlerAsync
           @ondblclick=@OnDoubleClickHandlerAsync
           @onmouseover=@OnMouseOverHandlerAsync
           @onmouseout=@OnMouseOutHandlerAsync>
            <CascadingValue Value=this Name="Container" IsFixed=true>@Icons</CascadingValue>
        </span>
     */
    protected override string GetRootTag() => "span";

    protected override List<string> AddClasses(List<string> classes) {
        classes = base.AddClasses(classes);
        classes.Add("fa-layers");

        return classes;
    }
}
