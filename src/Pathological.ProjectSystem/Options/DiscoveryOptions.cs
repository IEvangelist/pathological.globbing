// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Options;

/// <summary>
/// An object used to refine the discovery of <see cref="Solution"/>, <see cref="Project"/>, and <see cref="Dockerfile"/>.
/// </summary>
/// <param name="SolutionIgnorePatterns">A set of patterns to ignore when globbing for solutions.</param>
/// <param name="ProjectIgnorePatterns">A set of patterns to ignore when globbing for projects.</param>
/// <param name="DockerfileIgnorePatterns">A set of patterns to ignore when globbing for dockerfiles.</param>
public sealed record class DiscoveryOptions(
    IEnumerable<string>? SolutionIgnorePatterns = null,
    IEnumerable<string>? ProjectIgnorePatterns = null,
    IEnumerable<string>? DockerfileIgnorePatterns = null)
{
    /// <summary>
    /// A set of patterns to use when globbing for solutions.
    /// Defaults to <c>["**/*.sln", "**/*.slnx"]</c>.
    /// </summary>
    public IEnumerable<string> SolutionPatterns { get; init; } = DefaultPatterns.SolutionPatterns;

    /// <summary>
    /// A set of patterns to use when globbing for projects.
    /// Defaults to <c>["**/*.csproj", "**/*.fsproj", "**/*.vbproj"]</c>.
    /// </summary>
    public IEnumerable<string> ProjectPatterns { get; init; } = DefaultPatterns.ProjectPatterns;

    /// <summary>
    /// A set of patterns to use when globbing for dockerfiles.
    /// Defaults to ["**/Dockerfile"].
    /// </summary>
    public IEnumerable<string> DockerfilePatterns { get; init; } = DefaultPatterns.DockerfilePatterns;
}

internal static class DefaultPatterns
{
    public static readonly string[] SolutionPatterns = ["**/*.sln", "**/*.slnx"];
    public static readonly string[] ProjectPatterns = ["**/*.csproj", "**/*.fsproj", "**/*.vbproj"];
    public static readonly string[] DockerfilePatterns = ["**/Dockerfile"];
}