//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Tests;

[TestClass]
public sealed class SnippetGeneratorTests {
    private static ExplorerIconEntry House => ExplorerCatalogue.Default.Find("house")
        ?? throw new InvalidOperationException("The 'house' icon is missing from the catalogue.");

    private static ExplorerIconEntry Github => ExplorerCatalogue.Default.Find("github")
        ?? throw new InvalidOperationException("The 'github' icon is missing from the catalogue.");

    private static Snippet Generate(ExplorerIconEntry entry, ExplorerPreviewState state, SnippetKind kind = SnippetKind.Razor)
        => SnippetGenerator.Generate(entry, state, kind, PreviewIconFactory.BuildAnimation(state), PreviewIconFactory.BuildTransform(state));

    [TestMethod]
    public void Razor_Emits_IconName_Reference_And_Style() {
        var snippet = Generate(House, new ExplorerPreviewState());

        snippet.Language.Should().Be("razor");
        snippet.Code.Should().Contain("IconName=\"@(IconNames.Solid.House)\"");
        snippet.Code.Should().Contain("IconStyle=\"IconStyle.Solid\"");
        snippet.Code.StartsWith("<Icon", StringComparison.Ordinal).Should().BeTrue();
        snippet.Code.EndsWith("/>", StringComparison.Ordinal).Should().BeTrue();
    }

    [TestMethod]
    public void Razor_Omits_Default_Parameters() {
        var snippet = Generate(House, new ExplorerPreviewState());

        snippet.Code.Contains("Typeface", StringComparison.Ordinal).Should().BeFalse();
        snippet.Code.Contains("IconSize", StringComparison.Ordinal).Should().BeFalse();
        snippet.Code.Contains("WidthMode", StringComparison.Ordinal).Should().BeFalse();
        snippet.Code.Contains("FixedWidth", StringComparison.Ordinal).Should().BeFalse();
        snippet.Code.Contains("Color=", StringComparison.Ordinal).Should().BeFalse();
        snippet.Code.Contains("Rotate", StringComparison.Ordinal).Should().BeFalse();
        snippet.Code.Contains("Animation", StringComparison.Ordinal).Should().BeFalse();
        snippet.Code.Contains("Transform", StringComparison.Ordinal).Should().BeFalse();
    }

    [TestMethod]
    public void Razor_Uses_Selected_Values() {
        var state = new ExplorerPreviewState {
            Size = IconSize.Large,
            WidthMode = IconWidth.Fixed,
            FixedWidth = true,
            Bordered = true,
            Inverse = true,
            Rotate = IconRotate.Deg090,
        };

        var snippet = Generate(House, state);

        snippet.Code.Should().Contain("IconSize=\"IconSize.Large\"");
        snippet.Code.Should().Contain("WidthMode=\"IconWidth.Fixed\"");
        snippet.Code.Should().Contain("FixedWidth");
        snippet.Code.Should().Contain("Bordered");
        snippet.Code.Should().Contain("Inverse");
        snippet.Code.Should().Contain("Rotate=\"IconRotate.Deg090\"");
        snippet.Code.Contains("RotateAngle", StringComparison.Ordinal).Should().BeFalse();
    }

    [TestMethod]
    public void Razor_Emits_Rotate_Angle_As_Suffixed_Float() {
        var state = new ExplorerPreviewState {
            Rotate = IconRotate.RotateBy,
            RotateAngle = -30f,
        };

        var snippet = Generate(House, state);

        snippet.Code.Should().Contain("Rotate=\"IconRotate.RotateBy\"");
        snippet.Code.Should().Contain("RotateAngle=\"-30f\"");
    }

    [TestMethod]
    public void Razor_Emits_Transform_Expression() {
        var state = new ExplorerPreviewState {
            UseTransform = true,
            TransformGrow = false,
            TransformScale = 6f,
            TransformX = -2f,
            TransformRotate = -15f,
        };

        var snippet = Generate(House, state);

        snippet.Code.Should().Contain("Transform=");
        snippet.Code.Should().Contain("new Transform()");
        snippet.Code.Should().Contain(".Shrink(6)");
        snippet.Code.Should().Contain(".Left(2)");
        snippet.Code.Should().Contain(".Rotate(-15)");
    }

