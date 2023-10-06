// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Extensions;

/// <summary>
/// Provides extension methods for <see cref="GlobOptions"/> class.
/// </summary>
public static class GlobOptionsExtensions
{
    private readonly static GlobOptionsCache _cache = new();

    /// <summary>
    /// Determines whether the specified file matches the glob pattern using the specified options.
    /// </summary>
    /// <param name="options">The globbing options to use.</param>
    /// <param name="file">The file to match.</param>
    /// <returns><c>true</c> if the file matches the glob pattern; otherwise, <c>false</c>.</returns>
    public static bool IsMatch(this GlobOptions options, string file)
    {
        var matcher = _cache[options];

        var result = matcher.Match(file);

        return result.HasMatches;
    }

    /// <summary>
    /// Converts the specified <see cref="GlobOptions"/> instance to a <see cref="Matcher"/> instance.
    /// </summary>
    /// <param name="options">The <see cref="GlobOptions"/> instance to convert.</param>
    /// <returns>A new <see cref="Matcher"/> instance.</returns>
    internal static Matcher ToMatcher(this GlobOptions options)
    {
        var matcher = new Matcher(
            comparisonType: options.IgnoreCase
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal);

        matcher.AddIncludePatterns(options.Inclusions ?? []);
        matcher.AddExcludePatterns(options.Exclusions ?? []);

        return matcher;
    }

    /// <summary>
    /// Executes the evaluation of the globOptions options.
    /// </summary>
    /// <param name="options">The <see cref="GlobOptions"/> instance.</param>
    /// <returns>The <see cref="GlobEvaluationResult"/> instance.</returns>
    public static GlobEvaluationResult ExecuteEvaluation(this GlobOptions options)
    {
        var matcher = options.ToMatcher();

        var result = matcher.Execute(directoryInfo: options.ToDirectoryInfo());

        return GlobEvaluationResult.FromPatternMatchingResult(result, options);
    }

    /// <summary>
    /// Gets the globOptions matches for the specified patterns and ignore patterns.
    /// </summary>
    /// <param name="globOptions">The glob pattern to match against.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="GlobMatch"/> instances.</returns>
    public static IEnumerable<GlobMatch> GetGlobMatches(this GlobOptions globOptions)
    {
        var matcher = globOptions.ToMatcher();

        return matcher.GetResultsInFullPath(globOptions.BasePath)
            .Select(match => new GlobMatch(match));
    }

    /// <summary>
    /// Returns an enumerable collection of FileInfo objects that match the specified globOptions patterns.
    /// </summary>
    /// <param name="globOptions">The glob pattern to match against.</param>
    /// <returns>An enumerable collection of FileInfo objects that match the specified globOptions patterns.</returns>
    public static IEnumerable<FileInfo> GetMatchingFileInfos(this GlobOptions globOptions)
    {
        var matcher = globOptions.ToMatcher();

        return matcher.GetResultsInFullPath(globOptions.BasePath)
            .Select(match =>
                new GlobMatch(match).ToFileInfo(globOptions));
    }

    /// <summary>
    /// Asynchronously gets the globOptions matches for the specified patterns and ignore patterns.
    /// </summary>
    /// <param name="globOptions">The glob pattern to match against.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> of <see cref="GlobMatch"/> instances.</returns>
    public static async IAsyncEnumerable<GlobMatch> GetGlobMatchesAsync(
        this GlobOptions globOptions,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var glob = new Glob(
            globOptions.BasePath,
            globOptions.IgnoreCase);

        var matches = glob.GetMatchesAsync(
            (globOptions.Inclusions ?? []).ToArray(),
            (globOptions.Exclusions ?? []).ToArray(),
            cancellationToken);

        await foreach (var match in matches)
        {
            yield return new GlobMatch(match);
        }
    }

    /// <summary>
    /// Asynchronously gets the globOptions matches for the specified patterns and ignore patterns.
    /// </summary>
    /// <param name="globOptions">The glob pattern to match against.</param>
    /// <param name="signal">The time to wait before cancelling the operation.</param>
    /// <returns>An asynchronous enumerable of <see cref="GlobMatch"/> instances.</returns>
    public static async IAsyncEnumerable<GlobMatch> GetGlobMatchesAsync(
        this GlobOptions globOptions,
        [DisallowNull] TimeSpan signal)
    {
        var glob = new Glob(
            globOptions.BasePath,
            globOptions.IgnoreCase);

        var matches = glob.GetMatchesAsync(
            (globOptions.Inclusions ?? []).ToArray(),
            (globOptions.Exclusions ?? []).ToArray(),
            signal);

        await foreach (var match in matches)
        {
            yield return new GlobMatch(match);
        }
    }

    /// <summary>
    /// Asynchronously enumerates the file information of all files that match the specified globOptions patterns.
    /// </summary>
    /// <param name="globOptions">The glob pattern to match against.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An asynchronous enumerable of <see cref="FileInfo"/> objects that match the specified globOptions patterns.</returns>
    public static IAsyncEnumerable<FileInfo> GetMatchingFileInfosAsync(
        this GlobOptions globOptions,
        CancellationToken cancellationToken = default)
    {
        var glob = new Glob(
            globOptions.BasePath,
            globOptions.IgnoreCase);

        return glob.GetMatchingFileInfosAsync(
            (globOptions.Inclusions ?? []).ToArray(),
            (globOptions.Exclusions ?? []).ToArray(),
            cancellationToken);
    }

    /// <summary>
    /// Asynchronously enumerates the file information of all files that match the specified globOptions patterns.
    /// </summary>
    /// <param name="globOptions">The glob pattern to match against.</param>
    /// <param name="signal">The time to wait before cancelling the operation.</param>
    /// <returns>An asynchronous enumerable of file information.</returns>
    public static IAsyncEnumerable<FileInfo> GetMatchingFileInfosAsync(
        this GlobOptions globOptions,
        TimeSpan signal)
    {
        var glob = new Glob(
            globOptions.BasePath,
            globOptions.IgnoreCase);

        return glob.GetMatchingFileInfosAsync(
            (globOptions.Inclusions ?? []).ToArray(),
            (globOptions.Exclusions ?? []).ToArray(),
            signal);
    }
}
