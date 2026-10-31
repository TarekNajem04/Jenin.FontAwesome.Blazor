//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Contract;

[TestClass]
public class StaticAssetTests {
    // Resolved from the test assembly directory, not [CallerFilePath]: deterministic CI builds
    // (ContinuousIntegrationBuild) rewrite compile-time paths, so source-file discovery fails there.
    private static string RepoRoot() {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while(directory is not null && !File.Exists(Path.Combine(directory.FullName, "Jenin.FontAwesome.Blazor.slnx"))) {
            directory = directory.Parent;
        }

        directory.Should().NotBeNull("Could not locate the repository root.");

        return directory!.FullName;
    }

    private static string WwwRoot => Path.Combine(RepoRoot(), "src", "Jenin.FontAwesome.Blazor", "wwwroot");

    [TestMethod]
    [TestCategory("Contract")]
    [DataRow("js/fontawesome-config-nest.js")]
    [DataRow("js/fontawesome-config-webfont.js")]
    [DataRow("js/animation-triggers.js")]
    [DataRow("css/animation-triggers.css")]
    public void StaticAsset_Exists(string relativePath) {
        ArgumentNullException.ThrowIfNull(relativePath);

        var path = Path.Combine(WwwRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));

        File.Exists(path).Should().BeTrue($"Missing static asset: {path}");
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void NestConfig_SetsNestAutoReplaceSvg() {
        var content = File.ReadAllText(Path.Combine(WwwRoot, "js", "fontawesome-config-nest.js"));

        content.Should().Contain("autoReplaceSvg: \"nest\"");
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void WebfontConfig_DisablesAutoReplaceSvg() {
        var content = File.ReadAllText(Path.Combine(WwwRoot, "js", "fontawesome-config-webfont.js"));

        content.Should().Contain("autoReplaceSvg: false");
    }
}
