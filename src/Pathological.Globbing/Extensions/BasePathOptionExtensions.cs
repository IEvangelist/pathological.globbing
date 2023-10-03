// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Extensions;

internal static class BasePathOptionExtensions
{
    internal static string ResolvePath(this IBasePathOption basePathOption, string path)
    {
        var fullPath = Path.Combine(basePathOption.BasePath, path);

        return fullPath;
    }
}
