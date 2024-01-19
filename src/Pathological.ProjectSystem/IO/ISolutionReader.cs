// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.IO;

public interface ISolutionReader
{
    /// <summary>
    /// Reads the <see cref="Solution"/> at the specified <paramref name="solutionPath"/>.
    /// </summary>
    /// <param name="solutionPath">The path where to read the solution from.</param>
    /// <returns>
    /// A value-task representing the asynchronous operation of reading the <see cref="Solution"/>.
    /// </returns>
    ValueTask<Solution> ReadSolutionAsync(string solutionPath);
}
