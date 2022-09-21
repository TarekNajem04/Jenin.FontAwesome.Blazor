namespace Jenin.FontAwesome.Blazor.Components;

public class ElemenWithTransformations : Element {
    /// <summary>
    /// Add custom transformations
    /// <para>
    /// <a href="https://fontawesome.com/docs/web/style/power-transform">Power Transforms</a>
    /// </para>
    /// </summary>
    [Parameter]
    public Transform Transform { get; set; }

    protected override void OnParametersSet() {
        base.OnParametersSet();

        if (Transform is not null) {
            AdditionalAttributes ??= new();
            _ = AdditionalAttributes.UpdateOrRemoveIf("data-fa-transform", Transform.GetTransformation(), value => string.IsNullOrEmpty(Convert.ToString(value)));
        }
    }
}
