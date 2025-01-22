using System;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenTabletDriver.Daemon.Library.Reflection.Metadata;
using OpenTabletDriver.Interop;

namespace OpenTabletDriver.Daemon.Library.Reflection
{
    public class DesktopPluginContext : PluginContext
    {
        public DesktopPluginContext(DirectoryInfo directory)
        {
            Directory = directory;
            FriendlyName = Directory.Name;

            foreach (var plugin in Directory.EnumerateFiles("*.dll"))
            {
                // Ignore a plugin library build artifact
                // Loading it seems to stop loading any further DLLs from the directory
                if (string.Equals(plugin.Name, "OpenTabletDriver.dll", StringComparison.OrdinalIgnoreCase))
                    continue;

                LoadAssemblyFromFile(plugin);
            }
        }

        public DirectoryInfo Directory { get; }

        public string FriendlyName { get; }

        public PluginMetadata GetMetadata()
        {
            Directory.Refresh();
            if (Directory.Exists && Directory.EnumerateFiles().FirstOrDefault(f => f.Name == "metadata.json") is FileInfo file)
            {
                return Serialization.Deserialize<PluginMetadata>(file)!;
            }

            return new PluginMetadata
            {
                Name = FriendlyName
            };
        }

        private Assembly? LoadAssemblyFromFile(FileSystemInfo file)
        {
            try
            {
                return LoadFromAssemblyPath(file.FullName);
            }
            catch
            {
                Log.Write("Plugin", $"Failed loading assembly '{file.Name}'", LogLevel.Error);
                return null;
            }
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            if (Directory == null)
            {
                Log.Write("Plugin", $"Independent plugin does not support loading native library '{unmanagedDllName}'",
                    LogLevel.Warning);
                throw new NotSupportedException();
            }

            var runtimeFolder = new DirectoryInfo(Path.Join(Directory.FullName, "runtimes"));
            if (runtimeFolder.Exists)
            {
                var libraryFile = runtimeFolder.EnumerateFiles(ToDllName(unmanagedDllName), SearchOption.AllDirectories)
                    .FirstOrDefault();
                if (libraryFile != null)
                    return LoadUnmanagedDllFromPath(libraryFile.FullName);
            }

            return IntPtr.Zero;
        }

        private static string ToDllName(string dllName)
        {
            return SystemInterop.CurrentPlatform switch
            {
                SystemPlatform.Windows => $"{dllName}.dll",
                SystemPlatform.Linux => $"lib{dllName}.so",
                SystemPlatform.MacOS => $"lib{dllName}.dylib",
                _ => throw new PlatformNotSupportedException()
            };
        }
    }
}
