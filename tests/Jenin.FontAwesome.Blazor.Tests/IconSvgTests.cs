//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests;

[TestClass]
public class IconSvgTests {
    private const string SPRITE = """
        <svg xmlns="http://www.w3.org/2000/svg">
            <symbol id="fa-solid-house" viewBox="0 0 512 512"><path d="M0 0H1Z" fill="currentColor" /></symbol>
            <symbol id="fa-brands-github" viewBox="0 0 512 512"><path d="M2 2H3Z" fill="currentColor" /></symbol>
        </svg>
        """;

    [TestMethod]
    [TestCategory("Rendering")]
    public void ToSvgString_InlinesPathWithViewBox() {
        var markup = IconSvg.ToSvgString(SPRITE, "fa-house");

        StringAssert.Contains(markup, "<svg");
        StringAssert.Contains(markup, "viewBox=\"0 0 512 512\"");
        StringAssert.Contains(markup, "<path d=\"M0 0H1Z\"");
        StringAssert.Contains(markup, "aria-hidden=\"true\"");
        Assert.IsFalse(markup.Contains("aria-label", StringComparison.Ordinal));
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public void ToSvgString_ResolvesStyleSpecificSymbol() {
        var markup = IconSvg.ToSvgString(SPRITE, "fa-github", IconStyle.Brands);

        StringAssert.Contains(markup, "<path d=\"M2 2H3Z\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public void ToSvgString_AppliesColorSizeAndTitle() {
        var markup = IconSvg.ToSvgString(SPRITE, "house", color: "#e11d48", title: "Home", size: 32);

        StringAssert.Contains(markup, "fill=\"#e11d48\"");
        StringAssert.Contains(markup, "width=\"32\"");
        StringAssert.Contains(markup, "height=\"32\"");
        StringAssert.Contains(markup, "role=\"img\"");
        StringAssert.Contains(markup, "aria-label=\"Home\"");
        StringAssert.Contains(markup, "<title>Home</title>");
        Assert.IsFalse(markup.Contains("aria-hidden", StringComparison.Ordinal));
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public void ToSvgString_MissingSymbol_Throws() {
        Assert.ThrowsExactly<ArgumentException>(() => IconSvg.ToSvgString(SPRITE, "fa-nope"));
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public void ToSvgString_MalformedSprite_Throws() {
        Assert.ThrowsExactly<ArgumentException>(() => IconSvg.ToSvgString("<svg><oops", "fa-house"));
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public void ToSvgString_EmptyInputs_Throw() {
        Assert.ThrowsExactly<ArgumentException>(() => IconSvg.ToSvgString(string.Empty, "fa-house"));
        Assert.ThrowsExactly<ArgumentException>(() => IconSvg.ToSvgString(SPRITE, "  "));
    }
}
