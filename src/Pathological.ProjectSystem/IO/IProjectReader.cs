// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.IO;

public interface IProjectReader
{
    /// <summary>
    /// Reads the <see cref="Project"/> at the specified <paramref name="projectPath"/>.
    /// </summary>
    /// <param name="projectPath">The path where to read the project from.</param>
    /// <returns>
    /// A value-task representing the asynchronous operation of reading the <see cref="Project"/>.
    /// </returns>
    ValueTask<Project> ReadProjectAsync(string projectPath);
}
