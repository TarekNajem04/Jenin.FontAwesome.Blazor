//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Contract;

/// <summary>
/// Member-level, machine-verified public API baseline. Complements
/// <see cref="PublicApiSurfaceTests"/> (which freezes the exported type set) by freezing the full
/// externally visible member surface of every exported type.
/// </summary>
/// <remarks>
/// To intentionally change the surface, regenerate the baseline:
/// <c>$env:JFA_UPDATE_PUBLIC_API_BASELINE=1; dotnet test</c>, then review the diff before committing.
/// The generated <c>IconNames</c> catalogue is excluded because its thousands of icon constants
/// would swamp this baseline; its completeness and exact values are instead frozen by
/// <c>IconCatalogueTests</c> against the vendored metadata.
/// </remarks>
[TestClass]
public class PublicApiBaselineTests {
    private const string BASELINE_REPO_RELATIVE_PATH = "tests/Jenin.FontAwesome.Blazor.Tests/Contract/PublicApiBaseline.txt";
    private const string GENERATED_CATALOGUE_TYPE_PREFIX = "Jenin.FontAwesome.Blazor.IconNames";

    private static Assembly Library => typeof(Icon).Assembly;

    [TestMethod]
    [TestCategory("Contract")]
    public void PublicApi_MatchesFrozenMemberBaseline() {
        var snapshot = BuildSnapshot();
        var baselinePath = ResolveBaselinePath();

        if(string.Equals(Environment.GetEnvironmentVariable("JFA_UPDATE_PUBLIC_API_BASELINE"), "1", StringComparison.Ordinal)) {
            File.WriteAllText(baselinePath, snapshot, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));

            Assert.Inconclusive($"Public API baseline regenerated at '{baselinePath}'.");

            return;
        }

        File.Exists(baselinePath).Should().BeTrue($"Public API baseline not found at '{baselinePath}'. Regenerate with JFA_UPDATE_PUBLIC_API_BASELINE=1.");

        var expected = Normalize(File.ReadAllText(baselinePath));
        var actual = Normalize(snapshot);

