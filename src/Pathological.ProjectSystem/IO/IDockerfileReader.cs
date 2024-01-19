// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.IO;

public interface IDockerfileReader
{
    /// <summary>
    /// Reads the <see cref="Dockerfile"/> at the specified <paramref name="dockerfilePath"/>.
    /// </summary>
    /// <remarks>
    /// This is only useful for .NET images. Additionally, only <c>FROM</c> and
    /// <c>COPY</c> instructions are parsed.
    /// </remarks>
    /// <param name="dockerfilePath">The path of the <i>Dockerfile</i>.</param>
    /// <returns>
    /// A value-task representing the asynchronous operation of reading the <see cref="Dockerfile"/>.
    /// </returns>
    ValueTask<Dockerfile> ReadDockerfileAsync(string dockerfilePath);
}
