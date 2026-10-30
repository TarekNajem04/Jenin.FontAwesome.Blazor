//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Base element that adds Font Awesome power-transform support via <see cref="Transform"/>.
/// Not typically consumed directly.
/// </summary>
public class ElementWithTransformations : Element {
    /// <summary>
    /// Add custom transformations
    /// <para>
    /// <a href="https://fontawesome.com/docs/web/style/power-transform">Power Transforms</a>
    /// </para>
    /// </summary>
    [Parameter]
    public Transform? Transform { get; set; }

    /// <summary>
    /// Applies parameter changes before rendering, emitting the <c>data-fa-transform</c>
    /// attribute contract when a <see cref="Transform"/> is supplied.
    /// </summary>
    protected override void OnParametersSet() {
        base.OnParametersSet();

        AdditionalAttributes ??= [];

        // Always rewrite the key: the attribute dictionary persists between renders, so a transform
        // cleared by setting Transform to null must be removed rather than left in place.
        _ = AdditionalAttributes.UpdateOrRemoveIf
                                 (
                                    "data-fa-transform",
                                    Transform?.GetTransformation(),
                                    value => string.IsNullOrEmpty(Convert.ToString(value, CultureInfo.InvariantCulture))
                                 );
    }
}
