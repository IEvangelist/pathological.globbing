// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Extensions;

internal static class StringExtensions
{
    private const char NewLine = '\n';

    public static string? NullIfEmpty(this string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value;

    public static int GetLineNumberFromIndex(this string content, int index)
    {
        var lineNumber = 1;

        for (var i = 0; i < index; ++i)
        {
            if (content[i] is NewLine)
            {
                ++lineNumber;
            }
        }

        return lineNumber;
    }

    public static string? TraverseDirectoriesAndFindFile(this string? path, string fileName)
    {
        if (Directory.Exists(path))
        {
            var propsFile = Path.Combine(path, fileName);

            if (File.Exists(propsFile))
            {
                return propsFile;
            }
            else
            {
                var dirInfo = new DirectoryInfo(path);
                var parentDir = dirInfo.Parent;

                if (parentDir is null)
                {
                    return default;
                }
                else
                {
                    // If not, recursively call the method on the parent directory
                    return parentDir.FullName.TraverseDirectoriesAndFindFile(fileName);
                }
            }
        }

        return default;
    }
}
