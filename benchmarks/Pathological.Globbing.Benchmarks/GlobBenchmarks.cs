// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using System.IO.Abstractions;
using BenchmarkDotNet.Attributes;

namespace Pathological.Globbing.Benchmarks;

[JsonExporterAttribute.Full]
[JsonExporterAttribute.FullCompressed]
public class GlobBenchmarks
{
    private DirectoryInfo? _directory;

    public string[] Patterns { get; } =
    [
        "./{**/?{/**/?{/**/?{/**/?,,,,},,,,},,,,},,,}/**/*.txt",
        "./**/?/**/?/**/?/**/?/**/*.txt",
        "./**/[01]/**/[12]/**/[23]/**/[45]/**/*.txt",
        "./**/**/**/**/**/**/**/**/*.txt",
        "./**/*/**/*/**/*/**/*/**/*.txt",
        "./**/0/**/0/**/*.txt",
        "./**/0/**/0/**/0/**/0/**/*.txt",
        "{**/*.txt,**/?/**/*.txt,**/?/**/?/**/*.txt,**/?/**/?/**/?/**/*.txt,**/?/**/?/**/?/**/?/**/*.txt}",
        "{0000,0,1111,1}/{0000,0,1111,1}/{0000,0,1111,1}/**",
        "**",
        "**/!(0|9).txt",
        "**/????/????/????/????/*.txt",
        "**/[0-9]/**/*.txt",
        "**/*.txt",
        "**/*/*.txt",
        "**/*/**/*.txt",
        "**/*/**/*/**/*/**/*/**",
        "**/5555/0000/*.txt",
    ];

    [ParamsSource(nameof(Patterns))]
    public string Pattern { get; set; } = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var wd = Directory.GetCurrentDirectory();
        var tmp = Path.Combine(wd, "bench-working-dir", "fixture");

        var dir = Directory.CreateDirectory(tmp);

        if (!Directory.Exists(Path.Combine(tmp, "0")))
        {
            Console.WriteLine("Making benchmark fixtures");

            // Create 10_000 directories
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    for (var k = 0; k < 10; k++)
                    {
                        for (var l = 0; l < 10; l++)
                        {
                            var dirPath = Path.Combine(tmp, $"{i}{i}{i}{i}", $"{j}{j}{j}{j}", $"{k}{k}{k}{k}", $"{l}{l}{l}{l}");
                            Directory.CreateDirectory(dirPath);

                            // Create files within each directory
                            for (var m = 0; m < 10; m++)
                            {
                                var filePath = Path.Combine(dirPath, $"{m}{m}{m}{m}.txt");
                                File.Create(filePath).Close();
                            }
                        }
                    }
                }
            }
        }

        Console.WriteLine("Benchmark fixtures created successfully.");

        _directory = dir;
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _directory?.Delete(recursive: true);
    }

    [Benchmark]
    public string[] GetMatches()
    {
        var glob = new Glob(_directory!.FullName);

        var matches = glob.GetMatches(Pattern).ToArray();
        if (matches is { Length: 0 })
        {
            throw new($"""
                Invalid benchmark!
                No matches found for pattern: {Pattern}.
                """);
        }

        return matches;
    }

    [Benchmark]
    public string[] GlobCsExpand()
    {
        var fileSystem = new FileSystem();

        fileSystem.Directory.SetCurrentDirectory(_directory!.FullName);

        var names = Ganss.IO.Glob.ExpandNames(pattern: Pattern, ignoreCase: true).ToArray();
        if (names is { Length: 0 })
        {
            throw new($"""
                Invalid benchmark!
                No matches found for pattern: {Pattern}.
                """);
        }

        return names;
    }
}