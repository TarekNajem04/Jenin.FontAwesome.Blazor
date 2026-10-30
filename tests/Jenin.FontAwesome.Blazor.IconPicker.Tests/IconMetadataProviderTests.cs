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
        Assert.IsGreaterThanOrEqualTo(1900, Provider.Icons.Count);
        Assert.IsNotNull(Provider.Find("house"));
    }

    [TestMethod]
    public void Find_By_Name_Returns_Metadata() {
        var icon = Provider.Find("house");

        Assert.IsNotNull(icon);
        Assert.AreEqual("house", icon.Name);
        Assert.AreEqual("fa-house", icon.CssClass);
        Assert.AreEqual("House", icon.Label);
        Assert.AreEqual("house", icon.ToString());
        Assert.IsTrue(icon.SupportsStyle(IconStyle.Solid));
        Assert.IsTrue(icon.SupportsStyle(IconStyle.Regular));
        Assert.IsFalse(icon.SupportsStyle(IconStyle.Brands));
        Assert.AreEqual(IconStyle.Solid, icon.PrimaryStyle);
    }

    [TestMethod]
    public void Find_By_Css_Class_Returns_Metadata() {
        var byClass = Provider.Find("fa-house");
        var byName = Provider.Find("house");

        Assert.IsNotNull(byClass);
        Assert.AreSame(byName, byClass);
    }

    [TestMethod]
    [DataRow("HOUSE")]
    [DataRow("House")]
    [DataRow("fa-HOUSE")]
    [DataRow("  house  ")]
    public void Find_Is_Case_Insensitive_And_Trims(string query) {
        var icon = Provider.Find(query);

        Assert.IsNotNull(icon);
        Assert.AreEqual("house", icon.Name);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("definitely-not-an-icon")]
    public void Find_Unknown_Returns_Null(string query) => Assert.IsNull(Provider.Find(query));

    [TestMethod]
    public void Brands_Icon_Uses_Brands_Primary_Style() {
        var icon = Provider.Find("github");

        Assert.IsNotNull(icon);
        Assert.AreEqual(IconStyle.Brands, icon.PrimaryStyle);
        Assert.IsTrue(icon.SupportsStyle(IconStyle.Brands));
        Assert.IsFalse(icon.SupportsStyle(IconStyle.Solid));
    }

    [TestMethod]
    public void Search_By_Exact_Name_Ranks_First() {
        var results = Provider.Search("house");

        Assert.IsNotEmpty(results);
        Assert.AreEqual("house", results[0].Name);
    }

    [TestMethod]
    public void Search_By_Prefix_Finds_Icon() {
        var results = Provider.Search("mug");

        Assert.Contains(static icon => icon.Name == "mug-hot", results);
    }

    [TestMethod]
    public void Search_By_Alias_Finds_Canonical_Icon() {
        var results = Provider.Search("home");

        Assert.IsNotEmpty(results);
        Assert.AreEqual("house", results[0].Name);
    }

    [TestMethod]
    public void Search_By_Keyword_Finds_Icon() {
        var results = Provider.Search("zero");

        Assert.Contains(static icon => icon.Name == "0", results);
    }

    [TestMethod]
    public void Search_Filters_By_Style() {
        Assert.Contains(static icon => icon.Name == "github", Provider.Search("github", IconStyle.Brands));
        Assert.DoesNotContain(static icon => icon.Name == "github", Provider.Search("github", IconStyle.Solid));
        Assert.IsEmpty(Provider.Search("github", IconStyle.Regular));
    }

    [TestMethod]
    public void Search_With_Empty_Query_Returns_All_Icons() {
        Assert.HasCount(Provider.Icons.Count, Provider.Search(null));
        Assert.HasCount(Provider.Icons.Count, Provider.Search(""));
        Assert.HasCount(Provider.Icons.Count, Provider.Search("   "));
    }

    [TestMethod]
    public void Search_Respects_Max_Results() {
        Assert.HasCount(10, Provider.Search(null, maxResults: 10));
        Assert.HasCount(5, Provider.Search("a", maxResults: 5));
    }

    [TestMethod]
    public void Search_Is_Deterministic() {
        var first = Provider.Search("home");
        var second = Provider.Search("home");

        Assert.AreSequenceEqual(first.Select(static icon => icon.Name), second.Select(static icon => icon.Name));
    }

    [TestMethod]
    public void Search_Results_Respect_Style_Filter() {
        var results = Provider.Search("star", IconStyle.Regular);

        Assert.IsNotEmpty(results);
        Assert.IsTrue(results.All(static icon => icon.SupportsStyle(IconStyle.Regular)));
    }

    [TestMethod]
    public void Default_And_New_Instances_Share_The_Catalogue() {
        var other = new IconMetadataProvider();

        Assert.AreSame(Provider.Icons, other.Icons);
    }

    [TestMethod]
    public void Icons_Are_Ordered_By_Name() {
        var names = Provider.Icons.Select(static icon => icon.Name).ToList();
        var sorted = names.OrderBy(static name => name, StringComparer.Ordinal).ToList();

        Assert.AreSequenceEqual(sorted, names);
    }

    [TestMethod]
    public void Metadata_Exposes_Search_Terms_And_Aliases() {
        var icon = Provider.Find("house");

        Assert.IsNotNull(icon);
        Assert.Contains("home", icon.Aliases);
        Assert.Contains("residence", icon.SearchTerms);
        Assert.DoesNotContain("home-alt", icon.SearchTerms);
    }
}
