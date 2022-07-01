using Jenin.FontAwesome.Blazor.Extensions;

namespace Jenin.FontAwesome.Blazor.Components;

public class IconListItem : Element {
    [Parameter]
    public RenderFragment ItemIcon { get; set; }

    [Parameter]
    public RenderFragment ItemContent { get; set; }

    /// <summary>
    /// Set margin around icon.
    /// </summary>
    [Parameter]
    public string ItemMargin { get; set; }

    /// <summary>
    /// Set width of icon.
    /// </summary>
    [Parameter]
    public string ItemWidth { get; set; }

    protected override List<string> AddClasses(List<string> classes) {
        classes = base.AddClasses(classes);
        classes.Add("fa-li");

        return classes;
    }

    protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles)
        => base.AddStyles(styles)
               .AddOrUpdateIfNotNull("--fa-li-margin", ItemMargin)
               .AddOrUpdateIfNotNull("--fa-li-width", ItemWidth);

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        /*
            <ul @attributes=AdditionalAttributes
               @onclick=@OnClickHandlerAsync
               @ondblclick=@OnDoubleClickHandlerAsync
               @onmouseover=@OnMouseOverHandlerAsync
               @onmouseout=@OnMouseOutHandlerAsync>
                <CascadingValue Value=this Name="IconList" IsFixed=true>@Content</CascadingValue>
            </ul>
         */
        var sequence = 0;

        builder.OpenElement(sequence, "li");
        builder.OpenElement(sequence++, "span");
        _ = BuildElementRenderTree(ref sequence, builder);
        builder.AddContent(sequence++, ItemIcon);
        builder.CloseElement();
        builder.AddContent(sequence++, ItemContent);
        builder.CloseElement();
    }
}
