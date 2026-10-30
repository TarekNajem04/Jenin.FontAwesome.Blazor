//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.IconPicker.Tests.Contract;

/// <summary>
/// Guards the IconPicker package's independent trimming / Native AOT contract.
/// </summary>
/// <remarks>
/// The optional packages must carry their own <c>IsTrimmable</c>/<c>IsAotCompatible</c> contract;
/// consumer evidence is not inherited from the core package. Complementary to the
/// publish-level probes run by <c>eng/verify-optional-trim.ps1</c>, which prove the embedded
/// metadata remains loadable in a fully trimmed output.
/// </remarks>
[TestClass]
public class OptionalPackageTrimmingAotContractTests {
    private static Assembly IconPicker => typeof(IconMetadataProvider).Assembly;

    private static string RepoRoot([CallerFilePath] string sourceFile = "") {
        var directory = new DirectoryInfo(Path.GetDirectoryName(sourceFile)!);

        while(directory is not null && !File.Exists(Path.Combine(directory.FullName, "Jenin.FontAwesome.Blazor.slnx"))) {
            directory = directory.Parent;
        }

        Assert.IsNotNull(directory, "Could not locate the repository root.");

        return directory!.FullName;
    }

    private static string PackageCsproj => Path.Combine(RepoRoot(), "src", "Jenin.FontAwesome.Blazor.IconPicker", "Jenin.FontAwesome.Blazor.IconPicker.csproj");

    private static string? MetadataValue(string key) {
        foreach(var attribute in IconPicker.GetCustomAttributes<AssemblyMetadataAttribute>()) {
            if(string.Equals(attribute.Key, key, StringComparison.Ordinal)) {
                return attribute.Value;
            }
        }

        return null;
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void IconPickerPackage_ProjectDeclaresTrimmingAndAotCompatibility() {
        var csproj = File.ReadAllText(PackageCsproj);

        Assert.Contains("<IsTrimmable>true</IsTrimmable>", csproj, "The IconPicker package must set <IsTrimmable>true</IsTrimmable> so hosts can trim it independently.");
        Assert.Contains("<IsAotCompatible>true</IsAotCompatible>", csproj, "The IconPicker package must set <IsAotCompatible>true</IsAotCompatible> to keep its trim/AOT analyzers on.");
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void IconPickerPackage_AssemblyIsMarkedTrimmable()
        => Assert.AreEqual("True", MetadataValue("IsTrimmable"),
            "The linker must see IsTrimmable=true in the IconPicker assembly metadata.");
}
