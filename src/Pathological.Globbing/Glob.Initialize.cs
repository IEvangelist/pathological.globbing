// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing;

public sealed partial class Glob
{
    internal static Glob InitializeFromBuilder(GlobOptionsBuilder builder)
    {
        var options = builder.Build();

        var glob = new Glob(
            basePath: options.BasePath,
            isCaseInsensitive: options.IsCaseInsensitive)
            .InitializeMatcher(
                patterns: options.Patterns.ToArray(),
                ignorePatterns: options.IgnorePatterns.ToArray());

        return glob;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matcher"/> class with the specified include and exclude patterns.
    /// </summary>
    /// <param name="patterns">An array of include patterns.</param>
    /// <param name="ignorePatterns">An array of exclude patterns.</param>
    /// <returns>A new instance of the <see cref="Matcher"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either <paramref name="patterns"/> or <paramref name="ignorePatterns"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when both <paramref name="patterns"/> and <paramref name="ignorePatterns"/> are empty.</exception>
    /// <exception cref="ArgumentException">Thrown when any of the patterns in <paramref name="patterns"/> or <paramref name="ignorePatterns"/> 
    /// containers an empty, null or whitespace pattern.</exception>
    internal Glob InitializeMatcher(string[] patterns, string[] ignorePatterns)
    {
        var builder = new GlobOptionsBuilder(
            BasePath,
            isCaseInsensitive)
            .WithPatterns(patterns)
            .WithIgnorePatterns(ignorePatterns);

        _ = builder.Build(); // Validate

        _matcher = new Matcher(
            comparisonType: isCaseInsensitive
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal);

        if (patterns is { Length: > 0 })
        {
            _matcher.AddIncludePatterns(patterns);
        }

        if (ignorePatterns is { Length: > 0 })
        {
            _matcher.AddExcludePatterns(ignorePatterns);
        }

        return this;
    }
}
