// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Tests;

public class ProjectSystemExtensionsTests
{
    [Fact]
    public void AddDotNetProjectSystemAddsExpectedServices()
    {
        var services = new ServiceCollection();

        services.AddDotNetProjectSystem();

        Assert.Contains(services, svc => svc.ServiceType == typeof(IProjectReader));
        Assert.Contains(services, svc => svc.ServiceType == typeof(ISolutionReader));
        Assert.Contains(services, svc => svc.ServiceType == typeof(IDockerfileReader));
        Assert.Contains(services, svc => svc.ServiceType == typeof(IDiscoveryService));
    }
}
