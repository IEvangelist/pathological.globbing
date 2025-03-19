// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Models;

/// <summary>
/// Represents a <i>Dockerfile</i> and its associated image details. This type is only
/// helpful for .NET-based images, otherwise; only the <see cref="FullPath"/> is set.
/// </summary>
/// <remarks>
/// The <see cref="Dockerfile.ImageDetails"/> is <see langword="null" /> if
/// the <i>Dockerfile</i> is based on a non-.NET image.
/// </remarks>
public sealed record class Dockerfile
{
    /// <summary>
     /// The fully qualified path of the <i>Dockerfile</i>.
     /// </summary>
    public required string FullPath { get; init; }

    /// <summary>
    /// The image details for each <c>FROM</c> (or <c>COPY</c>) instruction in the <i>Dockerfile</i>.
    /// </summary>
    public ISet<ImageDetails>? ImageDetails { get; init; }

    /// <summary>
    /// A value indicating whether the <i>Dockerfile</i> is based on a non-.NET image.
    /// </summary>
    public bool IsNonDotNetBasedImage => ImageDetails is null;
}
