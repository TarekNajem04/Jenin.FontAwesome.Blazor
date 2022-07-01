using Microsoft.AspNetCore.Components;

namespace Jenin.FontAwesome.Blazor.Sample.Components;

public partial class SampleTab : ComponentBase {
    public SampleTab() {
        Id = "sample-tab_" + Guid.NewGuid().ToString("N");
        Enabled = true;
    }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> AdditionalAttributes { get; set; }

    /// <summary>
    /// Custom css classname.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; }

    [CascadingParameter(Name = "Parent")]
    private SampleTabs Parent { get; set; }

    [Parameter]
    public RenderFragment Samples { get; set; }

    [Parameter]
    public string Id { get; set; }

    [Parameter]
    public string Text { get; set; }

    [Parameter]
    public bool Enabled { get; set; }

    public bool Active { get; protected internal set; }

    protected override void OnInitialized() {
        if (Parent is null) {
            throw new ArgumentNullException(nameof(Parent), "SampleTab must exist within a SampleTabs");
        }

        base.OnInitialized();
        Parent.AddPage(this);
    }
}
