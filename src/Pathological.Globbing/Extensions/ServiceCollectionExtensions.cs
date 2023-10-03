// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.Globbing.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPathologicalGlobbing(this IServiceCollection services)
    {
        services.AddSingleton<IGlobEvaluator, DefaultGlobEvaluator>();

        return services;
    }
}

internal sealed class DefaultGlobEvaluator : IGlobEvaluator { }
