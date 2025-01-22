using Newtonsoft.Json;
using OpenTabletDriver.Daemon.Library.Diagnostics;
using OpenTabletDriver.Daemon.Library.Interop.AppInfo;
using OpenTabletDriver.Daemon.Library.Json.Converters;
using OpenTabletDriver.Daemon.Library.Json.Converters.Implementations;
using OpenTabletDriver.Devices;
using OpenTabletDriver.Platform.Display;
using OpenTabletDriver.Tablet;

namespace OpenTabletDriver.Daemon.Library.Json
{
    internal static class Utilities
    {
        internal static readonly JsonConverter[] Converters = new JsonConverter[]
        {
            new InterfaceConverter<IReportParser<IDeviceReport>, SerializableDeviceReportParser>(),
            new InterfaceConverter<IDeviceEndpoint, SerializableDeviceEndpoint>(),
            new InterfaceConverter<IDeviceEndpointStream, SerializableDeviceEndpointStream>(),
            new InterfaceConverter<IAppInfo, SerializableAppInfo>(),
            new InterfaceConverter<IDiagnosticInfo, SerializableDiagnosticInfo>(),
            new InterfaceConverter<IDisplay, SerializableDisplay>(),
            new VersionConverter()
        };
    }
}
