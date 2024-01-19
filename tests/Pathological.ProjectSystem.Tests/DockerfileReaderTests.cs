// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Pathological.ProjectSystem.Tests;

public sealed class DockerfileReaderTests
{
    [Fact]
    public async Task ReadDockerfileAndParsesCorrectly()
    {
        var dockerfilePath = "Dockerfile";

        try
        {
            await File.WriteAllTextAsync(dockerfilePath, TestInput.DockerfileWithMultipleTfms);

            var sut = DefaultDockerfileReader.Factory;

            var dockerfile = await sut.ReadDockerfileAsync(dockerfilePath);
            Assert.NotNull(dockerfile);
            Assert.NotNull(dockerfile.ImageDetails);
            Assert.Equal(6, dockerfile.ImageDetails.Count);

            Assert.Contains(dockerfile.ImageDetails, i => i.TargetFrameworkMoniker == "net35");
            Assert.Contains(dockerfile.ImageDetails, i => i.TargetFrameworkMoniker == "net471");
            Assert.Contains(dockerfile.ImageDetails, i => i.TargetFrameworkMoniker == "net48");
            Assert.Contains(dockerfile.ImageDetails, i => i.TargetFrameworkMoniker == "netcoreapp3.1.30");
            Assert.Contains(dockerfile.ImageDetails, i => i.TargetFrameworkMoniker == "net6.0");
            Assert.Contains(dockerfile.ImageDetails, i => i.TargetFrameworkMoniker == "net7.0");
        }
        finally
        {
            File.Delete(dockerfilePath);
        }
    }
}
