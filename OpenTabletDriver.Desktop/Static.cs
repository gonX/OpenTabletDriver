using OpenTabletDriver.Desktop.Reflection;

namespace OpenTabletDriver.Desktop
{
    public static class Static
    {
        public static DesktopPluginManager PluginManager { set; get; } = new DesktopPluginManager();
        public static PresetManager PresetManager { set; get; } = new PresetManager();
    }
}
