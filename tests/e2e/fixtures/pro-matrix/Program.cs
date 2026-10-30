// Fixture host (never packed, never published).
//
// The one-line setup under verification: mode and host-owned asset URLs come
// from the environment so licensed bytes are never committed.
//   FA_ASSET_CSS   - Font Awesome stylesheet URL (required).
//   FA_ASSET_JS    - Font Awesome runtime URL (empty for webfont-only hosts).
//   FA_EDITION     - "free" (default) or "pro"; rendered as data-edition so the spec asserts
//                    the matching contract (Free fallback vs licensed Pro resolution).
//   PRO_MATRIX_MODE - "nest" (default) or "webfont".

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// A consumer-owned glyph registered once and used through the same Icon API.
// Triangle path is fixture-owned art, not a Font Awesome glyph.
builder.Services.AddJeninFontAwesome(options => options.RegisterCustomIcon(
    "fa-acme-widget",
    "M256 8L8 488H504Z",
    "0 0 512 496",
    "Acme Widget",
    ["acme", "widget", "triangle"]));

var app = builder.Build();

if(!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseAntiforgery();

app.MapJeninFontAwesomeAssets(options => {
    options.AssetMode = string.Equals(Environment.GetEnvironmentVariable("PRO_MATRIX_MODE"), "webfont", StringComparison.OrdinalIgnoreCase)
        ? FontAwesomeAssetMode.Webfont
        : FontAwesomeAssetMode.SvgNest;
    options.StyleSheetUrl = Environment.GetEnvironmentVariable("FA_ASSET_CSS");
    options.JavaScriptUrl = Environment.GetEnvironmentVariable("FA_ASSET_JS");
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
