using System.Reflection;

namespace Jenin.FontAwesome.Blazor.Sample.Helpers;

public static class ResourceHelper {
    public static string PathToResourceName(string relativePath, Assembly assembly = null) {
        assembly ??= Assembly.GetExecutingAssembly();

        return string.IsNullOrEmpty(relativePath)
            ? null
            : relativePath.StartsWith('~')
                ? relativePath.Replace("~", assembly.GetName().Name).Replace('/', '.').Replace('\\', '.')
                : relativePath;
    }

    public static async Task<string> GetGetManifestResourceContentAsync(string resourceName, Assembly assembly = null) {
        assembly ??= Assembly.GetExecutingAssembly();

        using var resourceStream = assembly.GetManifestResourceStream(resourceName);

        if (resourceStream is not null) {
            using var streamReader = new StreamReader(resourceStream);

            return await streamReader.ReadToEndAsync().ConfigureAwait(false);
        }

        return null;
    }
}
