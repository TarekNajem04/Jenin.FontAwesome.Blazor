using Microsoft.AspNetCore.Components;

namespace Jenin.FontAwesome.Blazor.Sample.Components;

public partial class Sample : ComponentBase {
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> AdditionalAttributes { get; set; }

    /// <summary>
    /// Custom css classname.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; }

    [Parameter]
    public RenderFragment Snippet { get; set; }

    [Parameter]
    public RenderFragment SnippetCode { get; set; }

    [Parameter]
    public string SnippetFile { get; set; }

    [Parameter]
    public Type SnippetComponent { get; set; }

    [Parameter]
    public string Heading { get; set; }
}
