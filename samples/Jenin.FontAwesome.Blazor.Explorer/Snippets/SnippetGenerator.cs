//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Explorer.Snippets;

/// <summary>
/// The kinds of code snippet the explorer can generate for the current icon and live controls.
/// </summary>
public enum SnippetKind {
    /// <summary>A Razor <c>&lt;Icon /&gt;</c> element.</summary>
    Razor,

    /// <summary>A Razor preset component such as <c>&lt;HouseIcon /&gt;</c>.</summary>
    Preset,

    /// <summary>Plain HTML using the Font Awesome web font classes.</summary>
    Html,
}

/// <summary>
/// A generated snippet together with the language identifier used for display.
/// </summary>
/// <param name="Kind">The snippet kind.</param>
/// <param name="Language">The display language (for example <c>razor</c> or <c>html</c>).</param>
/// <param name="Code">The snippet source.</param>
public sealed record Snippet(SnippetKind Kind, string Language, string Code);

/// <summary>
/// Produces copyable Razor, preset-component, and HTML snippets for the selected icon and the current
/// live-control values, omitting every parameter left at its default.
/// </summary>
/// <remarks>
/// The generator mirrors the library's rendering rules so the emitted markup compiles and matches the
/// preview: presentation enums contribute their Font Awesome class tokens, animation classes and CSS
/// custom properties come from the public <see cref="Animation"/> serialization, and the power
/// transform is emitted as the <c>data-fa-transform</c> token via <see cref="Transform.GetTransformation"/>.
/// </remarks>
internal static class SnippetGenerator {
    /// <summary>
    /// Generates a snippet of the requested <paramref name="kind"/>.
    /// </summary>
    /// <param name="entry">The selected catalogue entry.</param>
    /// <param name="state">The current live-control values.</param>
    /// <param name="kind">The snippet kind to generate.</param>
    /// <param name="animation">The animation built from <paramref name="state"/>, or <c>null</c>.</param>
    /// <param name="transform">The transform built from <paramref name="state"/>, or <c>null</c>.</param>
    /// <returns>The generated snippet.</returns>
    public static Snippet Generate(
        ExplorerIconEntry entry,
        ExplorerPreviewState state,
        SnippetKind kind,
        Animation? animation,
        Transform? transform) {
        ArgumentNullException.ThrowIfNull(entry);
        ArgumentNullException.ThrowIfNull(state);

        return kind switch {
            SnippetKind.Preset => new Snippet(kind, "razor", BuildPreset(entry, state, animation, transform)),
            SnippetKind.Html => new Snippet(kind, "html", BuildHtml(entry, state, animation, transform)),
            _ => new Snippet(SnippetKind.Razor, "razor", BuildRazor(entry, state, animation, transform)),
        };
    }

    private static string BuildRazor(
        ExplorerIconEntry entry,
        ExplorerPreviewState state,
        Animation? animation,
        Transform? transform) {
        var attributes = new List<string>();

        var iconNameReference = IconNamesReference.Build(entry.CssClass, state.Style, entry.Styles);
        attributes.Add(iconNameReference is null
            ? $"IconName=\"{entry.CssClass}\""
            : $"IconName=\"@({iconNameReference})\"");

        AddCommonAttributes(attributes, state, animation, transform);
        return $"<Icon {string.Join(Environment.NewLine + "      ", attributes)} />";
    }

    private static string BuildPreset(
        ExplorerIconEntry entry,
        ExplorerPreviewState state,
        Animation? animation,
        Transform? transform) {
        var component = IconNamesReference.Build(entry.CssClass, state.Style, entry.Styles) is { } reference
            ? reference[(reference.LastIndexOf('.') + 1)..] + "Icon"
            : null;

        var attributes = new List<string>();
        AddCommonAttributes(attributes, state, animation, transform);

        return component is null
            ? $"@* No preset component is generated for {entry.CssClass}. *@\n" + BuildRazor(entry, state, animation, transform)
            : attributes.Count == 0
                ? $"<{component} />"
                : $"<{component} {string.Join(Environment.NewLine + "      ", attributes)} />";
    }

