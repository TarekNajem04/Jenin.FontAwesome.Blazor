//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.IconPicker.Tests;

[TestClass]
public class CustomIconMetadataTests {
    private static IconMetadataProvider ProviderWithAcme() {
        var options = new FontAwesomeOptions();
        options.RegisterCustomIcon("fa-acme-widget", "M0 0H24V24H0Z", "0 0 24 24", "Acme Widget", ["acme", "gadget"]);

        return IconMetadataProvider.Default.WithCustomIcons(options.CustomIcons.Values);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Find_ResolvesCustomByNameAndClass() {
        var provider = ProviderWithAcme();

        (provider.Find("acme-widget")?.Name).Should().Be("acme-widget");
        (provider.Find("fa-acme-widget")?.Name).Should().Be("acme-widget");
        provider.Find("fa-acme-widget")!.IsCustom.Should().BeTrue();
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Search_FindsCustomByNameAliasAndTerm() {
        var provider = ProviderWithAcme();

        provider.Search("acme-widget").Should().Contain(static icon => icon.Name == "acme-widget");
        provider.Search("gadget").Should().Contain(static icon => icon.Name == "acme-widget");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Search_IncludesCustomRegardlessOfStyle() {
        var provider = ProviderWithAcme();

        // Custom glyphs are style-agnostic: a style filter never excludes them.
        provider.Search("acme", IconStyle.Brands).Should().Contain(static icon => icon.Name == "acme-widget");
        provider.Search("acme", IconStyle.Thin).Should().Contain(static icon => icon.Name == "acme-widget");
    }

    [TestMethod]
    [TestCategory("Model")]
    public void WithCustomIcons_LeavesSharedCatalogueUntouched() {
        var before = IconMetadataProvider.Default.Icons.Count;
        var provider = ProviderWithAcme();

        provider.Icons.Count.Should().Be(before + 1);
        IconMetadataProvider.Default.Icons.Count.Should().Be(before);
        provider.Find("house")!.IsCustom.Should().BeFalse();
        // Empty overlays are free and preserve existing customs.
        IconMetadataProvider.Default.WithCustomIcons([]).Should().BeSameAs(IconMetadataProvider.Default);
        (provider.WithCustomIcons([]).Find("fa-acme-widget")?.Name).Should().Be("acme-widget");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Picker_MarksCustomIconsDistinctlyAsync() {
        var provider = ProviderWithAcme();
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(
                ("SearchText", "acme-widget"),
                ("Provider", provider)));

        markup.Should().Contain("is-custom");
        markup.Should().Contain("fa-acme-widget");
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public void AddJeninFontAwesomeIconPicker_FoldsInRegisteredCustoms() {
        var services = new ServiceCollection();
        services.AddJeninFontAwesome(options => options.RegisterCustomIcon("fa-acme-widget", "M0 0H24V24H0Z"));
        services.AddJeninFontAwesomeIconPicker();

        using var provider = services.BuildServiceProvider();
        var metadata = provider.GetRequiredService<IconMetadataProvider>();

        (metadata.Find("fa-acme-widget")?.Name).Should().Be("acme-widget");
        metadata.Find("fa-acme-widget")!.IsCustom.Should().BeTrue();
    }
}
