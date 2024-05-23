# Pathological: Project System

This library relies on `Pathological.Globbing` library to wrap some common glob patterns for matching on .NET projects, solutions (both _.sln_ and _.slnx_), and Dockerfile.

## 📦 Install

To install the `Pathological.ProjectSystem` NuGet package, run the following command:

```
dotnet add package Pathological.ProjectSystem
```

Alternatively, you can install the `Pathological.ProjectSystem` NuGet package from the package manager in Visual Studio.

## 🚀 Usage

To use the `Pathological.ProjectSystem` library, you must first register its services for DI. Consider the following example:

![Example app usage](https://raw.githubusercontent.com/IEvangelist/pathological.globbing/main/assets/project-system.png)
