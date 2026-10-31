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



        presets.Should().HaveCount(EXPECTED_COMPONENT_COUNT);



        foreach(var type in presets) {

            type.IsPublic.Should().BeTrue($"{type.FullName} should be public.");

            typeof(SvgIcon).IsAssignableFrom(type).Should().BeTrue($"{type.FullName} should derive from SvgIcon.");

            type.GetConstructor(Type.EmptyTypes).Should().NotBeNull($"{type.FullName} should expose a public parameterless constructor.");

        }

    }



    [TestMethod]

    public void AllGeneratedComponents_FollowTheStyleNamingConvention() {

        foreach(var type in PresetTypes()) {

            (type.Name.EndsWith("SolidIcon", StringComparison.Ordinal)

                || type.Name.EndsWith("RegularIcon", StringComparison.Ordinal)

                || type.Name.EndsWith("BrandsIcon", StringComparison.Ordinal))

                .Should().BeTrue($"{type.FullName} should end with a style suffix.");

        }

    }



    [TestMethod]

    public async Task HouseSolidIcon_RendersStandaloneSvgAsync() {

        var markup = await TestHost.RenderMarkupAsync<HouseSolidIcon>();



        markup.Should().Contain("<svg");

        markup.Should().Contain("viewBox=\"0 0 512 512\"");

        markup.Should().Contain("<path");

        markup.Should().Contain("fill=\"currentColor\"");

        markup.Should().Contain("aria-hidden=\"true\"");

        markup.Contains("data-fa-i2svg", StringComparison.Ordinal).Should().BeFalse();

        markup.Contains("<use", StringComparison.Ordinal).Should().BeFalse();

    }



    [TestMethod]

    public async Task NamedIcon_ExposesAnAccessibleNameAsync() {

        var markup = await TestHost.RenderMarkupAsync<StarSolidIcon>(TestHost.Parameters(

            ("Title", "Favorite")));



        markup.Should().Contain("role=\"img\"");

        markup.Should().Contain("aria-label=\"Favorite\"");

        markup.Should().Contain("<title>Favorite</title>");

        markup.Contains("aria-hidden", StringComparison.Ordinal).Should().BeFalse();

    }



    [TestMethod]

    public async Task CssClassAndUnmatchedAttributesFlowThroughAsync() {

        var markup = await TestHost.RenderMarkupAsync<GithubBrandsIcon>(TestHost.Parameters(

            ("CssClass", "custom"),

            ("width", "32")));



        markup.Should().Contain("class=\"custom\"");

        markup.Should().Contain("width=\"32\"");

    }

}

