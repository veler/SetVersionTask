# MSBuild SetVersion Task #

This is a simple MSBuild task allowing you to set or update the assembly version of a C# file

## Usage ##

    <SetVersion FileName="AssemblyInfo.cs" Version="1.2.+.=" />
    <SetVersion FileName="package.appxmanifest" Version="1.2.+.=" />

The rule `"1.2.+.="` means: set the Major version to 1, set the Minor version to 2, increment the Build and leave the Revision at whatever it was before. The `Version` will be changed - all other attributes will be left unchanged.

## Example ##

Here's a more complete example. We store the location of SetVersionTask.dll in a `PropertyGroup`.
We've got an `UpdateVersionNumber` target that only runs if it is a release build and if SetVersionTask.dll can be found. Then we increment the build number in for both `Version`. Since this example is in a csproj file, for our target to be automatically be called before a Release build from Visual Studio, we make the built-in "BeforeBuild" Target depend on our `UpdateVersionNumber` target.

    <PropertyGroup>
      <SetVersionPath>..\Tools\SetVersionTask.dll</SetVersionPath>
    </PropertyGroup>
    <UsingTask TaskName="SetVersion" AssemblyFile="$(SetVersionPath)" />
    <Target Name="UpdateVersionNumber" Condition="Exists('$(SetVersionPath)') AND '$(Configuration)' == 'Release'">
      <Message Text="Updating Version..." />
      <SetVersion FileName="Properties\AssemblyInfo.cs" Version="=.=.+.=" />
    </Target>
    <Target Name="BeforeBuild" DependsOnTargets="UpdateVersionNumber">  
    </Target>

**Note:** If you are compiling from within Visual Studio and have the `AssemblyInfo.cs` file open, then although `AssemblyInfo.cs` will get updated, the build will use the version of `AssemblyInfo.cs` as it was when you launched compilation. The workaround is to leave `AssemblyInfo.cs` closed when building, or build from the command line using msbuild. If I can find a fix to this I will include it in the next version. Another workaround is described [here](http://stackoverflow.com/questions/9261599/how-to-get-visual-studio-to-reread-source-files-after-beforebuild-processing)