        string.Equals(expected, actual, StringComparison.Ordinal).Should().BeTrue(
            "The public API member surface diverged from the frozen baseline.\n" + DescribeDifference(expected, actual));
    }

    private static string ResolveBaselinePath() {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while(dir is not null && !File.Exists(Path.Combine(dir.FullName, "Jenin.FontAwesome.Blazor.slnx"))) {
            dir = dir.Parent;
        }

        dir.Should().NotBeNull("Could not locate the repository root (Jenin.FontAwesome.Blazor.slnx).");

        return Path.Combine(dir!.FullName, BASELINE_REPO_RELATIVE_PATH.Replace('/', Path.DirectorySeparatorChar));
    }

    private static string Normalize(string value) => value.Replace("\r\n", "\n", StringComparison.Ordinal).TrimEnd('\n');

    private static string DescribeDifference(string expected, string actual) {
        var expectedLines = expected.Split('\n');
        var actualLines = actual.Split('\n');
        var message = new StringBuilder();

        _ = message.Append("Missing (in baseline, not in assembly): ")
                   .AppendJoin(" | ", expectedLines.Except(actualLines, StringComparer.Ordinal).Take(15)).AppendLine();
        _ = message.Append("Added (in assembly, not in baseline): ")
                   .AppendJoin(" | ", actualLines.Except(expectedLines, StringComparer.Ordinal).Take(15)).AppendLine();

        return message.ToString();
    }

    private static string BuildSnapshot() {
        var builder = new StringBuilder();

        foreach(var type in Library.GetExportedTypes()
            .Where(static t => !IsGeneratedCatalogueType(t))
            .OrderBy(static t => t.FullName, StringComparer.Ordinal)) {
            _ = builder.Append(DescribeTypeHeader(type)).Append('\n');

            var members = type
                .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(IsVisible)
                .Select(Describe)
                .Where(static line => line is not null)
                .OrderBy(static line => line, StringComparer.Ordinal);

            foreach(var member in members) {
                _ = builder.Append(member).Append('\n');
            }
        }

        return builder.ToString();
    }

    private static bool IsGeneratedCatalogueType(Type type) {
        if(type.FullName is not { } name) {
            return false;
        }

        return string.Equals(name, GENERATED_CATALOGUE_TYPE_PREFIX, StringComparison.Ordinal)
               || name.StartsWith(GENERATED_CATALOGUE_TYPE_PREFIX + "+", StringComparison.Ordinal);
    }

    private static string DescribeTypeHeader(Type type) {
        var kind = type switch {
            { IsEnum: true } => "enum",
            { IsValueType: true } => "struct",
            { IsInterface: true } => "interface",
            _ when typeof(Delegate).IsAssignableFrom(type) => "delegate",
            _ => "class"
        };

        var modifier = type.IsAbstract && !type.IsInterface && !type.IsSealed && !type.IsValueType ? "abstract " : string.Empty;

        var lineage = type.BaseType is null ? string.Empty : $" : {FormatType(type.BaseType)}";

        return $"TYPE {modifier}{kind} {FormatType(type)}{lineage}";
    }

    private static string? Describe(MemberInfo member) => member switch {
        Type => null,
        ConstructorInfo constructor => $"C {FormatType(constructor.DeclaringType!)}.{constructor.Name}({FormatParameters(constructor.GetParameters())})",
        MethodInfo method when method.IsSpecialName => null,
        MethodInfo method => $"M {FormatType(method.ReturnType)} {method.Name}{FormatTypeParameters(method)}({FormatParameters(method.GetParameters())})",
        PropertyInfo property => $"P {FormatType(property.PropertyType)} {property.Name}{FormatAccessors(property)}",
        FieldInfo field when field.IsSpecialName => null,
        FieldInfo field => $"F {FormatType(field.FieldType)} {field.Name}",
        EventInfo @event => $"E {FormatType(@event.EventHandlerType!)} {@event.Name}",
        _ => null
    };

    private static string FormatAccessors(PropertyInfo property) {
        var getter = property.GetMethod is { } get && IsVisible(get) ? "get; " : string.Empty;
        var setter = property.SetMethod is { } set && IsVisible(set) ? "set; " : string.Empty;

        return $" {{ {getter}{setter}}}";
    }

    private static string FormatParameters(ParameterInfo[] parameters)
        => string.Join(", ", parameters.Select(static p => FormatType(p.ParameterType)));

    private static string FormatTypeParameters(MethodInfo method)
        => method.IsGenericMethodDefinition
            ? $"<{string.Join(", ", method.GetGenericArguments().Select(static a => a.Name))}>"
            : string.Empty;

    private static string FormatType(Type type) {
        if(type.IsByRef) {
            return FormatType(type.GetElementType()!) + "&";
        }

        if(type.IsArray) {
            return FormatType(type.GetElementType()!) + "[]";
        }

        if(type.IsGenericParameter) {
            return type.Name;
        }

        if(type.IsGenericType) {
            var name = type.GetGenericTypeDefinition().FullName!;
            var tick = name.IndexOf('`', StringComparison.Ordinal);

            if(tick >= 0) {
                name = name[..tick];
            }

            return $"{name}<{string.Join(", ", type.GetGenericArguments().Select(FormatType))}>";
        }

        return type.FullName ?? type.Name;
    }

    private static bool IsVisible(MemberInfo member) => member switch {
        MethodBase method => IsVisibleMethod(method),
        FieldInfo field => field.IsPublic || field.IsFamily || field.IsFamilyOrAssembly,
        PropertyInfo property => (property.GetMethod is { } get && IsVisibleMethod(get))
                                 || (property.SetMethod is { } set && IsVisibleMethod(set)),
        EventInfo @event => (@event.AddMethod is { } add && IsVisibleMethod(add))
                            || (@event.RemoveMethod is { } remove && IsVisibleMethod(remove)),
        _ => false
    };

    private static bool IsVisibleMethod(MethodBase method) => method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly;
}
