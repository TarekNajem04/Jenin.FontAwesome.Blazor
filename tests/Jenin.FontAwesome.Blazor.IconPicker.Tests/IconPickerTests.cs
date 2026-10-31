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

        markup.Should().Contain("fa-icon-picker__search");
        markup.Should().Contain("fa-icon-picker__filter");
        markup.Should().Contain("Search icons...");
    }

    [TestMethod]
    public async Task RendersResultsForInitialSearchTextAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(TestHost.Parameters(("SearchText", "house")));

        markup.Should().Contain("fa-house");
        markup.Should().Contain(ITEM_TOKEN);
        markup.Should().Contain("House");
    }

    [TestMethod]
    public async Task FiltersOutNonMatchingIconsAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(TestHost.Parameters(("SearchText", "house")));

        markup.Contains("fa-github", StringComparison.Ordinal).Should().BeFalse();
    }

    [TestMethod]
    public async Task NoResultsRendersEmptyMessageAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(TestHost.Parameters(("SearchText", "zzzznotarealicon")));

        markup.Should().Contain("No icons found.");
        CountOccurrences(markup, ITEM_TOKEN).Should().Be(0);
    }

    [TestMethod]
    public async Task NoResultsTextIsConfigurableAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(("SearchText", "zzzznotarealicon"), ("NoResultsText", "Nothing here")));

        markup.Should().Contain("Nothing here");
    }

    [TestMethod]
    public async Task ShowLabelsFalseOmitsLabelsAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(("SearchText", "house"), ("ShowLabels", false)));

        markup.Should().Contain("fa-house");
        markup.Contains("fa-icon-picker__label", StringComparison.Ordinal).Should().BeFalse();
    }

    [TestMethod]
    public async Task StyleFilterRestrictsResultsAsync() {
        var brands = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(("SearchText", "github"), ("Style", IconStyle.Brands)));

        brands.Should().Contain("fa-github");

        var regular = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(("SearchText", "github"), ("Style", IconStyle.Regular)));

        regular.Should().Contain("No icons found.");
    }

    [TestMethod]
    public async Task CssClassIsAppliedAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(TestHost.Parameters(("CssClass", "my-picker")));

        markup.Should().Contain("my-picker");
    }

    [TestMethod]
    public async Task MaxResultsLimitsRenderedItemsAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(("SearchText", string.Empty), ("MaxResults", 5)));

        CountOccurrences(markup, ITEM_TOKEN).Should().Be(5);
    }

    [TestMethod]
    public async Task RendersIconWithPrimaryStyleAsync() {
        var house = await TestHost.RenderMarkupAsync<IconPicker>(TestHost.Parameters(("SearchText", "house")));
        house.Should().Contain("fa-solid");
        house.Should().Contain("fa-house");

        var github = await TestHost.RenderMarkupAsync<IconPicker>(TestHost.Parameters(("SearchText", "github")));
        github.Should().Contain("fa-brands");
        github.Should().Contain("fa-github");
    }

    [TestMethod]
    public async Task SelectedValueMarksMatchingItemAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(("SearchText", "house"), ("Value", "fa-house")));

        markup.Should().Contain("is-selected");
    }

    [TestMethod]
    public async Task ProviderIsResolvedFromDependencyInjectionAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(("SearchText", "house")),
            static services => services.AddJeninFontAwesomeIconPicker());

        markup.Should().Contain("fa-house");
    }

    [TestMethod]
    public async Task PlaceholderIsConfigurableAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(TestHost.Parameters(("Placeholder", "Find an icon")));

        markup.Should().Contain("Find an icon");
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
