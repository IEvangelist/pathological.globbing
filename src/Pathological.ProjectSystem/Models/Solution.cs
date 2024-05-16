// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Models;

/// <summary>
/// Represents a solution file (supports both .sln and .slnx formats).
/// </summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public record class Solution
{
    /// <summary>
    /// Represents the fully qualified solution file path.
    /// </summary>
    public required string FullPath { get; init; }

    /// <summary>
    /// Gets the name of the solution file.
    /// </summary>
    public string SolutionName => Path.GetFileNameWithoutExtension(FullPath);

    /// <summary>
    /// Gets or initializes the set of projects in the solution.
    /// </summary>
    public HashSet<Project> Projects { get; init; } = [];

    /// <summary>
    /// Gets the extension of the solution file.
    /// </summary>
    public string Extension => Path.GetExtension(FullPath);

    private string GetDebuggerDisplay()
    {
        var builder = new StringBuilder();

        builder.AppendFormat("Name={0}{1}", SolutionName, Extension);

        if (Projects.Count is > 0)
        {
            builder.Append(", Projects=[");
            builder.AppendJoin(", ", Projects.Select(p => p.ProjectName));
            builder.Append(']');
        }

        return builder.ToString();
    }
}
