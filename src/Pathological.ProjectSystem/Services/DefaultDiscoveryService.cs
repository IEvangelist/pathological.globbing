// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Services;

internal sealed class DefaultDiscoveryService(
    IDockerfileReader dockerfileReader,
    IProjectReader projectReader,
    ISolutionReader solutionReader) : IDiscoveryService
{
    // For testing purposes only.
    internal static IDiscoveryService Factory { get; } = new DefaultDiscoveryService(
        DefaultDockerfileReader.Factory,
        DefaultProjectReader.Factory,
        DefaultSolutionReader.Factory);

    public async ValueTask<DiscoveryResultSet> DiscoverAllAsync(string rootPath)
    {
        ConcurrentBag<Solution> solutions = [];
        ConcurrentBag<Project> projects = [];
        ConcurrentBag<Dockerfile> dockerfiles = [];

        await Task.WhenAll(
            AccumulateResourcesAsync(GetSolutionsAsync(rootPath), solutions),
            AccumulateResourcesAsync(GetProjectsAsync(rootPath), projects),
            AccumulateResourcesAsync(GetDockerfilesAsync(rootPath), dockerfiles));

        var standaloneProjects = projects.Except(
            solutions.SelectMany(static sln => sln.Projects));

        return new()
        {
            Solutions = solutions.ToFrozenSet(),
            StandaloneProjects = standaloneProjects.ToFrozenSet(),
            Dockerfiles = dockerfiles.ToFrozenSet()
        };

        static async Task AccumulateResourcesAsync<TResource>(
            IAsyncEnumerable<TResource> accumulateFunc, ConcurrentBag<TResource> bag)
        {
            await foreach (var resource in accumulateFunc)
            {
                bag.Add(resource);
            }
        }
    }

    private async IAsyncEnumerable<Solution> GetSolutionsAsync(string rootPath)
    {
        var glob = new Glob(basePath: rootPath);

        string[] solutionPatterns = ["**/*.sln", "**/*.slnx"];

        await foreach (var solutionPath in glob.GetMatchesAsync(solutionPatterns))
        {
            yield return await solutionReader.ReadSolutionAsync(solutionPath);
        }
    }

    private async IAsyncEnumerable<Project> GetProjectsAsync(string rootPath)
    {
        var glob = new Glob(basePath: rootPath);

        string[] projectPatterns = ["**/*.csproj", "**/*.fsproj", "**/*.vbproj"];

        await foreach (var projectPath in glob.GetMatchesAsync(projectPatterns))
        {
            yield return await projectReader.ReadProjectAsync(projectPath);
        }
    }

    private async IAsyncEnumerable<Dockerfile> GetDockerfilesAsync(string rootPath)
    {
        var glob = new Glob(basePath: rootPath);

        await foreach (var dockerfilePath in glob.GetMatchesAsync("**/Dockerfile"))
        {
            var dockerfile = await dockerfileReader.ReadDockerfileAsync(dockerfilePath);

            if (dockerfile.IsNonDotNetBasedImage)
            {
                continue;
            }

            yield return dockerfile;
        }
    }
}
