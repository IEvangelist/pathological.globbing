// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Options;

/// <summary>
/// Represents a builder for creating instances of <see cref="GlobOptions"/>.
/// </summary>
/// <remarks>
/// This builder allows you to configure the base path, case sensitivity, patterns to match, and patterns to ignore
/// when creating a new instance of <see cref="GlobMatching"/>.
/// </remarks>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public readonly record struct GlobOptionsBuilder(
    in string BasePath = GlobDefaults.BasePath,
    in bool IsCaseInsensitive = GlobDefaults.IsCaseInsensitive) : IBasePathOption
{
    /// <summary>
    /// Gets or sets the enumerable of glob patterns to match against.
    /// </summary>
    public IEnumerable<string> Patterns { get; init; } = [];

    /// <summary>
    /// Gets or sets the list of patterns to ignore when matching against file paths.
    /// </summary>
    public IEnumerable<string> IgnorePatterns { get; init; } = [];

    /// <summary>
    /// Concatenates two sequences and returns the concatenated sequence. If either of the input sequences is null, an empty sequence is used instead.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
    /// <param name="source">The first sequence to concatenate.</param>
    /// <param name="other">The second sequence to concatenate.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains the concatenated elements of the two input sequences.</returns>
    private static IEnumerable<T> CoalesceConcat<T>(
        IEnumerable<T>? source = null,
        IEnumerable<T>? other = null) =>
        [
            ..(source ?? Enumerable.Empty<T>()),
            ..(other ?? Enumerable.Empty<T>())
        ];

    /// <summary>
    /// Sets the base path for the glob matching builder.
    /// </summary>
    /// <param name="basePath">The base path to set.</param>
    /// <returns>A new instance of the <see cref="GlobOptionsBuilder"/> class with the specified base path.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="basePath"/> is null.</exception>
    public GlobOptionsBuilder WithBasePath(string basePath) =>
        basePath is null
            ? throw new ArgumentNullException(nameof(basePath))
            : this with { BasePath = basePath };

    /// <summary>
    /// Returns a new instance of <see cref="GlobOptionsBuilder"/> with the specified case-insensitivity setting.
    /// </summary>
    /// <param name="isCaseInsensitive">A boolean value indicating whether the matching should be case-insensitive.</param>
    /// <returns>A new instance of <see cref="GlobOptionsBuilder"/> with the specified case-insensitivity setting.</returns>
    public GlobOptionsBuilder WithCaseInsensitive(bool isCaseInsensitive) =>
        this with { IsCaseInsensitive = isCaseInsensitive };

    /// <summary>
    /// Adds a pattern to the list of patterns to match against.
    /// </summary>
    /// <param name="pattern">The pattern to add.</param>
    /// <returns>A new <see cref="GlobOptionsBuilder"/> instance with the added pattern.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="pattern"/> is null.</exception>
    public GlobOptionsBuilder WithPattern(string pattern) =>
        pattern is null
            ? throw new ArgumentNullException(nameof(pattern))
            : this with { Patterns = CoalesceConcat(Patterns, [pattern]) };

    /// <summary>
    /// Adds the specified patterns to the list of patterns to match against.
    /// </summary>
    /// <param name="patterns">The patterns to add.</param>
    /// <returns>A new <see cref="GlobOptionsBuilder"/> instance with the added patterns.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="patterns"/> is null.</exception>
    public GlobOptionsBuilder WithPatterns(string[] patterns) =>
        patterns is null
            ? throw new ArgumentNullException(nameof(patterns))
            : this with { Patterns = CoalesceConcat(Patterns, patterns) };

    /// <summary>
    /// Adds an ignore pattern to the current <see cref="GlobOptionsBuilder"/> instance.
    /// </summary>
    /// <param name="ignorePattern">The ignore pattern to add.</param>
    /// <returns>A new <see cref="GlobOptionsBuilder"/> instance with the ignore pattern added.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ignorePattern"/> is null.</exception>
    public GlobOptionsBuilder WithIgnorePattern(string ignorePattern) =>
        ignorePattern is null
            ? throw new ArgumentNullException(nameof(ignorePattern))
            : this with { IgnorePatterns = CoalesceConcat(IgnorePatterns, [ignorePattern]) };

    /// <summary>
    /// Adds the specified ignore patterns to the current <see cref="GlobOptionsBuilder"/> instance.
    /// </summary>
    /// <param name="ignorePatterns">An array of ignore patterns to add.</param>
    /// <returns>A new <see cref="GlobOptionsBuilder"/> instance with the specified ignore patterns added.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ignorePatterns"/> is null.</exception>
    public GlobOptionsBuilder WithIgnorePatterns(string[] ignorePatterns) =>
        ignorePatterns is null
            ? throw new ArgumentNullException(nameof(ignorePatterns))
            : this with { IgnorePatterns = CoalesceConcat(IgnorePatterns, ignorePatterns) };

    /// <summary>
    /// Builds a new <see cref="GlobOptions"/> instance using the current configuration.
    /// As part of the build, the options are validated and this method has the
    /// potential to <see langword="throw"/>.
    /// </summary>
    /// <returns>A new <see cref="GlobOptions"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either <paramref name="Patterns"/> or <paramref name="IgnorePatterns"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when both <paramref name="Patterns"/> and <paramref name="IgnorePatterns"/> are empty.</exception>
    /// <exception cref="ArgumentException">Thrown when any of the patterns in <paramref name="Patterns"/> or <paramref name="IgnorePatterns"/> 
    /// containers an empty, null or whitespace pattern.</exception>
    public GlobOptions Build()
    {
        ValidateArguments(Patterns, IgnorePatterns);

        return new GlobOptions(
            BasePath,
            IsCaseInsensitive,
            Inclusions: Patterns,
            Exclusions: IgnorePatterns);

        static void ValidateArguments(
            in IEnumerable<string> patterns,
            in IEnumerable<string> ignorePatterns)
        {
            ArgumentNullException.ThrowIfNull(patterns);

            foreach (var pattern in patterns)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(pattern);
            }

            ArgumentNullException.ThrowIfNull(ignorePatterns);

            foreach (var ignorePattern in ignorePatterns)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(ignorePattern);
            }

            if (IsEmpty(patterns) && IsEmpty(ignorePatterns))
            {
                throw new ArgumentException(
                    "At least one pattern or ignore pattern must be specified.");
            }

            static bool IsEmpty(IEnumerable<string> enumerable)
            {
                return enumerable switch
                {
                    IList<string> list => list.Count is 0,
                    ICollection<string> c => c.Count is 0,
                    Array array => array.Length is 0,

                    _ => enumerable.Any() is false
                };
            }
        }
    }

    private string GetDebuggerDisplay() => ToString();
}
