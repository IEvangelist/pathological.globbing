// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Pathological.ProjectSystem.Tests;

internal static class TestInput
{
    internal const string TestSolutionXml = """
        Microsoft Visual Studio Solution File, Format Version 12.00
        # Visual Studio Version 17
        VisualStudioVersion = 17.8.34004.107
        MinimumVisualStudioVersion = 10.0.40219.1
        Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Pathological.Globbing", "src\Pathological.Globbing\Pathological.Globbing.csproj", "{7DCC2D8B-E35D-4B52-9CC2-6554374E506C}"
        EndProject
        Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Pathological.Globbing.Tests", "tests\Pathological.Globbing.Tests\Pathological.Globbing.Tests.csproj", "{ED0D1D5E-C1E0-4DEA-BC32-8E444E43F07E}"
        EndProject
        Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "Solution Items", "Solution Items", "{1FBB26F1-0461-4E35-8DF2-708D44A4C96E}"
        	ProjectSection(SolutionItems) = preProject
        		.editorconfig = .editorconfig
        		.gitignore = .gitignore
        		Directory.Build.props = Directory.Build.props
        		LICENSE = LICENSE
        		README.md = README.md
        	EndProjectSection
        EndProject
        Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "Pathological.Globbing.Benchmarks", "benchmarks\Pathological.Globbing.Benchmarks\Pathological.Globbing.Benchmarks.csproj", "{AA3F64C0-FF46-4F90-B334-4B9A27C89C08}"
        EndProject
        Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Pathological.ProjectSystem", "src\Pathological.ProjectSystem\Pathological.ProjectSystem.csproj", "{062BB171-B541-4060-B6D3-E322E5E42AD3}"
        EndProject
        Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Pathological.ProjectSystem.Tests", "tests\Pathological.ProjectSystem.Tests\Pathological.ProjectSystem.Tests.csproj", "{D7FF57CD-799D-4201-9ACC-FB825918AE0E}"
        EndProject
        Global
        	GlobalSection(SolutionConfigurationPlatforms) = preSolution
        		Debug|Any CPU = Debug|Any CPU
        		Release|Any CPU = Release|Any CPU
        	EndGlobalSection
        	GlobalSection(ProjectConfigurationPlatforms) = postSolution
        		{7DCC2D8B-E35D-4B52-9CC2-6554374E506C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        		{7DCC2D8B-E35D-4B52-9CC2-6554374E506C}.Debug|Any CPU.Build.0 = Debug|Any CPU
        		{7DCC2D8B-E35D-4B52-9CC2-6554374E506C}.Release|Any CPU.ActiveCfg = Release|Any CPU
        		{7DCC2D8B-E35D-4B52-9CC2-6554374E506C}.Release|Any CPU.Build.0 = Release|Any CPU
        		{ED0D1D5E-C1E0-4DEA-BC32-8E444E43F07E}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        		{ED0D1D5E-C1E0-4DEA-BC32-8E444E43F07E}.Debug|Any CPU.Build.0 = Debug|Any CPU
        		{ED0D1D5E-C1E0-4DEA-BC32-8E444E43F07E}.Release|Any CPU.ActiveCfg = Release|Any CPU
        		{ED0D1D5E-C1E0-4DEA-BC32-8E444E43F07E}.Release|Any CPU.Build.0 = Release|Any CPU
        		{AA3F64C0-FF46-4F90-B334-4B9A27C89C08}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        		{AA3F64C0-FF46-4F90-B334-4B9A27C89C08}.Debug|Any CPU.Build.0 = Debug|Any CPU
        		{AA3F64C0-FF46-4F90-B334-4B9A27C89C08}.Release|Any CPU.ActiveCfg = Release|Any CPU
        		{AA3F64C0-FF46-4F90-B334-4B9A27C89C08}.Release|Any CPU.Build.0 = Release|Any CPU
        		{062BB171-B541-4060-B6D3-E322E5E42AD3}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        		{062BB171-B541-4060-B6D3-E322E5E42AD3}.Debug|Any CPU.Build.0 = Debug|Any CPU
        		{062BB171-B541-4060-B6D3-E322E5E42AD3}.Release|Any CPU.ActiveCfg = Release|Any CPU
        		{062BB171-B541-4060-B6D3-E322E5E42AD3}.Release|Any CPU.Build.0 = Release|Any CPU
        		{D7FF57CD-799D-4201-9ACC-FB825918AE0E}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        		{D7FF57CD-799D-4201-9ACC-FB825918AE0E}.Debug|Any CPU.Build.0 = Debug|Any CPU
        		{D7FF57CD-799D-4201-9ACC-FB825918AE0E}.Release|Any CPU.ActiveCfg = Release|Any CPU
        		{D7FF57CD-799D-4201-9ACC-FB825918AE0E}.Release|Any CPU.Build.0 = Release|Any CPU
        	EndGlobalSection
        	GlobalSection(SolutionProperties) = preSolution
        		HideSolutionNode = FALSE
        	EndGlobalSection
        	GlobalSection(ExtensibilityGlobals) = postSolution
        		SolutionGuid = {93DA9847-4BE8-4B6D-8953-38D6D93EAD69}
        	EndGlobalSection
        EndGlobal
        """;

    internal const string TestProjectXml = """
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net8.0</TargetFramework>
            <ImplicitUsings>enable</ImplicitUsings>
            <Nullable>enable</Nullable>

            <IsPackable>false</IsPackable>
            <IsTestProject>true</IsTestProject>
          </PropertyGroup>

          <ItemGroup>
            <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.6.0" />
            <PackageReference Include="xunit" Version="2.4.2" />
            <PackageReference Include="xunit.runner.visualstudio" Version="2.4.5">
              <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
              <PrivateAssets>all</PrivateAssets>
            </PackageReference>
            <PackageReference Include="coverlet.collector" Version="6.0.0">
              <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
              <PrivateAssets>all</PrivateAssets>
            </PackageReference>
          </ItemGroup>

          <ItemGroup>
            <ProjectReference Include="..\..\src\Pathological.ProjectSystem\Pathological.ProjectSystem.csproj" />
          </ItemGroup>

        </Project>
        """;

    internal const string DockerfileWithWeirdBits = """
        FROM microsoft/windowsservercore
        ADD publish/ /
        ENTRYPOINT ConsoleRandomAnswerGenerator.exe
        """;

    internal const string DockerfileWithMultipleTfms = """
        FROM mcr.microsoft.com/dotnet/aspnet:3.1.30-bionic AS build-env
        WORKDIR /App

        # Copy everything
        COPY . ./

        FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.16

        # Restore as distinct layers
        RUN dotnet restore

        FROM mcr.microsoft.com/dotnet/framework/sdk:4.7.1

        # Build and publish a release
        RUN dotnet publish -c Release -o out
        FROM mcr.microsoft.com/azure/bits:6.0

        COPY --from=mcr.microsoft.com/dotnet/framework/runtime:3.5-20221011-windowsservercore-ltsc2019 /usr/share/dotnet/shared

        FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-20221011-windowsservercore-ltsc2022

        # Build runtime image
        FROM mcr.microsoft.com/dotnet/aspnet:7.0
        WORKDIR /App
        COPY --from=build-env /App/out .
        ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]
        """;
}
