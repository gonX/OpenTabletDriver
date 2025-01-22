using System.IO;

namespace OpenTabletDriver.Daemon.Library
{
    public interface ISettingsManager
    {
        Settings Settings { set; get; }

        bool Load();
        void Save();

        bool Load(FileInfo file);
        void Save(FileInfo file);
    }
}
