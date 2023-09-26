// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Tests;

public partial class GlobTests
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
}