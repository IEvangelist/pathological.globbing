// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Extensions;

internal static class GlobOptionsExtensions
{
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
