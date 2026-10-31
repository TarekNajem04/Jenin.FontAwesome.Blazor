//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Analyzers.Tests;

/// <summary>
/// Analyzer tests using the Roslyn SDK harnesses. Each test compiles a snippet
/// against the real product assembly and asserts the reported diagnostics (or their absence),
/// so the documented mappings cannot drift from the implementation.
/// </summary>
[TestClass]
public class UnknownIconNameTests {
    [TestMethod]
    public async Task Typo_InObjectInitializer_ReportsJFA1001Async() {
        const string source = """
            using Jenin.FontAwesome.Blazor;
            using Jenin.FontAwesome.Blazor.Components;

            public class Consumer {
                public Icon Icon { get; } = new Icon { IconName = "fa-huose" };
            }
            """;

        var expected = new DiagnosticResult(JeninFontAwesomeAnalyzer.UNKNOWN_ICON_NAME_ID, DiagnosticSeverity.Warning)
            .WithSpan(5, 55, 5, 65)
            .WithArguments("fa-huose");

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Typo_InAttribute_ReportsJFA1001Async() {
        const string source = """
            using Jenin.FontAwesome.Blazor;

            public class Consumer {
                [Icon("fa-huose")]
                public string Name = string.Empty;
            }
            """;

        var expected = new DiagnosticResult(JeninFontAwesomeAnalyzer.UNKNOWN_ICON_NAME_ID, DiagnosticSeverity.Warning)
            .WithSpan(4, 11, 4, 21)
            .WithArguments("fa-huose");

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task Typo_InAssignment_ReportsJFA1001Async() {
        const string source = """
            using Jenin.FontAwesome.Blazor;
            using Jenin.FontAwesome.Blazor.Components;

            public class Consumer {
                public void Bind(Icon icon) {
                    icon.MaskIconName = "fa-not-an-icon";
                }
            }
            """;

        var expected = new DiagnosticResult(JeninFontAwesomeAnalyzer.UNKNOWN_ICON_NAME_ID, DiagnosticSeverity.Warning)
            .WithSpan(6, 29, 6, 45)
            .WithArguments("fa-not-an-icon");

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task KnownName_ProducesNoDiagnosticsAsync() {
        const string source = """
            using Jenin.FontAwesome.Blazor;
            using Jenin.FontAwesome.Blazor.Components;

            public class Consumer {
                public Icon Icon { get; } = new Icon { IconName = "fa-house", MaskIconName = "fa-circle" };

                [Icon("fa-star")]
                public string Name = string.Empty;
            }
            """;

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task LegacyAlias_ProducesNoDiagnosticsAsync() {
        // fa-home is a legacy alias of fa-house: it renders, so it must not warn.
        const string source = """
            using Jenin.FontAwesome.Blazor.Components;

            public class Consumer {
                public Icon Icon { get; } = new Icon { IconName = "fa-home" };
            }
            """;

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task CustomKitIconName_WarnsByDesignAsync() {
        // Kit-custom glyphs are unknown to the Free catalogue, so JFA1001 fires by design:
        // the docs prescribe a targeted suppression for these names (PRO-02).
        const string source = """
            using Jenin.FontAwesome.Blazor.Components;

            public class Consumer {
                public Icon Icon { get; } = new Icon { IconName = "fa-acme-widget" };
            }
            """;

        var expected = new DiagnosticResult(JeninFontAwesomeAnalyzer.UNKNOWN_ICON_NAME_ID, DiagnosticSeverity.Warning)
            .WithSpan(4, 55, 4, 71)
            .WithArguments("fa-acme-widget");

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task UnrelatedProperty_IsIgnoredAsync() {
        const string source = """
            public class Consumer {
                public string IconName { get; set; } = "fa-huose";
            }
            """;

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source);
    }
}