    [TestMethod]
    public void Razor_Quotes_Animation_Expression_With_Single_Quotes_When_Value_Contains_Literals() {
        var state = new ExplorerPreviewState {
            Animation = PreviewAnimation.Spin,
            AnimationTimingFunction = "ease-in-out",
        };

        var snippet = Generate(House, state);

        // The animation expression contains string literals, so the attribute must use single quotes.
        snippet.Code.Should().Contain("Animation='Animate.Spin(new SpinOptions { TimingFunction = \"ease-in-out\" })'");
    }

    [TestMethod]
    public void Razor_Emits_Parameterless_Animation_When_Defaults() {
        var state = new ExplorerPreviewState { Animation = PreviewAnimation.Beat };

        var snippet = Generate(House, state);

        snippet.Code.Should().Contain("Animation=\"Animate.Beat()\"");
    }

    [TestMethod]
    public void Preset_Uses_Group_Component_And_Omits_Defaults() {
        var snippet = Generate(House, new ExplorerPreviewState(), SnippetKind.Preset);

        snippet.Language.Should().Be("razor");
        snippet.Code.StartsWith("<HouseIcon", StringComparison.Ordinal).Should().BeTrue();
        snippet.Code.Should().Contain("IconStyle=\"IconStyle.Solid\"");
    }

    [TestMethod]
    public void Preset_Generates_Brands_Component() {
        var snippet = Generate(Github, new ExplorerPreviewState { Style = IconStyle.Brands }, SnippetKind.Preset);

        snippet.Code.StartsWith("<GithubIcon", StringComparison.Ordinal).Should().BeTrue();
        snippet.Code.Should().Contain("IconStyle=\"IconStyle.Brands\"");
    }

    [TestMethod]
    public void IconNamesReference_Uses_Selected_Style_Group() {
        IconNamesReference.Build("fa-house", IconStyle.Solid, [IconStyle.Solid, IconStyle.Regular]).Should().Be("IconNames.Solid.House");
        IconNamesReference.Build("fa-house", IconStyle.Regular, [IconStyle.Solid, IconStyle.Regular]).Should().Be("IconNames.Regular.House");
        IconNamesReference.Build("fa-github", IconStyle.Brands, [IconStyle.Brands]).Should().Be("IconNames.Brands.Github");
    }

    [TestMethod]
    public void IconNamesReference_Falls_Back_To_First_Style_When_Selection_Unavailable() {
        // A Pro-only selection falls back to the first Free style the icon is available in.
        var reference = IconNamesReference.Build("fa-house", IconStyle.Light, [IconStyle.Solid, IconStyle.Regular]);

        reference.Should().Be("IconNames.Solid.House");
    }

    [TestMethod]
    public void IconNamesReference_Returns_Null_When_No_Free_Group_Applies() =>
        // An empty style list defaults to Solid; a Pro-only first style yields no group.
        IconNamesReference.Build("fa-house", IconStyle.Light, [IconStyle.Light]).Should().BeNull();

    [TestMethod]
    public void IconNamesReference_Prefixes_Leading_Digit() => IconNamesReference.Build("fa-0", IconStyle.Solid, [IconStyle.Solid]).Should().Be("IconNames.Solid._0");

    [TestMethod]
    public void Html_Emits_Class_Tokens_And_Styles() {
        var state = new ExplorerPreviewState {
            UseColor = true,
            Color = "#0d6efd",
            Rotate = IconRotate.RotateBy,
            RotateAngle = -30f,
        };

        var snippet = Generate(House, state, SnippetKind.Html);

        snippet.Language.Should().Be("html");
        snippet.Code.Should().Contain("class=\"fa-house fa-solid fa-rotate-by\"");
        snippet.Code.Should().Contain("color:#0d6efd");
        snippet.Code.Should().Contain("--fa-rotate-angle:-30deg");
    }

    [TestMethod]
    public void Html_Emits_Transform_Token() {
        var state = new ExplorerPreviewState { UseTransform = true, TransformGrow = true, TransformScale = 8f };

        var snippet = Generate(House, state, SnippetKind.Html);

        snippet.Code.Should().Contain("data-fa-transform=\"");
    }

    [TestMethod]
    public void Html_Uses_Duotone_Custom_Properties() {
        var state = new ExplorerPreviewState {
            UseDuotone = true,
            DuotonePrimary = "#0d6efd",
            DuotoneSecondary = "#fd7e14",
        };

        var snippet = Generate(House, state, SnippetKind.Html);

        snippet.Code.Should().Contain("--fa-primary-color:#0d6efd");
        snippet.Code.Should().Contain("--fa-secondary-color:#fd7e14");
    }
}
