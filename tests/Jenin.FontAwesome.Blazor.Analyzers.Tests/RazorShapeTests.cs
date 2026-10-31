//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Analyzers.Tests;

/// <summary>
/// Razor-generated shape: the source generator emits
/// <c>AddComponentParameter(seq, nameof(...Icon.IconName), "...")</c>. The harness feeds the
/// exact generated shape as C# (it cannot run the Razor generator itself).
/// </summary>
[TestClass]
public class RazorShapeTests {
    [TestMethod]
    public async Task GeneratedAddComponentParameter_UnknownName_ReportsJFA1001Async() {
        const string source = """
            using Jenin.FontAwesome.Blazor.Components;
            using Microsoft.AspNetCore.Components.Rendering;

            public class Generated {
                public void Build(RenderTreeBuilder builder) {
                    builder.OpenComponent<Icon>(0);
                    builder.AddComponentParameter(1, nameof(global::Jenin.FontAwesome.Blazor.Components.Icon.IconName), "fa-huose");
                    builder.CloseComponent();
                }
            }
            """;

        var expected = new DiagnosticResult(JeninFontAwesomeAnalyzer.UNKNOWN_ICON_NAME_ID, DiagnosticSeverity.Warning)
            .WithSpan(7, 109, 7, 119)
            .WithArguments("fa-huose");

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task GeneratedAddComponentParameter_ClassString_ReportsJFA1002Async() {
        const string source = """
            using Jenin.FontAwesome.Blazor.Components;
            using Microsoft.AspNetCore.Components.Rendering;

            public class Generated {
                public void Build(RenderTreeBuilder builder) {
                    builder.OpenComponent<Icon>(0);
                    builder.AddComponentParameter(1, nameof(global::Jenin.FontAwesome.Blazor.Components.Icon.MaskIconName), "fa-solid fa-circle");
                    builder.CloseComponent();
                }
            }
            """;

        var expected = new DiagnosticResult(JeninFontAwesomeAnalyzer.RAW_CLASS_STRING_ID, DiagnosticSeverity.Warning)
            .WithSpan(7, 113, 7, 133)
            .WithArguments("fa-solid fa-circle", "fa-circle");

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task PlainAddAttribute_IsIgnoredAsync() {
        // Plain HTML AddAttribute calls never carry component parameters, so they stay out.
        const string source = """
            using Microsoft.AspNetCore.Components.Rendering;

            public class HandWritten {
                public void Build(RenderTreeBuilder builder) {
                    builder.OpenElement(0, "i");
                    builder.AddAttribute(1, "class", "fa-solid fa-house");
                    builder.CloseElement();
                }
            }
            """;

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source);
    }
}
