//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Tests.Animations;

/// <summary>
/// Guards the shipped <c>animation-triggers.css</c> reduced-motion contract:
/// the media query must neutralize every library trigger and transition, and every
/// <see cref="AnimationTrigger"/> (except <c>None</c>) must have a gated CSS class, so
/// C#-side and CSS-side changes cannot drift apart.
/// </summary>
[TestClass]
public class ReducedMotionCssTests {
    private const string CSS_REPO_RELATIVE_PATH = "src/Jenin.FontAwesome.Blazor/wwwroot/css/animation-triggers.css";

    private static string reducedMotionBlock = string.Empty;

    [TestInitialize]
    public void LoadReducedMotionBlock() {
        var css = File.ReadAllText(StylesheetPath());
        var start = css.IndexOf("@media (prefers-reduced-motion: reduce)", StringComparison.Ordinal);

        Assert.AreNotEqual(-1, start, "The reduced-motion media query is missing from animation-triggers.css.");

        reducedMotionBlock = css.Substring(start);
    }

    [TestMethod]
    [TestCategory("Contract")]
    [DataRow(".jfa-anim-hover")]
    [DataRow(".jfa-anim-parent-hover")]
    [DataRow(".jfa-anim-focus")]
    [DataRow(".jfa-anim-reveal-target")]
    [DataRow(".jfa-anim-transition-layer")]
    public void ReducedMotionBlock_CoversLibraryOwnedMotion(string selector) {
        Assert.Contains(selector, reducedMotionBlock, $"The reduced-motion block must neutralize '{selector}'.");
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void ReducedMotionBlock_RemovesAnimationAndTransitions() {
        Assert.Contains("animation: none", reducedMotionBlock, "Trigger keyframes must be disabled, not just paused.");
        Assert.Contains("transition: none", reducedMotionBlock, "Icon transitions must complete instantly, not animate.");
        Assert.Contains("opacity: 1", reducedMotionBlock, "Scroll-reveal targets must stay visible instead of hidden.");
    }

    [TestMethod]
    [TestCategory("Contract")]
    [DataRow(AnimationTrigger.None, null)]
    [DataRow(AnimationTrigger.Hover, ".jfa-anim-hover")]
    [DataRow(AnimationTrigger.ParentHover, ".jfa-anim-parent-hover")]
    [DataRow(AnimationTrigger.Focus, ".jfa-anim-focus")]
    [DataRow(AnimationTrigger.ScrollReveal, ".jfa-anim-reveal-target")]
    public void EveryTrigger_HasAReducedMotionGate(AnimationTrigger trigger, string? cssClass) {
        if(trigger == AnimationTrigger.None || cssClass is null) {
            return;
        }

        Assert.Contains(
            cssClass,
            reducedMotionBlock,
            $"AnimationTrigger.{trigger} renders '{cssClass}', which the reduced-motion block must neutralize.");
    }

    // Resolved from the test assembly directory, not [CallerFilePath]: deterministic CI builds
    // (ContinuousIntegrationBuild) rewrite compile-time paths, so source-file discovery fails there.
    private static string RepoRoot() {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while(directory is not null && !File.Exists(Path.Combine(directory.FullName, "Jenin.FontAwesome.Blazor.slnx"))) {
            directory = directory.Parent;
        }

        Assert.IsNotNull(directory, "Could not locate the repository root (Jenin.FontAwesome.Blazor.slnx).");
        return directory.FullName;
    }

    private static string StylesheetPath() {
        var path = Path.Combine(RepoRoot(), CSS_REPO_RELATIVE_PATH.Replace('/', Path.DirectorySeparatorChar));

        Assert.IsTrue(File.Exists(path), $"Shipped stylesheet not found at '{path}'.");

        return path;
    }
}
