//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<DocsApp>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddJeninFontAwesome();
builder.Services.AddJeninFontAwesomeIconPicker();

await builder.Build().RunAsync();
