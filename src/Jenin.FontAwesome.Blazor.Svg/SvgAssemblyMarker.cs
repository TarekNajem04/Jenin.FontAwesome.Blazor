//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Svg;

/// <summary>
/// Anchors a metadata reference to <see cref="Microsoft.AspNetCore.Components"/> in this
/// assembly's public surface.
/// </summary>
/// <remarks>
/// The generated icon components derive from <see cref="ComponentBase"/> without directly
/// referencing any <see cref="Microsoft.AspNetCore.Components"/> namespace members beyond the base
/// class. The Razor compiler only scans a referenced assembly for components when that assembly
/// references an <c>Microsoft.AspNetCore.*</c> assembly, so without this anchor the generated
/// icons are never discovered as tag helpers and produce <c>RZ10012</c> in consuming projects.
/// Deriving this type from <see cref="ComponentBase"/> keeps the required assembly reference in a
/// public signature (so it also survives into the reference assembly); marking it
/// <see langword="abstract"/> prevents it from being treated as a component itself.
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class SvgAssemblyMarker : ComponentBase {
}
