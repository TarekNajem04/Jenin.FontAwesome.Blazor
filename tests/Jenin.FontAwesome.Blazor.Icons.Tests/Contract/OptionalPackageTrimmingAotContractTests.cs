//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Icons.Tests.Contract;

/// <summary>
/// Guards the Icons package's independent trimming / Native AOT contract.
/// </summary>
/// <remarks>
/// The optional packages must carry their own <c>IsTrimmable</c>/<c>IsAotCompatible</c> contract;
/// consumer evidence is not inherited from the core package. Complementary to the
/// publish-level probes run by <c>eng/verify-optional-trim.ps1</c>, which prove a referenced
/// preset survives full trimming while unreferenced presets are removed.
/// </remarks>
[TestClass]
public class OptionalPackageTrimmingAotContractTests {
    private static Assembly Icons => typeof(IconsAssemblyMarker).Assembly;

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

    private static string PackageCsproj => Path.Combine(RepoRoot(), "src", "Jenin.FontAwesome.Blazor.Icons", "Jenin.FontAwesome.Blazor.Icons.csproj");

    private static string? MetadataValue(string key) {
        foreach(var attribute in Icons.GetCustomAttributes<AssemblyMetadataAttribute>()) {
            if(string.Equals(attribute.Key, key, StringComparison.Ordinal)) {
                return attribute.Value;
            }
        }

        return null;
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void IconsPackage_ProjectDeclaresTrimmingAndAotCompatibility() {
        var csproj = File.ReadAllText(PackageCsproj);

        csproj.Should().Contain("<IsTrimmable>true</IsTrimmable>", "The Icons package must set <IsTrimmable>true</IsTrimmable> so hosts can trim it independently.");
        csproj.Should().Contain("<IsAotCompatible>true</IsAotCompatible>", "The Icons package must set <IsAotCompatible>true</IsAotCompatible> to keep its trim/AOT analyzers on.");
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void IconsPackage_AssemblyIsMarkedTrimmable() =>
        MetadataValue("IsTrimmable").Should().Be("True",
            "The linker must see IsTrimmable=true in the Icons assembly metadata.");
}
