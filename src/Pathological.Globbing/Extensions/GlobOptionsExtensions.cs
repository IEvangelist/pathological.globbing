// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Extensions;

/// <summary>
/// Provides extension methods for <see cref="GlobOptions"/> class.
/// </summary>
public static class GlobOptionsExtensions
{
    /// <summary>
    /// Executes the evaluation of the glob options.
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
    /// Converts the specified <see cref="GlobOptions"/> instance to a <see cref="Matcher"/> instance.
    /// </summary>
    /// <param name="options">The <see cref="GlobOptions"/> instance to convert.</param>
    /// <returns>A new <see cref="Matcher"/> instance.</returns>
    internal static Matcher ToMatcher(this GlobOptions options)
    {
        var matcher = new Matcher(
            comparisonType: options.IsCaseInsensitive
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal);

        matcher.AddIncludePatterns(options.Inclusions ?? []);
        matcher.AddExcludePatterns(options.Exclusions ?? []);

        return matcher;
    }
}
