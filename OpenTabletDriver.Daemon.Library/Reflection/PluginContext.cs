using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace OpenTabletDriver.Daemon.Library.Reflection
{
    public class PluginContext : AssemblyLoadContext
    {
        private const string PLUGIN_ASSEMBLY_NAMESPACE = nameof(OpenTabletDriver);
        private static readonly Assembly? PluginAssembly = Default.Assemblies.FirstOrDefault(asm => asm.GetName().FullName == PLUGIN_ASSEMBLY_NAMESPACE);

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            // Reference shared dependency instead of duplicating it into this PluginContext
            return assemblyName.Name == PLUGIN_ASSEMBLY_NAMESPACE ? PluginAssembly : null;
        }
    }
}
