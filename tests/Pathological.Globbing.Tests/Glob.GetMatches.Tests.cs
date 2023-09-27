// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Tests;

public sealed partial class GlobTests
{
    [Fact]
    public void GetMatchesFromDefaultBaseReturnsFiles()
    {
        var glob = new Glob();

        var files = glob.GetMatches(
            pattern: "../../../*.cs");

        Assert.NotEmpty(files);
    }

    [Fact]
    public void GetMatchesFromExplicitBaseReturnsFiles()
    {
        var glob = new Glob("../../../");

        var files = glob.GetMatches(
            pattern: "*.cs");

        Assert.NotEmpty(files);
    }

    [Fact]
    public void GetMatchesHonorsCaseSensitivity()
    {
        var glob = new Glob("../../../", isCaseInsensitive: false);

        var files = glob.GetMatches(
            pattern: "*.CS");

        Assert.Empty(files);
    }

    [Fact]
    public void GetMatchesHonorsPatternsAndIgnorePatterns()
    {
        var glob = new Glob("../../../");

        var files = glob.GetMatches(
            pattern: "*.*", ignorePattern: "*.csproj");

        Assert.NotEmpty(files);
        Assert.DoesNotContain(
            files, file => file.EndsWith(".csproj"));
    }
}