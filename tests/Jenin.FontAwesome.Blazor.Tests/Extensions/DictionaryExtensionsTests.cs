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

        Assert.AreEqual("a", dictionary["k"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Combine_ExistingEntry_PrependsNewValue() {
        var dictionary = new Dictionary<string, string> { ["k"] = "a" };

        _ = dictionary.Combine("k", "b");

        Assert.AreEqual("ba", dictionary["k"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Concat_EmptyExistingValue_ReturnsValue() => Assert.AreEqual("a", DictionaryExtensions.Concat("a", ""));

    [TestMethod]
    [TestCategory("Model")]
    public void Concat_NonNullValues_ConcatenatesNewBeforeExisting() => Assert.AreEqual("ab", DictionaryExtensions.Concat("a", "b"));

    [TestMethod]
    [TestCategory("Model")]
    public void AddIfNotNull_SkipsNull() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddIfNotNull("k", (string?)null);

        Assert.IsFalse(dictionary.ContainsKey("k"));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddIfNotNull_SkipsEmptyString() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddIfNotNull("k", "");

        Assert.IsFalse(dictionary.ContainsKey("k"));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddIfNotNull_AddsNonNullValue() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddIfNotNull("k", "v");

        Assert.AreEqual("v", dictionary["k"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddIfNotNull_Float_FormatsInvariantlyWithTwoDecimals() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddIfNotNull("k", (float?)2f);

        Assert.AreEqual("2.00", dictionary["k"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddIfNotNull_Int_FormatsInvariantly() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddIfNotNull("k", (int?)5);

        Assert.AreEqual("5", dictionary["k"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddOrUpdate_OverwritesExisting() {
        var dictionary = new Dictionary<string, string> { ["k"] = "old" };

        _ = dictionary.AddOrUpdate("k", "new");

        Assert.AreEqual("new", dictionary["k"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddOrUpdateIfNotNull_Float_NullLeavesEntryAbsent() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddOrUpdateIfNotNull("k", (float?)null);

        Assert.IsFalse(dictionary.ContainsKey("k"));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddOrUpdateIfNotNull_Float_FormatsValue() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddOrUpdateIfNotNull("k", (float?)1.5f);

        Assert.AreEqual("1.50", dictionary["k"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddOrUpdateIf_Enum_RespectsPredicate() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddOrUpdateIf("k", IconSize.X02, static _ => true);

        Assert.AreEqual("fa-2x", dictionary["k"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void AddOrUpdateIf_Enum_PredicateFalse_LeavesEntryAbsent() {
        var dictionary = new Dictionary<string, string>();

        _ = dictionary.AddOrUpdateIf("k", IconSize.X02, static _ => false);

        Assert.IsFalse(dictionary.ContainsKey("k"));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void UpdateOrRemoveIf_NullValue_RemovesEntry() {
        var dictionary = new Dictionary<string, string> { ["k"] = "v" };

        _ = dictionary.UpdateOrRemoveIf("k", (string?)null);

        Assert.IsFalse(dictionary.ContainsKey("k"));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void UpdateOrRemoveIf_PredicateTrue_RemovesEntry() {
        var dictionary = new Dictionary<string, string> { ["k"] = "v" };

        _ = dictionary.UpdateOrRemoveIf("k", "v", static _ => true);

        Assert.IsFalse(dictionary.ContainsKey("k"));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void UpdateOrRemoveIf_PredicateFalse_UpdatesEntry() {
        var dictionary = new Dictionary<string, string> { ["k"] = "old" };

        _ = dictionary.UpdateOrRemoveIf("k", "new", static _ => false);

        Assert.AreEqual("new", dictionary["k"]);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void JoinAsStyles_EmptyDictionary_ReturnsEmptyString() => Assert.AreEqual("", new Dictionary<string, string>().JoinAsStyles());

    [TestMethod]
    [TestCategory("Model")]
    public void JoinAsStyles_EmitsEachEntryWithTrailingSemicolon() {
        var dictionary = new Dictionary<string, string> { ["a"] = "1", ["b"] = "2" };

        Assert.AreEqual("a:1;b:2;", dictionary.JoinAsStyles());
    }

    [TestMethod]
    [TestCategory("Model")]
    public void JoinAsStyles_SkipsEmptyValues() {
        var dictionary = new Dictionary<string, string> { ["a"] = "1", ["b"] = "" };

        Assert.AreEqual("a:1;", dictionary.JoinAsStyles());
    }
}
