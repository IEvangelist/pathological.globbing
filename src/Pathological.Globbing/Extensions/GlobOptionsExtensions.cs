// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Extensions;

/// <summary>
/// Provides extension methods for <see cref="GlobOptions"/> class.
/// </summary>
internal static class GlobOptionsExtensions
{
    /// <summary>
    /// Executes the evaluation of the glob options.
    /// </summary>
    /// <param name="options">The <see cref="GlobOptions"/> instance.</param>
    /// <returns>The <see cref="GlobEvaluationResult"/> instance.</returns>
    internal static GlobEvaluationResult ExecuteEvaluation(this GlobOptions options)
    {
        var matcher = new Matcher(
            options.IsCaseInsensitive
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal);

        matcher.AddIncludePatterns(options.Inclusions ?? []);
        matcher.AddExcludePatterns(options.Exclusions ?? []);

        var result = matcher.Execute(options.ToDirectoryInfo());

        return GlobEvaluationResult.FromPatternMatchingResult(result);
    }
}
