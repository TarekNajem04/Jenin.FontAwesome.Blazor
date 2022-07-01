using Jenin.FontAwesome.Blazor.Extensions;

namespace Jenin.FontAwesome.Blazor.Components;

public partial class CounterLayer : ElemenWithTransformations {
    [Parameter]
    public string Text { get; set; }

    [Parameter]
    public string LineHeight { get; set; }

    [Parameter]
    public string MaxWidth { get; set; }

    [Parameter]
    public string MinWidth { get; set; }

    [Parameter]
    public string Scale { get; set; }

    /// <summary>
    /// Set top offset.
    /// </summary>
    [Parameter]
    public string Top { get; set; }

    /// <summary>
    /// Set right offset.
    /// </summary>
    [Parameter]
    public string Right { get; set; }

    /// <summary>
    /// Set bottom offset.
    /// </summary>
    [Parameter]
    public string Bottom { get; set; }

    /// <summary>
    /// Set left offset.
    /// </summary>
    [Parameter]
    public string Left { get; set; }

    protected override List<string> AddClasses(List<string> classes) {
        classes = base.AddClasses(classes);
        classes.Add("fa-layers-counter");

        return classes;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        var sequence = 0;

        builder.OpenElement(sequence, "span");
        _ = BuildElementRenderTree(ref sequence, builder);

        if (!string.IsNullOrEmpty(Text)) {
            builder.AddContent(sequence++, Text.Trim());
        }

        builder.CloseElement();
    }

    protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles)
        => base.AddStyles(styles)
               .AddOrUpdateIfNotNull("--fa-counter-line-height", LineHeight)
               .AddOrUpdateIfNotNull("--fa-counter-max-width", MaxWidth)
               .AddOrUpdateIfNotNull("--fa-counter-min-width", MinWidth)
               .AddOrUpdateIfNotNull("--fa-counter-scale", Scale)
               .AddOrUpdateIfNotNull("--fa-top", Top)
               .AddOrUpdateIfNotNull("--fa-right", Right)
               .AddOrUpdateIfNotNull("--fa-bottom", Bottom)
               .AddOrUpdateIfNotNull("--fa-left", Left);
}
