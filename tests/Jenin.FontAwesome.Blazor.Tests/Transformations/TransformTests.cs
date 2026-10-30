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
    public void GetTransformation_Default_IsEmpty() => Assert.AreEqual("", new Transform().GetTransformation());

    [TestMethod]
    [TestCategory("Model")]
    public void GetTransformation_PreservesInsertionOrder() {
        var value = new Transform().Shrink(8).Up(6).Rotate(-30).FlipV().GetTransformation();

        Assert.AreEqual("shrink-8.00 up-6.00 rotate--30.00 flip-v", value);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Grow_EmitsGrowToken() => Assert.AreEqual("grow-2.00", new Transform().Grow(2).GetTransformation());

    [TestMethod]
    [TestCategory("Model")]
    public void Shrink_EmitsShrinkToken() => Assert.AreEqual("shrink-4.00", new Transform().Shrink(4).GetTransformation());

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

        Assert.AreEqual(expected, value.GetTransformation());
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Rotate_AllowsNegativeAndUnboundedValues() {
        Assert.AreEqual("rotate--90.00", new Transform().Rotate(-90).GetTransformation());
        Assert.AreEqual("rotate-720.00", new Transform().Rotate(720).GetTransformation());
    }

    [TestMethod]
    [TestCategory("Model")]
    public void FlipV_EmitsFlipVerticalToken() => Assert.AreEqual("flip-v", new Transform().FlipV().GetTransformation());

    [TestMethod]
    [TestCategory("Model")]
    public void FlipH_EmitsFlipHorizontalToken() => Assert.AreEqual("flip-h", new Transform().FlipH().GetTransformation());

    [TestMethod]
    [TestCategory("Model")]
    public void FlipBoth_EmitsVerticalThenHorizontal() => Assert.AreEqual("flip-v flip-h", new Transform().FlipBoth().GetTransformation());

    [TestMethod]
    [TestCategory("Model")]
    public void Add_NullArguments_AreIgnored() {
        var value = new Transform().Grow(2).Add(null!).GetTransformation();

        Assert.AreEqual("grow-2.00", value);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Operations_AreImmutable() {
        var original = new Transform();

        var grown = original.Grow(2);

        Assert.AreEqual("", original.GetTransformation());
        Assert.AreEqual("grow-2.00", grown.GetTransformation());
        Assert.AreNotSame(original, grown);
    }
}
