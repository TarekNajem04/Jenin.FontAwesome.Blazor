//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Transformations;

/// <summary>
/// Base immutable value object for Font Awesome Power Transform tokens.
/// <para>
/// Concrete transformations serialize to a single ordered invariant token (for example <c>grow-6</c>,
/// <c>rotate--90</c>, or <c>flip-h</c>). Values are normalized at construction; <see cref="GetTransformation"/>
/// is pure and never mutates.
/// </para>
/// <para>
/// Custom transformations may subclass this type (model extension contract).
/// </para>
/// </summary>
/// <remarks>
/// Initializes a transformation with the given <paramref name="value"/>.
/// </remarks>
/// <param name="value">The raw transform value before domain normalization.</param>
public abstract class Transformation(float value) {

    /// <summary>
    /// The normalized value carried by this transformation (unused by value-less tokens such as flips).
    /// </summary>
    protected float Value { get; } = value;

    /// <summary>
    /// Serializes this transformation to its Font Awesome Power Transform token.
    /// <para>Pure, deterministic, invariant.</para>
    /// <returns>The Font Awesome Power Transform token for this transformation.</returns>
    /// </summary>
    public abstract string GetTransformation();

    /// <summary>
    /// Formats <see cref="Value"/> with two decimals, invariant culture.
    /// <returns><see cref="Value"/> formatted with two decimals using the invariant culture.</returns>
    /// </summary>
    protected string GetFormattedValue() => Value.ToString("F", CultureInfo.InvariantCulture);
}
