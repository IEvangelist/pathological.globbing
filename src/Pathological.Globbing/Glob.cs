﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing;

/// <summary>
/// Provides methods for matching file paths against glob patterns.
/// </summary>
/// <param name="basePath"></param>
public sealed class Glob(string basePath = ".")
{
    /// <summary>
    /// Returns an enumerable collection of file or directory paths that match a specified pattern and ignore pattern.
    /// </summary>
    /// <param name="pattern">The pattern to match.</param>
    /// <param name="ignorePattern">The pattern to ignore.</param>
    /// <returns>An enumerable collection of file or directory paths that match the specified pattern and ignore pattern.</returns>
    public IEnumerable<string> GetMatches(string pattern, string ignorePattern) =>
        GetMatches(patterns: [pattern], ignorePatterns: [ignorePattern]);

    /// <summary>
    /// Returns an enumerable collection of file or directory paths that match a specified pattern.
    /// </summary>
    /// <param name="pattern">The pattern to match against file or directory paths.</param>
    /// <param name="ignorePatterns">An array of patterns to ignore when searching for matches.</param>
    /// <returns>An enumerable collection of file or directory paths that match the specified pattern.</returns>
    public IEnumerable<string> GetMatches(string pattern, string[] ignorePatterns) =>
        GetMatches(patterns: [pattern], ignorePatterns);

    /// <summary>
    /// Returns an enumerable collection of file or directory names that match the specified pattern.
    /// </summary>
    /// <param name="pattern">The search pattern to match against the names of files or directories in the file system.</param>
    /// <returns>An enumerable collection of file or directory names that match the specified pattern.</returns>
    public IEnumerable<string> GetMatches(string pattern) =>
        GetMatches(patterns: [pattern]);

    /// <summary>
    /// Returns an enumerable collection of file or directory paths that match the specified patterns.
    /// </summary>
    /// <param name="patterns">An array of globbing patterns to match against file or directory paths.</param>
    /// <returns>An enumerable collection of file or directory paths that match the specified patterns.</returns>
    public IEnumerable<string> GetMatches(string[] patterns) =>
        GetMatches(patterns, ignorePatterns: []);

    /// <summary>
    /// Gets a collection of file paths that match the specified patterns and do not match the specified ignore patterns.
    /// </summary>
    /// <param name="patterns">An array of glob patterns to match against.</param>
    /// <param name="ignorePatterns">An array of glob patterns to exclude from the results.</param>
    /// <returns>An enumerable collection of file paths that match the specified patterns and do not match the specified ignore patterns.</returns>
    public IEnumerable<string> GetMatches(string[] patterns, string[] ignorePatterns)
    {
        var matcher = new Matcher();

        if (patterns is { Length: > 0 })
        {
            matcher.AddIncludePatterns(patterns);
        }

        if (ignorePatterns is { Length: > 0 })
        {
            matcher.AddExcludePatterns(ignorePatterns);
        }

        return matcher.GetResultsInFullPath(basePath);
    }

    /// <summary>
    /// Asynchronously gets all file and directory matches for the specified pattern, while optionally ignoring specified patterns.
    /// </summary>
    /// <param name="pattern">The pattern to match against.</param>
    /// <param name="ignorePattern">The pattern to ignore.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An asynchronous enumerable of all file and directory matches for the specified pattern.</returns>
    public IAsyncEnumerable<string> GetMatchesAsync(
        string pattern,
        string ignorePattern,
        CancellationToken cancellationToken = default) =>
        GetMatchesAsync(patterns: [pattern], ignorePatterns: [ignorePattern], cancellationToken);

    /// <summary>
    /// Asynchronously retrieves a collection of file or directory paths that match the specified pattern.
    /// </summary>
    /// <param name="pattern">The pattern to match against file or directory paths.</param>
    /// <param name="ignorePatterns">An array of patterns to ignore when searching for matches.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An asynchronous enumerable collection of file or directory paths that match the specified pattern.</returns>
    public IAsyncEnumerable<string> GetMatchesAsync(
        string pattern,
        string[] ignorePatterns,
        CancellationToken cancellationToken = default) =>
        GetMatchesAsync(patterns: [pattern], ignorePatterns, cancellationToken);

    /// <summary>
    /// Asynchronously returns an enumerable collection of file or directory paths that match the specified globbing pattern.
    /// </summary>
    /// <param name="pattern">The globbing pattern to match.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> that yields the matched file or directory paths.</returns>
    public IAsyncEnumerable<string> GetMatchesAsync(
        string pattern,
        CancellationToken cancellationToken = default) =>
        GetMatchesAsync(patterns: [pattern], ignorePatterns: [], cancellationToken);

    /// <summary>
    /// Asynchronously searches for files and directories that match the specified patterns.
    /// </summary>
    /// <param name="patterns">The patterns to search for.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the search.</param>
    /// <returns>An asynchronous enumerable of file and directory paths that match the specified patterns.</returns>
    public IAsyncEnumerable<string> GetMatchesAsync(
        string[] patterns,
        CancellationToken cancellationToken = default) =>
        GetMatchesAsync(patterns, ignorePatterns: [], cancellationToken);

    /// <summary>
    /// Asynchronously enumerates all files that match the specified patterns and do not match the specified ignore patterns.
    /// </summary>
    /// <param name="patterns">An array of globbing patterns to match.</param>
    /// <param name="ignorePatterns">An array of globbing patterns to ignore.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An asynchronous enumerable of file paths that match the specified patterns and do not match the specified ignore patterns.</returns>
    public async IAsyncEnumerable<string> GetMatchesAsync(
        string[] patterns,
        string[] ignorePatterns,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var matcher = new Matcher();

        if (patterns is { Length: > 0 })
        {
            matcher.AddIncludePatterns(patterns);
        }

        if (ignorePatterns is { Length: > 0 })
        {
            matcher.AddExcludePatterns(ignorePatterns);
        }

        await foreach (var file in EnumerateFilesAsync(Matches, cancellationToken))
        {
            yield return file;
        }

        bool Matches(string filePath)
        {
            var result = matcher.Match(basePath, filePath);

            return result.HasMatches;
        }
    }

    private async IAsyncEnumerable<string> EnumerateFilesAsync(
        Func<string, bool> matches,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var channel = Channel.CreateUnbounded<string>();

        var writer = channel.Writer;

        var writeTask = Task.Run(
            async () => await WriteFilesAsync(basePath, writer, matches, cancellationToken),
            cancellationToken);

        var reader = channel.Reader;

        await foreach (var filePath in reader.ReadAllAsync(cancellationToken))
        {
            yield return filePath;
        }

        await writeTask; // Ensure completion (capturing exceptions).
    }

    private static async ValueTask WriteFilesAsync(
        string basePath,
        ChannelWriter<string> writer,
        Func<string, bool> matches,
        CancellationToken cancellationToken)
    {
        try
        {
            var options = new EnumerationOptions
            {
                RecurseSubdirectories = true,
                MatchCasing = MatchCasing.CaseInsensitive
            };

            var files =
                Directory.EnumerateFiles(basePath, "*", options);

            foreach (var filePath in files)
            {
                if (matches(filePath) is false)
                {
                    continue;
                }

                await writer.WaitToWriteAsync(cancellationToken);

                writer.TryWrite(filePath);
            }

            writer.Complete();
        }
        catch (Exception ex)
        {
            writer.TryComplete(ex);
        }
    }
}