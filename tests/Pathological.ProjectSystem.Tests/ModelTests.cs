// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Tests;

public class ModelTests
{
    [Fact]
    public async Task TwoProjectsWithTheSameValueAreEqualTest()
    {
        var reader = DefaultProjectReader.Factory;

        var path = "../../../../../src/Pathological.Globbing/Pathological.Globbing.csproj";

        var projOne = await reader.ReadProjectAsync(path);
        var projTwo = await reader.ReadProjectAsync(path);

        Assert.Equal(projOne, projTwo);
        Assert.Empty(new[] { projOne }.Except([projTwo]));
    }
}
