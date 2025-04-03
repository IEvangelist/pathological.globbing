// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Tests;

public sealed class DiscoveryServiceTests
{
    [Fact(
        Skip = "Fails in CI/CD for now."
    )]
    public async Task DiscoverAllAsyncReturnsExpectedResultSet()
    {
        var solutionDirectoryPath = "../../../../../";

        var sut = DefaultDiscoveryService.Factory;

        var result = await sut.DiscoverAllAsync(solutionDirectoryPath);
        Assert.NotNull(result);
        Assert.Empty(result.StandaloneProjects);
        Assert.Empty(result.Dockerfiles);
        Assert.NotNull(result.Solutions);
        Assert.Single(result.Solutions);

        var solution = result.Solutions.Single();
        Assert.NotEmpty(solution.Projects);

        foreach (var project in solution.Projects)
        {
            Assert.NotNull(project);
            Assert.Equal("net9.0", project.TargetFrameworkMonikers[0]);
            Assert.Equal("Microsoft.NET.Sdk", project.Sdk);
        }
    }

    [Fact(
        Skip = "Fails in CI/CD for now."
    )]
    public async Task DiscoverWithOptionsAsyncReturnsExpectedResultSet()
    {
        var solutionDirectoryPath = "../../../../../";

        var sut = DefaultDiscoveryService.Factory;

        var result = await sut.DiscoverAllAsync(
            solutionDirectoryPath,
            new(SolutionIgnorePatterns: ["**/*.slnx"])
            {
                SolutionPatterns = ["**/*.sln"]
            });

        Assert.NotNull(result);
        Assert.NotEmpty(result.StandaloneProjects);
        Assert.Empty(result.Dockerfiles);
        Assert.Empty(result.Solutions);
    }
}
