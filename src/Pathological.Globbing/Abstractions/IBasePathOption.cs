// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Abstractions;

/// <summary>
/// Represents an option that has a base path.
/// </summary>
public interface IBasePathOption
{
    /// <summary>
    /// Gets the base path for the option.
    /// </summary>
    string BasePath { get; }
}
