// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="Glob"/> class.
/// </summary>
public static class GlobExtensions
{
    /// <summary>
    /// Gets the glob matches for the specified patterns and ignore patterns.
    /// </summary>
    /// <param name="glob">The <see cref="Glob"/> instance.</param>
    /// <param name="patterns">The patterns to match.</param>
    /// <param name="ignorePatterns">The patterns to ignore.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="GlobMatch"/> instances.</returns>
    public static IEnumerable<GlobMatch> GetGlobMatches(
        this Glob glob,
        string[] patterns,
        string[] ignorePatterns)
    {
        var matches = glob.GetMatches(patterns, ignorePatterns);

        return matches.Select(match => new GlobMatch(match));
    }

    /// <summary>
    /// Asynchronously gets the glob matches for the specified patterns and ignore patterns.
    /// </summary>
    /// <param name="glob">The <see cref="Glob"/> instance.</param>
    /// <param name="patterns">The patterns to match.</param>
    /// <param name="ignorePatterns">The patterns to ignore.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> of <see cref="GlobMatch"/> instances.</returns>
    public static async IAsyncEnumerable<GlobMatch> GetGlobMatchesAsync(
        this Glob glob,
        string[] patterns,
        string[] ignorePatterns)
    {
        var matches = glob.GetMatchesAsync(patterns, ignorePatterns);

        await foreach (var match in matches)
        {
            yield return new GlobMatch(match);
        }
    }

    internal static GlobMatchingResult ExecuteMatcher(this Glob glob)
    {
        var wrapper = new DirectoryInfoWrapper(
            directoryInfo: new DirectoryInfo(
                path: glob.BasePath));

        var result = glob._matcher!.Execute(wrapper);

        return (GlobMatchingResult)result;
    }
}
