//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Components;

[TestClass]
public class ConfigurationTests {
    private const string DECORATIVE = " aria-hidden=\"true\" role=\"presentation\"";

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task DiDefaults_ApplyIconStyleAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-house")),
            services => services.AddJeninFontAwesome(options => options.IconStyle = IconStyle.Regular));

        Assert.AreEqual($"<span><i class=\"fa-regular fa-house\"{DECORATIVE}></i></span>", markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task DiDefaults_ApplyTypefaceAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-house")),
            services => services.AddJeninFontAwesome(options => options.Typeface = Typeface.Sharp));

        Assert.AreEqual($"<span><i class=\"fa-sharp fa-solid fa-house\"{DECORATIVE}></i></span>", markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task DiDefaults_ApplyIconSizeAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-house")),
            services => services.AddJeninFontAwesome(options => options.IconSize = IconSize.X02));

        Assert.AreEqual($"<span><i class=\"fa-2x fa-solid fa-house\"{DECORATIVE}></i></span>", markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task DiDefaults_ApplyDecorativeAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-house"), ("Title", "Home")),
            services => services.AddJeninFontAwesome(options => options.Decorative = true));

        Assert.AreEqual(
            "<span><i class=\"fa-solid fa-house\" title=\"Home\" aria-hidden=\"true\" role=\"presentation\"></i></span>",
            markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task DiDefaults_ApplyMeaningfulAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-house")),
            services => services.AddJeninFontAwesome(options => options.Decorative = false));

        Assert.AreEqual(
            "<span><i class=\"fa-solid fa-house\" role=\"img\" aria-hidden=\"false\"></i></span>",
            markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task DiDefaults_Unconfigured_LeavesMarkupUnchangedAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-house")),
            services => services.AddJeninFontAwesome());

        Assert.AreEqual($"<span><i class=\"fa-solid fa-house\"{DECORATIVE}></i></span>", markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task ExplicitParameter_OverridesDiDefaultsAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-house"), ("IconStyle", IconStyle.Solid)),
            services => services.AddJeninFontAwesome(options => options.IconStyle = IconStyle.Regular));

        Assert.AreEqual($"<span><i class=\"fa-solid fa-house\"{DECORATIVE}></i></span>", markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task ExplicitIconSize_OverridesDiDefaultsAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-house"), ("IconSize", IconSize.Large)),
            services => services.AddJeninFontAwesome(options => options.IconSize = IconSize.X02));

        Assert.AreEqual($"<span><i class=\"fa-lg fa-solid fa-house\"{DECORATIVE}></i></span>", markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task ExplicitDecorative_OverridesDiDefaultsAsync() {
        var markup = await TestHost.RenderMarkupAsync<Icon>(
            TestHost.Parameters(("IconName", "fa-house"), ("Decorative", false)),
            services => services.AddJeninFontAwesome(options => options.Decorative = true));

        Assert.AreEqual(
            "<span><i class=\"fa-solid fa-house\" role=\"img\" aria-hidden=\"false\"></i></span>",
            markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task CascadeDefaults_ApplyIconStyleAsync() {
        var markup = await TestHost.RenderMarkupAsync<FontAwesomeDefaults>(TestHost.Parameters(
            ("IconStyle", IconStyle.Regular),
            ("ChildContent", IconFragment("fa-house"))));

        Assert.AreEqual($"<span><i class=\"fa-regular fa-house\"{DECORATIVE}></i></span>", markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task CascadeDefaults_OverrideDiDefaultsAsync() {
        var markup = await TestHost.RenderMarkupAsync<FontAwesomeDefaults>(
            TestHost.Parameters(
                ("IconStyle", IconStyle.Brands),
                ("ChildContent", IconFragment("fa-house"))),
            services => services.AddJeninFontAwesome(options => options.IconStyle = IconStyle.Regular));

        Assert.AreEqual($"<span><i class=\"fa-brands fa-house\"{DECORATIVE}></i></span>", markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task NestedCascade_OverridesOuterAsync() {
        var inner = DefaultsFragment(nameof(FontAwesomeDefaults.IconStyle), IconStyle.Brands, IconFragment("fa-house"));

        var markup = await TestHost.RenderMarkupAsync<FontAwesomeDefaults>(TestHost.Parameters(
            ("IconStyle", IconStyle.Regular),
            ("ChildContent", inner)));

        Assert.AreEqual($"<span><i class=\"fa-brands fa-house\"{DECORATIVE}></i></span>", markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task NestedCascade_InheritsUnsetValuesAsync() {
        var inner = DefaultsFragment(nameof(FontAwesomeDefaults.IconSize), IconSize.X03, IconFragment("fa-house"));

        var markup = await TestHost.RenderMarkupAsync<FontAwesomeDefaults>(TestHost.Parameters(
            ("IconStyle", IconStyle.Regular),
            ("ChildContent", inner)));

        Assert.AreEqual($"<span><i class=\"fa-3x fa-regular fa-house\"{DECORATIVE}></i></span>", markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task ExplicitParameter_OverridesCascadeAndDiAsync() {
        var markup = await TestHost.RenderMarkupAsync<FontAwesomeDefaults>(
            TestHost.Parameters(
                ("IconStyle", IconStyle.Brands),
                ("ChildContent", (RenderFragment)(builder => {
                    builder.OpenComponent<Icon>(0);
                    builder.AddAttribute(1, "IconName", "fa-house");
                    builder.AddAttribute(2, "IconStyle", IconStyle.Solid);
                    builder.CloseComponent();
                }))),
            services => services.AddJeninFontAwesome(options => options.IconStyle = IconStyle.Regular));

        Assert.AreEqual($"<span><i class=\"fa-solid fa-house\"{DECORATIVE}></i></span>", markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public async Task Container_GlobalDefaultsApplyAsync() {
        var markup = await TestHost.RenderMarkupAsync<IconContainer>(
            null,
            services => services.AddJeninFontAwesome(options => {
                options.IconSize = IconSize.Large;
                options.Decorative = true;
            }));

        Assert.AreEqual("<div class=\"fa-lg\" aria-hidden=\"true\"></div>", markup);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public void AddJeninFontAwesome_RegistersConfiguredOptions() {
        var services = new ServiceCollection();

        var returned = services.AddJeninFontAwesome(options => {
            options.IconStyle = IconStyle.Regular;
            options.Typeface = Typeface.Sharp;
            options.IconSize = IconSize.X04;
            options.Decorative = true;
        });

        Assert.AreSame(services, returned);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<FontAwesomeOptions>();

        Assert.AreEqual(IconStyle.Regular, options.IconStyle);
        Assert.AreEqual(Typeface.Sharp, options.Typeface);
        Assert.AreEqual(IconSize.X04, options.IconSize);
        Assert.IsTrue(options.Decorative);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public void AddJeninFontAwesome_WithoutConfiguration_RegistersEmptyOptions() {
        var services = new ServiceCollection();

        _ = services.AddJeninFontAwesome();

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<FontAwesomeOptions>();

        Assert.IsNull(options.IconStyle);
        Assert.IsNull(options.Typeface);
        Assert.IsNull(options.IconSize);
        Assert.IsNull(options.Decorative);
    }

    private static RenderFragment IconFragment(string iconName) => builder => {
        builder.OpenComponent<Icon>(0);
        builder.AddAttribute(1, "IconName", iconName);
        builder.CloseComponent();
    };

    [TestMethod]
    [TestCategory("Configuration")]
    public void UseFontAwesomeKit_SetsKitUrlAndClearsStylesheet() {
        var options = new FontAwesomeOptions { StyleSheetUrl = "/fontawesome/css/all.css" };

        options.UseFontAwesomeKit("a1b2c3d4e5");

        Assert.AreEqual("a1b2c3d4e5", options.KitCode);
        Assert.AreEqual("https://kit.fontawesome.com/a1b2c3d4e5.js", options.JavaScriptUrl);
        Assert.IsNull(options.StyleSheetUrl);
    }

    [TestMethod]
    [TestCategory("Configuration")]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("has space")]
    [DataRow("https://kit.fontawesome.com/a1b2c3d4e5.js")]
    public void UseFontAwesomeKit_RejectsNonCodes(string? kitCode) {
        var options = new FontAwesomeOptions();

        Assert.ThrowsExactly<ArgumentException>(() => options.UseFontAwesomeKit(kitCode!));
    }

    [TestMethod]
    [TestCategory("Configuration")]
    public void UseFontAwesomeKit_PreservesExplicitConfigScriptUrl() {
        var options = new FontAwesomeOptions { ConfigScriptUrl = "/custom/config.js" };

        options.UseFontAwesomeKit("a1b2c3d4e5");

        Assert.AreEqual("/custom/config.js", options.ConfigScriptUrl);
        Assert.AreEqual("https://kit.fontawesome.com/a1b2c3d4e5.js", options.JavaScriptUrl);
    }

    private static RenderFragment DefaultsFragment(string parameterName, object? value, RenderFragment childContent) => builder => {
        builder.OpenComponent<FontAwesomeDefaults>(0);
        builder.AddAttribute(1, parameterName, value);
        builder.AddAttribute(2, nameof(FontAwesomeDefaults.ChildContent), childContent);
        builder.CloseComponent();
    };
}
