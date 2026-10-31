//

// Copyright (c) Jeninnet.

// Part of the Jeninnet Platform.

// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/

// Licensed under the MIT License.

//

namespace Jenin.FontAwesome.Blazor.Tests.Components;



[TestClass]

public class IconThemeTests {

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task Palette_EmitsAllFourVarsAsync() {

        var markup = await TestHost.RenderMarkupAsync<IconTheme>(TestHost.Parameters(

            ("Palette", DuotonePalettes.Info),

            ("ChildContent", (RenderFragment)(builder => builder.AddContent(0, "x")))));



        markup.Should().Contain("class=\"jfa-duotone-theme\"");
        markup.Should().Contain("--duo-primary:#0d6efd;");
        markup.Should().Contain("--duo-secondary:#9ec5fe;");
        markup.Should().Contain("--duo-primary-dark:#8bb9fe;");
        markup.Should().Contain("--duo-secondary-dark:#084298;");
    }



    [TestMethod]

    [TestCategory("Rendering")]

    public async Task ExplicitPair_OverridesPaletteForBothModesAsync() {

        var markup = await TestHost.RenderMarkupAsync<IconTheme>(TestHost.Parameters(

            ("Palette", DuotonePalettes.Info),

            ("PrimaryColor", "#111111"),

            ("SecondaryColor", "#222222")));



        markup.Should().Contain("--duo-primary:#111111;");
        markup.Should().Contain("--duo-secondary:#222222;");
        markup.Should().Contain("--duo-primary-dark:#111111;");
        markup.Should().Contain("--duo-secondary-dark:#222222;");
    }



    [TestMethod]

    [TestCategory("Rendering")]

    public async Task Dark_FallsBackToLightValuesAsync() {

        var markup = await TestHost.RenderMarkupAsync<IconTheme>(TestHost.Parameters(

            ("LightPrimary", "#111111"),

            ("LightSecondary", "#222222")));



        markup.Should().Contain("--duo-primary-dark:#111111;");
        markup.Should().Contain("--duo-secondary-dark:#222222;");
    }



    [TestMethod]

    [TestCategory("Rendering")]

    public async Task EmptyTheme_RendersPlainWrapperAsync() {

        var markup = await TestHost.RenderMarkupAsync<IconTheme>(TestHost.Parameters(

            ("ChildContent", (RenderFragment)(builder => builder.AddContent(0, "x")))));



        markup.Should().Be("<span>x</span>");

    }

}

