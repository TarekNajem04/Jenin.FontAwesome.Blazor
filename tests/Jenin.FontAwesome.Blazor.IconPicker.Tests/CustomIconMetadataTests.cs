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

        Assert.AreEqual("acme-widget", provider.Find("acme-widget")?.Name);
        Assert.AreEqual("acme-widget", provider.Find("fa-acme-widget")?.Name);
        Assert.IsTrue(provider.Find("fa-acme-widget")!.IsCustom);
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Search_FindsCustomByNameAliasAndTerm() {
        var provider = ProviderWithAcme();

        Assert.IsTrue(provider.Search("acme-widget").Any(static icon => icon.Name == "acme-widget"));
        Assert.IsTrue(provider.Search("gadget").Any(static icon => icon.Name == "acme-widget"));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void Search_IncludesCustomRegardlessOfStyle() {
        var provider = ProviderWithAcme();

        // Custom glyphs are style-agnostic: a style filter never excludes them.
        Assert.IsTrue(provider.Search("acme", IconStyle.Brands).Any(static icon => icon.Name == "acme-widget"));
        Assert.IsTrue(provider.Search("acme", IconStyle.Thin).Any(static icon => icon.Name == "acme-widget"));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void WithCustomIcons_LeavesSharedCatalogueUntouched() {
        var before = IconMetadataProvider.Default.Icons.Count;
        var provider = ProviderWithAcme();

        Assert.AreEqual(before + 1, provider.Icons.Count);
        Assert.AreEqual(before, IconMetadataProvider.Default.Icons.Count);
        Assert.IsFalse(provider.Find("house")!.IsCustom);
        // Empty overlays are free and preserve existing customs.
        Assert.AreSame(IconMetadataProvider.Default, IconMetadataProvider.Default.WithCustomIcons([]));
        Assert.AreEqual("acme-widget", provider.WithCustomIcons([]).Find("fa-acme-widget")?.Name);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Picker_MarksCustomIconsDistinctlyAsync() {
        var provider = ProviderWithAcme();
        var markup = await TestHost.RenderMarkupAsync<IconPicker>(
            TestHost.Parameters(
                ("SearchText", "acme-widget"),
                ("Provider", provider)));

        Assert.Contains("is-custom", markup);
        Assert.Contains("fa-acme-widget", markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public void AddJeninFontAwesomeIconPicker_FoldsInRegisteredCustoms() {
        var services = new ServiceCollection();
        services.AddJeninFontAwesome(options => options.RegisterCustomIcon("fa-acme-widget", "M0 0H24V24H0Z"));
        services.AddJeninFontAwesomeIconPicker();

        using var provider = services.BuildServiceProvider();
        var metadata = provider.GetRequiredService<IconMetadataProvider>();

        Assert.AreEqual("acme-widget", metadata.Find("fa-acme-widget")?.Name);
        Assert.IsTrue(metadata.Find("fa-acme-widget")!.IsCustom);
    }
}
