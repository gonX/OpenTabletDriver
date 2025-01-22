using System;

namespace OpenTabletDriver.Daemon.Library.Updater
{
    public sealed class SerializedUpdateInfo
    {
        public SerializedUpdateInfo() { }

        public SerializedUpdateInfo(UpdateInfo updateInfo)
        {
            Version = updateInfo.Version;
        }

        public Version Version { get; set; } = new Version();
    }
}
