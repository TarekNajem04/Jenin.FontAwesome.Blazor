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
        Catalogue.TotalCount.Should().BeGreaterThanOrEqualTo(1900);
        Catalogue.Icons.Should().HaveCount(IconMetadataProvider.Default.Icons.Count);
    }

    [TestMethod]
    public void Icons_Are_Ordered_By_Ordinal_Name() {
        var names = Catalogue.Icons.Select(static icon => icon.Name).ToList();
        var sorted = names.OrderBy(static name => name, StringComparer.Ordinal).ToList();

        names.Should().Equal(sorted);
    }

    [TestMethod]
    public void Style_Counts_Match_Metadata() {
        foreach(var style in new[] { IconStyle.Solid, IconStyle.Regular, IconStyle.Brands }) {
            var expected = IconMetadataProvider.Default.Icons.Count(icon => icon.SupportsStyle(style));
            Catalogue.CountForStyle(style).Should().Be(expected, style.ToString());
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

        entry.Should().NotBeNull();
        entry!.Name.Should().Be(expected);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("definitely-not-an-icon")]
    public void Find_Unknown_Or_Empty_Returns_Null(string input) => Catalogue.Find(input).Should().BeNull();

    [TestMethod]
    public void Find_Matches_Provider() {
        var provider = IconMetadataProvider.Default;

        foreach(var name in new[] { "house", "github", "0", "star" }) {
            var expected = provider.Find(name);
            var actual = Catalogue.Find(name);

            expected.Should().NotBeNull();
            actual.Should().NotBeNull();
            actual!.Name.Should().Be(expected!.Name);
        }
    }

    [TestMethod]
    public void Empty_Query_Returns_Full_Style_Ordered_Set() {
        var results = Catalogue.Search(null);

        results.Should().HaveCount(Catalogue.TotalCount);
        results.Select(static icon => icon.Name).Should().Equal(Catalogue.Icons.Select(static icon => icon.Name));
    }

    [TestMethod]
    public void Empty_Query_Respects_Style_Facet() {
        var results = Catalogue.Search("   ", IconStyle.Brands);

        results.Should().HaveCount(Catalogue.CountForStyle(IconStyle.Brands));
        results.All(static icon => icon.SupportsStyle(IconStyle.Brands)).Should().BeTrue();
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

            actual.Should().Equal(expected, $"token '{token}'");
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

                actual.Should().Equal(expected, $"token '{token}', style {style}");
            }
        }
    }

    [TestMethod]
    public void Multi_Token_Search_Applies_And_Rule() {
        // "home" (an alias of house) and "residence" (a search term of house) both resolve to house,
        // so the AND of the two tokens keeps it; a token that matches nothing excludes everything.
        Catalogue.Search("house zzzznotarealicon").Should().BeEmpty();

        var results = Catalogue.Search("home residence");

        results.Should().NotBeEmpty();
        results[0].Name.Should().Be("house");
    }

    [TestMethod]
    public void Exact_Name_Ranks_First() {
        var results = Catalogue.Search("house");

        results.Should().NotBeEmpty();
        results[0].Name.Should().Be("house");
    }

    [TestMethod]
    public void Search_Is_Deterministic() {
        var first = Catalogue.Search("home").Select(static entry => entry.Name).ToList();
        var second = Catalogue.Search("home").Select(static entry => entry.Name).ToList();

        second.Should().Equal(first);
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
