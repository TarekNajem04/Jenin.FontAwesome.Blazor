//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Tests;

[TestClass]
public sealed class ExplorerRouteTests {
    private static NavigationManager Navigation => new TestNavigationManager("https://example.test/");
    private static TestNavigationManager NavigationAt(string baseUri) => new(baseUri);

    [TestMethod]
    public void Parse_Null_Or_Empty_Returns_Empty_State() {
        Assert.AreEqual(ExplorerRouteState.Empty, ExplorerRoute.Parse(null));
        Assert.AreEqual(ExplorerRouteState.Empty, ExplorerRoute.Parse(string.Empty));
        Assert.AreEqual(ExplorerRouteState.Empty, ExplorerRoute.Parse("?"));
    }

    [TestMethod]
    public void Parse_Reads_All_Known_Keys() {
        var state = ExplorerRoute.Parse("?q=house&style=brands&icon=github");

        Assert.AreEqual("house", state.Query);
        Assert.AreEqual(IconStyle.Brands, state.Style);
        Assert.AreEqual("github", state.Icon);
    }

    [TestMethod]
    public void Parse_Ignores_Unknown_Keys() {
        var state = ExplorerRoute.Parse("?q=house&unknown=1&style=regular");

        Assert.AreEqual("house", state.Query);
        Assert.AreEqual(IconStyle.Regular, state.Style);
        Assert.IsNull(state.Icon);
    }

    [TestMethod]
    [DataRow("solid", IconStyle.Solid)]
    [DataRow("regular", IconStyle.Regular)]
    [DataRow("brands", IconStyle.Brands)]
    [DataRow("BRANDS", IconStyle.Brands)]
    public void Parse_Recognises_Known_Style_Tokens(string token, IconStyle expected) {
        var state = ExplorerRoute.Parse($"?style={token}");

        Assert.AreEqual(expected, state.Style);
    }

    [TestMethod]
    [DataRow("light")]
    [DataRow("thing")]
    [DataRow("")]
    public void Parse_Rejects_Unknown_Style_Tokens(string token) {
        var state = ExplorerRoute.Parse($"?style={token}");

        Assert.IsNull(state.Style);
    }

    [TestMethod]
    public void Parse_Decodes_Encoded_Query_Values() {
        var state = ExplorerRoute.Parse("?q=house%20with%20space");

        Assert.AreEqual("house with space", state.Query);
    }

    [TestMethod]
    public void Parse_Blank_Icon_Becomes_Null() {
        var state = ExplorerRoute.Parse("?icon=");

        Assert.IsNull(state.Icon);
    }

    [TestMethod]
    public void Build_Omits_Defaults() {
        var uri = ExplorerRoute.Build(Navigation, ExplorerRouteState.Empty);

        Assert.AreEqual("https://example.test/explorer", uri);
    }

    [TestMethod]
    public void Build_Encodes_Non_Default_State() {
        var uri = ExplorerRoute.Build(Navigation, new ExplorerRouteState("house", IconStyle.Brands, "github"));

        Assert.AreEqual("https://example.test/explorer?q=house&style=brands&icon=github", uri);
    }

    [TestMethod]
    public void Build_Trims_Query_And_Omits_Blank() {
        var uri = ExplorerRoute.Build(Navigation, new ExplorerRouteState("   ", null, "  "));

        Assert.AreEqual("https://example.test/explorer", uri);
    }

    [TestMethod]
    public void Build_Preserves_Base_Path_Then_Appends_Explorer() {
        var uri = ExplorerRoute.Build(NavigationAt("https://example.test/app/"), new ExplorerRouteState("house", null, null));

        Assert.AreEqual("https://example.test/app/explorer?q=house", uri);
    }

    [TestMethod]
    public void Parse_Build_Round_Trips() {
        var original = new ExplorerRouteState("house arrow", IconStyle.Regular, "0");

        var uri = ExplorerRoute.Build(Navigation, original);
        var query = uri[uri.IndexOf('?', StringComparison.Ordinal)..];
        var roundTripped = ExplorerRoute.Parse(query);

        Assert.AreEqual(original, roundTripped);
    }

    private sealed class TestNavigationManager : NavigationManager {
        public TestNavigationManager(string baseUri) => Initialize(baseUri, baseUri);

        protected override void NavigateToCore(string uri, bool forceLoad) {
        }
    }
}
