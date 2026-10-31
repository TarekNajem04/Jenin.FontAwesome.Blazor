//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.IconPicker.Tests;

[TestClass]
public sealed class IconMetadataProviderTests {
    private static IconMetadataProvider Provider => IconMetadataProvider.Default;

    [TestMethod]
    public void Icons_AreLoaded_From_Embedded_Metadata() {
        Provider.Icons.Count.Should().BeGreaterThanOrEqualTo(1900);
        Provider.Find("house").Should().NotBeNull();
    }

    [TestMethod]
    public void Find_By_Name_Returns_Metadata() {
        var icon = Provider.Find("house");

        icon.Should().NotBeNull();
        icon!.Name.Should().Be("house");
        icon.CssClass.Should().Be("fa-house");
        icon.Label.Should().Be("House");
        icon.ToString().Should().Be("house");
        icon.SupportsStyle(IconStyle.Solid).Should().BeTrue();
        icon.SupportsStyle(IconStyle.Regular).Should().BeTrue();
        icon.SupportsStyle(IconStyle.Brands).Should().BeFalse();
        icon.PrimaryStyle.Should().Be(IconStyle.Solid);
    }

    [TestMethod]
    public void Find_By_Css_Class_Returns_Metadata() {
        var byClass = Provider.Find("fa-house");
        var byName = Provider.Find("house");

        byClass.Should().NotBeNull();
        byClass.Should().BeSameAs(byName);
    }

    [TestMethod]
    [DataRow("HOUSE")]
    [DataRow("House")]
    [DataRow("fa-HOUSE")]
    [DataRow("  house  ")]
    public void Find_Is_Case_Insensitive_And_Trims(string query) {
        var icon = Provider.Find(query);

        icon.Should().NotBeNull();
        icon!.Name.Should().Be("house");
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("definitely-not-an-icon")]
    public void Find_Unknown_Returns_Null(string query) => Provider.Find(query).Should().BeNull();

    [TestMethod]
    public void Brands_Icon_Uses_Brands_Primary_Style() {
        var icon = Provider.Find("github");

        icon.Should().NotBeNull();
        icon!.PrimaryStyle.Should().Be(IconStyle.Brands);
        icon.SupportsStyle(IconStyle.Brands).Should().BeTrue();
        icon.SupportsStyle(IconStyle.Solid).Should().BeFalse();
    }

    [TestMethod]
    public void Search_By_Exact_Name_Ranks_First() {
        var results = Provider.Search("house");

        results.Should().NotBeEmpty();
        results[0].Name.Should().Be("house");
    }

    [TestMethod]
    public void Search_By_Prefix_Finds_Icon() {
        var results = Provider.Search("mug");

        results.Should().Contain(static icon => icon.Name == "mug-hot");
    }

    [TestMethod]
    public void Search_By_Alias_Finds_Canonical_Icon() {
        var results = Provider.Search("home");

        results.Should().NotBeEmpty();
        results[0].Name.Should().Be("house");
    }

    [TestMethod]
    public void Search_By_Keyword_Finds_Icon() {
        var results = Provider.Search("zero");

        results.Should().Contain(static icon => icon.Name == "0");
    }

    [TestMethod]
    public void Search_Filters_By_Style() {
        Provider.Search("github", IconStyle.Brands).Should().Contain(static icon => icon.Name == "github");
        Provider.Search("github", IconStyle.Solid).Should().NotContain(static icon => icon.Name == "github");
        Provider.Search("github", IconStyle.Regular).Should().BeEmpty();
    }

    [TestMethod]
    public void Search_With_Empty_Query_Returns_All_Icons() {
        Provider.Search(null).Should().HaveCount(Provider.Icons.Count);
        Provider.Search("").Should().HaveCount(Provider.Icons.Count);
        Provider.Search("   ").Should().HaveCount(Provider.Icons.Count);
    }

    [TestMethod]
    public void Search_Respects_Max_Results() {
        Provider.Search(null, maxResults: 10).Should().HaveCount(10);
        Provider.Search("a", maxResults: 5).Should().HaveCount(5);
    }

    [TestMethod]
    public void Search_Is_Deterministic() {
        var first = Provider.Search("home");
        var second = Provider.Search("home");

        second.Select(static icon => icon.Name).Should().Equal(first.Select(static icon => icon.Name));
    }

    [TestMethod]
    public void Search_Results_Respect_Style_Filter() {
        var results = Provider.Search("star", IconStyle.Regular);

        results.Should().NotBeEmpty();
        results.All(static icon => icon.SupportsStyle(IconStyle.Regular)).Should().BeTrue();
    }

    [TestMethod]
    public void Default_And_New_Instances_Share_The_Catalogue() {
        var other = new IconMetadataProvider();

        ReferenceEquals(other.Icons, Provider.Icons).Should().BeTrue();
    }

    [TestMethod]
    public void Icons_Are_Ordered_By_Name() {
        var names = Provider.Icons.Select(static icon => icon.Name).ToList();
        var sorted = names.OrderBy(static name => name, StringComparer.Ordinal).ToList();

        names.Should().Equal(sorted);
    }

    [TestMethod]
    public void Metadata_Exposes_Search_Terms_And_Aliases() {
        var icon = Provider.Find("house");

        icon.Should().NotBeNull();
        icon!.Aliases.Should().Contain("home");
        icon.SearchTerms.Should().Contain("residence");
        icon.SearchTerms.Should().NotContain("home-alt");
    }
}
