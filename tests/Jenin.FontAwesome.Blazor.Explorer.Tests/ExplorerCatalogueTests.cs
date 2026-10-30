//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Tests;

[TestClass]
public sealed class ExplorerCatalogueTests {
    private static ExplorerCatalogue Catalogue => ExplorerCatalogue.Default;

    [TestMethod]
    public void Catalogue_Loads_From_Embedded_Metadata() {
        Assert.IsGreaterThanOrEqualTo(1900, Catalogue.TotalCount);
        Assert.HasCount(IconMetadataProvider.Default.Icons.Count, Catalogue.Icons);
    }

    [TestMethod]
    public void Icons_Are_Ordered_By_Ordinal_Name() {
        var names = Catalogue.Icons.Select(static icon => icon.Name).ToList();
        var sorted = names.OrderBy(static name => name, StringComparer.Ordinal).ToList();

        Assert.AreSequenceEqual(sorted, names);
    }

    [TestMethod]
    public void Style_Counts_Match_Metadata() {
        foreach(var style in new[] { IconStyle.Solid, IconStyle.Regular, IconStyle.Brands }) {
            var expected = IconMetadataProvider.Default.Icons.Count(icon => icon.SupportsStyle(style));
            Assert.AreEqual(expected, Catalogue.CountForStyle(style), style.ToString());
        }
    }

    [TestMethod]
    [DataRow("house", "house", DisplayName = "canonical name")]
    [DataRow("fa-house", "house", DisplayName = "fa- prefix")]
    [DataRow("HOUSE", "house", DisplayName = "upper case")]
    [DataRow("fa-HOUSE", "house", DisplayName = "prefix + case")]
    [DataRow("  house  ", "house", DisplayName = "trim")]
    public void Find_Resolves_Name_Or_Class(string input, string expected) {
        var entry = Catalogue.Find(input);

        Assert.IsNotNull(entry);
        Assert.AreEqual(expected, entry.Name);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("definitely-not-an-icon")]
    public void Find_Unknown_Or_Empty_Returns_Null(string input) => Assert.IsNull(Catalogue.Find(input));

    [TestMethod]
    public void Find_Matches_Provider() {
        var provider = IconMetadataProvider.Default;

        foreach(var name in new[] { "house", "github", "0", "star" }) {
            var expected = provider.Find(name);
            var actual = Catalogue.Find(name);

            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Name, actual.Name);
        }
    }

    [TestMethod]
    public void Empty_Query_Returns_Full_Style_Ordered_Set() {
        var results = Catalogue.Search(null);

        Assert.HasCount(Catalogue.TotalCount, results);
        Assert.AreSequenceEqual(
            Catalogue.Icons.Select(static icon => icon.Name),
            results.Select(static icon => icon.Name));
    }

    [TestMethod]
    public void Empty_Query_Respects_Style_Facet() {
        var results = Catalogue.Search("   ", IconStyle.Brands);

        Assert.HasCount(Catalogue.CountForStyle(IconStyle.Brands), results);
        Assert.IsTrue(results.All(static icon => icon.SupportsStyle(IconStyle.Brands)));
    }

    // Normative IE-02 B.1: a single-token query must yield the same result set the library search
    // yields. The explorer tokenises on whitespace, so parity is asserted over whitespace-free
    // tokens (canonical names plus the defining alias/keyword vocabulary from the provider).
    [TestMethod]
    public void Single_Token_Search_Matches_Provider_Search() {
        var provider = IconMetadataProvider.Default;
        var tokens = CollectSingleTokens(provider);

        foreach(var token in tokens) {
            var expected = provider.Search(token).Select(static icon => icon.Name).ToList();
            var actual = Catalogue.Search(token).Select(static entry => entry.Name).ToList();

            Assert.AreSequenceEqual(expected, actual, $"token '{token}'");
        }
    }

    [TestMethod]
    public void Single_Token_Search_Matches_Provider_Search_With_Style_Facet() {
        var provider = IconMetadataProvider.Default;
        var tokens = new[] { "star", "house", "circle", "arrow" };

        foreach(var token in tokens) {
            foreach(var style in new[] { IconStyle.Solid, IconStyle.Regular, IconStyle.Brands }) {
                var expected = provider.Search(token, style).Select(static icon => icon.Name).ToList();
                var actual = Catalogue.Search(token, style).Select(static entry => entry.Name).ToList();

                Assert.AreSequenceEqual(expected, actual, $"token '{token}', style {style}");
            }
        }
    }

    [TestMethod]
    public void Multi_Token_Search_Applies_And_Rule() {
        // "home" (an alias of house) and "residence" (a search term of house) both resolve to house,
        // so the AND of the two tokens keeps it; a token that matches nothing excludes everything.
        Assert.IsEmpty(Catalogue.Search("house zzzznotarealicon"));

        var results = Catalogue.Search("home residence");

        Assert.IsNotEmpty(results);
        Assert.AreEqual("house", results[0].Name);
    }

    [TestMethod]
    public void Exact_Name_Ranks_First() {
        var results = Catalogue.Search("house");

        Assert.IsNotEmpty(results);
        Assert.AreEqual("house", results[0].Name);
    }

    [TestMethod]
    public void Search_Is_Deterministic() {
        var first = Catalogue.Search("home").Select(static entry => entry.Name).ToList();
        var second = Catalogue.Search("home").Select(static entry => entry.Name).ToList();

        Assert.AreSequenceEqual(first, second);
    }

    private static List<string> CollectSingleTokens(IconMetadataProvider provider) {
        var tokens = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        void Add(string value) {
            // The explorer tokenises on whitespace, so only whitespace-free values are single tokens.
            if(!value.Any(char.IsWhiteSpace)) {
                tokens.Add(value);
            }
        }

        foreach(var icon in provider.Icons) {
            Add(icon.Name);

            foreach(var alias in icon.Aliases) {
                Add(alias);
            }

            foreach(var term in icon.SearchTerms) {
                Add(term);
            }
        }

        return [.. tokens];
    }
}
