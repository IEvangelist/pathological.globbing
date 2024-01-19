// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Models;

public record class Dockerfile
{
    /// <summary>
     /// The fully qualified path of the <i>Dockerfile</i>.
     /// </summary>
    public required string FullPath { get; init; }

    /// <summary>
    /// The image details for each <c>FROM</c> (or <c>COPY</c>) instruction in the <i>Dockerfile</i>.
    /// </summary>
    public ISet<ImageDetails>? ImageDetails { get; init; }
}
