// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics;
using BenchmarkDotNet.Running;
using Pathological.Globbing.Benchmarks;

if (Debugger.IsAttached)
{
    var benchmarks = new GlobBenchmarks();

    try
    {
        benchmarks.GlobalSetup();

        Debugger.Break();
        benchmarks.GlobCsExpand();
    }
    finally
    {
        benchmarks.GlobalCleanup();
    }
}
else
{
    BenchmarkRunner.Run<GlobBenchmarks>();
}
