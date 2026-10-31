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
    public void GetStringValue_ReturnsMappedValue() => IconSize.X02.GetStringValue().Should().Be("fa-2x");

    [TestMethod]
    [TestCategory("Model")]
    public void GetStringValue_UnmappedValue_ReturnsNull() => Plain.A.GetStringValue().Should().BeNull();

    [TestMethod]
    [TestCategory("Model")]
    public void GetStringValue_UnmappedValue_ReturnsSuppliedDefault() => Plain.A.GetStringValue("fallback").Should().Be("fallback");

    [TestMethod]
    [TestCategory("Model")]
    public void GetDescription_WithAttribute_ReturnsDescription() => Described.A.GetDescription().Should().Be("hello");

    [TestMethod]
    [TestCategory("Model")]
    public void GetDescription_WithoutAttribute_ReturnsName() => Described.B.GetDescription().Should().Be("B");

    [TestMethod]
    [TestCategory("Model")]
    public void GetField_ReturnsFieldCarryingValueAttribute() {
        var attribute = IconSize.X02.GetField()?.GetCustomAttribute<ValueAttribute>();

        attribute.Should().NotBeNull();
        attribute!.Value.Should().Be("fa-2x");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void GetField_ReflectsAttributePresence() {
        (IconSize.X02.GetField()?.GetCustomAttribute<ValueAttribute>()).Should().NotBeNull();
        (Plain.A.GetField()?.GetCustomAttribute<ValueAttribute>()).Should().BeNull();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void ToEnum_ParsesDefinedName() => "Duotone".ToEnum<IconStyle>().Should().Be(IconStyle.Duotone);

    [TestMethod]
    [TestCategory("Model")]
    public void ToEnum_EmptyString_ReturnsDefault() => "".ToEnum<IconStyle>().Should().Be(IconStyle.Solid);

    [TestMethod]
    [TestCategory("Model")]
    public void ToEnum_UnknownName_ReturnsDefault() => "nonsense".ToEnum<IconStyle>().Should().Be(IconStyle.Solid);

    [TestMethod]
    [TestCategory("Model")]
    public void GetAllFlags_YieldsEachSetFlagInDefinitionOrder() {
        var flags = (Flagsy.A | Flagsy.C).GetAllFlags().ToList();

        flags.Should().Equal([Flagsy.A, Flagsy.C]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void GetAllFlags_None_YieldsNoFlags() {
        var flags = Flagsy.None.GetAllFlags().ToList();

        flags.Should().BeEmpty();
    }
}
