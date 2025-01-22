using System;
using System.Collections.Generic;

namespace OpenTabletDriver.Daemon.Library.Reflection
{
    public interface IPluginFactory
    {
        T? Construct<T>(PluginSettings settings, params object[] args) where T : class;
        T? Construct<T>(string fullPath, params object[] args) where T : class;
        Type? GetPluginType(string path);
        IEnumerable<Type> GetMatchingTypes(Type baseType);
        string? GetFriendlyName(string path);
    }
}
