// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.IO;

internal sealed partial class DefaultSolutionReader(IProjectReader projectReader) : ISolutionReader
{
    // For testing purposes only.
    internal static ISolutionReader Factory { get; } = new DefaultSolutionReader(DefaultProjectReader.Factory);

    public async ValueTask<Solution> ReadSolutionAsync(string solutionPath)
    {
        Solution solution = new()
        {
            FullPath = Path.GetFullPath(solutionPath)
        };

        if (File.Exists(solutionPath))
        {
            var solutionDirectory = Path.GetDirectoryName(solution.FullPath);

            var solutionText = await File.ReadAllTextAsync(solution.FullPath);

            var regex = solution.Extension switch
            {
                ".sln" => SlnProjectPathRegex(),
                ".slnx" => SlnxProjectPathRegex(),
                _ => null
            };

            if (regex is Regex projectPathRegex)
            {
                await AddProjectReferencesAsync(
                    projectPathRegex, solution, solutionDirectory, solutionText);
            }
        }

        return solution;
    }

    private async Task AddProjectReferencesAsync(Regex expression, Solution solution, string? solutionDirectory, string solutionText)
    {
        var matches = expression.Matches(solutionText);

        foreach (var match in matches.Cast<Match>())
        {
            var path = match.Groups["Path"].Value;

            var fullPath = Path.Combine(solutionDirectory!, path);

            if (File.Exists(fullPath) is false ||
                File.GetAttributes(fullPath).HasFlag(FileAttributes.Directory))
            {
                continue;
            }

            var project = await projectReader.ReadProjectAsync(fullPath);

            solution.Projects.Add(project);
        }
    }

    [GeneratedRegex(
        pattern: "^Project\\(\"{(?<TypeId>[A-F0-9-]+)}\"\\) = \"(?<Name>.*?)\", \"(?<Path>.*?)\", \"{(?<Id>[A-F0-9-]+)}\"(?<Sections>(.|\\n|\\r)*?)EndProject(\\n|\\r)",
        options: RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture,
        cultureName: "en-US")]
    private static partial Regex SlnProjectPathRegex();

    [GeneratedRegex(
        pattern: "<Project\\s+Path=\"(?<Path>[^\"]+)\"",
        options: RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture,
        cultureName: "en-US")]
    private static partial Regex SlnxProjectPathRegex();
}
