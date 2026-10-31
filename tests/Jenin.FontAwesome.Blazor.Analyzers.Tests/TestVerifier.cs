//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Analyzers.Tests;

/// <summary>
/// Minimal <see cref="IVerifier"/> backed by Jeninnet.Assertions. The Roslyn SDK's MSTest verifier
/// package is obsolete, so the project implements the small contract directly instead of
/// depending on it.
/// </summary>
internal sealed class TestVerifier : IVerifier {
    public void Empty<T>(string? message, IEnumerable<T> collection) =>
        collection.Should().BeEmpty(message);

    public void Equal<T>(T expected, T actual, string? message = null) =>
        actual.Should().Be(expected, message);

    public void True(bool assert, string? message = null) =>
        assert.Should().BeTrue(message);

    public void False(bool assert, string? message = null) =>
        assert.Should().BeFalse(message);

    [DoesNotReturn]
    public void Fail(string? message = null) =>
        throw new AssertionFailedException(message ?? "Verification failed.");

    public void LanguageIsSupported(string language) {
        if(language != LanguageNames.CSharp) {
            throw new AssertionFailedException("Unsupported language: " + language);
        }
    }

    public void NotEmpty<T>(string? message, IEnumerable<T> collection) =>
        collection.Should().NotBeEmpty(message);

    public IVerifier PushContext(string context) => this;

    public void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? comparer, string? message = null) =>
        expected.SequenceEqual(actual, comparer).Should().BeTrue(message);
}
