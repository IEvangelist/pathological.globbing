// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Options;

/// <summary>
/// Represents the options to use when performing a glob search. Options are assumed to be 
/// valid, having been built with <see cref="GlobOptionsBuilder.Build"/>.
/// </summary>
/// <param name="BasePath">The base path to use when evaluating the glob pattern.</param>
/// <param name="IsCaseInsensitive">Whether or not to ignore case when evaluating the glob pattern.</param>
/// <param name="Inclusions">The list of glob patterns to include in the search.</param>
/// <param name="Exclusions">The list of glob patterns to exclude from the search.</param>
public readonly record struct GlobOptions(
    [DisallowNull] string BasePath,
    bool IsCaseInsensitive,
    [DisallowNull] IEnumerable<string> Inclusions,
    [DisallowNull] IEnumerable<string> Exclusions) : IBasePathOption;