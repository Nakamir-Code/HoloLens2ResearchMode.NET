# Modernizing UWP to .NET 9 on HoloLens 2

This sample demonstrates how to modernize a UWP app for HoloLens 2 using .NET 9. It shows how to reference native WinRT components, such as **HoloLens2ResearchMode**, and integrate the **Microsoft.MixedReality.QR** NuGet package in a .NET 9 C# project.

## Features

- Targets `.NET 9.0` with `net9.0-windows10.0.22621.0`
- Uses `CsWinRT` to interop with native WinRT components
- Supports direct referencing of C++/WinRT `.vcxproj` projects
- Demonstrates HoloLens 2 sensor access and QR code tracking
- Enables access to Research Mode sensors (depth, IR, etc.)

## Referencing WinRT Components

To consume WinRT components like `HoloLens2ResearchMode`, add the following to your `.csproj`:

```xml
<PropertyGroup>
  <CsWinRTIncludes>HoloLens2ResearchMode</CsWinRTIncludes>
  <CsWinRTGeneratedFilesDir>$(OutDir)</CsWinRTGeneratedFilesDir>
</PropertyGroup>
```
- `CsWinRTIncludes`: Specifies the WinRT namespace to include.
- `CsWinRTGeneratedFilesDir`: Points to the output directory containing generated interop files.

With this setup, the C# project can directly use types from the native component.

## Using Microsoft.MixedReality.QR

The Microsoft.MixedReality.QR NuGet package may include an incorrect native DLL when used in a .NET 9 UWP app, causing a runtime exception where StereoKit fails to load.

To ensure proper interop, manually include the .winmd file for CsWinRT projection and the correct native arm64 DLL:

```xml
<ItemGroup>
  <CsWinRTInputs Include="../Microsoft.MixedReality.QR/0.5.3037/lib/uap10.0.18362/Microsoft.MixedReality.QR.winmd" />
  <Content Include="../Microsoft.MixedReality.QR/0.5.3037/runtimes/win10-arm64/native/Microsoft.MixedReality.QR.dll"
           CopyToOutputDirectory="Always"
           DeploymentContent="true"
           Visible="false" />
</ItemGroup>
```
This setup ensures CsWinRT generates the correct C# projection and that the appropriate native DLL is included.
