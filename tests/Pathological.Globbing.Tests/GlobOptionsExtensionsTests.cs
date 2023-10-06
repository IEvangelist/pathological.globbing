// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Tests;

public class GlobOptionsExtensionsTests
{
    [Fact]
    public void IsMatchReturnsTrueWhenFileMatchesPattern()
    {
        // Arrange
        var options = new GlobOptions(
            "../../../", IgnoreCase: false, ["*.cs"], []);

        var file = "fake-file.cs";

        // Act
        var result = options.IsMatch(file);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsMatchReturnsFalseWhenFileDoesNotMatchPattern()
    {
        // Arrange
        var options = new GlobOptions(
            "../../../", IgnoreCase: false, ["*.cs"], []);

        var file = "fake-file.png";

        // Act
        var result = options.IsMatch(file);

        // Assert
        Assert.False(result);
    }
}
