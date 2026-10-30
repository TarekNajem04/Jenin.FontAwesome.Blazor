//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Svg.Tests.Contract;

/// <summary>
/// Guards the Svg package's independent trimming / Native AOT contract (same shape as the other
/// optional packages). The Svg package must carry its own <c>IsTrimmable</c>/<c>IsAotCompatible</c>
/// contract; consumer evidence is not inherited from the core package. Complementary to the
/// publish-level probes run by <c>eng/verify-optional-trim.ps1</c>.
/// </summary>
[TestClass]
public class SvgPackageTrimmingAotContractTests {
    private static Assembly Svg => typeof(SvgAssemblyMarker).Assembly;

    private static string RepoRoot([CallerFilePath] string sourceFile = "") {
        var directory = new DirectoryInfo(Path.GetDirectoryName(sourceFile)!);

        while(directory is not null && !File.Exists(Path.Combine(directory.FullName, "Jenin.FontAwesome.Blazor.slnx"))) {
            directory = directory.Parent;
        }

        Assert.IsNotNull(directory, "Could not locate the repository root.");

        return directory!.FullName;
    }

    private static string PackageCsproj => Path.Combine(RepoRoot(), "src", "Jenin.FontAwesome.Blazor.Svg", "Jenin.FontAwesome.Blazor.Svg.csproj");

    private static string? MetadataValue(string key) {
        foreach(var attribute in Svg.GetCustomAttributes<AssemblyMetadataAttribute>()) {
            if(string.Equals(attribute.Key, key, StringComparison.Ordinal)) {
                return attribute.Value;
            }
        }

        return null;
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void SvgPackage_ProjectDeclaresTrimmingAndAotCompatibility() {
        var csproj = File.ReadAllText(PackageCsproj);

        Assert.Contains("<IsTrimmable>true</IsTrimmable>", csproj, "The Svg package must set <IsTrimmable>true</IsTrimmable> so hosts can trim it independently.");
        Assert.Contains("<IsAotCompatible>true</IsAotCompatible>", csproj, "The Svg package must set <IsAotCompatible>true</IsAotCompatible> to keep its trim/AOT analyzers on.");
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void SvgPackage_AssemblyIsMarkedTrimmable() {
        Assert.AreEqual("True", MetadataValue("IsTrimmable"),
            "The linker must see IsTrimmable=true in the Svg assembly metadata.");
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void SvgPackage_DeclaresNoJavaScriptInteropSurface() {
        var jsRuntimeReferences = Svg.GetReferencedAssemblies()
            .Where(static name => string.Equals(name.Name, "Microsoft.JSInterop", StringComparison.Ordinal));

        Assert.HasCount(0, jsRuntimeReferences.ToList(), "The Svg package must stay interop-free: no Microsoft.JSInterop reference allowed.");
    }
}
