//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Enums;

[TestClass]
public class ValueEnumTests {
    [TestMethod]
    [TestCategory("Model")]
    [DataRow(IconSize.NotSet, "")]
    [DataRow(IconSize.Inherits, "")]
    [DataRow(IconSize.X01, "fa-1x")]
    [DataRow(IconSize.X10, "fa-10x")]
    [DataRow(IconSize.ExtraSmall_2, "fa-2xs")]
    [DataRow(IconSize.ExtraSmall, "fa-xs")]
    [DataRow(IconSize.Small, "fa-sm")]
    [DataRow(IconSize.Large, "fa-lg")]
    [DataRow(IconSize.ExtraLarge, "fa-xl")]
    [DataRow(IconSize.ExtraLarge_2, "fa-2xl")]
    public void IconSize_MapsToFontAwesomeClass(IconSize value, string expected) => Assert.AreEqual(expected, value.GetStringValue());

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(IconStyle.Solid, "fa-solid")]
    [DataRow(IconStyle.Regular, "fa-regular")]
    [DataRow(IconStyle.Light, "fa-light")]
    [DataRow(IconStyle.Thin, "fa-thin")]
    [DataRow(IconStyle.Duotone, "fa-duotone fa-solid")]
    [DataRow(IconStyle.Brands, "fa-brands")]
    public void IconStyle_MapsToFontAwesomeClass(IconStyle value, string expected) => Assert.AreEqual(expected, value.GetStringValue());

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(IconRotate.NotSet, "")]
    [DataRow(IconRotate.Deg090, "fa-rotate-90")]
    [DataRow(IconRotate.Deg180, "fa-rotate-180")]
    [DataRow(IconRotate.Deg270, "fa-rotate-270")]
    [DataRow(IconRotate.FlipHorizontal, "fa-flip-horizontal")]
    [DataRow(IconRotate.FlipVertical, "fa-flip-vertical")]
    [DataRow(IconRotate.FlipBoth, "fa-flip-both")]
    [DataRow(IconRotate.RotateBy, "fa-rotate-by")]
    public void IconRotate_MapsToFontAwesomeClass(IconRotate value, string expected) => Assert.AreEqual(expected, value.GetStringValue());

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(IconStackSize.NotSet, "")]
    [DataRow(IconStackSize.Same, "fa-stack-1x")]
    [DataRow(IconStackSize.Larger, "fa-stack-2x")]
    public void IconStackSize_MapsToFontAwesomeClass(IconStackSize value, string expected) => Assert.AreEqual(expected, value.GetStringValue());

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(Typeface.Classic, "")]
    [DataRow(Typeface.Sharp, "fa-sharp")]
    [DataRow(Typeface.Duotone, "fa-duotone")]
    [DataRow(Typeface.SharpDuotone, "fa-sharp-duotone")]
    public void Typeface_MapsToFontAwesomeClass(Typeface value, string expected) => Assert.AreEqual(expected, value.GetStringValue());

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(PullPosition.None, "")]
    [DataRow(PullPosition.Left, "fa-pull-left")]
    [DataRow(PullPosition.Right, "fa-pull-right")]
    [DataRow(PullPosition.Start, "fa-pull-start")]
    [DataRow(PullPosition.End, "fa-pull-end")]
    public void PullPosition_MapsToFontAwesomeClass(PullPosition value, string expected) => Assert.AreEqual(expected, value.GetStringValue());

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(IconWidth.NotSet, "")]
    [DataRow(IconWidth.Auto, "fa-width-auto")]
    [DataRow(IconWidth.Fixed, "fa-width-fixed")]
    public void IconWidth_MapsToFontAwesomeClass(IconWidth value, string expected) => Assert.AreEqual(expected, value.GetStringValue());

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(CanvasPadding.NotSet, "")]
    [DataRow(CanvasPadding.Square, "fa-canvas-square")]
    [DataRow(CanvasPadding.Roomy, "fa-canvas-roomy")]
    public void CanvasPadding_MapsToFontAwesomeClass(CanvasPadding value, string expected) => Assert.AreEqual(expected, value.GetStringValue());

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(AnimationDirection.None, "")]
    [DataRow(AnimationDirection.Normal, "normal")]
    [DataRow(AnimationDirection.Reverse, "reverse")]
    [DataRow(AnimationDirection.Alternate, "alternate")]
    [DataRow(AnimationDirection.AlternateReverse, "alternate-reverse")]
    public void AnimationDirection_MapsToCssToken(AnimationDirection value, string expected) => Assert.AreEqual(expected, value.GetStringValue());

    [TestMethod]
    [TestCategory("Model")]
    [DataRow(BorderStyle.None, "none")]
    [DataRow(BorderStyle.Hidden, "hidden")]
    [DataRow(BorderStyle.Dotted, "dotted")]
    [DataRow(BorderStyle.Dashed, "dashed")]
    [DataRow(BorderStyle.Solid, "solid")]
    [DataRow(BorderStyle.Double, "double")]
    [DataRow(BorderStyle.Groove, "groove")]
    [DataRow(BorderStyle.Ridge, "ridge")]
    [DataRow(BorderStyle.Inset, "inset")]
    [DataRow(BorderStyle.Outset, "outset")]
    public void BorderStyle_MapsToCssToken(BorderStyle value, string expected) => Assert.AreEqual(expected, value.GetStringValue());
}
