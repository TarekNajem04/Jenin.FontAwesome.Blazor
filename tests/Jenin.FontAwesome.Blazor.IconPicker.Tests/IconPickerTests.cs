//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.IconPicker.Tests;

[TestClass]
public sealed class IconPickerTests {
    private const string ITEM_TOKEN = "fa-icon-picker__item";

    [TestMethod]
    public async Task RendersSearchControlsAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>();

        Assert.Contains("fa-icon-picker__search", markup);
        Assert.Contains("fa-icon-picker__filter", markup);
        Assert.Contains("Search icons...", markup);
    }

    [TestMethod]
    public async Task RendersResultsForInitialSearchTextAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(TestHost.Parameters(("SearchText", "house")));

        Assert.Contains("fa-house", markup);
        Assert.Contains(ITEM_TOKEN, markup);
        Assert.Contains("House", markup);
    }

    [TestMethod]
    public async Task FiltersOutNonMatchingIconsAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(TestHost.Parameters(("SearchText", "house")));

        Assert.IsFalse(markup.Contains("fa-github", StringComparison.Ordinal));
    }

    [TestMethod]
    public async Task NoResultsRendersEmptyMessageAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(TestHost.Parameters(("SearchText", "zzzznotarealicon")));

        Assert.Contains("No icons found.", markup);
        Assert.AreEqual(0, CountOccurrences(markup, ITEM_TOKEN));
    }

    [TestMethod]
    public async Task NoResultsTextIsConfigurableAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(("SearchText", "zzzznotarealicon"), ("NoResultsText", "Nothing here")));

        Assert.Contains("Nothing here", markup);
    }

    [TestMethod]
    public async Task ShowLabelsFalseOmitsLabelsAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(("SearchText", "house"), ("ShowLabels", false)));

        Assert.Contains("fa-house", markup);
        Assert.IsFalse(markup.Contains("fa-icon-picker__label", StringComparison.Ordinal));
    }

    [TestMethod]
    public async Task StyleFilterRestrictsResultsAsync() {
        var brands = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(("SearchText", "github"), ("Style", IconStyle.Brands)));

        Assert.Contains("fa-github", brands);

        var regular = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(("SearchText", "github"), ("Style", IconStyle.Regular)));

        Assert.Contains("No icons found.", regular);
    }

    [TestMethod]
    public async Task CssClassIsAppliedAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(TestHost.Parameters(("CssClass", "my-picker")));

        Assert.Contains("my-picker", markup);
    }

    [TestMethod]
    public async Task MaxResultsLimitsRenderedItemsAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(("SearchText", string.Empty), ("MaxResults", 5)));

        Assert.AreEqual(5, CountOccurrences(markup, ITEM_TOKEN));
    }

    [TestMethod]
    public async Task RendersIconWithPrimaryStyleAsync() {
        var house = await TestHost.RenderMarkupAsync<IconPicker>(TestHost.Parameters(("SearchText", "house")));
        Assert.Contains("fa-solid", house);
        Assert.Contains("fa-house", house);

        var github = await TestHost.RenderMarkupAsync<IconPicker>(TestHost.Parameters(("SearchText", "github")));
        Assert.Contains("fa-brands", github);
        Assert.Contains("fa-github", github);
    }

    [TestMethod]
    public async Task SelectedValueMarksMatchingItemAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(("SearchText", "house"), ("Value", "fa-house")));

        Assert.Contains("is-selected", markup);
    }

    [TestMethod]
    public async Task ProviderIsResolvedFromDependencyInjectionAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(("SearchText", "house")),
            static services => services.AddJeninFontAwesomeIconPicker());

        Assert.Contains("fa-house", markup);
    }

    [TestMethod]
    public async Task PlaceholderIsConfigurableAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(TestHost.Parameters(("Placeholder", "Find an icon")));

        Assert.Contains("Find an icon", markup);
    }

    private static int CountOccurrences(string value, string token) {
        var count = 0;
        var index = 0;

        while((index = value.IndexOf(token, index, StringComparison.Ordinal)) >= 0) {
            count++;
            index += token.Length;
        }

        return count;
    }
}
