//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Transformations;

[TestClass]
public class TransformationTests {
    private sealed class CustomTransformation(float value) : Transformation(value) {
        public override string GetTransformation() => $"custom-{GetFormattedValue()}";
    }

    [TestMethod]
    [TestCategory("Model")]
    public void GetFormattedValue_FormatsInvariantlyWithTwoDecimals() {
        var transformation = new CustomTransformation(2f);

        transformation.GetTransformation().Should().Be("custom-2.00");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void CustomSubclass_CanComposeIntoTransform() {
        var value = new Transform().Add(new CustomTransformation(1.5f)).GetTransformation();

        value.Should().Be("custom-1.50");
    }
}
