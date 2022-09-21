namespace Jenin.FontAwesome.Blazor.Components;

public class IconList : ElementBase {
    [Parameter]
    public RenderFragment Items { get; set; }

    [Parameter]
    public bool OrderedList { get; set; }

    /// <summary>
    /// Set margin around icon.
    /// </summary>
    [Parameter]
    public string ListItemMargin { get; set; }

    /// <summary>
    /// Set width of icon.
    /// </summary>
    [Parameter]
    public string ListItemWidth { get; set; }

    protected override List<string> AddClasses(List<string> classes) {
        classes = base.AddClasses(classes);
        classes.Add("fa-ul");

        return classes;
    }

    protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles)
        => base.AddStyles(styles)
               .AddOrUpdateIfNotNull("--fa-li-margin", ListItemMargin)
               .AddOrUpdateIfNotNull("--fa-li-margin", ListItemWidth);

    protected override void BuildRenderTree(RenderTreeBuilder builder, int sequence) {
        /*
            <[ul | ol] @attributes=AdditionalAttributes>
                <CascadingValue Value=this Name="IconList" IsFixed=true>@Items</CascadingValue>
            </[ul | ol]>
         */

        builder.OpenElement(sequence, OrderedList ? "ol" : "ul");
        _ = BuildElementRenderTree(ref sequence, builder);
        builder.AddContent(sequence++, Items);
        builder.CloseElement();
    }
}
