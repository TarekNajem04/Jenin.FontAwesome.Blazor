//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Contract;

/// <summary>
/// Freezes the generated <see cref="IconNames"/> catalogue against the vendored Font Awesome
/// metadata. Kept separate from <see cref="PublicApiBaselineTests"/> (which excludes the catalogue) so the
/// thousands of icon constants are verified by exact value rather than a line-by-line snapshot.
/// </summary>
[TestClass]
public class IconCatalogueTests {
    private const string METADATA_REPO_RELATIVE_PATH = "eng/metadata/fontawesome-7.3.1-free-icons.json";

    // Resolved from the test assembly directory, not [CallerFilePath]: deterministic CI builds
    // (ContinuousIntegrationBuild) rewrite compile-time paths, so source-file discovery fails there.
    private static string RepoRoot() {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while(directory is not null && !File.Exists(Path.Combine(directory.FullName, "Jenin.FontAwesome.Blazor.slnx"))) {
            directory = directory.Parent;
        }

        Assert.IsNotNull(directory, "Could not locate the repository root.");

        return directory!.FullName;
    }

    private static Dictionary<string, string[]> LoadVendoredMetadata() {
        var path = Path.Combine(RepoRoot(), METADATA_REPO_RELATIVE_PATH.Replace('/', Path.DirectorySeparatorChar));

        Assert.IsTrue(File.Exists(path), $"Missing vendored metadata: {path}");

        using var document = JsonDocument.Parse(File.ReadAllText(path));
        var styles = document.RootElement.GetProperty("styles");
        var metadata = new Dictionary<string, string[]>(StringComparer.Ordinal);

        foreach(var style in styles.EnumerateObject()) {
            metadata[style.Name] = [.. style.Value.EnumerateArray().Select(static element => element.GetString()!)];
        }

        return metadata;
    }

    private static string[] CatalogueValues(string style) {
        var nested = typeof(IconNames).GetNestedType(style, BindingFlags.Public);

        Assert.IsNotNull(nested, $"IconNames.{style} was not found.");

        return [.. nested!.GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(static field => field.IsLiteral && field.FieldType == typeof(string))
            .Select(static field => (string)field.GetRawConstantValue()!)];
    }

    [TestMethod]
    [TestCategory("Contract")]
    [DataRow("Solid", "fa-house", "House")]
    [DataRow("Regular", "fa-star", "Star")]
    [DataRow("Brands", "fa-github", "Github")]
    [DataRow("Solid", "fa-circle-plus", "CirclePlus")]
    [DataRow("Solid", "fa-x-ray", "XRay")]
    [DataRow("Solid", "fa-0", "_0")]
    [DataRow("Brands", "fa-500px", "_500px")]
    [DataRow("Brands", "fa-42-group", "_42Group")]
    public void Catalogue_KnownNames_MapToExpectedCssClasses(string style, string expectedCssClass, string constant) {
        var nested = typeof(IconNames).GetNestedType(style, BindingFlags.Public);
        var field = nested!.GetField(constant, BindingFlags.Public | BindingFlags.Static);

        Assert.IsNotNull(field, $"IconNames.{style}.{constant} was not found.");
        Assert.AreEqual(expectedCssClass, field!.GetRawConstantValue());
    }

    [TestMethod]
    [TestCategory("Contract")]
    [DataRow("Solid", "solid")]
    [DataRow("Regular", "regular")]
    [DataRow("Brands", "brands")]
    public void Catalogue_MatchesVendoredMetadata(string style, string metadataKey) {
        var metadata = LoadVendoredMetadata();
        var expected = metadata[metadataKey].Select(static name => "fa-" + name).ToHashSet(StringComparer.Ordinal);
        var actual = CatalogueValues(style).ToHashSet(StringComparer.Ordinal);

        Assert.HasCount(expected.Count, actual,
            $"IconNames.{style} has {actual.Count} entries but the vendored metadata lists {expected.Count}.");

        var missing = expected.Except(actual, StringComparer.Ordinal).Take(15).ToArray();
        var unexpected = actual.Except(expected, StringComparer.Ordinal).Take(15).ToArray();

        Assert.IsTrue(expected.SetEquals(actual),
            $"IconNames.{style} diverged from the vendored metadata.\n" +
            $"Missing: {string.Join(", ", missing)}\nAdded: {string.Join(", ", unexpected)}");
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void Catalogue_ValuesAreUniqueAndPrefixedPerStyle() {
        foreach(var style in new[] { "Solid", "Regular", "Brands" }) {
            var values = CatalogueValues(style);

            Assert.IsTrue(values.All(static value => value.StartsWith("fa-", StringComparison.Ordinal)),
                $"IconNames.{style} contains a value without the 'fa-' prefix.");

            Assert.HasCount(values.Length, values.Distinct(StringComparer.Ordinal),
                $"IconNames.{style} contains duplicate values.");
        }
    }
}
