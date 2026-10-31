//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Extensions;

[TestClass]
public class DictionaryExtensionsTests {
    [TestMethod]
    [TestCategory("Model")]
    public void Combine_AddsNewEntry() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.Combine("k", "a");

        dictionary["k"].Should().Be("a");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Combine_ExistingEntry_PrependsNewValue() {
        var dictionary = new Dictionary<string, string> { ["k"] = "a" };

        _ = dictionary.Combine("k", "b");

        dictionary["k"].Should().Be("ba");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Concat_EmptyExistingValue_ReturnsValue() => DictionaryExtensions.Concat("a", "").Should().Be("a");

    [TestMethod]
    [TestCategory("Model")]
    public void Concat_NonNullValues_ConcatenatesNewBeforeExisting() => DictionaryExtensions.Concat("a", "b").Should().Be("ab");

    [TestMethod]
    [TestCategory("Model")]
    public void AddIfNotNull_SkipsNull() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddIfNotNull("k", (string?)null);

        dictionary.ContainsKey("k").Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddIfNotNull_SkipsEmptyString() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddIfNotNull("k", "");

        dictionary.ContainsKey("k").Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddIfNotNull_AddsNonNullValue() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddIfNotNull("k", "v");

        dictionary["k"].Should().Be("v");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddIfNotNull_Float_FormatsInvariantlyWithTwoDecimals() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddIfNotNull("k", (float?)2f);

        dictionary["k"].Should().Be("2.00");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddIfNotNull_Int_FormatsInvariantly() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddIfNotNull("k", (int?)5);

        dictionary["k"].Should().Be("5");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddOrUpdate_OverwritesExisting() {
        var dictionary = new Dictionary<string, string> { ["k"] = "old" };

        _ = dictionary.AddOrUpdate("k", "new");

        dictionary["k"].Should().Be("new");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddOrUpdateIfNotNull_Float_NullLeavesEntryAbsent() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddOrUpdateIfNotNull("k", (float?)null);

        dictionary.ContainsKey("k").Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddOrUpdateIfNotNull_Float_FormatsValue() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddOrUpdateIfNotNull("k", (float?)1.5f);

        dictionary["k"].Should().Be("1.50");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddOrUpdateIf_Enum_RespectsPredicate() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddOrUpdateIf("k", IconSize.X02, static _ => true);

        dictionary["k"].Should().Be("fa-2x");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddOrUpdateIf_Enum_PredicateFalse_LeavesEntryAbsent() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddOrUpdateIf("k", IconSize.X02, static _ => false);

        dictionary.ContainsKey("k").Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void UpdateOrRemoveIf_NullValue_RemovesEntry() {
        var dictionary = new Dictionary<string, string> { ["k"] = "v" };

        _ = dictionary.UpdateOrRemoveIf("k", (string?)null);

        dictionary.ContainsKey("k").Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void UpdateOrRemoveIf_PredicateTrue_RemovesEntry() {
        var dictionary = new Dictionary<string, string> { ["k"] = "v" };

        _ = dictionary.UpdateOrRemoveIf("k", "v", static _ => true);

        dictionary.ContainsKey("k").Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void UpdateOrRemoveIf_PredicateFalse_UpdatesEntry() {
        var dictionary = new Dictionary<string, string> { ["k"] = "old" };

        _ = dictionary.UpdateOrRemoveIf("k", "new", static _ => false);

        dictionary["k"].Should().Be("new");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void JoinAsStyles_EmptyDictionary_ReturnsEmptyString() => new Dictionary<string, string>().JoinAsStyles().Should().Be("");

    [TestMethod]
    [TestCategory("Model")]
    public void JoinAsStyles_EmitsEachEntryWithTrailingSemicolon() {
        var dictionary = new Dictionary<string, string> { ["a"] = "1", ["b"] = "2" };

        dictionary.JoinAsStyles().Should().Be("a:1;b:2;");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void JoinAsStyles_SkipsEmptyValues() {
        var dictionary = new Dictionary<string, string> { ["a"] = "1", ["b"] = "" };

        dictionary.JoinAsStyles().Should().Be("a:1;");
    }
}
