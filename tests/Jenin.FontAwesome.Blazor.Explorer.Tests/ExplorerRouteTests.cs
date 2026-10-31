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
        ExplorerRoute.Parse(null).Should().Be(ExplorerRouteState.Empty);
        ExplorerRoute.Parse(string.Empty).Should().Be(ExplorerRouteState.Empty);
        ExplorerRoute.Parse("?").Should().Be(ExplorerRouteState.Empty);
    }

    [TestMethod]
    public void Parse_Reads_All_Known_Keys() {
        var state = ExplorerRoute.Parse("?q=house&style=brands&icon=github");

        state.Query.Should().Be("house");
        state.Style.Should().Be(IconStyle.Brands);
        state.Icon.Should().Be("github");
    }

    [TestMethod]
    public void Parse_Ignores_Unknown_Keys() {
        var state = ExplorerRoute.Parse("?q=house&unknown=1&style=regular");

        state.Query.Should().Be("house");
        state.Style.Should().Be(IconStyle.Regular);
        state.Icon.Should().BeNull();
    }

    [TestMethod]
    [DataRow("solid", IconStyle.Solid)]
    [DataRow("regular", IconStyle.Regular)]
    [DataRow("brands", IconStyle.Brands)]
    [DataRow("BRANDS", IconStyle.Brands)]
    public void Parse_Recognises_Known_Style_Tokens(string token, IconStyle expected) {
        var state = ExplorerRoute.Parse($"?style={token}");

        state.Style.Should().Be(expected);
    }

    [TestMethod]
    [DataRow("light")]
    [DataRow("thing")]
    [DataRow("")]
    public void Parse_Rejects_Unknown_Style_Tokens(string token) {
        var state = ExplorerRoute.Parse($"?style={token}");

        state.Style.Should().BeNull();
    }

    [TestMethod]
    public void Parse_Decodes_Encoded_Query_Values() {
        var state = ExplorerRoute.Parse("?q=house%20with%20space");

        state.Query.Should().Be("house with space");
    }

    [TestMethod]
    public void Parse_Blank_Icon_Becomes_Null() {
        var state = ExplorerRoute.Parse("?icon=");

        state.Icon.Should().BeNull();
    }

    [TestMethod]
    public void Build_Omits_Defaults() {
        var uri = ExplorerRoute.Build(Navigation, ExplorerRouteState.Empty);

        uri.Should().Be("https://example.test/explorer");
    }

    [TestMethod]
    public void Build_Encodes_Non_Default_State() {
        var uri = ExplorerRoute.Build(Navigation, new ExplorerRouteState("house", IconStyle.Brands, "github"));

        uri.Should().Be("https://example.test/explorer?q=house&style=brands&icon=github");
    }

    [TestMethod]
    public void Build_Trims_Query_And_Omits_Blank() {
        var uri = ExplorerRoute.Build(Navigation, new ExplorerRouteState("   ", null, "  "));

        uri.Should().Be("https://example.test/explorer");
    }

    [TestMethod]
    public void Build_Preserves_Base_Path_Then_Appends_Explorer() {
        var uri = ExplorerRoute.Build(NavigationAt("https://example.test/app/"), new ExplorerRouteState("house", null, null));

        uri.Should().Be("https://example.test/app/explorer?q=house");
    }

    [TestMethod]
    public void Parse_Build_Round_Trips() {
        var original = new ExplorerRouteState("house arrow", IconStyle.Regular, "0");

        var uri = ExplorerRoute.Build(Navigation, original);
        var query = uri[uri.IndexOf('?', StringComparison.Ordinal)..];
        var roundTripped = ExplorerRoute.Parse(query);

        roundTripped.Should().Be(original);
    }

    private sealed class TestNavigationManager : NavigationManager {
        public TestNavigationManager(string baseUri) => Initialize(baseUri, baseUri);

        protected override void NavigateToCore(string uri, bool forceLoad) {
        }
    }
}
