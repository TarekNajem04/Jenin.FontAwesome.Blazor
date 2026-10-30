//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Analyzers.Tests;

/// <summary>
/// JFA1002 plus the code fix: raw class strings in name positions.
/// </summary>
[TestClass]
public class RawClassStringTests {
    [TestMethod]
    public async Task ClassString_InObjectInitializer_ReportsAndFixesAsync() {
        const string source = """
            using Jenin.FontAwesome.Blazor.Components;

            public class Consumer {
                public Icon Icon { get; } = new Icon { IconName = "fa-solid fa-house" };
            }
            """;

        const string fixedSource = """
            using Jenin.FontAwesome.Blazor;
            using Jenin.FontAwesome.Blazor.Components;

            public class Consumer {
                public Icon Icon { get; } = new Icon { IconName = IconNames.Solid.House };
            }
            """;

        var expected = new DiagnosticResult(JeninFontAwesomeAnalyzer.RawClassStringId, DiagnosticSeverity.Warning)
            .WithSpan(4, 55, 4, 74)
            .WithArguments("fa-solid fa-house", "fa-house");

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source, expected);
        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [TestMethod]
    public async Task AliasClassString_FixesToCanonicalMemberAsync() {
        const string source = """
            using Jenin.FontAwesome.Blazor.Components;

            public class Consumer {
                public Icon Icon { get; } = new Icon { IconName = "fa-solid fa-home" };
            }
            """;

        const string fixedSource = """
            using Jenin.FontAwesome.Blazor;
            using Jenin.FontAwesome.Blazor.Components;

            public class Consumer {
                public Icon Icon { get; } = new Icon { IconName = IconNames.Solid.House };
            }
            """;

        var expected = new DiagnosticResult(JeninFontAwesomeAnalyzer.RawClassStringId, DiagnosticSeverity.Warning)
            .WithSpan(4, 55, 4, 73)
            .WithArguments("fa-solid fa-home", "fa-home");

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source, expected);
        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyCodeFixAsync(source, expected, fixedSource);
    }

    [TestMethod]
    public async Task ClassStringWithSizeToken_ReportsWithoutFixAsync() {
        // Extra tokens (fa-2x) need human mapping: diagnostic fires, but the fix requires an
        // exact two-token string, so no code action is offered for this span.
        const string source = """
            using Jenin.FontAwesome.Blazor.Components;

            public class Consumer {
                public Icon Icon { get; } = new Icon { IconName = "fa-solid fa-house fa-2x" };
            }
            """;

        var expected = new DiagnosticResult(JeninFontAwesomeAnalyzer.RawClassStringId, DiagnosticSeverity.Warning)
            .WithSpan(4, 55, 4, 80)
            .WithArguments("fa-solid fa-house fa-2x", "fa-house");

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task BareName_ProducesNoDiagnosticsAsync() {
        const string source = """
            using Jenin.FontAwesome.Blazor.Components;

            public class Consumer {
                public Icon Icon { get; } = new Icon { IconName = "fa-house" };
            }
            """;

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source);
    }
}
