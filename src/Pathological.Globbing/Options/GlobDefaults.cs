// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Options;

/// <summary>
/// Contains default values for globbing operations.
/// </summary>
internal static class GlobDefaults
{
    /// <summary>
    /// The base path used for globbing. Defaults to the current directory.
    /// </summary>
    internal const string BasePath = ".";

    /// <summary>
    /// Determines whether globbing operations should be case-insensitive by default.
    /// </summary>
    internal const bool IsCaseInsensitive = true;
}
