using OpenTabletDriver.Tablet;

namespace OpenTabletDriver.Daemon.Library.Json.Converters.Implementations
{
    internal sealed class SerializableDeviceReportParser : Serializable, IReportParser<IDeviceReport>
    {
        public IDeviceReport Parse(byte[] report) => throw NotSupported();
    }
}
