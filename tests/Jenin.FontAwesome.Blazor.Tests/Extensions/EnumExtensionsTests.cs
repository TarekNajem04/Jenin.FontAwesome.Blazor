//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Extensions;

[TestClass]
public class EnumExtensionsTests {
    private enum Plain {
        A = 0,
        B = 1
    }

    private enum Described {
        [System.ComponentModel.Description("hello")]
        A = 0,
        B = 1
    }

    [Flags]
    private enum Flagsy {
        None = 0,
        A = 1,
        B = 2,
        C = 4
    }

    [TestMethod]
    [TestCategory("Model")]
    public void GetStringValue_ReturnsMappedValue() => Assert.AreEqual("fa-2x", IconSize.X02.GetStringValue());

    [TestMethod]
    [TestCategory("Model")]
    public void GetStringValue_UnmappedValue_ReturnsNull() => Assert.IsNull(Plain.A.GetStringValue());

    [TestMethod]
    [TestCategory("Model")]
    public void GetStringValue_UnmappedValue_ReturnsSuppliedDefault() => Assert.AreEqual("fallback", Plain.A.GetStringValue("fallback"));

    [TestMethod]
    [TestCategory("Model")]
    public void GetDescription_WithAttribute_ReturnsDescription() => Assert.AreEqual("hello", Described.A.GetDescription());

    [TestMethod]
    [TestCategory("Model")]
    public void GetDescription_WithoutAttribute_ReturnsName() => Assert.AreEqual("B", Described.B.GetDescription());

    [TestMethod]
    [TestCategory("Model")]
    public void GetField_ReturnsFieldCarryingValueAttribute() {
        var attribute = IconSize.X02.GetField()?.GetCustomAttribute<ValueAttribute>();

        Assert.IsNotNull(attribute);
        Assert.AreEqual("fa-2x", attribute.Value);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void GetField_ReflectsAttributePresence() {
        Assert.IsNotNull(IconSize.X02.GetField()?.GetCustomAttribute<ValueAttribute>());
        Assert.IsNull(Plain.A.GetField()?.GetCustomAttribute<ValueAttribute>());
    }

    [TestMethod]
    [TestCategory("Model")]
    public void ToEnum_ParsesDefinedName() => Assert.AreEqual(IconStyle.Duotone, "Duotone".ToEnum<IconStyle>());

    [TestMethod]
    [TestCategory("Model")]
    public void ToEnum_EmptyString_ReturnsDefault() => Assert.AreEqual(IconStyle.Solid, "".ToEnum<IconStyle>());

    [TestMethod]
    [TestCategory("Model")]
    public void ToEnum_UnknownName_ReturnsDefault() => Assert.AreEqual(IconStyle.Solid, "nonsense".ToEnum<IconStyle>());

    [TestMethod]
    [TestCategory("Model")]
    public void GetAllFlags_YieldsEachSetFlagInDefinitionOrder() {
        var flags = (Flagsy.A | Flagsy.C).GetAllFlags().ToList();

        Assert.AreSequenceEqual([Flagsy.A, Flagsy.C], flags);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void GetAllFlags_None_YieldsNoFlags() {
        var flags = Flagsy.None.GetAllFlags().ToList();

        Assert.IsEmpty(flags);
    }
}