    private static void AddCommonAttributes(
        List<string> attributes,
        ExplorerPreviewState state,
        Animation? animation,
        Transform? transform) {
        if(state.Typeface != Typeface.Classic) {
            attributes.Add($"Typeface=\"Typeface.{state.Typeface}\"");
        }

        attributes.Add($"IconStyle=\"IconStyle.{state.Style}\"");

        if(state.Size != IconSize.NotSet) {
            attributes.Add($"IconSize=\"IconSize.{state.Size}\"");
        }

        if(state.WidthMode != IconWidth.NotSet) {
            attributes.Add($"WidthMode=\"IconWidth.{state.WidthMode}\"");
        }

        if(state.FixedWidth) {
            attributes.Add("FixedWidth");
        }

        if(state.UseColor) {
            attributes.Add($"Color=\"{state.Color}\"");
        }

        if(state.UseBackgroundColor) {
            attributes.Add($"BackgroundColor=\"{state.BackgroundColor}\"");
        }

        if(state.Rotate != IconRotate.NotSet) {
            attributes.Add($"Rotate=\"IconRotate.{state.Rotate}\"");
        }

        if(state.Rotate == IconRotate.RotateBy) {
            attributes.Add(Attr("RotateAngle", FormatFloatLiteral(state.RotateAngle)));
        }

        if(state.CombiningRotate != IconRotate.NotSet) {
            attributes.Add($"CombiningRotate=\"IconRotate.{state.CombiningRotate}\"");
        }

        if(state.PullPosition != PullPosition.None) {
            attributes.Add($"PullPosition=\"PullPosition.{state.PullPosition}\"");
        }

        if(state.Bordered) {
            attributes.Add("Bordered");
        }

        if(state.Inverse) {
            attributes.Add("Inverse");
        }

        if(state.SwapOpacity) {
            attributes.Add("SwapOpacity");
        }

        if(state.UseDuotone) {
            attributes.Add($"PrimaryColor=\"{state.DuotonePrimary}\"");
            attributes.Add($"SecondaryColor=\"{state.DuotoneSecondary}\"");
        }

        if(state.UseDuotoneOpacity) {
            attributes.Add(Attr("PrimaryOpacity", FormatFloatLiteral(state.DuotonePrimaryOpacity)));
            attributes.Add(Attr("SecondaryOpacity", FormatFloatLiteral(state.DuotoneSecondaryOpacity)));
        }

        if(state.UseMask) {
            attributes.Add($"MaskIconName=\"{state.MaskIconName}\"");
            attributes.Add($"MaskIconStyle=\"IconStyle.{state.MaskIconStyle}\"");
        }

        if(animation is not null) {
            attributes.Add(Attr("Animation", BuildAnimationExpression(animation)));
        }

        if(transform is not null) {
            attributes.Add(Attr("Transform", "@(" + BuildTransformExpression(state) + ")"));
        }
    }

    private static string BuildAnimationExpression(Animation animation) {
        var name = animation.GetType().Name;
        var factory = name.EndsWith("Animation", StringComparison.Ordinal) ? name[..^"Animation".Length] : name;

        var options = animation.Options;
        var argumentList = new List<string>();

        if(options.Duration.HasValue) {
            argumentList.Add($"Duration = TimeSpan.FromSeconds({FormatFloat((float)options.Duration.Value.TotalSeconds)})");
        }

        if(options.Delay.HasValue) {
            argumentList.Add($"Delay = TimeSpan.FromSeconds({FormatFloat((float)options.Delay.Value.TotalSeconds)})");
        }

        if(options.IterationCount.HasValue) {
            argumentList.Add($"IterationCount = {FormatFloat(options.IterationCount.Value)}");
        }

        if(options.Direction != AnimationDirection.None) {
            argumentList.Add($"Direction = AnimationDirection.{options.Direction}");
        }

        if(!string.IsNullOrWhiteSpace(options.TimingFunction)) {
            argumentList.Add($"TimingFunction = \"{options.TimingFunction}\"");
        }

        var optionsType = animation.Options.GetType().Name;
        return argumentList.Count == 0
            ? $"Animate.{factory}()"
            : $"Animate.{factory}(new {optionsType} {{ {string.Join(", ", argumentList)} }})";
    }

