//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.IconPicker;

/// <summary>
/// Registration helpers for the optional icon discovery services.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    /// Registers the <see cref="IconMetadataProvider"/> for dependency injection. Registered custom
    /// glyphs (see <c>FontAwesomeOptions.RegisterCustomIcon</c>) are folded in automatically when
    /// <c>AddJeninFontAwesome</c> ran first. The picker falls back to
    /// <see cref="IconMetadataProvider.Default"/> when no provider is registered.
    /// </summary>
    /// <param name="services">The service collection to register the provider with.</param>
    /// <returns>The same <see cref="IServiceCollection"/> so calls can be chained.</returns>
    public static IServiceCollection AddJeninFontAwesomeIconPicker(this IServiceCollection services) {
        ArgumentNullException.ThrowIfNull(services);

        _ = services.AddSingleton<IconMetadataProvider>(provider => IconMetadataProvider.Default.WithCustomIcons(
            provider.GetService<FontAwesomeOptions>()?.CustomIcons.Values ?? []));

        return services;
    }
}
