# Pathological: Globbing

[![.NET](https://github.com/IEvangelist/pathological.globbing/actions/workflows/dotnet.yml/badge.svg)](https://github.com/IEvangelist/pathological.globbing/actions/workflows/dotnet.yml)

## Install

```
dotnet add package Pathological.Globbing
```

## Usage

```csharp
using Pathological.Globbing;

var glob = new Glob();

// All js files, but don't look in node_modules
var files = glob.GetMatches("**/*.js", "node_modules/**");

foreach (var file in files)
{
    // Use file...
}
```

### Streaming API

```csharp
using Pathological.Globbing;

var glob = new Glob();

// All js files, but don't look in node_modules
var files = glob.GetMatchesAsync("**/*.js", "node_modules/**");

await foreach (var file in files)
{
    // Use file...
}
```

## File-system globbing

This library relies on the `Microsoft.Extensions.FileSystemGlobbing` NuGet package for file-system globbing.

All of the following patterns are supported:

- Exact directory or file name
  
  - `some-file.txt`
  - `path/to/file.txt`

- Wildcards `*` in file and directory names that represent zero to many characters not including separator characters.

    | Value          | Description                                                            |
    |----------------|------------------------------------------------------------------------|
    | `*.txt`        | All files with *.txt* file extension.                                  |
    | `*.*`          | All files with an extension.                                           |
    | `*`            | All files in top-level directory.                                      |
    | `.*`           | File names beginning with '.'.                                         |
    | `*word*`       | All files with 'word' in the filename.                                 |
    | `readme.*`     | All files named 'readme' with any file extension.                      |
    | `styles/*.css` | All files with extension '.css' in the directory 'styles/'.            |
    | `scripts/*/*`  | All files in 'scripts/' or one level of subdirectory under 'scripts/'. |
    | `images*/*`    | All files in a folder with name that is or begins with 'images'.       |

- Arbitrary directory depth (`/**/`).

    | Value | Description |
    | --- | --- |
    | `**/*` | All files in any subdirectory. |
    | `dir/` | All files in any subdirectory under 'dir/'. |
    | `dir/**/*` | All files in any subdirectory under 'dir/'. |

For more information, see [File globbing in .NET: Pattern formats](https://learn.microsoft.com/dotnet/core/extensions/file-globbing#pattern-formats).