    private static string BuildTransformExpression(ExplorerPreviewState state) {
        var builder = new StringBuilder("new Transform()");
        _ = builder.Append(state.TransformGrow
            ? $".Grow({FormatFloat(state.TransformScale)})"
            : $".Shrink({FormatFloat(state.TransformScale)})");

        if(state.TransformX > 0) {
            _ = builder.Append(CultureInfo.InvariantCulture, $".Right({FormatFloat(state.TransformX)})");
        } else if(state.TransformX < 0) {
            _ = builder.Append(CultureInfo.InvariantCulture, $".Left({FormatFloat(-state.TransformX)})");
        }

        if(state.TransformY > 0) {
            _ = builder.Append(CultureInfo.InvariantCulture, $".Down({FormatFloat(state.TransformY)})");
        } else if(state.TransformY < 0) {
            _ = builder.Append(CultureInfo.InvariantCulture, $".Up({FormatFloat(-state.TransformY)})");
        }

        if(state.TransformRotate != 0) {
            _ = builder.Append(CultureInfo.InvariantCulture, $".Rotate({FormatFloat(state.TransformRotate)})");
        }

        if(state.TransformFlipHorizontal && state.TransformFlipVertical) {
            _ = builder.Append(".FlipBoth()");
        } else if(state.TransformFlipHorizontal) {
            _ = builder.Append(".FlipH()");
        } else if(state.TransformFlipVertical) {
            _ = builder.Append(".FlipV()");
        }

        return builder.ToString();
    }

    private static string BuildHtml(
        ExplorerIconEntry entry,
        ExplorerPreviewState state,
        Animation? animation,
        Transform? transform) {
        var classes = new List<string> { "fa-" + entry.Name };

        AddClass(classes, FontAwesomeClassTokens.For(state.Typeface));
        AddClass(classes, FontAwesomeClassTokens.For(state.Style));
        AddClass(classes, FontAwesomeClassTokens.For(state.Size));
        AddClass(classes, FontAwesomeClassTokens.For(state.WidthMode));
        AddClass(classes, FontAwesomeClassTokens.For(state.Rotate));
        AddClass(classes, FontAwesomeClassTokens.For(state.CombiningRotate));
        AddClass(classes, FontAwesomeClassTokens.For(state.PullPosition));

        if(state.FixedWidth) {
            classes.Add("fa-fw");
        }

        if(state.Bordered) {
            classes.Add("fa-border");
        }

        if(state.Inverse) {
            classes.Add("fa-inverse");
        }

        if(state.SwapOpacity) {
            classes.Add("fa-swap-opacity");
        }

        if(animation is not null) {
            AddClass(classes, animation.BuildClasses());
        }

        var attributes = new List<string> { "style=\"" + BuildHtmlStyles(state, animation) + "\"" };
        if(transform is not null) {
            attributes.Add($"data-fa-transform=\"{transform.GetTransformation()}\"");
        }

        return $"<i class=\"{string.Join(' ', classes)}\" {string.Join(' ', attributes)}></i>";
    }

    private static string BuildHtmlStyles(ExplorerPreviewState state, Animation? animation) {
        var styles = new List<string>();

        if(state.UseColor) {
            styles.Add($"color:{state.Color}");
        }

        if(state.UseBackgroundColor) {
            styles.Add($"background-color:{state.BackgroundColor}");
        }

        if(state.Rotate == IconRotate.RotateBy) {
            styles.Add($"--fa-rotate-angle:{FormatFloat(state.RotateAngle)}deg");
        }

        if(state.UseDuotone) {
            styles.Add($"--fa-primary-color:{state.DuotonePrimary}");
            styles.Add($"--fa-secondary-color:{state.DuotoneSecondary}");
        }

        if(state.UseDuotoneOpacity) {
            styles.Add($"--fa-primary-opacity:{FormatFloat(state.DuotonePrimaryOpacity)}");
            styles.Add($"--fa-secondary-opacity:{FormatFloat(state.DuotoneSecondaryOpacity)}");
        }

        if(animation is not null) {
            foreach(var (key, value) in animation.BuildStyles()) {
                styles.Add($"{key}:{value}");
            }
        }

        return string.Join(';', styles);
    }

    private static void AddClass(List<string> classes, string? token) {
        if(!string.IsNullOrEmpty(token)) {
            classes.Add(token);
        }
    }

    private static string FormatFloat(float value)
        => value.ToString("0.####", CultureInfo.InvariantCulture);

    private static string FormatFloatLiteral(float value) => FormatFloat(value) + "f";

    // Chooses the quote character so that a value containing one kind of quote stays a single,
    // well-formed Razor attribute (for example an animation expression containing string literals).
    private static string Attr(string name, string value) {
        var quote = value.Contains('"', StringComparison.Ordinal) ? '\'' : '"';
        return $"{name}={quote}{value}{quote}";
    }
}
