//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Icons.Tests;

/// <summary>
/// Verifies the generated per-icon preset components: that they exist in the expected shape, bake the
/// right name and Free primary style, and render identically to the equivalent generic <see cref="Icon"/>.
/// </summary>
[TestClass]
public sealed class GeneratedIconPresetTests {
    private const int EXPECTED_COMPONENT_COUNT = 1992;

    [TestMethod]
    public void AllPresetComponents_ArePublicIconSubclassesWithParameterlessConstructor() {
        var presetTypes = PresetTypes();

        Assert.HasCount(EXPECTED_COMPONENT_COUNT, presetTypes);

        foreach(var type in presetTypes) {
            Assert.IsTrue(type.IsPublic, $"{type.FullName} should be public.");
            Assert.IsNotNull(type.GetConstructor(Type.EmptyTypes), $"{type.FullName} should expose a public parameterless constructor.");
        }
    }

    [TestMethod]
    public void AllPresetComponents_BakeAUniqueFaNameAndAFreePrimaryStyle() {
        var names = new HashSet<string>(StringComparer.Ordinal);

        foreach(var type in PresetTypes()) {
            var icon = (Icon)Activator.CreateInstance(type)!;

            Assert.IsNotNull(icon.IconName, $"{type.FullName} should bake an IconName.");
            Assert.IsTrue(icon.IconName!.StartsWith("fa-", StringComparison.Ordinal), $"{type.FullName} IconName '{icon.IconName}' should start with 'fa-'.");
            Assert.IsTrue(names.Add(icon.IconName!), $"Duplicate IconName '{icon.IconName}'.");
            Assert.IsTrue(icon.IconStyle is IconStyle.Solid or IconStyle.Brands, $"{type.FullName} primary style '{icon.IconStyle}' should be a Free style.");
        }

        Assert.HasCount(EXPECTED_COMPONENT_COUNT, names);
    }

    [TestMethod]
    public async Task PresetRendersIdenticallyToTheEquivalentGenericIconAsync() {
        await AssertPresetMatchesGenericAsync<HouseIcon>("fa-house", IconStyle.Solid);
        await AssertPresetMatchesGenericAsync<StarIcon>("fa-star", IconStyle.Solid);
        await AssertPresetMatchesGenericAsync<UserIcon>("fa-user", IconStyle.Solid);
        await AssertPresetMatchesGenericAsync<GithubIcon>("fa-github", IconStyle.Brands);
        await AssertPresetMatchesGenericAsync<_500pxIcon>("fa-500px", IconStyle.Brands);
        await AssertPresetMatchesGenericAsync<_42GroupIcon>("fa-42-group", IconStyle.Brands);
    }

    [TestMethod]
    public async Task PresetsBakeTheExpectedFontAwesomeClassesAsync() {
        var house = await TestHost.RenderMarkupAsync<HouseIcon>();
        var github = await TestHost.RenderMarkupAsync<GithubIcon>();

        Assert.Contains("fa-solid", house);
        Assert.Contains("fa-house", house);
        Assert.Contains("fa-brands", github);
        Assert.Contains("fa-github", github);
    }

    [TestMethod]
    public async Task PresetAllowsTheBakedStyleToBeOverriddenByAnExplicitParameterAsync() {
        var overridden = await TestHost.RenderMarkupAsync<HouseIcon>(TestHost.Parameters(("IconStyle", IconStyle.Brands)));
        var generic = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("IconStyle", IconStyle.Brands)));

        Assert.AreEqual(generic, overridden);
        Assert.Contains("fa-brands", overridden);
    }

    [TestMethod]
    public async Task PresetAllowsOtherParametersToBeOverriddenAsync() {
        var preset = await TestHost.RenderMarkupAsync<HouseIcon>(TestHost.Parameters(
            ("Color", "red"),
            ("Bordered", true)));
        var generic = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("IconStyle", IconStyle.Solid),
            ("Color", "red"),
            ("Bordered", true)));

        Assert.AreEqual(generic, preset);
    }

    private static async Task AssertPresetMatchesGenericAsync<TComponent>(string iconName, IconStyle iconStyle)
        where TComponent : IComponent {
        var preset = await TestHost.RenderMarkupAsync<TComponent>();
        var generic = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", iconName),
            ("IconStyle", iconStyle)));

        Assert.AreEqual(generic, preset, $"Preset for '{iconName}' should render identically to the generic Icon.");
    }

    private static Type[] PresetTypes() => [.. typeof(HouseIcon).Assembly
        .GetTypes()
        .Where(type => type.IsClass && !type.IsAbstract && typeof(Icon).IsAssignableFrom(type))
        .OrderBy(type => type.FullName, StringComparer.Ordinal)];
}
