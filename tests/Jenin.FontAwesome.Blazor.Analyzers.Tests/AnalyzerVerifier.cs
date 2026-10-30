//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Analyzers.Tests;

/// <summary>
/// Roslyn SDK harnesses wired to the real product assembly: snippets compile against
/// <c>Jenin.FontAwesome.Blazor</c> so owner checks exercise the production types, and the
/// embedded icon catalogue loads from the analyzer under test.
/// </summary>
internal static class AnalyzerVerifier<TAnalyzer>
    where TAnalyzer : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, new() {
    // Analysis-only references: the product assembly targets net10.0, so the compilation under
    // test must resolve net10 ref assemblies (the cached 10.0.9 targeting pack). The analyzed
    // code is never executed, so one ref set serves every test leg (net8/9/10).
    private static readonly ReferenceAssemblies References = new(
        "net10.0",
        new PackageIdentity("Microsoft.NETCore.App.Ref", "10.0.9"),
        Path.Combine("ref", "net10.0"));

    public static async Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected) {
        var test = new CSharpAnalyzerTest<TAnalyzer, TestVerifier> {
            ReferenceAssemblies = References,
        };

        test.TestState.Sources.Add(source);
        test.TestState.AdditionalReferences.Add(typeof(Icon).Assembly);
        test.TestState.AdditionalReferences.Add(typeof(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder).Assembly);
        test.ExpectedDiagnostics.AddRange(expected);

        await test.RunAsync();
    }

    public static async Task VerifyCodeFixAsync(string source, DiagnosticResult expected, string fixedSource, int index = 0) {
        var test = new CSharpCodeFixTest<JeninFontAwesomeAnalyzer, RawClassStringCodeFixProvider, TestVerifier> {
            ReferenceAssemblies = References,
        };

        test.TestState.Sources.Add(source);
        test.TestState.AdditionalReferences.Add(typeof(Icon).Assembly);
        test.ExpectedDiagnostics.Add(expected);
        test.FixedState.Sources.Add(fixedSource);
        test.CodeActionIndex = index;

        await test.RunAsync();
    }
}
