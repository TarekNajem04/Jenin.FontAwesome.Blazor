//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Application wiring for the one-line setup: serves the generated Font Awesome mode-config
/// script so the registered <see cref="FontAwesomeAssetMode"/> can never drift from the served
/// config.
/// </summary>
/// <remarks>
/// Implemented as path-scoped middleware (rather than endpoint routing) so the call works on
/// every host pipeline with a single <c>netstandard2.0</c> abstractions dependency — a library
/// also consumed by browser-hosted WebAssembly clients cannot take a server-framework
/// reference. The middleware handles only exact <c>GET</c> hits on the configured path; every
/// other request falls through untouched.
/// </remarks>
public static class ApplicationBuilderExtensions {
    /// <summary>
    /// The default path the generated mode-config script is served from.
    /// </summary>
    public const string DefaultConfigEndpointPath = "/_content/Jenin.FontAwesome.Blazor/js/fontawesome-config-auto.js";

    /// <summary>
    /// Serves a <c>GET</c> endpoint that returns the mode-config script matching the registered
    /// <see cref="FontAwesomeOptions"/> (applied <paramref name="configure"/> last), and points
    /// the options at that endpoint unless <see cref="FontAwesomeOptions.ConfigScriptUrl"/> was
    /// set explicitly. Pair with <c>services.AddJeninFontAwesome(...)</c> and a single
    /// <see cref="Components.FontAwesomeHeadAssets"/> in <c>&lt;head&gt;</c>: no hand-written
    /// <c>&lt;script&gt;</c> tags are needed.
    /// </summary>
    /// <param name="app">The application builder (typically the <c>WebApplication</c>).</param>
    /// <param name="configure">
    /// Optional callback applied to the registered options after the service registration, so the
    /// mode and host-owned asset URLs can live next to the mapping call.
    /// </param>
    /// <returns>The same <see cref="IApplicationBuilder"/> so calls can be chained.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="app"/> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">
    /// <c>AddJeninFontAwesome()</c> was not called during service registration.
    /// </exception>
    public static IApplicationBuilder MapJeninFontAwesomeAssets(this IApplicationBuilder app, Action<FontAwesomeOptions>? configure = null) {
        ArgumentNullException.ThrowIfNull(app);

        var options = app.ApplicationServices.GetService<FontAwesomeOptions>()
            ?? throw new InvalidOperationException(
                "MapJeninFontAwesomeAssets requires the library options. Call services.AddJeninFontAwesome() during service registration first.");

        configure?.Invoke(options);

        var path = string.IsNullOrEmpty(options.ConfigEndpointPath) ? DefaultConfigEndpointPath : options.ConfigEndpointPath;

        if(string.IsNullOrEmpty(options.ConfigScriptUrl)) {
            options.ConfigScriptUrl = path;
        }

        var script = FontAwesomeConfigScript.GetScript(options.AssetMode ?? FontAwesomeAssetMode.SvgNest);

        return app.Use(async (context, next) => {
            if(HttpMethods.IsGet(context.Request.Method)
                && context.Request.Path.Equals(path, StringComparison.Ordinal)) {
                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/javascript; charset=utf-8";
                context.Response.Headers["Cache-Control"] = "public, max-age=86400";
                await context.Response.WriteAsync(script, context.RequestAborted).ConfigureAwait(false);
                return;
            }

            await next().ConfigureAwait(false);
        });
    }
}
