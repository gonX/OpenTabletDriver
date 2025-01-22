using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using OpenTabletDriver.Daemon.Library.Reflection.Metadata;

namespace OpenTabletDriver.Daemon.Library.Reflection
{
    public interface IPluginManager
    {
        event EventHandler<PluginEventType>? AssembliesChanged;

        IEnumerable<Assembly> Assemblies { get; }
        IReadOnlyList<DesktopPluginContext> Plugins { get; }
        IEnumerable<Type> ExportedTypes { get; }
        IEnumerable<Type> LibraryTypes { get; }
        IEnumerable<Type> PluginTypes { get; }

        void Clean();
        void Load();
        bool InstallPlugin(string filePath);
        Task<bool> DownloadPlugin(PluginMetadata metadata);
        bool InstallPlugin(DirectoryInfo target, DirectoryInfo source);
        bool UninstallPlugin(DesktopPluginContext plugin);
        bool UpdatePlugin(DesktopPluginContext plugin, DirectoryInfo source);
        bool UnloadPlugin(DesktopPluginContext context);
        string GetStateHash();
    }
}
