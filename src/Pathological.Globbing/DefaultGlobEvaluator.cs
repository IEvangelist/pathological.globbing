// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing;

/// <summary>
/// The default implementation of the <see cref="IGlobEvaluator"/> interface.
/// </summary>
internal sealed class DefaultGlobEvaluator : IGlobEvaluator
{
    /// <summary>
    /// Gets the default instance of the <see cref="IGlobEvaluator"/> interface.
    /// </summary>
    public static IGlobEvaluator Default { get; } = new DefaultGlobEvaluator();
}
