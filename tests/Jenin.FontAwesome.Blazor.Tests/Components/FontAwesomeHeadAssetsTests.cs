//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class FontAwesomeHeadAssetsTests {
    private const string NEST_CONFIG = "_content/Jenin.FontAwesome.Blazor/js/fontawesome-config-nest.js";
    private const string WEBFONT_CONFIG = "_content/Jenin.FontAwesome.Blazor/js/fontawesome-config-webfont.js";

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task Defaults_RenderNestConfigOnlyAsync() {
        var markup = await TestHost.RenderMarkupAsync<FontAwesomeHeadAssets>();

        Assert.AreEqual(
            $"<script src=\"{NEST_CONFIG}\" type=\"text/javascript\"></script>",
            markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task WebfontOption_RendersWebfontConfigAsync() {
        var markup = await TestHost.RenderMarkupAsync<FontAwesomeHeadAssets>(
            null,
            services => services.AddJeninFontAwesome(options => options.AssetMode = FontAwesomeAssetMode.Webfont));

        Assert.AreEqual(
            $"<script src=\"{WEBFONT_CONFIG}\" type=\"text/javascript\"></script>",
            markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task HostAssetUrls_RenderStylesheetAndRuntimeAroundConfigAsync() {
        var markup = await TestHost.RenderMarkupAsync<FontAwesomeHeadAssets>(
            null,
            services => services.AddJeninFontAwesome(options => {
                options.StyleSheetUrl = "/fontawesome/css/all.css";
                options.JavaScriptUrl = "/fontawesome/js/all.min.js";
            }));

        Assert.AreEqual(
            "<link rel=\"stylesheet\" href=\"/fontawesome/css/all.css\" />"
            + $"<script src=\"{NEST_CONFIG}\" type=\"text/javascript\"></script>"
            + "<script src=\"/fontawesome/js/all.min.js\" type=\"text/javascript\"></script>",
            markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ExplicitParameters_OverrideRegisteredOptionsAsync() {
        var markup = await TestHost.RenderMarkupAsync<FontAwesomeHeadAssets>(
            TestHost.Parameters(
                ("Mode", FontAwesomeAssetMode.Webfont),
                ("StyleSheetUrl", "/custom/all.css"),
                ("JavaScriptUrl", "/custom/all.min.js"),
                ("ConfigScriptUrl", "/custom/config.js")),
            services => services.AddJeninFontAwesome(options => {
                options.AssetMode = FontAwesomeAssetMode.SvgNest;
                options.StyleSheetUrl = "/fontawesome/css/all.css";
                options.JavaScriptUrl = "/fontawesome/js/all.min.js";
            }));

        Assert.AreEqual(
            "<link rel=\"stylesheet\" href=\"/custom/all.css\" />"
            + "<script src=\"/custom/config.js\" type=\"text/javascript\"></script>"
            + "<script src=\"/custom/all.min.js\" type=\"text/javascript\"></script>",
            markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task ExplicitMode_WithoutOptions_RendersMatchingStaticConfigAsync() {
        var markup = await TestHost.RenderMarkupAsync<FontAwesomeHeadAssets>(
            TestHost.Parameters(("Mode", FontAwesomeAssetMode.Webfont)));

        Assert.AreEqual(
            $"<script src=\"{WEBFONT_CONFIG}\" type=\"text/javascript\"></script>",
            markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task MappedConfigUrl_UsedWhenRegisteredAsync() {
        var markup = await TestHost.RenderMarkupAsync<FontAwesomeHeadAssets>(
            null,
            services => services.AddJeninFontAwesome(options =>
                options.ConfigScriptUrl = ApplicationBuilderExtensions.DefaultConfigEndpointPath));

        Assert.AreEqual(
            $"<script src=\"{ApplicationBuilderExtensions.DefaultConfigEndpointPath}\" type=\"text/javascript\"></script>",
            markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task KitOptions_RenderKitScriptWithoutStylesheetAsync() {
        var markup = await TestHost.RenderMarkupAsync<FontAwesomeHeadAssets>(
            null,
            services => services.AddJeninFontAwesome(options => options.UseFontAwesomeKit("a1b2c3d4e5")));

        Assert.AreEqual(
            "<script src=\"_content/Jenin.FontAwesome.Blazor/js/fontawesome-config-nest.js\" type=\"text/javascript\"></script>"
            + "<script src=\"https://kit.fontawesome.com/a1b2c3d4e5.js\" type=\"text/javascript\"></script>",
            markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task CustomKitIconName_RendersAnchorAsIsAsync() {
        // Kit-custom glyphs are unknown to the Free catalogue (JFA1001 warns by design);
        // the anchor still renders so kits resolve them at runtime.
        var markup = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-acme-widget"),
            ("IconStyle", IconStyle.Solid)));

        Assert.AreEqual(
            "<span><i class=\"fa-solid fa-acme-widget\" aria-hidden=\"true\" role=\"presentation\"></i></span>",
            markup);
    }

    [TestMethod]
    [TestCategory("Rendering")]
    public async Task EmptyUrls_OmitTagsAsync() {
        var markup = await TestHost.RenderMarkupAsync<FontAwesomeHeadAssets>(
            TestHost.Parameters(
                ("StyleSheetUrl", string.Empty),
                ("JavaScriptUrl", string.Empty)));

        Assert.AreEqual(
            $"<script src=\"{NEST_CONFIG}\" type=\"text/javascript\"></script>",
            markup);
    }
}
