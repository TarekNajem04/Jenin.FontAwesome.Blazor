//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Analyzers.Tests;

/// <summary>
/// JFA1003: Pro-only styles and typefaces without licensed assets.
/// </summary>
[TestClass]
public class ProStyleTests {
    [TestMethod]
    public async Task LightStyle_ReportsJFA1003Async() {
        const string source = """
            using Jenin.FontAwesome.Blazor;
            using Jenin.FontAwesome.Blazor.Components;

            public class Consumer {
                public Icon Icon { get; } = new Icon { IconName = "fa-house", IconStyle = IconStyle.Light };
            }
            """;

        var expected = new DiagnosticResult(JeninFontAwesomeAnalyzer.ProStyleId, DiagnosticSeverity.Warning)
            .WithSpan(5, 79, 5, 94)
            .WithArguments("IconStyle.Light");

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task SharpTypeface_ReportsJFA1003Async() {
        const string source = """
            using Jenin.FontAwesome.Blazor;
            using Jenin.FontAwesome.Blazor.Components;

            public class Consumer {
                public Icon Icon { get; } = new Icon { IconName = "fa-house", Typeface = Typeface.Sharp };
            }
            """;

        var expected = new DiagnosticResult(JeninFontAwesomeAnalyzer.ProStyleId, DiagnosticSeverity.Warning)
            .WithSpan(5, 78, 5, 92)
            .WithArguments("Typeface.Sharp");

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source, expected);
    }

    [TestMethod]
    public async Task FreeStyles_ProduceNoDiagnosticsAsync() {
        const string source = """
            using Jenin.FontAwesome.Blazor;
            using Jenin.FontAwesome.Blazor.Components;

            public class Consumer {
                public Icon Solid { get; } = new Icon { IconName = "fa-house", IconStyle = IconStyle.Solid };
                public Icon Brands { get; } = new Icon { IconName = "fa-github", IconStyle = IconStyle.Brands, Typeface = Typeface.Classic };
            }
            """;

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task UnrelatedLightMember_IsIgnoredAsync() {
        const string source = """
            public enum Theme { Light }

            public class Consumer {
                public Theme Theme { get; } = Theme.Light;
            }
            """;

        await AnalyzerVerifier<JeninFontAwesomeAnalyzer>.VerifyAnalyzerAsync(source);
    }
}
