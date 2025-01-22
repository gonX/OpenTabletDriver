using System;

namespace OpenTabletDriver.Daemon.Library.Migration
{
    internal interface IMigrate<out T>
    {
        T? Migrate(IServiceProvider serviceProvider);
    }
}
