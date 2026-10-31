//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Transformations;

[TestClass]
public class TransformTests {
    [TestMethod]
    [TestCategory("Model")]
    public void GetTransformation_Default_IsEmpty() => new Transform().GetTransformation().Should().Be("");

    [TestMethod]
    [TestCategory("Model")]
    public void GetTransformation_PreservesInsertionOrder() {
        var value = new Transform().Shrink(8).Up(6).Rotate(-30).FlipV().GetTransformation();

        value.Should().Be("shrink-8.00 up-6.00 rotate--30.00 flip-v");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Grow_EmitsGrowToken() => new Transform().Grow(2).GetTransformation().Should().Be("grow-2.00");

    [TestMethod]
    [TestCategory("Model")]
    public void Shrink_EmitsShrinkToken() => new Transform().Shrink(4).GetTransformation().Should().Be("shrink-4.00");

    [TestMethod]
    [TestCategory("Model")]
    [DataRow("up-3.00")]
    [DataRow("down-3.00")]
    [DataRow("left-3.00")]
    [DataRow("right-3.00")]
    public void DirectionalOperations_EmitPositioningToken(string expected) {
        var value = expected switch {
            "up-3.00" => new Transform().Up(3),
            "down-3.00" => new Transform().Down(3),
            "left-3.00" => new Transform().Left(3),
            _ => new Transform().Right(3)
        };

        value.GetTransformation().Should().Be(expected);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Rotate_AllowsNegativeAndUnboundedValues() {
        new Transform().Rotate(-90).GetTransformation().Should().Be("rotate--90.00");
        new Transform().Rotate(720).GetTransformation().Should().Be("rotate-720.00");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void FlipV_EmitsFlipVerticalToken() => new Transform().FlipV().GetTransformation().Should().Be("flip-v");

    [TestMethod]
    [TestCategory("Model")]
    public void FlipH_EmitsFlipHorizontalToken() => new Transform().FlipH().GetTransformation().Should().Be("flip-h");

    [TestMethod]
    [TestCategory("Model")]
    public void FlipBoth_EmitsVerticalThenHorizontal() => new Transform().FlipBoth().GetTransformation().Should().Be("flip-v flip-h");

    [TestMethod]
    [TestCategory("Model")]
    public void Add_NullArguments_AreIgnored() {
        var value = new Transform().Grow(2).Add(null!).GetTransformation();

        value.Should().Be("grow-2.00");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Operations_AreImmutable() {
        var original = new Transform();

        var grown = original.Grow(2);

        original.GetTransformation().Should().Be("");
        grown.GetTransformation().Should().Be("grow-2.00");
        ReferenceEquals(original, grown).Should().BeFalse();
    }
}
