// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Results;

/// <summary>
/// Represents the result of a glob matching operation.
/// </summary>
/// <param name="HasMatches">A boolean value indicating whether or not there were any matches.</param>
/// <param name="Matches">The collection of matches.</param>
public readonly record struct GlobMatchingResult(
    bool HasMatches,
    IEnumerable<GlobMatch> Matches)
{
    /// <summary>
    /// Explicitly converts a <see cref="PatternMatchingResult"/> object to a <see cref="GlobMatchingResult"/> value.
    /// </summary>
    /// <param name="patternMatchingResult">The <see cref="PatternMatchingResult"/> object to convert.</param>
    public static explicit operator GlobMatchingResult(
        PatternMatchingResult patternMatchingResult) =>
        new(HasMatches: patternMatchingResult.HasMatches,
            Matches: patternMatchingResult.Files.Select(
                selector: static filePatternMatch => (GlobMatch)filePatternMatch));
}
