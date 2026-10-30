//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Sample.Shared.Helpers;

public static class ResourceHelper {
    public static string? PathToResourceName(string? relativePath, Assembly? assembly = null) {
        assembly ??= Assembly.GetExecutingAssembly();

        return string.IsNullOrEmpty(relativePath)
            ? null
            : relativePath.StartsWith('~')
                ? relativePath.Replace("~", assembly.GetName().Name).Replace('/', '.').Replace('\\', '.')
                : relativePath;
    }

    public static async Task<string?> GetGetManifestResourceContentAsync(string resourceName, Assembly? assembly = null) {
        assembly ??= Assembly.GetExecutingAssembly();

        await using var resourceStream = assembly.GetManifestResourceStream(resourceName);

        if(resourceStream is not null) {
            using var streamReader = new StreamReader(resourceStream);

            return await streamReader.ReadToEndAsync().ConfigureAwait(false);
        }

        return null;
    }
}
