//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Analyzers;

/// <summary>
/// Compile-time guidance for the typed Font Awesome API. Nothing here ever
/// errors: every diagnostic defaults to <c>Warning</c> and honors <c>.editorconfig</c> severity
/// (<c>dotnet_diagnostic.JFA100x.severity</c>).
/// </summary>
/// <remarks>
/// <para>
/// Detection is syntactic plus a light semantic owner check, so it stays fast and dependency-free:
/// string literals assigned to <c>IconName</c>/<c>MaskIconName</c> (initializers, assignments, and
/// the <c>Icon</c> attribute), the <c>AddComponentParameter("IconName", ...)</c> shape Razor
/// generates for component parameters, and <c>IconStyle</c>/<c>Typeface</c> member accesses
/// for the Pro-assets diagnostic.
/// </para>
/// <para>
/// Arbitrary <c>class="fa-..."</c> markup and plain HTML <c>AddAttribute</c> calls are
/// deliberately out of scope: documentation snippets show raw Font Awesome HTML on purpose,
/// and HTML attributes never carry component parameters.
/// </para>
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class JeninFontAwesomeAnalyzer : DiagnosticAnalyzer {
    /// <summary>Diagnostic ID for unknown icon names (warning).</summary>
    public const string UNKNOWN_ICON_NAME_ID = "JFA1001";
    /// <summary>Diagnostic ID for raw class strings in name positions (warning, with code fix).</summary>
    public const string RAW_CLASS_STRING_ID = "JFA1002";
    /// <summary>Diagnostic ID for Pro-only styles without licensed assets (warning).</summary>
    public const string PRO_STYLE_ID = "JFA1003";

    private static readonly DiagnosticDescriptor _unknownIconName = new(
        UNKNOWN_ICON_NAME_ID,
        "Unknown Font Awesome icon name",
        "Icon name '{0}' is not a known Font Awesome 7 Free icon; it will render nothing. Use an IconNames constant or fix the spelling.",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Typo'd, removed, or Pro-only icon names render nothing at runtime. Prefer the generated IconNames constants so renames become compile errors.");

    private static readonly DiagnosticDescriptor _rawClassString = new(
        RAW_CLASS_STRING_ID,
        "Raw Font Awesome classes in a name position",
        "Replace the raw class string '{0}' with the icon name '{1}'",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "IconName and MaskIconName take a bare icon name (for example fa-house), not a full class string. A code fix offers the typed IconNames equivalent when one exists.");

    private static readonly DiagnosticDescriptor _proStyle = new(
        PRO_STYLE_ID,
        "Pro-only style without licensed assets",
        "'{0}' needs licensed Pro assets for full rendering",
        "Usage",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Light, Thin, and Duotone styles (and the Sharp/Duotone typefaces) need licensed Pro assets for full rendering. Host licensed assets or switch style; suppress per project when licensed (dotnet_diagnostic.JFA1003.severity = none).");

    /// <summary>Gets the supported diagnostics (JFA1001, JFA1002, JFA1003).</summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        [_unknownIconName, _rawClassString, _proStyle];

    /// <summary>Registers the syntax-node actions for name positions and Pro styles.</summary>
    /// <param name="context">The analysis context.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context"/> is <c>null</c>.</exception>
    public override void Initialize(AnalysisContext context) {
        if(context is null) {
            throw new ArgumentNullException(nameof(context));
        }

        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeAttributeArgument, SyntaxKind.AttributeArgument);
        context.RegisterSyntaxNodeAction(AnalyzeAssignment, SyntaxKind.SimpleAssignmentExpression);
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
    }

    private static void AnalyzeAttributeArgument(SyntaxNodeAnalysisContext context) {
        var argument = (AttributeArgumentSyntax)context.Node;
        if(argument.NameEquals is not null
            || argument.Expression is not LiteralExpressionSyntax literal
            || !literal.IsKind(SyntaxKind.StringLiteralExpression)) {
            return;
        }

        // Only the name (the first positional argument) is an icon name; named arguments such
        // as Color = "#0d6efd" are not.
        if(argument.Parent is not AttributeArgumentListSyntax list
            || list.Arguments.Count == 0
            || !ReferenceEquals(list.Arguments[0], argument)) {
            return;
        }

        var attribute = argument.FirstAncestorOrSelf<AttributeSyntax>();
        if(attribute is null) {
            return;
        }

        var symbol = context.SemanticModel.GetSymbolInfo(attribute, context.CancellationToken).Symbol;
        if(symbol is not IMethodSymbol constructor
            || !string.Equals(constructor.ContainingType.Name, "IconAttribute", StringComparison.Ordinal)
            || !IsProductNamespace(constructor.ContainingType)) {
            return;
        }

        AnalyzeNameLiteral(context, literal, location: literal.GetLocation());
    }

    private static void AnalyzeAssignment(SyntaxNodeAnalysisContext context) {
        var assignment = (AssignmentExpressionSyntax)context.Node;
        if(assignment.Right is not LiteralExpressionSyntax literal || !literal.IsKind(SyntaxKind.StringLiteralExpression)) {
            AnalyzeStyleMember(context, assignment.Left, assignment.Right);
            return;
        }

        var name = LeftMemberName(assignment.Left);
        if(name is not ("IconName" or "MaskIconName")) {
            AnalyzeStyleMember(context, assignment.Left, assignment.Right);
            return;
        }

        if(!IsProductProperty(context, assignment.Left, name)) {
            return;
        }

        AnalyzeNameLiteral(context, literal, location: literal.GetLocation());
    }

    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context) {
        // Component-parameter shape: Razor generates
        // builder.AddComponentParameter(seq, nameof(global::....Icon.IconName), "fa-...").
        // Plain AddAttribute calls carry HTML attributes, never component parameters, so only
        // AddComponentParameter is covered here.
        if(context.Node is not InvocationExpressionSyntax invocation
            || invocation.Expression is not MemberAccessExpressionSyntax memberAccess
            || memberAccess.Name.Identifier.Text != "AddComponentParameter"
            || invocation.ArgumentList?.Arguments.Count != 3) {
            return;
        }

        var name = ResolveParameterName(context, invocation.ArgumentList.Arguments[1].Expression);
        if(name is null) {
            return;
        }

        var valueArgument = invocation.ArgumentList.Arguments[2].Expression;
        if(name is "IconName" or "MaskIconName") {
            if(valueArgument is LiteralExpressionSyntax valueLiteral && valueLiteral.IsKind(SyntaxKind.StringLiteralExpression)) {
                AnalyzeNameLiteral(context, valueLiteral, location: valueLiteral.GetLocation());
            }

            return;
        }

        if(name is "IconStyle" or "Typeface") {
            AnalyzeStyleValue(context, valueArgument);
        }
    }

    private static string? ResolveParameterName(SyntaxNodeAnalysisContext context, ExpressionSyntax nameExpression) {
        // The Razor generator emits nameof(...) pointing at the product property; resolve it
        // semantically so only this library's parameters are covered.
        if(nameExpression is InvocationExpressionSyntax nameofCall
            && nameofCall.Expression is IdentifierNameSyntax { Identifier.Text: "nameof" }
            && nameofCall.ArgumentList?.Arguments.Count == 1) {
            var target = context.SemanticModel.GetSymbolInfo(nameofCall.ArgumentList.Arguments[0].Expression, context.CancellationToken).Symbol;
            if(target is IPropertySymbol property && IsProductPropertySymbol(property, property.Name)) {
                return property.Name;
            }

            return null;
        }

        // Hand-written render trees may pass the plain name; those names have no meaning outside
        // this library's API, so they are covered as-is.
        if(nameExpression is LiteralExpressionSyntax literal && literal.IsKind(SyntaxKind.StringLiteralExpression)) {
            return literal.Token.ValueText is "IconName" or "MaskIconName" or "IconStyle" or "Typeface"
                ? literal.Token.ValueText
                : null;
        }

        return null;
    }

    private static void AnalyzeStyleMember(SyntaxNodeAnalysisContext context, ExpressionSyntax left, ExpressionSyntax right) {
        var name = LeftMemberName(left);
        if(name is not ("IconStyle" or "Typeface")) {
            return;
        }

        if(!IsProductProperty(context, left, name)) {
            return;
        }

        AnalyzeStyleValue(context, right);
    }

    private static void AnalyzeStyleValue(SyntaxNodeAnalysisContext context, ExpressionSyntax value) {
        if(value is not MemberAccessExpressionSyntax memberAccess) {
            return;
        }

        var symbol = context.SemanticModel.GetSymbolInfo(memberAccess, context.CancellationToken).Symbol;
        if(symbol is not IFieldSymbol field
            || field.ContainingType?.ToDisplayString() is not ("Jenin.FontAwesome.Blazor.IconStyle" or "Jenin.FontAwesome.Blazor.Typeface")) {
            return;
        }

        if(field.Name is "Light" or "Thin" or "Duotone" or "Sharp" or "SharpDuotone") {
            context.ReportDiagnostic(Diagnostic.Create(_proStyle, memberAccess.GetLocation(), field.ContainingType.Name + "." + field.Name));
        }
    }

    private static void AnalyzeNameLiteral(SyntaxNodeAnalysisContext context, LiteralExpressionSyntax literal, Location location) {
        var value = literal.Token.ValueText;
        if(string.IsNullOrWhiteSpace(value)) {
            context.ReportDiagnostic(Diagnostic.Create(_unknownIconName, location, "(empty)"));
            return;
        }

        if(ClassString.TryParse(value, out _, out var name, out _)) {
            context.ReportDiagnostic(Diagnostic.Create(_rawClassString, location, value, name));
            return;
        }

        if(!KnownIconNames.Contains(value)) {
            context.ReportDiagnostic(Diagnostic.Create(_unknownIconName, location, value));
        }
    }

    private static string? LeftMemberName(ExpressionSyntax left) => left switch {
        IdentifierNameSyntax identifier => identifier.Identifier.Text,
        MemberAccessExpressionSyntax memberAccess => memberAccess.Name.Identifier.Text,
        _ => null,
    };

    private static bool IsProductNamespace(ISymbol symbol) {
        var ns = symbol.ContainingNamespace?.ToDisplayString();
        return ns == "Jenin.FontAwesome.Blazor" || (ns?.StartsWith("Jenin.FontAwesome.Blazor.", StringComparison.Ordinal) ?? false);
    }

    private static bool IsProductProperty(SyntaxNodeAnalysisContext context, ExpressionSyntax left, string name) {
        var symbol = context.SemanticModel.GetSymbolInfo(left, context.CancellationToken).Symbol;
        return symbol is IPropertySymbol property && IsProductPropertySymbol(property, name);
    }

    private static bool IsProductPropertySymbol(IPropertySymbol property, string name) =>
        string.Equals(property.Name, name, StringComparison.Ordinal)
        && property.ContainingAssembly?.Name.StartsWith("Jenin.FontAwesome.Blazor", StringComparison.Ordinal) is true;
}
