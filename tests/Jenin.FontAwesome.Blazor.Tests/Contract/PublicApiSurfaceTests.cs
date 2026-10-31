//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Tests.Contract;

[TestClass]
public class PublicApiSurfaceTests {
    private static readonly string[] _expectedExportedTypes = [
        "Jenin.FontAwesome.Blazor.Animations.Animate",
        "Jenin.FontAwesome.Blazor.Animations.Animation",
        "Jenin.FontAwesome.Blazor.Animations.Animation`2",
        "Jenin.FontAwesome.Blazor.Animations.AnimationDirection",
        "Jenin.FontAwesome.Blazor.Animations.AnimationOptions",
        "Jenin.FontAwesome.Blazor.AnimationTrigger",
        "Jenin.FontAwesome.Blazor.Animations.Beat",
        "Jenin.FontAwesome.Blazor.Animations.BeatFade",
        "Jenin.FontAwesome.Blazor.Animations.BeatFadeOptions",
        "Jenin.FontAwesome.Blazor.Animations.BeatOptions",
        "Jenin.FontAwesome.Blazor.Animations.Bounce",
        "Jenin.FontAwesome.Blazor.Animations.BounceOptions",
        "Jenin.FontAwesome.Blazor.Animations.Buzz",
        "Jenin.FontAwesome.Blazor.Animations.BuzzOptions",
        "Jenin.FontAwesome.Blazor.Animations.Fade",
        "Jenin.FontAwesome.Blazor.Animations.FadeOptions",
        "Jenin.FontAwesome.Blazor.Animations.Flip",
        "Jenin.FontAwesome.Blazor.Animations.Flip360",
        "Jenin.FontAwesome.Blazor.Animations.Flip360Options",
        "Jenin.FontAwesome.Blazor.Animations.FlipOptions",
        "Jenin.FontAwesome.Blazor.Animations.Float",
        "Jenin.FontAwesome.Blazor.Animations.FloatOptions",
        "Jenin.FontAwesome.Blazor.Animations.Jello",
        "Jenin.FontAwesome.Blazor.Animations.JelloOptions",
        "Jenin.FontAwesome.Blazor.Animations.Shake",
        "Jenin.FontAwesome.Blazor.Animations.ShakeOptions",
        "Jenin.FontAwesome.Blazor.Animations.Spin",
        "Jenin.FontAwesome.Blazor.Animations.SpinOptions",
        "Jenin.FontAwesome.Blazor.Animations.SpinSnap",
        "Jenin.FontAwesome.Blazor.Animations.SpinSnapOptions",
        "Jenin.FontAwesome.Blazor.Animations.SpinSnapSize",
        "Jenin.FontAwesome.Blazor.Animations.Swing",
        "Jenin.FontAwesome.Blazor.Animations.SwingOptions",
        "Jenin.FontAwesome.Blazor.Animations.Wag",
        "Jenin.FontAwesome.Blazor.Animations.WagOptions",
        "Jenin.FontAwesome.Blazor.ApplicationBuilderExtensions",
        "Jenin.FontAwesome.Blazor.AvatarPresence",
        "Jenin.FontAwesome.Blazor.BorderStyle",
        "Jenin.FontAwesome.Blazor.StatDelta",
        "Jenin.FontAwesome.Blazor.CanvasPadding",
        "Jenin.FontAwesome.Blazor.DuotonePalette",
        "Jenin.FontAwesome.Blazor.DuotonePalettes",
        "Jenin.FontAwesome.Blazor.Components.ActivityItem",
        "Jenin.FontAwesome.Blazor.Components.AvatarIcon",
        "Jenin.FontAwesome.Blazor.Components.ComposedIconElement",
        "Jenin.FontAwesome.Blazor.Components.CounterLayer",
        "Jenin.FontAwesome.Blazor.Components.EmptyState",
        "Jenin.FontAwesome.Blazor.Components.Element",
        "Jenin.FontAwesome.Blazor.Components.ElementBase",
        "Jenin.FontAwesome.Blazor.Components.ElementWithTransformations",
        "Jenin.FontAwesome.Blazor.Components.FontAwesomeDefaults",
        "Jenin.FontAwesome.Blazor.Components.FavoriteButton",
        "Jenin.FontAwesome.Blazor.Components.FontAwesomeHeadAssets",
        "Jenin.FontAwesome.Blazor.Components.Icon",
        "Jenin.FontAwesome.Blazor.Components.IconButton",
        "Jenin.FontAwesome.Blazor.Components.IconContainer",
        "Jenin.FontAwesome.Blazor.Components.IconFor`1",
        "Jenin.FontAwesome.Blazor.Components.IconLayersContainer",
        "Jenin.FontAwesome.Blazor.Components.IconList",
        "Jenin.FontAwesome.Blazor.Components.IconListItem",
        "Jenin.FontAwesome.Blazor.Components.IconProgress",
        "Jenin.FontAwesome.Blazor.Components.IconRating",
        "Jenin.FontAwesome.Blazor.Components.IconStackContainer",
        "Jenin.FontAwesome.Blazor.Components.IconTheme",
        "Jenin.FontAwesome.Blazor.Components.IconToggleButton",
        "Jenin.FontAwesome.Blazor.Components.IconTransition",
        "Jenin.FontAwesome.Blazor.Components.NotificationBadge",
        "Jenin.FontAwesome.Blazor.Components.ShareButtons",
        "Jenin.FontAwesome.Blazor.Components.StatCard",
        "Jenin.FontAwesome.Blazor.Components.StatusDot",
        "Jenin.FontAwesome.Blazor.Components.SpriteIcon",
        "Jenin.FontAwesome.Blazor.Components.TextLayer",
        "Jenin.FontAwesome.Blazor.Components.TwoStateIcon",
        "Jenin.FontAwesome.Blazor.CustomIconDefinition",
        "Jenin.FontAwesome.Blazor.IconAttribute",
        "Jenin.FontAwesome.Blazor.IconNames",
        "Jenin.FontAwesome.Blazor.IconNames+Brands",
        "Jenin.FontAwesome.Blazor.IconNames+Regular",
        "Jenin.FontAwesome.Blazor.IconNames+Solid",
        "Jenin.FontAwesome.Blazor.FontAwesomeAssetMode",
        "Jenin.FontAwesome.Blazor.FontAwesomeOptions",
        "Jenin.FontAwesome.Blazor.IconRotate",
        "Jenin.FontAwesome.Blazor.IconSize",
        "Jenin.FontAwesome.Blazor.IconStackSize",
        "Jenin.FontAwesome.Blazor.IconStyle",
        "Jenin.FontAwesome.Blazor.IconSvg",
        "Jenin.FontAwesome.Blazor.IconTransitionKind",
        "Jenin.FontAwesome.Blazor.IconWidth",
        "Jenin.FontAwesome.Blazor.PullPosition",
        "Jenin.FontAwesome.Blazor.ServiceCollectionExtensions",
        "Jenin.FontAwesome.Blazor.Transformations.FlipHorizontal",
        "Jenin.FontAwesome.Blazor.Transformations.FlipVertical",
        "Jenin.FontAwesome.Blazor.Transformations.Positioning",
        "Jenin.FontAwesome.Blazor.Transformations.Rotating",
        "Jenin.FontAwesome.Blazor.Transformations.Scaling",
        "Jenin.FontAwesome.Blazor.Transformations.Transform",
        "Jenin.FontAwesome.Blazor.Transformations.Transformation",
        "Jenin.FontAwesome.Blazor.Typeface",
        "Jenin.FontAwesome.Blazor._Imports"
    ];

