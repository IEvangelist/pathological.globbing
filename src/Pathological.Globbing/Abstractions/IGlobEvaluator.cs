// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Abstractions;

public interface IGlobEvaluator
{
    public GlobMatchingResult Evaluate(GlobOptionsBuilder builder)
    {
        var glob = Glob.InitializeFromBuilder(builder);

        return glob.ExecuteMatcher();
    }
}
