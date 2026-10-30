//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Contract;

[TestClass]
public class StaticAssetTests {
    private static string RepoRoot([CallerFilePath] string sourceFile = "") {
        var directory = new DirectoryInfo(Path.GetDirectoryName(sourceFile)!);

        while(directory is not null && !File.Exists(Path.Combine(directory.FullName, "Jenin.FontAwesome.Blazor.slnx"))) {
            directory = directory.Parent;
        }

        Assert.IsNotNull(directory, "Could not locate the repository root.");

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

        Assert.IsTrue(File.Exists(path), $"Missing static asset: {path}");
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void NestConfig_SetsNestAutoReplaceSvg() {
        var content = File.ReadAllText(Path.Combine(WwwRoot, "js", "fontawesome-config-nest.js"));

        Assert.Contains("autoReplaceSvg: \"nest\"", content);
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void WebfontConfig_DisablesAutoReplaceSvg() {
        var content = File.ReadAllText(Path.Combine(WwwRoot, "js", "fontawesome-config-webfont.js"));

        Assert.Contains("autoReplaceSvg: false", content);
    }
}
