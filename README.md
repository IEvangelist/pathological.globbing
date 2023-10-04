﻿# Pathological: Globbing

[![.NET](https://github.com/IEvangelist/pathological.globbing/actions/workflows/dotnet.yml/badge.svg)](https://github.com/IEvangelist/pathological.globbing/actions/workflows/dotnet.yml)

Welcome to the Pathological Globbing library. This library is a wrapper around the `Microsoft.Extensions.FileSystemGlobbing` NuGet package, which provides a fast, efficient, and cross-platform globbing functionality for .NET.

This library strives to simplify globbing by exposing a `Glob` type, which provides an approachable API for matching file paths against globbing patterns.

## 📦 Install

To install the `Pathological.Globbing` NuGet package, run the following command:

```
dotnet add package Pathological.Globbing
```

Alternatively, you can install the `Pathological.Globbing` NuGet package from the package manager in Visual Studio.

## *️⃣ Usage

To use the `Pathological.Globbing` library, you must first create a `Glob` instance. The `Glob` instance is the entry point to the library, and provides a number of methods for matching file paths against globbing patterns.

```csharp
using Pathological.Globbing;

var glob = new Glob();

// All js files, but don't look in node_modules
var files = glob.GetMatches("**/*.js", "node_modules/**");

foreach (var filePath in files)
{
    // Use file...
}
```

> **Note**
> There are various extensions methods that expose, in addition to a `string` representing the fully qualified file path of the matched file, a `GlobMatch` instance. The `GlobMatch` instance provides additional information about the matched file, it can also be used to easily convert to a `FileInfo` type as well.

### 📂 Streaming API

To use the streaming API, call any of the available `IAsyncEnumerable<T>` returning extension methods.

```csharp
using Pathological.Globbing;

var glob = new Glob(BasePath: "../");

// All js files, but don't look in node_modules
var files = glob.GetMatchesAsync("**/*.js", "node_modules/**");

await foreach (var file in files)
{
    // Use file...
}
```

> **Important**
> All streaming APIs support `CancellationToken` for cancellation, or `TimeSpan` for signaling cancellation after a certain amount of time.

### ☑️ Builder-pattern fluent API

To use the builder-pattern fluent API, create a new `GlobOptionsBuilder` instance, and chain any available `With*` method. When you're done, call `Build` which validates the options, and returns a `GlobOptions` instance. The `GlobOptions` instance can be used to execute the globbing operation.

```csharp
using Pathological.Globbing;
using Pathological.Globbing.Options;

// Build glob options.
var builder = new GlobOptionsBuilder()
    .WithBasePath("../../")
    .WithCaseInsensitive(isCaseInsensitive: true)
    .WithPattern("**/*.cs")
    .WithIgnorePatterns(["**/bin/**", "**/obj/**"]);

// Build the globbing options.
// Execute the globbing operation, evaluating results.
var result = builder.Build()
    .ExecuteEvaluation();

_ = result.HasMatches;  // true
_ = result.Matches;     // IEnumerable<GlobMatch>
```

## 🔥 File-system globbing

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
