using System.IO;

namespace OpenTabletDriver.Daemon.Library.Interop.AppInfo
{
    using static FileUtilities;

    public class WindowsAppInfo : AppInfo
    {
        public WindowsAppInfo()
        {
            AppDataDirectory = GetExistingPathOrLast(AppDataDirectory, Path.Join(ProgramDirectory, "userdata"), "$LOCALAPPDATA\\OpenTabletDriver");
        }
    }
}
