//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Renders standalone Font Awesome SVG markup strings from a self-hosted sprite document,
/// for targets Blazor components cannot reach: HTML email, PDFs, server-generated reports.
/// The same sprite file that feeds <see cref="Components.SpriteIcon"/> in the browser doubles
/// as the glyph source here, so web and document rendering can never drift apart.
/// </summary>
/// <remarks>
/// <para>
/// The output is a self-contained <c>&lt;svg&gt;</c> with inlined path data (no external
/// references, which email clients would block), trim- and NativeAOT-safe: plain XML parsing,
/// no reflection, no dynamic code.
/// </para>
/// <para>
/// Only what the sprite carries can render: Free solid/regular/brands single-color glyphs.
/// Duotone layer colors, masks, power transforms, and animations do not translate and are not
/// accepted; a plain color fills the whole glyph.
/// </para>
/// </remarks>
public static class IconSvg {
    /// <summary>
    /// Builds a standalone SVG document string for an icon in a sprite document.
    /// </summary>
    /// <param name="spriteXml">The sprite document text (as produced by <c>eng/fa_sprite.py</c>).</param>
    /// <param name="iconName">The Font Awesome icon name, including its style-independent <c>fa-</c> prefixed identifier.</param>
    /// <param name="iconStyle">The style family to address (<c>fa-solid-*</c>, <c>fa-regular-*</c>, <c>fa-brands-*</c>).</param>
    /// <param name="color">An optional fill color applied to the glyph paths. <c>null</c> keeps the sprite default (<c>currentColor</c>).</param>
    /// <param name="title">An optional accessible name, emitted as a <c>&lt;title&gt;</c> child and an <c>aria-label</c>. Without it the icon is marked decorative.</param>
    /// <param name="size">Optional rendered size in CSS pixels (emitted as <c>width</c>/<c>height</c>, which mail clients honor where CSS is stripped).</param>
    /// <returns>A standalone SVG string ready to embed in HTML email, PDFs, or reports.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="spriteXml"/> is empty or malformed, when <paramref name="iconName"/> is empty, or when the sprite contains no matching symbol.</exception>
    public static string ToSvgString(string spriteXml, string iconName, IconStyle iconStyle = IconStyle.Solid, string? color = null, string? title = null, int? size = null) {
        if(string.IsNullOrWhiteSpace(spriteXml)) {
            throw new ArgumentException("A sprite document is required.", nameof(spriteXml));
        }

        if(string.IsNullOrWhiteSpace(iconName)) {
            throw new ArgumentException("An icon name is required.", nameof(iconName));
        }

        var name = iconName.StartsWith("fa-", StringComparison.OrdinalIgnoreCase) ? iconName[3..] : iconName;
        var symbolId = iconStyle switch {
            IconStyle.Regular => $"fa-regular-{name}",
            IconStyle.Brands => $"fa-brands-{name}",
            _ => $"fa-solid-{name}",
        };

        XDocument document;

        try {
            document = XDocument.Parse(spriteXml, LoadOptions.PreserveWhitespace);
        }
        catch(System.Xml.XmlException exception) {
            throw new ArgumentException("The sprite document is not well-formed XML.", nameof(spriteXml), exception);
        }

        // Static local (no captures) keeps this trim- and AOT-safe.
        static bool MatchesId(System.Xml.Linq.XElement element, string id)
            => element.Name.LocalName == "symbol" && (string?)element.Attribute("id") == id;

        var symbol = document.Descendants().FirstOrDefault(element => MatchesId(element, symbolId))
            ?? throw new ArgumentException($"The sprite contains no symbol '{symbolId}'.", nameof(iconName));

        var viewBox = (string?)symbol.Attribute("viewBox") ?? "0 0 512 512";
        XNamespace ns = "http://www.w3.org/2000/svg";
        var svg = new XElement(ns + "svg");

        svg.SetAttributeValue("viewBox", viewBox);

        if(size.HasValue) {
            svg.SetAttributeValue("width", size.Value.ToString(CultureInfo.InvariantCulture));
            svg.SetAttributeValue("height", size.Value.ToString(CultureInfo.InvariantCulture));
        }

        var label = string.IsNullOrWhiteSpace(title) ? null : title.Trim();

        if(label is null) {
            svg.SetAttributeValue("aria-hidden", "true");
        } else {
            svg.SetAttributeValue("role", "img");
            svg.SetAttributeValue("aria-label", label);
            svg.Add(new XElement(ns + "title", label));
        }

        foreach(var child in symbol.Elements()) {
            var copy = new XElement(child);

            if(!string.IsNullOrWhiteSpace(color) && string.Equals(copy.Name.LocalName, "path", StringComparison.OrdinalIgnoreCase)) {
                copy.SetAttributeValue("fill", color);
            }

            svg.Add(copy);
        }

        return svg.ToString(SaveOptions.DisableFormatting);
    }
}
