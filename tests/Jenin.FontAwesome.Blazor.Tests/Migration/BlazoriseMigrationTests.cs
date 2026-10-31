//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Migration;

/// <summary>
/// Verifies the Blazorise migration guide (<c>docs/migration/from-blazorise.md</c>): every
/// mapping-table row's target side renders the documented Font Awesome output. Source excerpts
/// follow the Blazorise icons docs (v2.4.x) as of 2026-10-26.
/// </summary>
[TestClass]
public class BlazoriseMigrationTests {
    [TestMethod]
    [TestCategory("Rendering")]
    public async Task SharedEnumName_MapsToConcreteIconAsync() {
        // Before: <Icon Name="IconName.Mail" />
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-envelope")));

        markup.Should().Contain("class=\"fa-solid fa-envelope\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ProviderConstant_MapsToCatalogueConstantAsync() {
        // Before: <Icon Name="Blazorise.Icons.FontAwesome.FontAwesomeIcons.Phone" />
        // ("fa-phone" on both sides; the render below proves the catalogue constant resolves.)
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", IconNames.Solid.Phone),
            ("IconStyle", IconStyle.Solid)));

        markup.Should().Contain("class=\"fa-solid fa-phone\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task CustomStringName_MapsDirectlyAsync() {
        // Before: <Icon Name="@("fa-phone")" />
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-phone")));

        markup.Should().Contain("fa-phone");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task BlazoriseStyle_MapsToSameTokenAsync() {
        // Before: <Icon Name="IconName.Mail" IconStyle="IconStyle.Regular" />
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-envelope"),
            ("IconStyle", IconStyle.Regular)));

        markup.Should().Contain("class=\"fa-regular fa-envelope\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task BlazoriseSizes_MapTokenForTokenAsync() {
        // Before: IconSize="IconSize.Small" / Large / x2
        var small = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-envelope"),
            ("IconSize", IconSize.Small)));
        var large = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-envelope"),
            ("IconSize", IconSize.Large)));
        var x2 = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-envelope"),
            ("IconSize", IconSize.X02)));

        small.Should().Contain("fa-sm");
        large.Should().Contain("fa-lg");
        x2.Should().Contain("fa-2x");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task GlobalOptions_MoveToServiceDefaultsAsync() {
        // Before: services.AddBlazorise(options => { options.IconStyle = ...; options.IconSize = ...; })
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-envelope")),
            services => services.AddJeninFontAwesome(options => {
                options.IconStyle = IconStyle.Regular;
                options.IconSize = IconSize.Large;
            }));

        markup.Should().Contain("class=\"fa-lg fa-regular fa-envelope\"");
    }
}
