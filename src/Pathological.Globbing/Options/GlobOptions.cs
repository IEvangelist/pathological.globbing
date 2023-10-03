// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Options;

public readonly record struct GlobOptions(
    string BasePath,
    bool IsCaseInsensitive,
    IEnumerable<string> Patterns,
    IEnumerable<string> IgnorePatterns);
