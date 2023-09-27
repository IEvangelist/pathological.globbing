// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing;

/// <summary>
/// Provides methods for matching file paths against glob patterns.
/// </summary>
/// <param name="basePath">
/// The base path to start the globbing search from.
/// Defaults to <c>"."</c> which is the current directory.
/// </param>
/// <param name="isCaseInsensitive">
/// Whether or not to evaluate matches ignoring case sensitivity.
/// Defaults to <see langword="true"/>.
/// </param>
public sealed partial class Glob(
    string basePath = GlobDefaults.BasePath,
    bool isCaseInsensitive = GlobDefaults.IsCaseInsensitive)
{
    /// <summary>
    /// Gets the base path used for globbing, as assigned from <paramref name="basePath"/>.
    /// Defaults to <c>"."</c> which is the current directory.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="basePath"/> is null.</exception>
    public string BasePath { get; } = basePath
        ?? throw new ArgumentNullException(nameof(basePath));

    /// <summary>
    /// Returns an enumerable collection of file or directory paths that match a specified ignorePattern and ignore ignorePattern.
    /// </summary>
    /// <param name="pattern">The ignorePattern to match.</param>
    /// <param name="ignorePattern">The ignorePattern to ignore.</param>
    /// <returns>An enumerable collection of file or directory paths that match the specified ignorePattern and ignore ignorePattern.</returns>
    public IEnumerable<string> GetMatches(string pattern, string ignorePattern) =>
        GetMatches(patterns: [pattern], ignorePatterns: [ignorePattern]);

    /// <summary>
    /// Returns an enumerable collection of file or directory paths that match a specified ignorePattern.
    /// </summary>
    /// <param name="pattern">The ignorePattern to match against file or directory paths.</param>
    /// <param name="ignorePatterns">An array of patterns to ignore when searching for matches.</param>
    /// <returns>An enumerable collection of file or directory paths that match the specified ignorePattern.</returns>
    public IEnumerable<string> GetMatches(string pattern, string[] ignorePatterns) =>
        GetMatches(patterns: [pattern], ignorePatterns);

    /// <summary>
    /// Returns an enumerable collection of file or directory names that match the specified ignorePattern.
    /// </summary>
    /// <param name="pattern">The search ignorePattern to match against the names of files or directories in the file system.</param>
    /// <returns>An enumerable collection of file or directory names that match the specified ignorePattern.</returns>
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
    /// Returns an enumerable collection of strings that match the specified globbing patterns,
    /// while excluding any matches that also match the specified ignore pattern.
    /// </summary>
    /// <param name="patterns">An array of globbing patterns to match against.</param>
    /// <param name="ignorePattern">A globbing pattern to exclude from the matches.</param>
    /// <returns>An enumerable collection of strings that match the specified globbing patterns,
    /// while excluding any matches that also match the specified ignore pattern.</returns>
    public IEnumerable<string> GetMatches(string[] patterns, string ignorePattern) =>
        GetMatches(patterns, ignorePatterns: [ignorePattern]);

    /// <summary>
    /// Gets a collection of file paths that match the specified patterns and do not match the specified ignore patterns.
    /// </summary>
    /// <param name="patterns">An array of glob patterns to match against.</param>
    /// <param name="ignorePatterns">An array of glob patterns to exclude from the results.</param>
    /// <returns>An enumerable collection of file paths that match the specified patterns and do not match the specified ignore patterns.</returns>
    public IEnumerable<string> GetMatches(string[] patterns, string[] ignorePatterns)
    {
        var matcher = GetInitializedMatcher(patterns, ignorePatterns, isCaseInsensitive);

        return matcher.GetResultsInFullPath(BasePath);
    }
}
