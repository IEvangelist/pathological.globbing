// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Pathological.ProjectSystem.Tests;

public sealed class SolutionFileReaderTests
{
    [Fact]
    public async Task ReadSolutionAsyncTest()
    {
        var solutionPath = "../../../../../pathological.globbing.sln";

        var sut = DefaultSolutionReader.Factory;

        var solution = await sut.ReadSolutionAsync(solutionPath);
        Assert.NotNull(solution);
        Assert.Equal(Path.GetFullPath(solutionPath), solution.FullPath);
        Assert.NotEmpty(solution.Projects);

        foreach (var project in solution.Projects)
        {
            Assert.NotNull(project);
            Assert.Equal("net8.0", project.TargetFrameworkMonikers[0]);
            Assert.Equal("Microsoft.NET.Sdk", project.Sdk);
        }
    }
}
