// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Services;

public interface IDiscoveryService
{
    /// <summary>
    /// Discovers all <see cref="Solution"/>s, <see cref="Project"/>s, and <see cref="Dockerfile"/>s.
    /// </summary>
    /// <param name="rootPath">The root path to discover all solution, projects, and dockerfiles.</param>
    /// <returns>
    /// A value-task representing the asynchronous operation of discovering the results.
    /// </returns>
    ValueTask<DiscoveryResultSet> DiscoverAllAsync(string rootPath);
}
