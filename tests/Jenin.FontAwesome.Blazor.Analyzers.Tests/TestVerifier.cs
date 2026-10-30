//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Analyzers.Tests;

/// <summary>
/// Minimal <see cref="IVerifier"/> backed by MSTest asserts. The Roslyn SDK's MSTest verifier
/// package is obsolete, so the project implements the small contract directly instead of
/// depending on it.
/// </summary>
internal sealed class TestVerifier : IVerifier {
    public void Empty<T>(string? message, IEnumerable<T> collection) =>
        Assert.AreEqual(0, collection.Count(), message);

    public void Equal<T>(T expected, T actual, string? message = null) =>
        Assert.AreEqual(expected, actual, message);

    public void True(bool assert, string? message = null) =>
        Assert.IsTrue(assert, message);

    public void False(bool assert, string? message = null) =>
        Assert.IsFalse(assert, message);

    [DoesNotReturn]
    public void Fail(string? message = null) =>
        Assert.Fail(message ?? "Verification failed.");

    public void LanguageIsSupported(string language) {
        if(language != LanguageNames.CSharp) {
            Assert.Fail("Unsupported language: " + language);
        }
    }

    public void NotEmpty<T>(string? message, IEnumerable<T> collection) =>
        Assert.AreNotEqual(0, collection.Count(), message);

    public IVerifier PushContext(string context) => this;

    public void SequenceEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? comparer, string? message = null) =>
        Assert.IsTrue(expected.SequenceEqual(actual, comparer), message);
}
