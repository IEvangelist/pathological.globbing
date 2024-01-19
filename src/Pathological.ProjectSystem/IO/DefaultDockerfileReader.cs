// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.IO;

internal sealed partial class DefaultDockerfileReader : IDockerfileReader
{
    // For testing purposes only.
    internal static IDockerfileReader Factory { get; } = new DefaultDockerfileReader();

    public async ValueTask<Dockerfile> ReadDockerfileAsync(string dockerfilePath)
    {
        Dockerfile dockerfile = new()
        {
            FullPath = dockerfilePath
        };

        if (File.Exists(dockerfilePath))
        {
            var dockerfileContent = await File.ReadAllTextAsync(dockerfilePath);
            var fromMatches = FromRegex().Matches(dockerfileContent);
            foreach (var match in fromMatches.Cast<Match>())
            {
                dockerfile = ParseMatch(dockerfile, dockerfileContent, match);
            }

            var copyMatches = CopyRegex().Matches(dockerfileContent);
            foreach (var match in copyMatches.Cast<Match>())
            {
                dockerfile = ParseMatch(dockerfile, dockerfileContent, match);
            }
        }

        return dockerfile;
    }

    static Dockerfile ParseMatch(Dockerfile dockerfile, string dockerfileContent, Match match)
    {
        var group = match.Groups["tag"];
        (var index, var tag) = (group.Index, group.Value);
        var image = match.Groups["image"].Value;
        if (image is not null && tag is not null)
        {
            if (dockerfile.ImageDetails is null)
            {
                dockerfile = dockerfile with
                {
                    ImageDetails = new HashSet<ImageDetails>()
                };
            }

            var lineNumber = dockerfileContent.GetLineNumberFromIndex(index);

            var isFramework = image.Contains("framework");

            tag = tag.Contains('-') ? tag.Split("-")[0] : tag;

            var firstNumber = int.TryParse(tag[0].ToString(), out var number) ? number : -1;

            var tfm = isFramework switch
            {
                true => $"net{tag.Replace(".", "")}",
                false when firstNumber < 4 => $"netcoreapp{tag}",
                _ => $"net{tag}"
            };

            dockerfile.ImageDetails.Add(
                new ImageDetails(image, tag, tfm, lineNumber));
        }

        return dockerfile;
    }

    [GeneratedRegex(
        pattern: @"FROM (?<image>.+?dotnet.+?):(?<tag>.+?)[\s|\n]",
        options: RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture,
        cultureName: "en-US")]
    private static partial Regex FromRegex();

    [GeneratedRegex(
        pattern: @"COPY --from=(?<image>.+?dotnet.+?):(?<tag>.+?)[\s|\n]",
        options: RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture,
        cultureName: "en-US")]
    private static partial Regex CopyRegex();
}
