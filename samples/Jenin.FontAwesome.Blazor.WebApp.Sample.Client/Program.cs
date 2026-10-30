//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddJeninFontAwesome();

await builder.Build().RunAsync();
