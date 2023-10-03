// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="GlobOptionsBuilder"/> class.
/// </summary>
internal static class GlobOptionsBuilderExtensions
{
    /// <summary>
    /// Evaluates the glob options and returns the result.
    /// </summary>
    /// <param name="builder">The <see cref="GlobOptionsBuilder"/> instance.</param>
    /// <returns>The <see cref="GlobEvaluationResult"/> instance.</returns>
    internal static GlobEvaluationResult Evaluate(this GlobOptionsBuilder builder)
    {
        var options = builder.Build();

        var matcher = new Matcher();

        matcher.AddIncludePatterns(options.Inclusions);
        matcher.AddExcludePatterns(options.Exclusions);

        var result = matcher.Execute(options.ToDirectoryInfo());

        return GlobEvaluationResult.FromPatternMatchingResult(result);
    }
}
