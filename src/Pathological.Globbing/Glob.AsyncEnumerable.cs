// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing;

public sealed partial class Glob
{
    /// <summary>
    /// Asynchronously returns an <see cref="IAsyncEnumerable{T}"/> that yields all file and directory paths that match the specified globbing pattern, while ignoring the specified ignore pattern.
    /// </summary>
    /// <param name="pattern">The ignorePattern to match against.</param>
    /// <param name="ignorePattern">The ignorePattern to ignore.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> that yields all file and directory paths that match the specified globbing pattern, while ignoring the specified ignore pattern.</returns>
    public IAsyncEnumerable<string> GetMatchesAsync(
        string pattern,
        string ignorePattern,
        CancellationToken cancellationToken = default) =>
        GetMatchesAsync(patterns: [pattern], ignorePatterns: [ignorePattern], cancellationToken);

    /// <summary>
    /// Asynchronously returns an <see cref="IAsyncEnumerable{T}"/> that yields all file and directory paths that match the specified globbing pattern, while ignoring the specified ignore patterns.
    /// </summary>
    /// <param name="pattern">The ignorePattern to match against file or directory paths.</param>
    /// <param name="ignorePatterns">An array of patterns to ignore when searching for matches.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> that yields all file and directory paths that match the specified globbing pattern, while ignoring the specified ignore patterns.</returns>
    public IAsyncEnumerable<string> GetMatchesAsync(
        string pattern,
        string[] ignorePatterns,
        CancellationToken cancellationToken = default) =>
        GetMatchesAsync(patterns: [pattern], ignorePatterns, cancellationToken);

    /// <summary>
    /// Asynchronously returns an <see cref="IAsyncEnumerable{T}"/> that yields all file and directory paths that match the specified globbing pattern.
    /// </summary>
    /// <param name="pattern">The globbing ignorePattern to match.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> that yields all file and directory paths that match the specified globbing pattern.</returns>
    public IAsyncEnumerable<string> GetMatchesAsync(
        string pattern,
        CancellationToken cancellationToken = default) =>
        GetMatchesAsync(patterns: [pattern], ignorePatterns: [], cancellationToken);

    /// <summary>
    /// Asynchronously returns an <see cref="IAsyncEnumerable{T}"/> that yields all file and directory paths that match the specified globbing patterns, while ignoring the specified ignore pattern.
    /// </summary>
    /// <param name="patterns">The patterns to search for.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the search.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> that yields all file and directory paths that match the specified globbing patterns.</returns>
    public IAsyncEnumerable<string> GetMatchesAsync(
        string[] patterns,
        CancellationToken cancellationToken = default) =>
        GetMatchesAsync(patterns, ignorePatterns: [], cancellationToken);

    /// <summary>
    /// Asynchronously returns an <see cref="IAsyncEnumerable{T}"/> that yields all file and directory paths that match the specified globbing patterns, while ignoring the specified ignore pattern.
    /// </summary>
    /// <param name="patterns">An array of globbing patterns to match against file and directory paths.</param>
    /// <param name="ignorePattern">A globbing pattern to ignore when matching file and directory paths.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> that yields all file and directory paths that match the specified globbing patterns, while ignoring the specified ignore pattern.</returns>
    public IAsyncEnumerable<string> GetMatchesAsync(
        string[] patterns,
        string ignorePattern,
        CancellationToken cancellationToken = default) =>
        GetMatchesAsync(patterns, ignorePatterns: [ignorePattern], cancellationToken);

    /// <summary>
    /// Asynchronously returns an <see cref="IAsyncEnumerable{T}"/> that yields all file and directory paths that match the specified globbing patterns, while ignoring the specified ignore patterns.
    /// </summary>
    /// <param name="patterns">The patterns to match.</param>
    /// <param name="ignorePatterns">The patterns to ignore.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{T}"/> that yields all file and directory paths that match the specified globbing patterns, while ignoring the specified ignore pattern.</returns>
    public async IAsyncEnumerable<string> GetMatchesAsync(
        string[] patterns,
        string[] ignorePatterns,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var matcher = GetInitializedMatcher(patterns, ignorePatterns, isCaseInsensitive);

        await foreach (var filePath in EnumerateFilesAsync(
            filePath => Matches(matcher, BasePath, filePath), cancellationToken))
        {
            yield return filePath;
        }

        static bool Matches(Matcher matcher, string basePath, string filePath)
        {
            var result = matcher.Match(basePath, filePath);

            return result.HasMatches;
        }
    }

    /// <summary>
    /// Enumerates all files in the directory tree rooted at the base path that match the specified pattern asynchronously.
    /// </summary>
    /// <param name="matches">A function that determines whether a file matches the specified pattern.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An asynchronous enumerable of file paths that match the specified pattern.</returns>
    private async IAsyncEnumerable<string> EnumerateFilesAsync(
        Func<string, bool> matches,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var channel = Channel.CreateUnbounded<string>();

        var writer = channel.Writer;

        var writeTask = Task.Run(
            function: () => WriteFilesAsync(BasePath, writer, matches, cancellationToken),
            cancellationToken);

        var reader = channel.Reader;

        var filePaths = reader.ReadAllAsync(cancellationToken);

        await foreach (var filePath in filePaths)
        {
            yield return filePath;
        }

        await writeTask; // Ensure completion (propagating exceptions).
    }

    /// <summary>
    /// Asynchronously writes the file paths that match the specified pattern to the provided <see cref="ChannelWriter{T}"/>.
    /// </summary>
    /// <param name="basePath">The base path to search for files.</param>
    /// <param name="writer">The <see cref="ChannelWriter{T}"/> to write the file paths to.</param>
    /// <param name="matches">A function that determines whether a file path matches the specified pattern.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task WriteFilesAsync(
        string basePath,
        ChannelWriter<string> writer,
        Func<string, bool> matches,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            var options = new EnumerationOptions
            {
                RecurseSubdirectories = true,
                MatchCasing = MatchCasing.CaseInsensitive,
                IgnoreInaccessible = true,
            };

            var files = Directory.EnumerateFiles(basePath, "*", options);

            foreach (var filePath in files.Where(matches))
            {
                if (await writer.WaitToWriteAsync(cancellationToken))
                {
                    writer.TryWrite(filePath);
                }
            }

            writer.Complete();
        }
        catch (Exception ex)
        {
            writer.TryComplete(ex);
        }
    }
}
