// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Models;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public record class Project
{
    /// <summary>
    /// The SDK value that the project is specifying.
    /// </summary>
    public string? Sdk { get; init; }

    /// <summary>
    /// Gets a value indicating whether the project is the new SDK-style format.
    /// </summary>
    public bool IsSdkStyle => Sdk is { Length: > 0 };

    /// <summary>
    /// The fully qualified path of the project.
    /// </summary>
    public required string FullPath { get; init; }

    /// <summary>
    /// The name of the project, for example; "Pathological.ProjectSystem".
    /// </summary>
    public string ProjectName => Path.GetFileNameWithoutExtension(FullPath);

    /// <summary>
    /// The file extension of the project, for example; ".csproj".
    /// </summary>
    public string Extension => Path.GetExtension(FullPath);

    /// <summary>
    /// The line number in the project file where the TargetFramework(s) element exists.
    /// </summary>
    public int TargetFrameworkMonikerLineNumber { get; init; } = -1;

    /// <summary>
    /// The raw string representation of the TargetFramework(s) element in the project file.
    /// </summary>
    internal string RawTargetFrameworkMonikers { get; init; } = null!;

    /// <summary>
    /// The target framework monikers, as parsed from the
    /// <c>TargetFrameworks</c> element of the project file.
    /// </summary>
    public string[] TargetFrameworkMonikers =>
        RawTargetFrameworkMonikers?.Split(';', StringSplitOptions.RemoveEmptyEntries)
        ?? [];

    private string GetDebuggerDisplay()
    {
        var builder = new StringBuilder();

        builder.AppendFormat("Name={0}{1}", ProjectName, Extension);

        if (IsSdkStyle)
        {
            builder.AppendFormat(", Sdk={0}", Sdk);
        }

        if (string.IsNullOrWhiteSpace(RawTargetFrameworkMonikers) is false)
        {
            builder.AppendFormat(", TargetFramework(s)={0}", RawTargetFrameworkMonikers);
        }

        return builder.ToString();
    }
}
