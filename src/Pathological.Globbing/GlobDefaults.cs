// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing;

/// <summary>
/// Contains default values for globbing operations.
/// </summary>
internal static class GlobDefaults
{
    /// <summary>
    /// The base path used for globbing. Defaults to the current directory.
    /// </summary>
    public const string BasePath = ".";

    /// <summary>
    /// Determines whether globbing operations should be case-insensitive by default.
    /// </summary>
    public const bool IsCaseInsensitive = true;
}
