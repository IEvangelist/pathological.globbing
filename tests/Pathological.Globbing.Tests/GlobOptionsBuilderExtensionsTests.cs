// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using Pathological.Globbing.Results;

namespace Pathological.Globbing.Tests;

public class GlobOptionsBuilderExtensionsTests
{
    [Fact]
    public void EvaluateReturnsExpectedResult()
    {
        // Arrange
        var builder = new GlobOptionsBuilder()
            .WithBasePath("../../../")
            .WithPattern("**/*.cs")
            .WithIgnorePattern("**/bin/**")
            .WithIgnorePattern("**/obj/**");

        // Act
        var result = builder.Evaluate();

        // Assert
        Assert.NotEqual(default, result);
        Assert.IsType<GlobEvaluationResult>(result);
        Assert.True(result.HasMatches);

        foreach (var match in result.Matches)
        {
            Assert.NotEqual(default, match);

            FileInfo file = match;
            Assert.IsType<FileInfo>(file);
            Assert.True(file.Exists);
            Assert.EndsWith(".cs", file.Extension);
        }
    }
}
