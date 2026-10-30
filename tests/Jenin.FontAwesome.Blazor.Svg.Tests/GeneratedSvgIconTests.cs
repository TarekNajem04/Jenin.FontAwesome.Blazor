//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Svg.Tests;

[TestClass]
public sealed class GeneratedSvgIconTests {
    private const int EXPECTED_COMPONENT_COUNT = 2163;

    private static IEnumerable<Type> PresetTypes() =>
        typeof(SvgAssemblyMarker).Assembly
            .GetTypes()
            .Where(static type => type.IsSubclassOf(typeof(SvgIcon)));

    [TestMethod]
    public void AllGeneratedComponents_ArePublicSvgIconsWithParameterlessConstructors() {
        var presets = PresetTypes().ToList();

        Assert.HasCount(EXPECTED_COMPONENT_COUNT, presets);

        foreach(var type in presets) {
            Assert.IsTrue(type.IsPublic, $"{type.FullName} should be public.");
            Assert.IsTrue(typeof(SvgIcon).IsAssignableFrom(type), $"{type.FullName} should derive from SvgIcon.");
            Assert.IsNotNull(type.GetConstructor(Type.EmptyTypes), $"{type.FullName} should expose a public parameterless constructor.");
        }
    }

    [TestMethod]
    public void AllGeneratedComponents_FollowTheStyleNamingConvention() {
        foreach(var type in PresetTypes()) {
            Assert.IsTrue(
                type.Name.EndsWith("SolidIcon", StringComparison.Ordinal)
                || type.Name.EndsWith("RegularIcon", StringComparison.Ordinal)
                || type.Name.EndsWith("BrandsIcon", StringComparison.Ordinal),
                $"{type.FullName} should end with a style suffix.");
        }
    }

    [TestMethod]
    public async Task HouseSolidIcon_RendersStandaloneSvgAsync() {
        var markup = await TestHost.RenderMarkupAsync<HouseSolidIcon>();

        StringAssert.Contains(markup, "<svg");
        StringAssert.Contains(markup, "viewBox=\"0 0 512 512\"");
        StringAssert.Contains(markup, "<path");
        StringAssert.Contains(markup, "fill=\"currentColor\"");
        StringAssert.Contains(markup, "aria-hidden=\"true\"");
        Assert.IsFalse(markup.Contains("data-fa-i2svg", StringComparison.Ordinal));
        Assert.IsFalse(markup.Contains("<use", StringComparison.Ordinal));
    }

    [TestMethod]
    public async Task NamedIcon_ExposesAnAccessibleNameAsync() {
        var markup = await TestHost.RenderMarkupAsync<StarSolidIcon>(TestHost.Parameters(
            ("Title", "Favorite")));

        StringAssert.Contains(markup, "role=\"img\"");
        StringAssert.Contains(markup, "aria-label=\"Favorite\"");
        StringAssert.Contains(markup, "<title>Favorite</title>");
        Assert.IsFalse(markup.Contains("aria-hidden", StringComparison.Ordinal));
    }

    [TestMethod]
    public async Task CssClassAndUnmatchedAttributesFlowThroughAsync() {
        var markup = await TestHost.RenderMarkupAsync<GithubBrandsIcon>(TestHost.Parameters(
            ("CssClass", "custom"),
            ("width", "32")));

        StringAssert.Contains(markup, "class=\"custom\"");
        StringAssert.Contains(markup, "width=\"32\"");
    }
}
