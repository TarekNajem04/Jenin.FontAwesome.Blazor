//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Sample.Shared.Components;

public partial class Sample : ComponentBase {
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Custom css classname.
    /// </summary>
    [Parameter]
    public string? CssClass { get; set; }

    [Parameter]
    public RenderFragment? Snippet { get; set; }

    [Parameter]
    public RenderFragment? SnippetCode { get; set; }

    [Parameter]
    public string? SnippetFile { get; set; }

    [Parameter]
    public Type? SnippetComponent { get; set; }

    [Parameter]
    public string? Heading { get; set; }
}
