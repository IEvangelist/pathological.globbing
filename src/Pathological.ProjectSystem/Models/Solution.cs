// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Models;

public record class Solution
{
    public required string FullPath { get; init; }

    public HashSet<Project> Projects { get; init; } = [];
}
