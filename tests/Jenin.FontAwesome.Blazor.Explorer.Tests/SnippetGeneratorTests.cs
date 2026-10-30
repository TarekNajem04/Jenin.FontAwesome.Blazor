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

        Assert.AreEqual("razor", snippet.Language);
        Assert.Contains("IconName=\"@(IconNames.Solid.House)\"", snippet.Code);
        Assert.Contains("IconStyle=\"IconStyle.Solid\"", snippet.Code);
        Assert.StartsWith("<Icon", snippet.Code, StringComparison.Ordinal);
        Assert.EndsWith("/>", snippet.Code, StringComparison.Ordinal);
    }

    [TestMethod]
    public void Razor_Omits_Default_Parameters() {
        var snippet = Generate(House, new ExplorerPreviewState());

        Assert.DoesNotContain("Typeface", snippet.Code, StringComparison.Ordinal);
        Assert.DoesNotContain("IconSize", snippet.Code, StringComparison.Ordinal);
        Assert.DoesNotContain("WidthMode", snippet.Code, StringComparison.Ordinal);
        Assert.DoesNotContain("FixedWidth", snippet.Code, StringComparison.Ordinal);
        Assert.DoesNotContain("Color=", snippet.Code, StringComparison.Ordinal);
        Assert.DoesNotContain("Rotate", snippet.Code, StringComparison.Ordinal);
        Assert.DoesNotContain("Animation", snippet.Code, StringComparison.Ordinal);
        Assert.DoesNotContain("Transform", snippet.Code, StringComparison.Ordinal);
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

        Assert.Contains("IconSize=\"IconSize.Large\"", snippet.Code);
        Assert.Contains("WidthMode=\"IconWidth.Fixed\"", snippet.Code);
        Assert.Contains("FixedWidth", snippet.Code);
        Assert.Contains("Bordered", snippet.Code);
        Assert.Contains("Inverse", snippet.Code);
        Assert.Contains("Rotate=\"IconRotate.Deg090\"", snippet.Code);
        Assert.DoesNotContain("RotateAngle", snippet.Code, StringComparison.Ordinal);
    }

    [TestMethod]
    public void Razor_Emits_Rotate_Angle_As_Suffixed_Float() {
        var state = new ExplorerPreviewState {
            Rotate = IconRotate.RotateBy,
            RotateAngle = -30f,
        };

        var snippet = Generate(House, state);

        Assert.Contains("Rotate=\"IconRotate.RotateBy\"", snippet.Code);
        Assert.Contains("RotateAngle=\"-30f\"", snippet.Code);
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

        Assert.Contains("Transform=", snippet.Code);
        Assert.Contains("new Transform()", snippet.Code);
        Assert.Contains(".Shrink(6)", snippet.Code);
        Assert.Contains(".Left(2)", snippet.Code);
        Assert.Contains(".Rotate(-15)", snippet.Code);
    }

    [TestMethod]
    public void Razor_Quotes_Animation_Expression_With_Single_Quotes_When_Value_Contains_Literals() {
        var state = new ExplorerPreviewState {
            Animation = PreviewAnimation.Spin,
            AnimationTimingFunction = "ease-in-out",
        };

        var snippet = Generate(House, state);

        // The animation expression contains string literals, so the attribute must use single quotes.
        Assert.Contains("Animation='Animate.Spin(new SpinOptions { TimingFunction = \"ease-in-out\" })'", snippet.Code);
    }

    [TestMethod]
    public void Razor_Emits_Parameterless_Animation_When_Defaults() {
        var state = new ExplorerPreviewState { Animation = PreviewAnimation.Beat };

        var snippet = Generate(House, state);

        Assert.Contains("Animation=\"Animate.Beat()\"", snippet.Code);
    }

    [TestMethod]
    public void Preset_Uses_Group_Component_And_Omits_Defaults() {
        var snippet = Generate(House, new ExplorerPreviewState(), SnippetKind.Preset);

        Assert.AreEqual("razor", snippet.Language);
        Assert.StartsWith("<HouseIcon", snippet.Code, StringComparison.Ordinal);
        Assert.Contains("IconStyle=\"IconStyle.Solid\"", snippet.Code);
    }

    [TestMethod]
    public void Preset_Generates_Brands_Component() {
        var snippet = Generate(Github, new ExplorerPreviewState { Style = IconStyle.Brands }, SnippetKind.Preset);

        Assert.StartsWith("<GithubIcon", snippet.Code, StringComparison.Ordinal);
        Assert.Contains("IconStyle=\"IconStyle.Brands\"", snippet.Code);
    }

    [TestMethod]
    public void IconNamesReference_Uses_Selected_Style_Group() {
        Assert.AreEqual("IconNames.Solid.House", IconNamesReference.Build("fa-house", IconStyle.Solid, [IconStyle.Solid, IconStyle.Regular]));
        Assert.AreEqual("IconNames.Regular.House", IconNamesReference.Build("fa-house", IconStyle.Regular, [IconStyle.Solid, IconStyle.Regular]));
        Assert.AreEqual("IconNames.Brands.Github", IconNamesReference.Build("fa-github", IconStyle.Brands, [IconStyle.Brands]));
    }

    [TestMethod]
    public void IconNamesReference_Falls_Back_To_First_Style_When_Selection_Unavailable() {
        // A Pro-only selection falls back to the first Free style the icon is available in.
        var reference = IconNamesReference.Build("fa-house", IconStyle.Light, [IconStyle.Solid, IconStyle.Regular]);

        Assert.AreEqual("IconNames.Solid.House", reference);
    }

    [TestMethod]
    public void IconNamesReference_Returns_Null_When_No_Free_Group_Applies() =>
        // An empty style list defaults to Solid; a Pro-only first style yields no group.
        Assert.IsNull(IconNamesReference.Build("fa-house", IconStyle.Light, [IconStyle.Light]));

    [TestMethod]
    public void IconNamesReference_Prefixes_Leading_Digit() => Assert.AreEqual("IconNames.Solid._0", IconNamesReference.Build("fa-0", IconStyle.Solid, [IconStyle.Solid]));

    [TestMethod]
    public void Html_Emits_Class_Tokens_And_Styles() {
        var state = new ExplorerPreviewState {
            UseColor = true,
            Color = "#0d6efd",
            Rotate = IconRotate.RotateBy,
            RotateAngle = -30f,
        };

        var snippet = Generate(House, state, SnippetKind.Html);

        Assert.AreEqual("html", snippet.Language);
        Assert.Contains("class=\"fa-house fa-solid fa-rotate-by\"", snippet.Code);
        Assert.Contains("color:#0d6efd", snippet.Code);
        Assert.Contains("--fa-rotate-angle:-30deg", snippet.Code);
    }

    [TestMethod]
    public void Html_Emits_Transform_Token() {
        var state = new ExplorerPreviewState { UseTransform = true, TransformGrow = true, TransformScale = 8f };

        var snippet = Generate(House, state, SnippetKind.Html);

        Assert.Contains("data-fa-transform=\"", snippet.Code);
    }

    [TestMethod]
    public void Html_Uses_Duotone_Custom_Properties() {
        var state = new ExplorerPreviewState {
            UseDuotone = true,
            DuotonePrimary = "#0d6efd",
            DuotoneSecondary = "#fd7e14",
        };

        var snippet = Generate(House, state, SnippetKind.Html);

        Assert.Contains("--fa-primary-color:#0d6efd", snippet.Code);
        Assert.Contains("--fa-secondary-color:#fd7e14", snippet.Code);
    }
}
