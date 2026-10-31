//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class IconTests {
    private const string DECORATIVE = " aria-hidden=\"true\" role=\"presentation\"";

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task RendersSpanWrappingEnhanceableIconElementAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(("IconName", "fa-house")));

        markup.Should().Be($"<span><i class=\"fa-solid fa-house\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task RendersFixedWidthSizeAndTransformContractAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-user"),
            ("Color", "red"),
            ("IconSize", IconSize.X02),
            ("FixedWidth", true),
            ("Transform", new Transform().Grow(2).Rotate(-30))));

        markup.Should().Be(
            $"<span><i class=\"fa-fw fa-2x fa-solid fa-user\" style=\"color:red;\"{DECORATIVE} data-fa-transform=\"grow-2.00 rotate--30.00\"></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Border_EmitsBorderClassAndCustomPropertiesAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-user"),
            ("Bordered", true),
            ("BorderColor", "blue"),
            ("BorderStyle", BorderStyle.Solid)));

        markup.Should().Be(
            $"<span><i class=\"fa-border fa-solid fa-user\" style=\"--fa-border-color:blue;--fa-border-style:solid;\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Animation_EmitsClassesAndAnimationStylesAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-spinner"),
            ("Animation", Animate.Spin().SetDelay(TimeSpan.FromMilliseconds(500)))));

        markup.Should().Be(
            $"<span><i class=\"fa-solid fa-spinner fa-spin\" style=\"--fa-animation-delay:500ms;\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task TypefaceAndIconStyle_AreEmittedAsClassesAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("IconStyle", IconStyle.Brands),
            ("Typeface", Typeface.Sharp)));

        markup.Should().Be($"<span><i class=\"fa-sharp fa-brands fa-house\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Rotate_EmitsRotationClassAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Rotate", IconRotate.Deg090)));

        markup.Should().Be($"<span><i class=\"fa-solid fa-house fa-rotate-90\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ReRender_RemovesStyleWhenColorResetAsync() {
        var markup = await TestHost.RenderMarkupSequenceAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-house"), ("Color", "red")),
            TestHost.Parameters(("IconName", "fa-house"), ("Color", null)));

        markup.Should().Be($"<span><i class=\"fa-solid fa-house\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ReRender_RemovesClassWhenFeatureResetAsync() {
        var markup = await TestHost.RenderMarkupSequenceAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-house"), ("Rotate", IconRotate.Deg090)),
            TestHost.Parameters(("IconName", "fa-house"), ("Rotate", IconRotate.NotSet)));

        markup.Should().Be($"<span><i class=\"fa-solid fa-house\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ReRender_RemovesTransformWhenResetAsync() {
        var markup = await TestHost.RenderMarkupSequenceAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-house"), ("Transform", new Transform().Grow(2).Rotate(-30))),
            TestHost.Parameters(("IconName", "fa-house"), ("Transform", null)));

        markup.Should().Be($"<span><i class=\"fa-solid fa-house\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ReRender_RemovesBackgroundColorWhenResetAsync() {
        var markup = await TestHost.RenderMarkupSequenceAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-house"), ("BackgroundColor", "#fff3cd")),
            TestHost.Parameters(("IconName", "fa-house"), ("BackgroundColor", null)));

        markup.Should().Be($"<span><i class=\"fa-solid fa-house\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ReRender_KeepsAuthorStyleWhenColorResetAsync() {
        var markup = await TestHost.RenderMarkupSequenceAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-house"), ("Color", "red"), ("style", "width:10px")),
            TestHost.Parameters(("IconName", "fa-house"), ("Color", null), ("style", "width:10px")));

        markup.Should().Be($"<span><i style=\"width:10px;\" class=\"fa-solid fa-house\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task RotateBy_EmitsRotationAngleCustomPropertyAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("Rotate", IconRotate.RotateBy),
            ("RotateAngle", 45f)));

        markup.Should().Be(
            $"<span><i class=\"fa-solid fa-house fa-rotate-by\" style=\"--fa-rotate-angle:45.00deg;\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task PullPosition_EmitsPullClassAndMarginStyleAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("PullPosition", PullPosition.Left),
            ("PullMargin", "1em")));

        markup.Should().Be(
            $"<span><i class=\"fa-solid fa-house fa-pull-left\" style=\"--fa-pull-margin:1em;\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task StackSizeAndZIndex_AreEmittedAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-circle"),
            ("StackSize", IconStackSize.Same),
            ("StackZIndex", 1)));

        markup.Should().Be(
            $"<span><i class=\"fa-solid fa-circle fa-stack-1x\" style=\"--fa-stack-z-index:1;\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task DuotoneCustomProperties_AreEmittedAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("IconStyle", IconStyle.Duotone),
            ("PrimaryColor", "red"),
            ("PrimaryOpacity", 0.5f),
            ("SecondaryColor", "blue"),
            ("SecondaryOpacity", 0.25f),
            ("SwapOpacity", true)));

        markup.Should().Be(
            $"<span><i class=\"fa-duotone fa-solid fa-house fa-swap-opacity\" style=\"--fa-primary-color:red;--fa-primary-opacity:0.50;--fa-secondary-color:blue;--fa-secondary-opacity:0.25;\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ProFamilyStylePairings_AreComposedAsync() {
        var duotoneRegular = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-user"),
            ("Typeface", Typeface.Duotone),
            ("IconStyle", IconStyle.Regular)));
        var sharpDuotoneSolid = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-user"),
            ("Typeface", Typeface.SharpDuotone),
            ("IconStyle", IconStyle.Solid)));
        var sharpThin = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-user"),
            ("Typeface", Typeface.Sharp),
            ("IconStyle", IconStyle.Thin)));

        duotoneRegular.Should().Contain("class=\"fa-duotone fa-regular fa-user\"");
        sharpDuotoneSolid.Should().Contain("class=\"fa-sharp-duotone fa-solid fa-user\"");
        sharpThin.Should().Contain("class=\"fa-sharp fa-thin fa-user\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task PrimaryOpacity_IsClampedToUnitIntervalAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("IconStyle", IconStyle.Duotone),
            ("PrimaryOpacity", 5f)));

        markup.Should().Contain("--fa-primary-opacity:1.00;");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task SecondaryOpacity_IsClampedToUnitIntervalAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("IconStyle", IconStyle.Duotone),
            ("SecondaryOpacity", -1f)));

        markup.Should().Contain("--fa-secondary-opacity:0.00;");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Mask_IsEmittedAsAdditionalAttributesAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("MaskIconStyle", IconStyle.Solid),
            ("MaskIconName", "fa-circle"),
            ("MaskId", "mask-1")));

        markup.Should().Contain("data-fa-mask=\"fa-solid fa-circle\"");
        markup.Should().Contain("data-fa-mask-id=\"mask-1\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task AdditionalAttributes_AreForwardedAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("AdditionalAttributes", new Dictionary<string, object> { ["data-x"] = "1", ["id"] = "i1" })));

        markup.Should().Be($"<span><i data-x=\"1\" id=\"i1\" class=\"fa-solid fa-house\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task UnmatchedParameters_AreForwardedAsAdditionalAttributesAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("id", "zz")));

        markup.Should().Be($"<span><i id=\"zz\" class=\"fa-solid fa-house\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task CssClass_PrependsCustomClassAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("CssClass", "my-icon")));

        markup.Should().Be($"<span><i class=\"my-icon fa-solid fa-house\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task CssClass_OrderIsDeterministicAsync() {
        var parameters = TestHost.Parameters(
            ("IconName", "fa-house"),
            ("MaskIconStyle", IconStyle.Solid),
            ("MaskIconName", "fa-circle"),
            ("MaskId", "m"));

        var first = await TestHost.RenderMarkupAsync<Icon>(parameters);
        var second = await TestHost.RenderMarkupAsync<Icon>(parameters);

        second.Should().Be(first);
        ExtractClass(first).Should().Be("fa-solid fa-house");
    }

    private static string ExtractClass(string markup) {
        const string token = "class=\"";
        var start = markup.IndexOf(token, StringComparison.Ordinal) + token.Length;
        var end = markup.IndexOf('"', start);

        return markup[start..end];
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task EmptyIconName_StillRendersEnhanceableElementAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>();

        markup.Should().Be($"<span><i class=\"fa-solid\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task WidthMode_EmitsFontAwesome7WidthClassAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("WidthMode", IconWidth.Auto)));

        markup.Should().Be($"<span><i class=\"fa-width-auto fa-solid fa-house\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task WidthMode_Fixed_EmitsLegacyReplacementClassAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("WidthMode", IconWidth.Fixed)));

        markup.Should().Be($"<span><i class=\"fa-width-fixed fa-solid fa-house\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task CanvasPadding_EmitsFontAwesome7UtilityClassAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("CanvasPadding", CanvasPadding.Roomy)));

        markup.Should().Be($"<span><i class=\"fa-canvas-roomy fa-solid fa-house\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task WidthMode_And_CanvasPadding_ClassOrderIsDeterministicAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("WidthMode", IconWidth.Fixed),
            ("CanvasPadding", CanvasPadding.Square),
            ("IconSize", IconSize.X02)));

        markup.Should().Be($"<span><i class=\"fa-2x fa-width-fixed fa-canvas-square fa-solid fa-house\"{DECORATIVE}></i></span>");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task PullPosition_StartAndEnd_EmitLogicalPullClassesAsync() {
        var start = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("PullPosition", PullPosition.Start)));

        var end = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("PullPosition", PullPosition.End)));

        start.Should().Be($"<span><i class=\"fa-solid fa-house fa-pull-start\"{DECORATIVE}></i></span>");
        end.Should().Be($"<span><i class=\"fa-solid fa-house fa-pull-end\"{DECORATIVE}></i></span>");
    }
}
