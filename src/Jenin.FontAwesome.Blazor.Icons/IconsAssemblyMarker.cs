//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Icons;

/// <summary>
/// Anchors a metadata reference to <see cref="Microsoft.AspNetCore.Components"/> in this
/// assembly's public surface.
/// </summary>
/// <remarks>
/// The preset icon components in this assembly derive from <c>Jenin.FontAwesome.Blazor.Components.Icon</c>
/// without directly referencing any type from <c>Microsoft.AspNetCore.Components</c>. The Razor
/// compiler only scans a referenced assembly for components when that assembly references an
/// <c>Microsoft.AspNetCore.*</c> assembly, so without this anchor the presets are never discovered
/// as tag helpers and produce <c>RZ10012</c> in consuming projects. Deriving this type from
/// <see cref="ComponentBase"/> keeps the required assembly reference in a public signature (so it
/// also survives into the reference assembly); marking it <c>abstract</c> prevents it from being
/// treated as a component itself.
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class IconsAssemblyMarker : ComponentBase;
