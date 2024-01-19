// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Extensions.DependencyInjection;

public static class ProjectSystemExtensions
{
    /// <summary>
    /// Registers the default implementations of following services:
    /// <list type="bullet">
    /// <item>
    /// <see cref="IDockerfileReader"/>: A Dockerfile reader used for reading a Dockerfile.
    /// </item>
    /// <item>
    /// <see cref="IProjectReader"/>: A project (for example, *.csproj) reader used for reading project files.
    /// </item>
    /// <item>
    /// <see cref="ISolutionReader"/>: A solution (*.sln) reader used for reading solution files.
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance with the added services.</returns>
    public static IServiceCollection AddDotNetProjectSystem(
        this IServiceCollection services)
    {
        services.AddSingleton<IProjectReader, DefaultProjectReader>();
        services.AddSingleton<ISolutionReader, DefaultSolutionReader>();
        services.AddSingleton<IDockerfileReader, DefaultDockerfileReader>();
        services.AddSingleton<IDiscoveryService, DefaultDiscoveryService>();

        return services;
    }
}
