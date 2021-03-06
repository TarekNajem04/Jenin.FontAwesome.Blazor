namespace Jenin.FontAwesome.Blazor.Components;

public class IconContainer : Element {
    [Parameter]
    public RenderFragment Icons { get; set; }

    protected virtual string GetRootTag() => "div";

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        /*
            <div @attributes=AdditionalAttributes
               @onclick=@OnClickHandlerAsync
               @ondblclick=@OnDoubleClickHandlerAsync
               @onmouseover=@OnMouseOverHandlerAsync
               @onmouseout=@OnMouseOutHandlerAsync>
                <CascadingValue Value=this Name="Container" IsFixed=true>@Icons</CascadingValue>
            </div>
         */
        var sequence = 0;

        builder.OpenElement(sequence, GetRootTag());
        _ = BuildElementRenderTree(ref sequence, builder);
        builder.AddContent(sequence++, Icons);
        builder.CloseElement();
    }
}
