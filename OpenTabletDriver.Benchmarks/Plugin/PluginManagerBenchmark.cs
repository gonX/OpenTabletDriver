using BenchmarkDotNet.Attributes;
using Moq;
using OpenTabletDriver.Daemon.Library.Interop.AppInfo;
using OpenTabletDriver.Daemon.Library.Reflection;

namespace OpenTabletDriver.Benchmarks.Plugin
{
    [DryJob]
    public class PluginManagerBenchmark
    {
        private PluginManager? _pluginManager;

        [Benchmark]
        public void PluginManagerCtor()
        {
            var appInfo = new Mock<AppInfo>();
            _pluginManager = new PluginManager(appInfo.Object);
        }
    }
}
