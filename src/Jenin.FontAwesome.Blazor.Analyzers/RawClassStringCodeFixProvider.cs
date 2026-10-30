//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Analyzers;

/// <summary>
/// Code fix for JFA1002: replaces a raw class string in a name position with the typed
/// <c>IconNames</c> equivalent (<c>"fa-solid fa-house"</c> becomes
/// <c>IconNames.Solid.House</c>), adding the <c>Jenin.FontAwesome.Blazor</c> using when missing.
/// Offered only for exact two-token strings whose style maps to an <c>IconNames</c> nested class
/// and whose icon exists in that style; longer strings carry sizing/modifier tokens that need
/// human mapping (the diagnostic message says so).
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RawClassStringCodeFixProvider)), Shared]
public sealed class RawClassStringCodeFixProvider : CodeFixProvider {
    /// <inheritdoc/>
    public override ImmutableArray<string> FixableDiagnosticIds => [JeninFontAwesomeAnalyzer.RawClassStringId];

    /// <inheritdoc/>
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc/>
    public override async Task RegisterCodeFixesAsync(CodeFixContext context) {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if(root?.FindNode(context.Span) is not LiteralExpressionSyntax literal
            || !literal.IsKind(SyntaxKind.StringLiteralExpression)) {
            return;
        }

        if(!ClassString.TryParse(literal.Token.ValueText, out var style, out var name, out var extraTokens)
            || extraTokens != 0
            || style is not ("Solid" or "Regular" or "Brands")) {
            return;
        }

        var canonical = KnownIconNames.Canonicalize(name);
        if(!KnownIconNames.SupportsStyle(style, canonical)) {
            return;
        }

        var member = "IconNames." + style + "." + ClassString.ToMemberName(canonical.Substring("fa-".Length));
        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Use " + member,
                createChangedDocument: cancellationToken => ReplaceWithMemberAsync(context.Document, literal, member, cancellationToken),
                equivalenceKey: member),
            context.Diagnostics);
    }

    private static async Task<Document> ReplaceWithMemberAsync(Document document, LiteralExpressionSyntax literal, string member, CancellationToken cancellationToken) {
        var root = (CompilationUnitSyntax)(await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false))!;
        var replacement = SyntaxFactory.ParseExpression(member).WithAdditionalAnnotations(Formatter.Annotation);
        var updated = root.ReplaceNode(literal, replacement);

        const string targetNamespace = "Jenin.FontAwesome.Blazor";
        if(!updated.Usings.Any(u => u.Name?.ToString() == targetNamespace)) {
            var insertion = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(targetNamespace)).WithAdditionalAnnotations(Formatter.Annotation);
            var index = updated.Usings.Count;
            for(var i = 0; i < updated.Usings.Count; i++) {
                if(string.Compare(updated.Usings[i].Name?.ToString(), targetNamespace, StringComparison.Ordinal) > 0) {
                    index = i;
                    break;
                }
            }

            updated = updated.WithUsings(updated.Usings.Insert(index, insertion));
        }

        return document.WithSyntaxRoot(updated);
    }
}
