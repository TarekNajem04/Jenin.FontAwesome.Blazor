//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Registration helpers for library-wide <see cref="FontAwesomeOptions"/>.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    /// Registers library-wide Font Awesome defaults consumed by every component. Supplying no
    /// configuration registers an empty options instance, which leaves component defaults unchanged.
    /// </summary>
    /// <param name="services">The service collection to register the options with.</param>
    /// <param name="configure">An optional callback used to configure the defaults.</param>
    /// <returns>The same <see cref="IServiceCollection"/> so calls can be chained.</returns>
    public static IServiceCollection AddJeninFontAwesome(this IServiceCollection services, Action<FontAwesomeOptions>? configure = null) {
        ArgumentNullException.ThrowIfNull(services);

        var options = new FontAwesomeOptions();
        configure?.Invoke(options);

        _ = services.AddSingleton(options);

        return services;
    }
}
