// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Models;

/// <summary>
/// A record representing the image details found within a single <i>Dockerfile</i>.
/// </summary>
/// <param name="Image">The image as defined in the <i>Dockerfile</i>.</param>
/// <param name="Tag">The image tag.</param>
/// <param name="TargetFrameworkMoniker">The target framework moniker.</param>
/// <param name="LineNumber">The line number where the image details was parsed on.</param>
public readonly record struct ImageDetails(
    string Image,
    string Tag,
    string TargetFrameworkMoniker,
    int LineNumber);
