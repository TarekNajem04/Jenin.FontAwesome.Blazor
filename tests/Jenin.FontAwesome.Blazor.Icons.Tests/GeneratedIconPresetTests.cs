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

        presetTypes.Should().HaveCount(EXPECTED_COMPONENT_COUNT);

        foreach(var type in presetTypes) {
            type.IsPublic.Should().BeTrue($"{type.FullName} should be public.");
            type.GetConstructor(Type.EmptyTypes).Should().NotBeNull($"{type.FullName} should expose a public parameterless constructor.");
        }
    }

    [TestMethod]
    public void AllPresetComponents_BakeAUniqueFaNameAndAFreePrimaryStyle() {
        var names = new HashSet<string>(StringComparer.Ordinal);

        foreach(var type in PresetTypes()) {
            var icon = (Icon)Activator.CreateInstance(type)!;

            icon.IconName.Should().NotBeNull($"{type.FullName} should bake an IconName.");
            icon.IconName!.StartsWith("fa-", StringComparison.Ordinal).Should().BeTrue($"{type.FullName} IconName '{icon.IconName}' should start with 'fa-'.");
            names.Add(icon.IconName!).Should().BeTrue($"Duplicate IconName '{icon.IconName}'.");
            (icon.IconStyle is IconStyle.Solid or IconStyle.Brands).Should().BeTrue($"{type.FullName} primary style '{icon.IconStyle}' should be a Free style.");
        }

        names.Should().HaveCount(EXPECTED_COMPONENT_COUNT);
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

        house.Should().Contain("fa-solid");
        house.Should().Contain("fa-house");
        github.Should().Contain("fa-brands");
        github.Should().Contain("fa-github");
    }

    [TestMethod]
    public async Task PresetAllowsTheBakedStyleToBeOverriddenByAnExplicitParameterAsync() {
        var overridden = await TestHost.RenderMarkupAsync<HouseIcon>(TestHost.Parameters(("IconStyle", IconStyle.Brands)));
        var generic = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", "fa-house"),
            ("IconStyle", IconStyle.Brands)));

        overridden.Should().Be(generic);
        overridden.Should().Contain("fa-brands");
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

        preset.Should().Be(generic);
    }

    private static async Task AssertPresetMatchesGenericAsync<TComponent>(string iconName, IconStyle iconStyle)
        where TComponent : IComponent {
        var preset = await TestHost.RenderMarkupAsync<TComponent>();
        var generic = await TestHost.RenderMarkupAsync<Icon>(TestHost.Parameters(
            ("IconName", iconName),
            ("IconStyle", iconStyle)));

        preset.Should().Be(generic, $"Preset for '{iconName}' should render identically to the generic Icon.");
    }

    private static Type[] PresetTypes() => [.. typeof(HouseIcon).Assembly
        .GetTypes()
        .Where(type => type.IsClass && !type.IsAbstract && typeof(Icon).IsAssignableFrom(type))
        .OrderBy(type => type.FullName, StringComparer.Ordinal)];
}
