﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Results;

/// <summary>
/// Represents a match between a file path and a glob pattern.
/// </summary>
/// <param name="Path">
/// The path that matched the glob pattern.
/// If the <see cref="Glob"/> searched for <c>"src/Project/**/*.cs"</c> and the pattern matcher found <c>"src/Project/Interfaces/IFile.cs"</c>,
/// then <see cref="Stem" /> = <c>"Interfaces/IFile.cs"</c> and <see cref="Path" /> = <c>"src/Project/Interfaces/IFile.cs"</c>.
/// </param>
/// <param name="Stem">
/// The stem of the path that matched the glob pattern.
/// If the <see cref="Glob"/> searched for <c>"src/Project/**/*.cs"</c> and the pattern matcher found <c>"src/Project/Interfaces/IFile.cs"</c>,
/// then <see cref="Stem" /> = <c>"Interfaces/IFile.cs"</c> and <see cref="Path" /> = <c>"src/Project/Interfaces/IFile.cs"</c>.
/// </param>
public readonly record struct GlobMatch(
    string Path,
    string? Stem = default)
{
    /// <summary>
    /// Converts the current <see cref="GlobMatch"/> instance to a <see cref="FileInfo"/> instance using the specified <see cref="IBasePathOption"/>.
    /// </summary>
    /// <param name="option">The <see cref="IBasePathOption"/> used to resolve the path.</param>
    /// <returns>A new <see cref="FileInfo"/> instance representing the current <see cref="GlobMatch"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FileInfo ToFileInfo(IBasePathOption option) =>
        new(fileName: option.ResolvePath(Path));

    /// <summary>
    /// Defines an explicit conversion of a <see cref="FilePatternMatch"/> object to a <see cref="GlobMatch"/> object.
    /// </summary>
    /// <param name="filePatternMatch">The <see cref="FilePatternMatch"/> object to convert.</param>
    /// <returns>A new instance of the <see cref="GlobMatch"/> class with the specified path and stem.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator GlobMatch(FilePatternMatch filePatternMatch) =>
        new(filePatternMatch.Path, filePatternMatch.Stem);
}