    private static Assembly Library => typeof(Icon).Assembly;

    [TestMethod]
    [TestCategory("Contract")]
    public void ExportedTypeSet_EqualsFrozenInventory() {
        var actual = Library.GetExportedTypes()
            .Select(static t => t.FullName!)
            .OrderBy(static x => x, StringComparer.Ordinal)
            .ToArray();

        actual.Should().Equal([.. _expectedExportedTypes.OrderBy(static x => x, StringComparer.Ordinal)]);
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void ExportedTypes_CarryNoObsoleteAttribute() {
        var obsolete = Library.GetExportedTypes()
            .Where(static t => t.GetCustomAttribute<ObsoleteAttribute>() is not null
                               || t.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                                   .Any(static m => m.GetCustomAttribute<ObsoleteAttribute>() is not null))
            .Select(static t => t.FullName!)
            .ToArray();

        obsolete.Should().BeEmpty("Obsolete members found: " + string.Join(", ", obsolete));
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void IconAnimationParameter_IsTypedAsAnimation() {
        var property = typeof(Icon).GetProperty(nameof(Icon.Animation));

        property.Should().NotBeNull();
        property!.PropertyType.Should().Be(typeof(Animation));
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void BoundaryVersion_IsNotPublic() {
        var property = typeof(ElementBase).GetProperty("BoundaryVersion", BindingFlags.Instance | BindingFlags.NonPublic);

        property.Should().NotBeNull();
        property!.GetMethod!.IsPublic.Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Contract")]
    [DataRow("Jenin.FontAwesome.Blazor.Extensions.DictionaryExtensions")]
    [DataRow("Jenin.FontAwesome.Blazor.Extensions.EnumExtensions")]
    [DataRow("Jenin.FontAwesome.Blazor.ValueAttribute")]
    [DataRow("Jenin.FontAwesome.Blazor.Transformations.ScalingKind")]
    [DataRow("Jenin.FontAwesome.Blazor.Transformations.PositioningKind")]
    public void ImplementationHelpers_AreInternal(string typeName) {
        var type = Library.GetType(typeName);

        type.Should().NotBeNull($"Type '{typeName}' was not found.");
        type!.IsVisible.Should().BeFalse($"Type '{typeName}' must not be exported.");
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void Library_DoesNotReferenceJsInterop() {
        var hasJsInterop = Library.GetReferencedAssemblies()
            .Any(static a => string.Equals(a.Name, "Microsoft.JSInterop", StringComparison.Ordinal));

        hasJsInterop.Should().BeFalse("The zero-interop contract requires no Microsoft.JSInterop reference.");
    }

    [TestMethod]
    [TestCategory("Contract")]
    public void Transformations_AreImmutableValueObjects() {
        var transform = new Transform().Grow(2);

        var appended = transform.Shrink(2);

        ReferenceEquals(transform, appended).Should().BeFalse();
        transform.GetTransformation().Should().Be("grow-2.00");
        appended.GetTransformation().Should().Be("grow-2.00 shrink-2.00");
    }
}
