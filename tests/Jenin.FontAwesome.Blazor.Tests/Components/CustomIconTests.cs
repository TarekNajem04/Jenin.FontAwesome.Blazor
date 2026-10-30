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

        Assert.AreSame(definition, options.CustomIcons["fa-acme-widget"]);
        Assert.AreEqual("0 0 512 512", definition.ViewBox);
        Assert.AreEqual("fa-acme-widget", definition.Label);
    }

    [TestMethod]
    [TestCategory("Model")]
    [DataRow("")]
    [DataRow("acme-widget")]
    [DataRow("fa-")]
    public void RegisterCustomIcon_RejectsBadNames(string name) {
        Assert.ThrowsExactly<ArgumentException>(() => new FontAwesomeOptions().RegisterCustomIcon(name, PATH));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void RegisterCustomIcon_RejectsEmptyPath() {
        Assert.ThrowsExactly<ArgumentException>(() => new FontAwesomeOptions().RegisterCustomIcon("fa-acme-widget", "  "));
    }

    [TestMethod]
    [TestCategory("Model")]
    public void ToRazorSnippet_EmitsIconUsage() {
        var definition = new CustomIconDefinition("fa-acme-widget", PATH);

        Assert.AreEqual("<Icon IconName=\"fa-acme-widget\" />", definition.ToRazorSnippet());
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task RegisteredName_RendersInlineSvgAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-acme-widget")),
            services => services.AddJeninFontAwesome(options => options.RegisterCustomIcon("fa-acme-widget", PATH, VIEWBOX)));

        Assert.AreEqual(
            "<span><svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 24 24\" height=\"1em\" aria-hidden=\"true\" role=\"presentation\" data-icon=\"acme-widget\"><path d=\"M0 0H24V24H0Z\" fill=\"currentColor\"></path></svg></span>",
            markup);
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

        Assert.Contains("class=\"hero\"", markup);
        Assert.Contains("title=\"Acme\"", markup);
        Assert.Contains("role=\"img\"", markup);
        Assert.Contains("<title>Acme</title>", markup);
        Assert.Contains("viewBox=\"0 0 512 512\"", markup);
        Assert.DoesNotContain("fa-solid", markup);
        Assert.DoesNotContain("fa-acme-widget\"", markup.Replace("<title>Acme</title>", string.Empty, StringComparison.Ordinal));
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task CustomIcon_HonorsWidthAndHeightAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(
                ("IconName", "fa-acme-widget"),
                ("AdditionalAttributes", new Dictionary<string, object?> { ["width"] = "64", ["height"] = "64" })),
            services => services.AddJeninFontAwesome(options => options.RegisterCustomIcon("fa-acme-widget", PATH)));

        Assert.Contains("width=\"64\"", markup);
        Assert.AreEqual(1, markup.Split("height=\"64\"", StringSplitOptions.None).Length - 1);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task UnregisteredName_KeepsClassicOutputAsync() {
        // Unknown, unregistered names still flow to Font Awesome (kit resolution, aliases,
        // future glyphs) and warn via JFA1001 at compile time — rendering stays unchanged.
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-not-registered"),
            ("IconStyle", IconStyle.Solid)));

        Assert.AreEqual(
            "<span><i class=\"fa-solid fa-not-registered\" aria-hidden=\"true\" role=\"presentation\"></i></span>",
            markup);
    }
}
