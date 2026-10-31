//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class CustomIconTests {
    private const string PATH = "M0 0H24V24H0Z";
    private const string VIEWBOX = "0 0 24 24";

    [TestMethod]
    [TestCategory("Model")]
    public void RegisterCustomIcon_StoresDefinition() {
        var options = new FontAwesomeOptions();

        var definition = options.RegisterCustomIcon("fa-acme-widget", PATH);

        options.CustomIcons["fa-acme-widget"].Should().BeSameAs(definition);
        definition.ViewBox.Should().Be("0 0 512 512");
        definition.Label.Should().Be("fa-acme-widget");
    }

    [TestMethod]
    [TestCategory("Model")]
    [DataRow("")]
    [DataRow("acme-widget")]
    [DataRow("fa-")]
    public void RegisterCustomIcon_RejectsBadNames(string name) => FluentActions.Invoking(() => new FontAwesomeOptions().RegisterCustomIcon(name, PATH)).Should().ThrowExactly<ArgumentException>();

    [TestMethod]
    [TestCategory("Model")]
    public void RegisterCustomIcon_RejectsEmptyPath() => FluentActions.Invoking(() => new FontAwesomeOptions().RegisterCustomIcon("fa-acme-widget", "  ")).Should().ThrowExactly<ArgumentException>();

    [TestMethod]
    [TestCategory("Model")]
    public void ToRazorSnippet_EmitsIconUsage() {
        var definition = new CustomIconDefinition("fa-acme-widget", PATH);

        definition.ToRazorSnippet().Should().Be("<Icon IconName=\"fa-acme-widget\" />");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task RegisteredName_RendersInlineSvgAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-acme-widget")),
            services => services.AddJeninFontAwesome(options => options.RegisterCustomIcon("fa-acme-widget", PATH, VIEWBOX)));

        markup.Should().Be(
            "<span><svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 24 24\" height=\"1em\" aria-hidden=\"true\" role=\"presentation\" data-icon=\"acme-widget\"><path d=\"M0 0H24V24H0Z\" fill=\"currentColor\"></path></svg></span>"
        );
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task CustomIcon_UsesTitleAndCssClassAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(
                ("IconName", "fa-acme-widget"),
                ("Title", "Acme"),
                ("CssClass", "hero")),
            services => services.AddJeninFontAwesome(options => options.RegisterCustomIcon("fa-acme-widget", PATH)));

        markup.Should().Contain("class=\"hero\"");
        markup.Should().Contain("title=\"Acme\"");
        markup.Should().Contain("role=\"img\"");
        markup.Should().Contain("<title>Acme</title>");
        markup.Should().Contain("viewBox=\"0 0 512 512\"");
        markup.Should().NotContain("fa-solid");
        markup.Replace("<title>Acme</title>", string.Empty, StringComparison.Ordinal).Should().NotContain("fa-acme-widget\"");
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task CustomIcon_HonorsWidthAndHeightAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(
                ("IconName", "fa-acme-widget"),
                ("AdditionalAttributes", new Dictionary<string, object?> { ["width"] = "64", ["height"] = "64" })),
            services => services.AddJeninFontAwesome(options => options.RegisterCustomIcon("fa-acme-widget", PATH)));

        markup.Should().Contain("width=\"64\"");
        (markup.Split("height=\"64\"", StringSplitOptions.None).Length - 1).Should().Be(1);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task UnregisteredName_KeepsClassicOutputAsync() {
        // Unknown, unregistered names still flow to Font Awesome (kit resolution, aliases,
        // future glyphs) and warn via JFA1001 at compile time — rendering stays unchanged.
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-not-registered"),
            ("IconStyle", IconStyle.Solid)));

        markup.Should().Be(
            "<span><i class=\"fa-solid fa-not-registered\" aria-hidden=\"true\" role=\"presentation\"></i></span>"
        );
    }
}
