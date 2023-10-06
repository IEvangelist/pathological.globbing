// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using CacheKey = Pathological.Globbing.Options.GlobOptions;

namespace Pathological.Globbing.Options;

/// <summary>
/// A cache for <see cref="Matcher"/> instances generated from <see cref="CacheKey"/>.
/// </summary>
internal sealed class GlobOptionsCache(byte maxCapacity = 20)
{
    private readonly ConcurrentDictionary<CacheKey, Matcher> _cache = new();
    private readonly ConcurrentQueue<CacheKey> _evictionQueue = new();

    /// <summary>
    /// Gets the matcher for the specified <see cref="CacheKey"/> or adds it to the cache if it doesn't exist.
    /// </summary>
    /// <param name="options">The <see cref="CacheKey"/> to get the matcher for.</param>
    /// <returns>The <see cref="Matcher"/> for the specified <see cref="CacheKey"/>.</returns>
    internal Matcher GetOrAdd(CacheKey options)
    {
        var matcher = _cache.GetOrAdd(
            key: options,
            valueFactory: static options => options.ToMatcher());

        EnforceMaxCapacity();

        return matcher;
    }

    /// <summary>
    /// Enforces the maximum capacity of the cache by removing the least recently used items.
    /// </summary>
    private void EnforceMaxCapacity()
    {
        while (_cache.Count > maxCapacity)
        {
            if (_evictionQueue.TryDequeue(out var keyToRemove))
            {
                _cache.TryRemove(keyToRemove, out _);
            }
        }
    }
}