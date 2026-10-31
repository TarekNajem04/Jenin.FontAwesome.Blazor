//

// Copyright (c) Jeninnet.

// Part of the Jeninnet Platform.

// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/

// Licensed under the MIT License.

//

namespace Jenin.FontAwesome.Blazor.Tests.Components;



[TestClass]

public class SpriteIconTests {

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task RendersUseReferenceWithDefaultSpriteAsync() {

        var markup = await TestHost.RenderMarkupAsync<SpriteIcon>(TestHost.Parameters(

            ("IconName", "fa-house"),

            ("IconStyle", IconStyle.Solid)));



        markup.Should().Be(
            "<svg viewBox=\"0 0 512 512\" height=\"1em\" style=\"vertical-align:-0.125em;\" aria-hidden=\"true\">" +

            "<use href=\"/fa-sprite.svg#fa-solid-house\"></use></svg>"

        );

    }



    [TestMethod]

    [TestCategory("Rendering")]

    public async Task BrandsStyle_SelectsBrandsSymbolAsync() {

        var markup = await TestHost.RenderMarkupAsync<SpriteIcon>(TestHost.Parameters(

            ("IconName", "fa-github"),

            ("IconStyle", IconStyle.Brands),

            ("Sprite", "/assets/icons.svg")));



        markup.Should().Contain("href=\"/assets/icons.svg#fa-brands-github\"");
    }



    [TestMethod]

    [TestCategory("Rendering")]

    public async Task NamedIcon_ExposesAccessibleNameAsync() {

        var markup = await TestHost.RenderMarkupAsync<SpriteIcon>(TestHost.Parameters(

            ("IconName", "fa-house"),

            ("Title", "Home")));



        markup.Should().Contain("role=\"img\"");
        markup.Should().Contain("aria-label=\"Home\"");
        markup.Should().Contain("<title>Home</title>");
        markup.Contains("aria-hidden", StringComparison.Ordinal).Should().BeFalse();

    }

}

