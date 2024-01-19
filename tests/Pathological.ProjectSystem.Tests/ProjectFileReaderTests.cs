// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Pathological.ProjectSystem.Tests;

public sealed class ProjectFileReaderTests
{
    [Fact]
    public async Task ReadProjectAndParseXmlCorrectly()
    {
        var projectPath = "test.csproj";

        try
        {
            await File.WriteAllTextAsync(projectPath, TestInput.TestProjectXml);

            var sut = DefaultProjectReader.Factory;

            var project = await sut.ReadProjectAsync(projectPath);
            Assert.Equal(4, project.TargetFrameworkMonikerLineNumber);
            Assert.Single(project.TargetFrameworkMonikers);
            Assert.Equal("net8.0", project.TargetFrameworkMonikers[0]);
            Assert.Equal("Microsoft.NET.Sdk", project.Sdk);
        }
        finally
        {
            File.Delete(projectPath);
        }
    }
}
