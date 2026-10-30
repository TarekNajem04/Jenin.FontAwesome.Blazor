//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Sample.Shared.Components;

public partial class CodeSnippet : ComponentBase {
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Custom css classname.
    /// </summary>
    [Parameter]
    public string? CssClass { get; set; }

    [Parameter]
    public string? Language { get; set; }

    [Parameter]
    public RenderFragment? Snippet { get; set; }

    [Parameter]
    public string? SnippetFile { get; set; }

    [Parameter]
    public Type? SnippetComponent { get; set; }

    [Parameter]
    public bool DrawBorder { get; set; }

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    private string Id { get; } = "code-snippet-" + Guid.NewGuid().ToString("n");
    private string? SnippetContent { get; set; }

    protected override async Task OnParametersSetAsync() {
        if(!string.IsNullOrEmpty(SnippetFile)) {
            var assembly = typeof(CodeSnippet).Assembly;
            var resourceName = ResourceHelper.PathToResourceName(SnippetFile, assembly);

            if(!string.IsNullOrEmpty(resourceName)) {
                SnippetContent = (await ResourceHelper.GetGetManifestResourceContentAsync(resourceName))?.Trim();
            }
        }

        if(SnippetComponent is not null) {
            var resourceName = SnippetComponent.FullName + ".razor";

            await using var stream = SnippetComponent.Assembly.GetManifestResourceStream(resourceName);

            if(stream is not null) {
                using var reader = new StreamReader(stream);

                var content = (await reader.ReadToEndAsync()).Trim();

                SnippetContent = !string.IsNullOrEmpty(SnippetContent)
                                    ? string.Join('\n', SnippetContent, content)
                                    : content;
            }
        }
    }

    private string GetLanguage() => Language ?? (SnippetComponent is null ? GetLanguageFromFileExtension() : "cshtml") ?? "cshtml";

    private string? GetLanguageFromFileExtension()
        => string.IsNullOrEmpty(SnippetFile)
                ? null
                : Path.GetExtension(SnippetFile).ToLowerInvariant() switch {
                    ".razor" => "cshtml",
                    ".cshtml" => "cshtml",
                    ".html" => "html",
                    ".css" => "css",
                    ".cs" => "csharp",
                    ".txt" => "none",
                    ".js" => "js",
                    _ => null
                };

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);

        if(firstRender) {
            await JSRuntime.HighlightElementAsync(elementId: Id, runAsync: true);
        }
    }

    /// <summary>
    /// static content
    /// </summary>
    /// <returns><see langword="false"/></returns>
    protected override bool ShouldRender() => false;
}
