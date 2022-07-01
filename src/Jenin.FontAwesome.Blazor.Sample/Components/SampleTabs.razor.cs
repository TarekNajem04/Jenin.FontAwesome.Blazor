using Microsoft.AspNetCore.Components;

namespace Jenin.FontAwesome.Blazor.Sample.Components;

public partial class SampleTabs : ComponentBase {
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> AdditionalAttributes { get; set; }

    /// <summary>
    /// Custom css classname.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; }

    [Parameter]
    public RenderFragment Tabs { get; set; }

    public List<SampleTab> Pages { get; } = new List<SampleTab>();
    public SampleTab ActivePage => Pages.Find(x => x.Active);

    internal void AddPage(SampleTab tabPage) {
        Pages.Add(tabPage);

        if (Pages.Count == 1) {
            tabPage.Active = true;
        }

        StateHasChanged();
    }

    internal void RemovePage(SampleTab tabPage) {
        _ = Pages.Remove(tabPage);

        if (Pages.Remove(tabPage) && tabPage.Active && Pages.Count > 0) {
            SetActivePage(Pages[0]);
        }

        StateHasChanged();
    }

    internal void SetActivePage(SampleTab tabPage) {
        if (tabPage is not null && Pages.Contains(tabPage) && tabPage.Enabled) {
            tabPage.Active = true;
        }
    }
}
