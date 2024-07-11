// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.IO;

internal sealed partial class DefaultProjectReader : IProjectReader
{
    // For testing purposes only.
    internal static IProjectReader Factory { get; } = new DefaultProjectReader();

    public async ValueTask<Project> ReadProjectAsync(string projectPath)
    {
        Project project = new()
        {
            FullPath = projectPath
        };

        if (File.Exists(projectPath))
        {
            var projectContent = await File.ReadAllTextAsync(projectPath);

            return await ParseXmlAsync(project, projectContent);
        }

        return project;
    }

    static async ValueTask<Project> ParseXmlAsync(Project project, string projectContent)
    {
        var (index, rawTargetFrameworkMonikers) = MatchExpression(TargetFrameworkRegex(), projectContent, "tfm");

        if (IsMSBuildExpression(rawTargetFrameworkMonikers) ||
            string.IsNullOrWhiteSpace(rawTargetFrameworkMonikers))
        {
            var (_, targetFrameworkMonikers) = await TryResolvingDirectoryBuildPropsAsync(
                rawTargetFrameworkMonikers, project.FullPath);

            rawTargetFrameworkMonikers = targetFrameworkMonikers;
        }

        var lineNumber = projectContent.GetLineNumberFromIndex(index);

        var (_, sdk) = MatchExpression(ProjectSdkRegex(), projectContent, "sdk");

        return project with
        {
            TargetFrameworkMonikerLineNumber = lineNumber,
            RawTargetFrameworkMonikers = rawTargetFrameworkMonikers!,
            Sdk = sdk.NullIfEmpty()
        };
    }

    private static async Task<(int, string?)> TryResolvingDirectoryBuildPropsAsync(
        string? rawTargetFrameworkMonikers, string fullPath)
    {
        var directory = Path.GetDirectoryName(fullPath);

        var directoryBuildProps = directory.TraverseDirectoriesAndFindFile("Directory.Build.props");

        if (File.Exists(directoryBuildProps))
        {
            var buildPropsContent = await File.ReadAllTextAsync(directoryBuildProps);

            if (string.IsNullOrWhiteSpace(rawTargetFrameworkMonikers))
            {
                return MatchExpression(TargetFrameworkRegex(), buildPropsContent, "tfm");
            }
            else
            {
                // Gets the MSBuild expression key:
                //   <TargetFrameworks>$(DefaultTargetFrameworks)</TargetFrameworks>
                //   In this example, the key is "DefaultTargetFrameworks"
                var (_, key) = MatchExpression(
                MSBuildExpressionKeyRegex(), rawTargetFrameworkMonikers, "key");

                return MatchExpression(
                    new Regex($"<{key}>(?<value>.+)</{key}>"), buildPropsContent, "value");
            }
        }

        return (-1, rawTargetFrameworkMonikers);
    }

    private static bool IsMSBuildExpression(string? rawTargetFrameworkMonikers)
    {
        // TFMs that start with '$' are MSBuild expressions.
        // For example: <TargetFrameworks>$(DefaultTargetFrameworks)</TargetFrameworks>
        return rawTargetFrameworkMonikers?.StartsWith('$') ?? false;
    }

    private static (int Index, string? Value) MatchExpression(
        Regex expression, string content, string groupName)
    {
        if (expression?.Match(content) is { } match)
        {
            var group = match.Groups[groupName];

            return (group.Index, group.Value);
        }

        return (0, null);
    }

    [GeneratedRegex(
        pattern: @"\<Project Sdk=""(?<sdk>.+?)""",
        options: RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture,
        cultureName: "en-US")]
    private static partial Regex ProjectSdkRegex();

    [GeneratedRegex(
        pattern: "TargetFramework(.*)>(?<tfm>.+?)</",
        options: RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture,
        cultureName: "en-US")]
    private static partial Regex TargetFrameworkRegex();

    [GeneratedRegex(
        pattern: "\\$\\((?<key>.+?)\\)",
        options: RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture,
        cultureName: "en-US")]
    private static partial Regex MSBuildExpressionKeyRegex();
}
