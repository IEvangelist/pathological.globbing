// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Tests;

public class GlobOptionsCacheTests
{
    [Fact]
    public void GetOrAddReturnsMatcherWhenCalledWithValidOptions()
    {
        // Arrange
        var options = new GlobOptions();
        var cache = new GlobOptionsCache();

        // Act
        var matcher = cache[options];

        // Assert
        Assert.NotNull(matcher);
        Assert.IsType<Matcher>(matcher);
    }

    [Fact]
    public void GetOrAddReturnsSameMatcherWhenCalledWithSameOptions()
    {
        // Arrange
        var options = new GlobOptions();
        var cache = new GlobOptionsCache();

        // Act
        var matcher1 = cache[options];
        var matcher2 = cache[options];

        // Assert
        Assert.Same(matcher1, matcher2);
    }

    [Fact]
    public void EnforceMaxCapacityRemovesLeastRecentlyUsedItemsWhenCacheExceedsMaxCapacity()
    {
        // Arrange
        var cache = new GlobOptionsCache(maxCapacity: 2);

        var options1 = new GlobOptions { IgnoreCase = true };
        var options2 = new GlobOptions { IgnoreCase = false };
        var options3 = new GlobOptions { IgnoreCase = true };

        // Act
        var matcher1 = cache[options1];
        var matcher2 = cache[options2];
        var matcher3 = cache[options3];

        // Assert
        Assert.NotNull(matcher1);
        Assert.NotNull(matcher2);
        Assert.NotNull(matcher3);

        Assert.Same(matcher1, matcher3);
        Assert.NotSame(matcher2, matcher3);
    }
}
