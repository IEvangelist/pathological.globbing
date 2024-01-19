// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Internals;

/// <summary>
/// Provides a class for asynchronously enumerating files in a directory.
/// Inspired by:
/// <a href="https://github.com/dotnet/runtime/issues/809#issuecomment-565138915">
/// .NET runtime issue #809
/// </a>
/// </summary>
internal sealed class DirectoryAsyncEnumerable
{
    /// <summary>
    /// Asynchronously enumerates files in a directory that match a specified search pattern and option.
    /// </summary>
    /// <param name="path">The path to the directory to search.</param>
    /// <param name="searchPattern">The search string to match against the names of files in path.</param>
    /// <param name="enumerationOptions">Specifies options for the enumeration operation.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An asynchronous enumerable collection of file paths that match the search pattern.</returns>
    internal static async IAsyncEnumerable<string> EnumerateFilesAsync(
        string path,
        string searchPattern,
        EnumerationOptions enumerationOptions,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        using var enumerator = await Task.Run(
            function: () => Directory.EnumerateFiles(path, searchPattern, enumerationOptions).GetEnumerator(),
            cancellationToken);

        while (await Task.Run(function: enumerator.MoveNext, cancellationToken))
        {
            yield return Path.GetFullPath(enumerator.Current);
        }
    }
}
