using JeninFaApp.Components;
using Jenin.FontAwesome.Blazor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddJeninFontAwesome();

var app = builder.Build();

if(!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// One-line Font Awesome setup: serves the generated mode-config script and points the head
// assets at it. Host-owned FA files stay host-owned — swap the CDN URLs for self-hosted paths.
app.MapJeninFontAwesomeAssets(options => {
    options.AssetMode = FontAwesomeAssetMode.JENIN_FA_MODE;
    options.StyleSheetUrl = "https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@7.3.1/css/all.min.css";
    options.JavaScriptUrl = "https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@7.3.1/js/all.min.js";
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
