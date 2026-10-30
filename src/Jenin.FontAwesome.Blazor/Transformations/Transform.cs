//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Transformations;

/// <summary>
/// Immutable, ordered composition of <see cref="Transformation"/> values.
/// <para>
/// Operations preserve insertion order and return a new <see cref="Transform"/>; this instance is never mutated.
/// </para>
/// <para>
/// Construct an empty instance and chain instance operations, for example:
/// <c>new Transform().Shrink(8).Up(6).Rotate(-30)</c>.
/// </para>
/// </summary>
public class Transform {
    private readonly Transformation[] _transformations;

    /// <summary>
    /// Creates an empty <see cref="Transform"/>.
    /// </summary>
    public Transform() => _transformations = [];

    private Transform(Transformation[] transformations) => _transformations = transformations;

    /// <summary>
    /// Appends the given transformations in order and returns a new <see cref="Transform"/>.
    /// <param name="transformations">The transformations to append in order.</param>
    /// <returns>A new <see cref="Transform"/> with <paramref name="transformations"/> appended in order.</returns>
    /// </summary>
    public Transform Add(params Transformation[] transformations)
        => new([.. _transformations, .. transformations ?? []]);

    /// <summary>
    /// Serializes the ordered transformations to a space-joined Font Awesome Power Transform value.
    /// <para>Pure, ordered, invariant.</para>
    /// <returns>The space-joined Font Awesome Power Transform value.</returns>
    /// </summary>
    public string GetTransformation() => string.Join(' ', _transformations.Select(x => x.GetTransformation()));

    /// <summary>
    /// Appends a grow scaling token in order and returns a new <see cref="Transform"/>.
    /// </summary>
    /// <param name="value">The scaling amount in 1/16em units.</param>
    /// <returns>A new <see cref="Transform"/> including this token.</returns>
    public Transform Grow(float value) => Add(new Scaling(value, ScalingKind.Grow));

    /// <summary>
    /// Appends a shrink scaling token in order and returns a new <see cref="Transform"/>.
    /// </summary>
    /// <param name="value">The scaling amount in 1/16em units.</param>
    /// <returns>A new <see cref="Transform"/> including this token.</returns>
    public Transform Shrink(float value) => Add(new Scaling(value, ScalingKind.Shrink));

    /// <summary>
    /// Appends an up positioning token in order and returns a new <see cref="Transform"/>.
    /// </summary>
    /// <param name="value">The offset in 1/16em units.</param>
    /// <returns>A new <see cref="Transform"/> including this token.</returns>
    public Transform Up(float value) => Add(new Positioning(value, PositioningKind.Up));

    /// <summary>
    /// Appends a down positioning token in order and returns a new <see cref="Transform"/>.
    /// </summary>
    /// <param name="value">The offset in 1/16em units.</param>
    /// <returns>A new <see cref="Transform"/> including this token.</returns>
    public Transform Down(float value) => Add(new Positioning(value, PositioningKind.Down));

    /// <summary>
    /// Appends a left positioning token in order and returns a new <see cref="Transform"/>.
    /// </summary>
    /// <param name="value">The offset in 1/16em units.</param>
    /// <returns>A new <see cref="Transform"/> including this token.</returns>
    public Transform Left(float value) => Add(new Positioning(value, PositioningKind.Left));

    /// <summary>
    /// Appends a right positioning token in order and returns a new <see cref="Transform"/>.
    /// </summary>
    /// <param name="value">The offset in 1/16em units.</param>
    /// <returns>A new <see cref="Transform"/> including this token.</returns>
    public Transform Right(float value) => Add(new Positioning(value, PositioningKind.Right));

    /// <summary>
    /// Appends a rotation token in order and returns a new <see cref="Transform"/>.
    /// </summary>
    /// <param name="value">The rotation in degrees (negative values allowed).</param>
    /// <returns>A new <see cref="Transform"/> including this token.</returns>
    public Transform Rotate(float value) => Add(new Rotating(value));

    /// <summary>
    /// Appends a vertical flip in order and returns a new <see cref="Transform"/>.
    /// </summary>
    /// <returns>A new <see cref="Transform"/> including this token.</returns>
    public Transform FlipV() => Add(new FlipVertical());

    /// <summary>
    /// Appends a horizontal flip in order and returns a new <see cref="Transform"/>.
    /// </summary>
    /// <returns>A new <see cref="Transform"/> including this token.</returns>
    public Transform FlipH() => Add(new FlipHorizontal());

    /// <summary>
    /// Appends both a vertical and a horizontal flip in order and returns a new <see cref="Transform"/>.
    /// </summary>
    /// <returns>A new <see cref="Transform"/> including these tokens.</returns>
    public Transform FlipBoth() => Add(new FlipVertical(), new FlipHorizontal());
}
