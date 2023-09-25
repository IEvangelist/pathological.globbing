// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing;

/// <summary>
/// Represents a thread-safe queue that allows asynchronous enqueue and dequeue operations.
/// </summary>
/// <typeparam name="T">The type of elements in the queue.</typeparam>
internal sealed class AsyncQueue<T>
{
    private readonly ConcurrentQueue<T> _queue = new();
    private readonly TaskCompletionSource<bool> _completionSource = new();

    /// <summary>
    /// Adds an item to the end of the queue.
    /// </summary>
    /// <param name="item">The item to add to the queue.</param>
    public void Enqueue(T item) => _queue.Enqueue(item);

    /// <summary>
    /// Adds an item to the queue and signals that an item has been added.
    /// </summary>
    /// <param name="item">The item to add to the queue.</param>
    public async ValueTask EnqueueAsync(T item)
    {
        _queue.Enqueue(item);
        _completionSource.TrySetResult(true);

        await Task.Yield(); // Yield control back to the caller to ensure fairness.
    }

    /// <summary>
    /// Dequeues all items in the queue asynchronously, yielding each item as it becomes available.
    /// </summary>
    /// <returns>An asynchronous enumerable of all items in the queue.</returns>
    public async IAsyncEnumerable<T> DequeueAllAsync()
    {
        while (true)
        {
            while (_queue.TryDequeue(out var item))
            {
                yield return item;
            }

            if (_completionSource.Task.IsCompleted)
                yield break;

            await Task.WhenAny(_completionSource.Task, Task.Delay(100));
        }
    }

    /// <summary>
    /// Completes the asynchronous queue by setting the completion source's result to true.
    /// </summary>
    public void Complete() => _completionSource.TrySetResult(true);
}
