// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Tests;

public sealed partial class GlobTests(TempFolderTestFixture fixture)
    : IClassFixture<TempFolderTestFixture>
{
    [Fact]
    public async Task GetMatchesAsyncRespectsCancellationToken()
    {
        string[] files =
        [
            "text-file.txt", "markdown.md", "image.png",
        ];

        string[] folders =
        [
            "folder1", "folder2", "folder3",
        ];

        foreach (var folder in folders)
        {
            var folderPath = Path.Combine(fixture.TempFolder, folder);
            Directory.CreateDirectory(folderPath);

            foreach (var file in files)
            {
                var filePath = Path.Combine(folderPath, file);
                await File.WriteAllTextAsync(filePath, null);
            }
        }

        var glob = new Glob(fixture.TempFolder);

        var tokenSource = new CancellationTokenSource();

        var actualCount = 0;

        try
        {
            await foreach (var file in glob.GetMatchesAsync(
                patterns: ["**/*.md", "**/*.txt"],
                ignorePatterns: ["folder3"],
                cancellationToken: tokenSource.Token))
            {
                ++actualCount;
                Assert.Contains(Path.GetFileName(file), files);
                if (actualCount is 1)
                {
                    await tokenSource.CancelAsync();
                }
            }
        }
        catch (TaskCanceledException)
        {
            Assert.True(actualCount <= 4);
        }
    }

    [Fact]
    public async Task GetMatchesAsyncReturnsAllFiles()
    {
        string[] files =
        [
            "text-file.txt", "markdown.md", "image.png",
        ];

        string[] folders = 
        [
            "folder1", "folder2", "folder3",
        ];

        foreach (var folder in folders)
        {
            var folderPath = Path.Combine(fixture.TempFolder, folder);
            Directory.CreateDirectory(folderPath);

            foreach (var file in files)
            {
                var filePath = Path.Combine(folderPath, file);
                await File.WriteAllTextAsync(filePath, null);
            }
        }

        var glob = new Glob(fixture.TempFolder);

        var actualCount = 0;
        await foreach (var file in glob.GetMatchesAsync(
            patterns: ["**/*.md", "**/*.txt"], ignorePatterns: ["folder3"]))
        {
            ++actualCount;
            Assert.Contains(Path.GetFileName(file), files);
        }

        Assert.Equal(4, actualCount);
    }
}
