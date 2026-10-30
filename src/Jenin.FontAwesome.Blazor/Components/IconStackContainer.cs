//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// A Font Awesome icon stack (<c>fa-stack</c>) that groups stacked icons and sets their
/// vertical alignment.
/// </summary>
public class IconStackContainer : IconContainer {
    /// <summary>
    /// sets vertical alignment of <see cref="Icon"/>
    /// <para>
    /// <a href="https://developer.mozilla.org/en-US/docs/Web/CSS/vertical-align">Any valid CSS vertical-align value</a>.
    /// </para>
    /// </summary>
    [Parameter]
    public string? VerticalAlign { get; set; }

    private protected override string GetRootTag() => "span";

    private protected override List<string?> AddClasses(List<string?> classes) {
        classes = base.AddClasses(classes);
        classes.Add("fa-stack");

        return classes;
    }

    private protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles)
        => base.AddStyles(styles)
               .AddOrUpdateIfNotNull("vertical-align", VerticalAlign);
}
