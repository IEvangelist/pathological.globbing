// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Tests;

public class GlobEvaluatorTests
{
    [Fact]
    public void EvaluateReturnsExpectedResult()
    {
        // Arrange
        var builder = new GlobOptionsBuilder()
            .WithPattern("../../../**/*.cs");

        // Act
        var result = builder.ExecuteEvaluation();

        // Assert
        Assert.NotEqual(default, result);
        Assert.NotEmpty(result.Matches);

        var file = GlobMatch.ToFileInfo(builder, result.Matches.ElementAt(0));
        Assert.True(file.Exists);
    }
}
