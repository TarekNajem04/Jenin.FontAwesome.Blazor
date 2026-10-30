//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

builder.Services.AddJeninFontAwesome();

var app = builder.Build();

if(app.Environment.IsDevelopment()) {
    app.UseWebAssemblyDebugging();
} else {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

// One-line setup: the mode-config script is served from the mapped endpoint and
// FontAwesomeHeadAssets in App.razor renders every Font Awesome tag — no hand-written script
// or stylesheet tags. Host-owned CDN assets stay host-owned (configured URLs only).
app.MapJeninFontAwesomeAssets(options => {
    options.AssetMode = FontAwesomeAssetMode.SvgNest;
    options.StyleSheetUrl = "https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@7.3.1/css/all.min.css";
    options.JavaScriptUrl = "https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@7.3.1/js/all.min.js";
});
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode()
   .AddInteractiveWebAssemblyRenderMode()
   .AddAdditionalAssemblies(typeof(FeatureShowcase).Assembly);

app.Run();
