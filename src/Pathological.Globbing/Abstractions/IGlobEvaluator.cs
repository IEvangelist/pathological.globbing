// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Abstractions;

/// <summary>
/// Defines the interface for evaluating glob patterns using the specified <see cref="GlobOptionsBuilder"/>.
/// </summary>
public interface IGlobEvaluator
{
    /// <summary>
    /// Gets the default instance of <see cref="IGlobEvaluator"/>.
    /// </summary>
    public static IGlobEvaluator Default { get; } = new DefaultGlobEvaluator();

    /// <summary>
    /// Evaluates the glob pattern using the specified <see cref="GlobOptionsBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="GlobOptionsBuilder"/> used to evaluate the glob pattern.</param>
    /// <returns>A <see cref="GlobEvaluationResult"/> object containing the results of the evaluation.</returns>
    public GlobEvaluationResult Evaluate(GlobOptionsBuilder builder)
    {
        var globEvaluationResult = builder.Evaluate();

        return globEvaluationResult;
    }
}
