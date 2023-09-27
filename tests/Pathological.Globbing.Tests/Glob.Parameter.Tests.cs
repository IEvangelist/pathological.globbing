// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Tests;

public sealed partial class GlobTests
{
    [Fact]
    public void GlobBasePathDefaultsToDot()
    {
        var glob = new Glob();

        Assert.Equal(GlobDefaults.BasePath, glob.BasePath);
    }

    [Fact]
    public void GlobCtorThrowsOnNullBasePath()
    {
        Assert.Throws<ArgumentNullException>(
            () => new Glob(basePath: null!));
    }

    [Fact]
    public void GlobThrowsOnNullPattern()
    {
        var glob = new Glob();

        Assert.Throws<ArgumentNullException>(
            () => glob.GetMatches(pattern: null!));
    }

    [Fact]
    public void GlobThrowsOnNullIgnorePattern()
    {
        var glob = new Glob();

        Assert.Throws<ArgumentNullException>(
            () => glob.GetMatches(pattern: "*.*", ignorePattern: null!));
    }

    [Fact]
    public void GlobThrowsOnBothEmptyPatterns()
    {
        var glob = new Glob();

        Assert.Throws<ArgumentException>(
            () => glob.GetMatches(pattern: string.Empty, ignorePattern: string.Empty));
    }
}
