using Jenin.FontAwesome.Blazor.Extensions;

namespace Jenin.FontAwesome.Blazor.Components;

public class IconStackContainer : IconContainer {
    /// <summary>
    /// sets vertical alignment of <see cref="Icon"/>
    /// <para>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/CSS/vertical-align">Any valid CSS vertical-align value</a>.
    /// </para>
    /// </summary>
    [Parameter]
    public string VerticalAlign { get; set; }

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
        classes.Add("fa-stack");

        return classes;
    }

    protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles)
        => base.AddStyles(styles)
               .AddOrUpdateIfNotNull("vertical-align", VerticalAlign);
}
