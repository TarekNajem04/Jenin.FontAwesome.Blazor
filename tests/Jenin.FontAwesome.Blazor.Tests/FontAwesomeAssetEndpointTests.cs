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

        var exception = FluentActions.Invoking(() => app.MapJeninFontAwesomeAssets()).Should().ThrowExactly<InvalidOperationException>().Which;

        exception.Message.Should().Contain("AddJeninFontAwesome");
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task DefaultMode_ServesNestConfigAsync() {
        await using var app = CreateHost(services => services.AddJeninFontAwesome());
        await app.StartAsync();

        using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
        using var response = await client.GetAsync(ApplicationBuilderExtensions.DEFAULT_CONFIG_ENDPOINT_PATH);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (response.Content.Headers.ContentType?.ToString()).Should().Be("application/javascript; charset=utf-8");
        string.Join(",", response.Headers.GetValues("Cache-Control")).Should().Be("public, max-age=86400");

        var script = await response.Content.ReadAsStringAsync();
        script.Should().Contain("autoReplaceSvg: \"nest\"");
        script.Should().NotContain("autoReplaceSvg: false");
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task WebfontMode_ServesWebfontConfigAsync() {
        await using var app = CreateHost(services => services.AddJeninFontAwesome(options => options.AssetMode = FontAwesomeAssetMode.Webfont));
        await app.StartAsync();

        using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
        var script = await client.GetStringAsync(ApplicationBuilderExtensions.DEFAULT_CONFIG_ENDPOINT_PATH);

        script.Should().Contain("autoReplaceSvg: false");
        script.Should().NotContain("\"nest\"");
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

        options.ConfigScriptUrl.Should().Be(ApplicationBuilderExtensions.DEFAULT_CONFIG_ENDPOINT_PATH);
        options.StyleSheetUrl.Should().Be("/fontawesome/css/all.css");
        options.JavaScriptUrl.Should().Be("/fontawesome/js/all.min.js");
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task ExplicitConfigScriptUrl_IsPreservedAsync() {
        await using var app = CreateHost(
            services => services.AddJeninFontAwesome(options => options.ConfigScriptUrl = "/custom/config.js"));
        await app.StartAsync();

        var options = app.Services.GetRequiredService<FontAwesomeOptions>();

        options.ConfigScriptUrl.Should().Be("/custom/config.js");

        // The mapped endpoint still serves; the head component simply points elsewhere.
        using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
        using var response = await client.GetAsync(ApplicationBuilderExtensions.DEFAULT_CONFIG_ENDPOINT_PATH);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task NonGetRequests_FallThroughAsync() {
        await using var app = CreateHost(services => services.AddJeninFontAwesome());
        await app.StartAsync();

        using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
        using var response = await client.PostAsync(ApplicationBuilderExtensions.DEFAULT_CONFIG_ENDPOINT_PATH, new StringContent(string.Empty));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task UnrelatedPaths_FallThroughAsync() {
        await using var app = CreateHost(services => services.AddJeninFontAwesome());
        await app.StartAsync();

        using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
        using var response = await client.GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
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

        markup.Should().Contain($"src=\"{options.ConfigScriptUrl}\"");

        using var client = new HttpClient { BaseAddress = new Uri(app.Urls.First()) };
        var script = await client.GetStringAsync(options.ConfigScriptUrl);

        script.Should().Contain("autoReplaceSvg: false");
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
