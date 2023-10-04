// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IBasePathOption"/> instances.
/// </summary>
internal static class BasePathOptionExtensions
{
    /// <summary>
    /// Resolves the full path for the given <paramref name="path"/> by combining it with the base path of the <paramref name="basePathOption"/>.
    /// </summary>
    /// <param name="basePathOption">The <see cref="IBasePathOption"/> instance.</param>
    /// <param name="path">The path to resolve.</param>
    /// <returns>The full path.</returns>
    internal static string ResolvePath(
        this IBasePathOption basePathOption, string path)
    {
        var fullPath = Path.Combine(basePathOption.BasePath, path);

        return fullPath;
    }

    /// <summary>
    /// Converts the <see cref="IBasePathOption"/> instance to a <see cref="DirectoryInfoBase"/> instance.
    /// </summary>
    /// <param name="basePathOption">The <see cref="IBasePathOption"/> instance.</param>
    /// <returns>The <see cref="DirectoryInfoBase"/> instance.</returns>
    internal static DirectoryInfoBase ToDirectoryInfo(
        this IBasePathOption basePathOption)
    {
        var wrapper = new DirectoryInfoWrapper(
            directoryInfo: new DirectoryInfo(
                path: basePathOption.BasePath));

        return wrapper;
    }
}
