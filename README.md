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