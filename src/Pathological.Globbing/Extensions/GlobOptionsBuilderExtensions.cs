// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="GlobOptionsBuilder"/> class.
/// </summary>
public static class GlobOptionsBuilderExtensions
{
    /// <summary>
    /// Executes an evaluation of the glob options and returns the result.
    /// </summary>
    /// <param name="builder">The <see cref="GlobOptionsBuilder"/> instance.</param>
    /// <returns>The <see cref="GlobEvaluationResult"/> instance.</returns>
    public static GlobEvaluationResult ExecuteEvaluation(this GlobOptionsBuilder builder)
    {
        var options = builder.Build();

        return options.ExecuteEvaluation();
    }
}
