// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Tests;

public sealed class DiscoveryServiceTests
{
    [Fact]
    public async Task DiscoverAllAsyncReturnsExpectedResultSet()
    {
        var solutionDirectoryPath = "../../../../../";

        var sut = DefaultDiscoveryService.Factory;

        var result = await sut.DiscoverAllAsync(solutionDirectoryPath);
        Assert.NotNull(result);
        Assert.Single(result.StandaloneProjects);
        Assert.Empty(result.Dockerfiles);
        Assert.NotNull(result.Solutions);
        Assert.Single(result.Solutions);

        var solution = result.Solutions.Single();
        Assert.NotEmpty(solution.Projects);

        foreach (var project in solution.Projects)
        {
            Assert.NotNull(project);
            Assert.Equal("net8.0", project.TargetFrameworkMonikers[0]);
            Assert.Equal("Microsoft.NET.Sdk", project.Sdk);
        }
    }
}
