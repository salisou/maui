# .NET MAUI Workloads

.NET Workloads are a new concept in .NET 6.

The idea, is a project to be able to set `$(UseMaui)`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net6.0-android;net6.0-ios</TargetFrameworks>
    <OutputType>Exe</OutputType>
    <UseMaui>true</UseMaui>
  </PropertyGroup>
</Project>
```

`$(UseMaui)` automatically brings in the following workload packs:

* `Microsoft.NET.Sdk.Maui`
* `Microsoft.Maui.Controls.Sdk`
* `Microsoft.Maui.Resizetizer.Sdk`
* `Microsoft.Maui.Core.Ref.[platform]`
* `Microsoft.Maui.Core.Runtime.[platform]`
* `Microsoft.Maui.Controls.Ref.[platform]`
* `Microsoft.Maui.Controls.Runtime.[platform]`
* `Microsoft.Maui.Dependencies`
* `Microsoft.Maui.Essentials.Ref.[platform]`
* `Microsoft.Maui.Essentials.Runtime.[platform]`
* `Microsoft.Maui.Templates`

BlazorWebView is an addition to MAUI, project can currently opt into
it by adding `.Razor` to the `Sdk` attribute.

`<Project Sdk="Microsoft.NET.Sdk.Razor">` sets
`$(UsingMicrosoftNETSdkRazor)`, which triggers the MAUI workload to
include:

* `Microsoft.Maui.Controls.BlazorWebView.Sdk`
* `Microsoft.Maui.Controls.BlazorWebView.Ref.[platform]`
* `Microsoft.Maui.Controls.BlazorWebView.Runtime.[platform]`
* `Microsoft.Maui.Controls.BlazorWebView.Dependencies`

If you are a .NET 6 project, but don't want to use
Microsoft.Maui.Controls you could bring in partial parts of MAUI.

`$(UseMauiAssets)` brings in `Microsoft.Maui.Resizetizer.Sdk`.

`$(UseMauiCore)` brings in:

* `Microsoft.Maui.Core.Ref.[platform]`
* `Microsoft.Maui.Core.Runtime.[platform]`

`$(UseEssentials)` brings in:

* `Microsoft.Maui.Essentials.Ref.[platform]`
* `Microsoft.Maui.Essentials.Runtime.[platform]`

Special files:

* `AutoImport.props` - defines the default includes (or wildcards) for
  Maui projects will go. Note that this is imported by *all* .NET 6
  project types -- *even non-mobile ones*.
* `WorkloadManifest.json` - general .NET workload configuration
* `WorkloadManifest.targets` - imports `Microsoft.Maui.Controls.Sdk` when
  `$(UseMaui)` is `true`. Note that this is imported by *all* .NET 6
  project types -- *even non-mobile ones*.

For further details about .NET Workloads, see these .NET design docs:

* [.NET Optional SDK Workloads](https://github.com/dotnet/designs/blob/main/accepted/2020/workloads/workloads.md)
* [Workload Resolvers](https://github.com/dotnet/designs/blob/main/accepted/2020/workloads/workload-resolvers.md)
* [Workload Manifests](https://github.com/mhutch/designs/blob/b82449a228c0addb95b5a4995bb838749ea6f8cc/accepted/2020/workloads/workload-manifest.md)

## .NET MAUI Workload Ids

A .NET "workload" is a collection of packs.

.NET MAUI will have several workload ids depending on what needs to be
installed:

* `microsoft-maui-controls-sdk-full`: everything
* `microsoft-maui-controls-sdk-mobile`: iOS & Android
* `microsoft-maui-controls-sdk-desktop`: Mac Catalyst & Windows
* `microsoft-maui-controls-sdk-core`: required by all platforms
* `microsoft-maui-controls-sdk-android`
* `microsoft-maui-controls-sdk-maccatalyst`
* `microsoft-maui-controls-sdk-macos`
* `microsoft-maui-controls-sdk-windows`

Eventually, Android will have a `microsoft-android-sdk-minimal`
workload id that excludes AOT compilers. We'll need to modify some of
the MAUI workload ids when this is available.

These ids will not map exactly to the Visual Studio Installer's
concept of a "workload". Consider the following diagram for what .NET
developers would get from the choices of `mobile`, `maui`, or
`desktop`:

![Workload Diagram](docs/workload-diagram.png)

## Using the .NET MAUI Workload

After you've done a build, such as:

```dotnetcli
$ dotnet cake
```

You'll have various `artifacts/*.nupkg` files produced, as well as the
proper files copied to `./bin/dotnet`.

At this point, you can build the samples using `-p:UseWorkload=true`.
This uses the workload instead of the `<ProjectReference/>` that are
declared:

```dotnetcli
$ git clean -dxf src/Controls/samples/
$ ./bin/dotnet/dotnet build Microsoft.Maui.Samples-net6.slnf -p:UseWorkload=true
```

### Install System-Wide

Once you have `artifacts/*.nupkg` locally, you can install them in a
system-wide dotnet install in `/usr/local/share/dotnet/` or
`C:\Program Files\dotnet\`.

On macOS, you could do:

```dotnetcli
$ sudo dotnet build src/DotNet/DotNet.csproj -t:Install
```

On Windows, you would use an Administrator command prompt:

```dotnetcli
> dotnet build src/DotNet/DotNet.csproj -t:Install
```

`DotNet.csproj` will install the workload in the instance of `dotnet`
that you run it under.

### CI for dotnet/maui

On CI in order to test the workload, we download the `.nupkg` files to
`artifacts` and provision a .NET 6 without mobile workload packs via
`-p:InstallWorkloadPacks=false`:

```dotnetcli
$ dotnet build src/DotNet/DotNet.csproj -p:InstallWorkloadPacks=false
```

Next, we can use the new `Install` target to extract from `artifacts/*.nupkg`:

```dotnetcli
$ ./bin/dotnet/dotnet build src/DotNet/DotNet.csproj -t:Install
```

Then we can build samples with `-p:UseWorkload=true`:

```dotnetcli
$ ./bin/dotnet/dotnet build Microsoft.Maui.Samples-net6.slnf -p:UseWorkload=true
```
