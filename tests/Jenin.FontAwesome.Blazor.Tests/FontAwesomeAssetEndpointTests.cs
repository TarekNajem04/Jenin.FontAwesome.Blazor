//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Tests;

[TestClass]
public class FontAwesomeAssetEndpointTests {
    [TestMethod]
    [TestCategory("Configuration")]
    public async Task MissingServiceRegistration_ThrowsHelpfulErrorAsync() {
        await using var app = WebApplication.Create();

        var exception = Assert.ThrowsExactly<InvalidOperationException>(() => app.MapJeninFontAwesomeAssets());

        Assert.Contains("AddJeninFontAwesome", exception.Message);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task DefaultMode_ServesNestConfigAsync() {
        await using var app = CreateHost(services => services.AddJeninFontAwesome());
        await app.StartAsync();

        using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
        using var response = await client.GetAsync(ApplicationBuilderExtensions.DefaultConfigEndpointPath);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("application/javascript; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.AreEqual("public, max-age=86400", string.Join(",", response.Headers.GetValues("Cache-Control")));

        var script = await response.Content.ReadAsStringAsync();
        Assert.Contains("autoReplaceSvg: \"nest\"", script);
        Assert.DoesNotContain("autoReplaceSvg: false", script);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task WebfontMode_ServesWebfontConfigAsync() {
        await using var app = CreateHost(services => services.AddJeninFontAwesome(options => options.AssetMode = FontAwesomeAssetMode.Webfont));
        await app.StartAsync();

        using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
        var script = await client.GetStringAsync(ApplicationBuilderExtensions.DefaultConfigEndpointPath);

        Assert.Contains("autoReplaceSvg: false", script);
        Assert.DoesNotContain("\"nest\"", script);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task MapCall_PointsOptionsAtMappedEndpointAsync() {
        await using var app = CreateHost(
            services => services.AddJeninFontAwesome(),
            options => {
                options.StyleSheetUrl = "/fontawesome/css/all.css";
                options.JavaScriptUrl = "/fontawesome/js/all.min.js";
            });
        await app.StartAsync();

        var options = app.Services.GetRequiredService<FontAwesomeOptions>();

        Assert.AreEqual(ApplicationBuilderExtensions.DefaultConfigEndpointPath, options.ConfigScriptUrl);
        Assert.AreEqual("/fontawesome/css/all.css", options.StyleSheetUrl);
        Assert.AreEqual("/fontawesome/js/all.min.js", options.JavaScriptUrl);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task ExplicitConfigScriptUrl_IsPreservedAsync() {
        await using var app = CreateHost(
            services => services.AddJeninFontAwesome(options => options.ConfigScriptUrl = "/custom/config.js"));
        await app.StartAsync();

        var options = app.Services.GetRequiredService<FontAwesomeOptions>();

        Assert.AreEqual("/custom/config.js", options.ConfigScriptUrl);

        // The mapped endpoint still serves; the head component simply points elsewhere.
        using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
        using var response = await client.GetAsync(ApplicationBuilderExtensions.DefaultConfigEndpointPath);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task NonGetRequests_FallThroughAsync() {
        await using var app = CreateHost(services => services.AddJeninFontAwesome());
        await app.StartAsync();

        using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
        using var response = await client.PostAsync(ApplicationBuilderExtensions.DefaultConfigEndpointPath, new StringContent(string.Empty));

        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task UnrelatedPaths_FallThroughAsync() {
        await using var app = CreateHost(services => services.AddJeninFontAwesome());
        await app.StartAsync();

        using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
        using var response = await client.GetAsync("/");

        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task ModeAgreement_HeadTagsMatchServedConfigAsync() {
        await using var app = CreateHost(
            services => services.AddJeninFontAwesome(options => options.AssetMode = FontAwesomeAssetMode.Webfont),
            options => options.StyleSheetUrl = "/fontawesome/css/all.css");
        await app.StartAsync();

        var options = app.Services.GetRequiredService<FontAwesomeOptions>();

        var markup = await TestHost.RenderMarkupAsync<FontAwesomeHeadAssets>(
            null,
            services => services.AddJeninFontAwesome(setup => {
                setup.AssetMode = options.AssetMode;
                setup.StyleSheetUrl = options.StyleSheetUrl;
                setup.ConfigScriptUrl = options.ConfigScriptUrl;
            }));

        Assert.Contains($"src=\"{options.ConfigScriptUrl}\"", markup);

        using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
        var script = await client.GetStringAsync(options.ConfigScriptUrl);

        Assert.Contains("autoReplaceSvg: false", script);
    }

    private static WebApplication CreateHost(Action<IServiceCollection> register, Action<FontAwesomeOptions>? configure = null) {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls("http://127.0.0.1:0");
        register(builder.Services);

        var app = builder.Build();
        app.MapJeninFontAwesomeAssets(configure);
        return app;
    }
}
