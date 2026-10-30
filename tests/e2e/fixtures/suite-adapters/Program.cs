// Fixture host (never packed, never published).
//
// FA_VENDOR_DIR (optional): an on-disk Font Awesome tree served at /fa-vendored so the
// MudBlazor slot (which consumes raw FA CSS) renders real glyphs without copying assets.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddBlazorise()
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

var app = builder.Build();

if(!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

var vendorDir = Environment.GetEnvironmentVariable("FA_VENDOR_DIR");
if(!string.IsNullOrWhiteSpace(vendorDir) && Directory.Exists(vendorDir)) {
    app.UseStaticFiles(new StaticFileOptions {
        FileProvider = new PhysicalFileProvider(vendorDir),
        RequestPath = "/fa-vendored",
    });
}

app.MapStaticAssets();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
