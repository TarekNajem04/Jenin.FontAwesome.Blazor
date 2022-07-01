namespace Jeninnet.FontAwesome.Components;

public class ElementInverseable : ElemenWithTransformations {
    /// <summary>
    /// Inverts the color of the icon displayed at base size when stacked
    /// </summary>
    [Parameter]
    public bool? Inverse { get; set; }

    protected override List<string> AddClasses(List<string> classes) {
        classes = base.AddClasses(classes);

        if (Inverse is true) {
            classes.Add("fa-inverse");
        }

        return classes;
    }
}
