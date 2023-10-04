// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Tests;

public class GlobOptionsBuilderTests
{
    [Fact]
    public void InvalidArgumentsThrowTest()
    {
        // Arrange
        var builder = new GlobOptionsBuilder(null!);

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => builder.Build());
    }

    [Fact]
    public void ParameterlessCtorBuildThrowsTest()
    {
        // Arrange
        var builder = new GlobOptionsBuilder();

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => builder.Build());
    }

    [Fact]
    public void WithIgnorePatternBuildsValidDefaultsTest()
    {
        var builder = new GlobOptionsBuilder()
            .WithIgnorePattern("**/*.*");

        var options = builder.Build();

        Assert.Equal(GlobDefaults.BasePath, options.BasePath);
        Assert.Equal(GlobDefaults.IgnoreCase, options.IgnoreCase);
        Assert.Empty(options.Inclusions);
        Assert.Single(options.Exclusions);
    }

    [Fact]
    public void WithBasePathNullThrowsArgumentNullException()
    {
        // Arrange
        var builder = new GlobOptionsBuilder();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => builder.WithBasePath(null!));
    }

    [Fact]
    public void WithCaseInsensitiveCorrectlyFlowsValue()
    {
        // Arrange
        var builder = new GlobOptionsBuilder();

        // Act & Assert
        var result = builder.WithCaseInsensitive(true);
        Assert.True(result.IgnoreCase);

        result = builder.WithCaseInsensitive(false);
        Assert.False(result.IgnoreCase);
    }

    [Fact]
    public void WithPatternNullThrowsArgumentNullException()
    {
        // Arrange
        var builder = new GlobOptionsBuilder();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => builder.WithPattern(null!));
    }

    [Fact]
    public void WithPatternsNullThrowsArgumentNullException()
    {
        // Arrange
        var builder = new GlobOptionsBuilder();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => builder.WithPatterns(null!));
    }

    [Fact]
    public void WithIgnorePatternNullThrowsArgumentNullException()
    {
        // Arrange
        var builder = new GlobOptionsBuilder();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => builder.WithIgnorePattern(null!));
    }

    [Fact]
    public void WithIgnorePatternsNullThrowsArgumentNullException()
    {
        // Arrange
        var builder = new GlobOptionsBuilder();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => builder.WithIgnorePatterns(null!));
    }

    [Fact]
    public void BuildPatternsAndIgnorePatternsAreNullThrowsArgumentNullException()
    {
        // Arrange
        var builder = new GlobOptionsBuilder();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.Build());
    }

    [Fact]
    public void BuildPatternsAndIgnorePatternsAreEmptyThrowsArgumentException()
    {
        // Arrange
        var builder = new GlobOptionsBuilder()
            .WithPatterns([])
            .WithIgnorePatterns([]);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.Build());
    }

    [Fact]
    public void BuildValidOptionsReturnsGlobOptions()
    {
        // Arrange
        var builder = new GlobOptionsBuilder()
            .WithBasePath("c:\\temp")
            .WithCaseInsensitive(true)
            .WithPattern("*.txt")
            .WithIgnorePattern("bin");

        // Act
        var result = builder.Build();

        // Assert
        Assert.Equal("c:\\temp", result.BasePath);
        Assert.True(result.IgnoreCase);
        Assert.Equal(["*.txt"], result.Inclusions);
        Assert.Equal(["bin"], result.Exclusions);
    }
}
