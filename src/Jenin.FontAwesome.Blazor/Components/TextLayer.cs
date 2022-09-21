namespace Jenin.FontAwesome.Blazor.Components;

public partial class TextLayer : ElemenWithTransformations {
    [Parameter]
    public string Text { get; set; }

    protected override List<string> AddClasses(List<string> classes) {
        classes = base.AddClasses(classes);
        classes.Add("fa-layers-text");

        return classes;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder, int sequence) {
        builder.OpenElement(sequence, "span");
        _ = BuildElementRenderTree(ref sequence, builder);

        if (!string.IsNullOrEmpty(Text)) {
            builder.AddContent(sequence++, Text.Trim());
        }

        builder.CloseElement();
    }
}
