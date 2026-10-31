//

// Copyright (c) Jeninnet.

// Part of the Jeninnet Platform.

// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/

// Licensed under the MIT License.

//

namespace Jenin.FontAwesome.Blazor.Tests.Components;



[TestClass]

public class IconTransitionTests {

    [TestMethod]

    [TestCategory("Rendering")]

    public async Task RendersTwoStackedLayersWithFirstVisibleAsync() {

        var markup = await TestHost.RenderMarkupAsync<IconTransition>(TestHost.Parameters(

            ("ChildContent", (RenderFragment)(builder => builder.AddContent(0, "First"))),

            ("AlternateContent", (RenderFragment)(builder => builder.AddContent(0, "Second")))));



        markup.Should().Contain("class=\"jfa-anim-transition\"");
        CountOccurrences(markup, "jfa-anim-transition-layer").Should().Be(2);

        markup.Should().Contain("opacity:1;");
        markup.Should().Contain("opacity:0;");
        markup.Should().Contain(">First<");
        markup.Should().Contain(">Second<");
    }



    [TestMethod]

    [TestCategory("Rendering")]

    public async Task ReRender_SwapsVisibleLayerAsync() {

        var markup = await TestHost.RenderMarkupSequenceAsync<IconTransition>(

            TestHost.Parameters(("ShowAlternate", false)),

            TestHost.Parameters(("ShowAlternate", true)));



        markup.Should().Contain("class=\"jfa-anim-transition-layer\" style=\"opacity:0;transform:none;visibility:hidden;\">");
        markup.Should().Contain("class=\"jfa-anim-transition-layer\" style=\"opacity:1;transform:none;visibility:visible;\">");
    }



    [TestMethod]

    [TestCategory("Rendering")]

    [DataRow(IconTransitionKind.Crossfade, "none")]

    [DataRow(IconTransitionKind.Scale, "scale(0.2)")]

    [DataRow(IconTransitionKind.Flip, "rotateY(-89deg)")]

    [DataRow(IconTransitionKind.Slide, "translateX(35%)")]

    public async Task HiddenLayer_InlinesKindTransformAsync(IconTransitionKind kind, string transform) {

        var markup = await TestHost.RenderMarkupAsync<IconTransition>(TestHost.Parameters(

            ("Transition", kind)));



        markup.Should().Contain($"transform:{transform};");
        markup.Should().Contain("visibility:hidden;");
    }



    [TestMethod]

    [TestCategory("Rendering")]

    public async Task Duration_EmitsCustomPropertyAsync() {

        var markup = await TestHost.RenderMarkupAsync<IconTransition>(TestHost.Parameters(

            ("Duration", TimeSpan.FromMilliseconds(350))));



        markup.Should().Contain("--jfa-transition-duration:350ms;");
    }



    [TestMethod]

    [TestCategory("Rendering")]

    public async Task TwoStateIcon_RendersBothGlyphsAsync() {

        var markup = await TestHost.RenderMarkupAsync<TwoStateIcon>(TestHost.Parameters(

            ("IconName", "fa-play"),

            ("AlternateIconName", "fa-pause"),

            ("ShowAlternate", true)));



        markup.Should().Contain("fa-play");
        markup.Should().Contain("fa-pause");
        markup.Should().Contain("jfa-anim-transition");
    }



    private static int CountOccurrences(string source, string token) {

        var count = 0;

        var index = 0;



        while((index = source.IndexOf(token, index, StringComparison.Ordinal)) >= 0) {

            count++;

            index += token.Length;

        }



        return count;

    }

}

