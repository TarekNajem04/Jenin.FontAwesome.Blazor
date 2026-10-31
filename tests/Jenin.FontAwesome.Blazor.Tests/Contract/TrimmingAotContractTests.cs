//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Contract;

/// <summary>
/// Guards the library's trimming / Native AOT posture.
/// </summary>
/// <remarks>
/// Two complementary checks are used because the SDK surface differs by target framework: the
/// csproj declaration is asserted directly (stable on every TFM), and the emitted
/// <c>AssemblyMetadata("IsTrimmable", "True")</c> attribute is asserted at runtime (proves the
/// linker will actually see the annotation). The <c>IsAotCompatible</c> metadata attribute is only
/// emitted by newer SDKs, so it is validated through the project declaration rather than the
/// compiled attribute; the build-time <c>EnableAotAnalyzer</c> switch is what enforces zero AOT
/// warnings and is exercised by every CI build.
/// </remarks>
[TestClass]
public class TrimmingAotContractTests {
    private static Assembly Library => typeof(Icon).Assembly;

    // Resolved from the test assembly directory, not [CallerFilePath]: deterministic CI builds
    // (ContinuousIntegrationBuild) rewrite compile-time paths, so source-file discovery fails there.
    private static string RepoRoot() {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while(directory is not null && !File.Exists(Path.Combine(directory.FullName, "Jenin.FontAwesome.Blazor.slnx"))) {
            directory = directory.Parent;
        }

        directory.Should().NotBeNull("Could not locate the repository root.");

        return directory!.FullName;
    }

    private static string LibraryCsproj => Path.Combine(RepoRoot(), "src", "Jenin.FontAwesome.Blazor", "Jenin.FontAwesome.Blazor.csproj");

    private static string? MetadataValue(string key) {
        foreach(var attribute in Library.GetCustomAttributes<AssemblyMetadataAttribute>()) {
            if(string.Equals(attribute.Key, key, StringComparison.Ordinal)) {
                return attribute.Value;
            }
        }

        return null;
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void Library_ProjectDeclaresTrimmingAndAotCompatibility() {
        var csproj = File.ReadAllText(LibraryCsproj);

        csproj.Should().Contain("<IsTrimmable>true</IsTrimmable>", "The library must set <IsTrimmable>true</IsTrimmable> so hosts can trim safely.");
        csproj.Should().Contain("<IsAotCompatible>true</IsAotCompatible>", "The library must set <IsAotCompatible>true</IsAotCompatible> to keep the trim/AOT analyzers on.");
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void Library_AssemblyIsMarkedTrimmable() =>
        MetadataValue("IsTrimmable").Should().Be("True",
            "The linker must see IsTrimmable=true in the compiled assembly metadata.");
}
