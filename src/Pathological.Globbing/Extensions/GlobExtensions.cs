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
    /// Returns an enumerable collection of FileInfo objects that match the specified glob patterns.
    /// </summary>
    /// <param name="glob">The Glob instance to use for matching.</param>
    /// <param name="patterns">The glob patterns to match.</param>
    /// <param name="ignorePatterns">The glob patterns to ignore.</param>
    /// <returns>An enumerable collection of FileInfo objects that match the specified glob patterns.</returns>
    public static IEnumerable<FileInfo> GetMatchingFileInfos(
        this Glob glob,
        string[] patterns,
        string[] ignorePatterns)
    {
        foreach (var match in glob.GetGlobMatches(patterns, ignorePatterns))
        {
            yield return match.ToFileInfo(glob);
        }
    }

    /// <summary>
    /// Asynchronously gets the glob matches for the specified patterns and ignore patterns.
    /// </summary>
    /// <param name="glob">The <see cref="Glob"/> instance.</param>
    /// <param name="patterns">The patterns to match.</param>
    /// <param name="ignorePatterns">The patterns to ignore.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> of <see cref="GlobMatch"/> instances.</returns>
    public static async IAsyncEnumerable<GlobMatch> GetGlobMatchesAsync(
        this Glob glob,
        string[] patterns,
        string[] ignorePatterns,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var matches = glob.GetMatchesAsync(patterns, ignorePatterns, cancellationToken);

        await foreach (var match in matches)
        {
            yield return new GlobMatch(match);
        }
    }

    /// <summary>
    /// Asynchronously gets the glob matches for the specified patterns and ignore patterns.
    /// </summary>
    /// <param name="glob">The glob instance.</param>
    /// <param name="patterns">The patterns to match.</param>
    /// <param name="ignorePatterns">The patterns to ignore.</param>
    /// <param name="signal">The time to wait before cancelling the operation.</param>
    /// <returns>An asynchronous enumerable of <see cref="GlobMatch"/> instances.</returns>
    public static async IAsyncEnumerable<GlobMatch> GetGlobMatchesAsync(
        this Glob glob,
        string[] patterns,
        string[] ignorePatterns,
        TimeSpan signal)
    {
        var matches = glob.GetMatchesAsync(patterns, ignorePatterns, signal);

        await foreach (var match in matches)
        {
            yield return new GlobMatch(match);
        }
    }

    /// <summary>
    /// Asynchronously enumerates the file information of all files that match the specified glob patterns.
    /// </summary>
    /// <param name="glob">The glob pattern to match against.</param>
    /// <param name="patterns">The glob patterns to match.</param>
    /// <param name="ignorePatterns">The glob patterns to ignore.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An asynchronous enumerable of <see cref="FileInfo"/> objects that match the specified glob patterns.</returns>
    public static async IAsyncEnumerable<FileInfo> GetMatchingFileInfosAsync(
        this Glob glob,
        string[] patterns,
        string[] ignorePatterns,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var match in glob.GetGlobMatchesAsync(patterns, ignorePatterns, cancellationToken))
        {
            yield return match.ToFileInfo(glob);
        }
    }

    /// <summary>
    /// Asynchronously enumerates the file information of all files that match the specified glob patterns.
    /// </summary>
    /// <param name="glob">The glob pattern to match against.</param>
    /// <param name="patterns">The patterns to match.</param>
    /// <param name="ignorePatterns">The patterns to ignore.</param>
    /// <param name="signal">The time to wait before cancelling the operation.</param>
    /// <returns>An asynchronous enumerable of file information.</returns>
    public static async IAsyncEnumerable<FileInfo> GetMatchingFileInfosAsync(
        this Glob glob,
        string[] patterns,
        string[] ignorePatterns,
        TimeSpan signal)
    {
        await foreach (var match in glob.GetGlobMatchesAsync(patterns, ignorePatterns, signal))
        {
            yield return match.ToFileInfo(glob);
        }
    }
}
