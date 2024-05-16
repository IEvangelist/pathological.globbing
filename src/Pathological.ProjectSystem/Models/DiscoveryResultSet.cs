// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Models;

/// <summary>
/// Represents the result of a discovery operation.
/// </summary>
public record class DiscoveryResultSet
{
    /// <summary>
    /// A collection of <see cref="Project"/>s that <i>aren't</i> part of a solution.
    /// </summary>
    public required FrozenSet<Project> StandaloneProjects { get; init; }

    /// <summary>
    /// A collection of <see cref="Solution"/>s that contain one or more <see cref="Project"/>s.
    /// </summary>
    public required FrozenSet<Solution> Solutions { get; init; }

    /// <summary>
    /// A collection of <see cref="Dockerfile"/>s that contain .NET image references.
    /// </summary>
    public required FrozenSet<Dockerfile> Dockerfiles { get; init; }
}
