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

        markup.Should().Contain("<svg");
        markup.Should().Contain("viewBox=\"0 0 512 512\"");
        markup.Should().Contain("<path d=\"M0 0H1Z\"");
        markup.Should().Contain("aria-hidden=\"true\"");
        markup.Contains("aria-label", StringComparison.Ordinal).Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public void ToSvgString_ResolvesStyleSpecificSymbol() {
        var markup = IconSvg.ToSvgString(SPRITE, "fa-github", IconStyle.Brands);

        markup.Should().Contain("<path d=\"M2 2H3Z\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public void ToSvgString_AppliesColorSizeAndTitle() {
        var markup = IconSvg.ToSvgString(SPRITE, "house", color: "#e11d48", title: "Home", size: 32);

        markup.Should().Contain("fill=\"#e11d48\"");
        markup.Should().Contain("width=\"32\"");
        markup.Should().Contain("height=\"32\"");
        markup.Should().Contain("role=\"img\"");
        markup.Should().Contain("aria-label=\"Home\"");
        markup.Should().Contain("<title>Home</title>");
        markup.Contains("aria-hidden", StringComparison.Ordinal).Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public void ToSvgString_MissingSymbol_Throws() => FluentActions.Invoking(() => IconSvg.ToSvgString(SPRITE, "fa-nope")).Should().ThrowExactly<ArgumentException>();

    [TestMethod]
    [TestCategory("Rendering")]
    public void ToSvgString_MalformedSprite_Throws() => FluentActions.Invoking(() => IconSvg.ToSvgString("<svg><oops", "fa-house")).Should().ThrowExactly<ArgumentException>();

    [TestMethod]
    [TestCategory("Rendering")]
    public void ToSvgString_EmptyInputs_Throw() {
        FluentActions.Invoking(() => IconSvg.ToSvgString(string.Empty, "fa-house")).Should().ThrowExactly<ArgumentException>();
        FluentActions.Invoking(() => IconSvg.ToSvgString(SPRITE, "  ")).Should().ThrowExactly<ArgumentException>();
    }
}
