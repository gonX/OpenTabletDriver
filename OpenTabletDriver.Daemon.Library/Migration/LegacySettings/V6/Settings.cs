using System;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OpenTabletDriver.Daemon.Library.Profiles;
using OpenTabletDriver.Daemon.Library.Reflection;
using OpenTabletDriver.Output;

namespace OpenTabletDriver.Daemon.Library.Migration.LegacySettings.V6
{
    internal class Settings : IMigrate<OpenTabletDriver.Daemon.Library.Settings>
    {
        [JsonProperty("Profiles")]
        public Collection<Profile>? Profiles { set; get; }

        [JsonProperty("LockUsableAreaDisplay")]
        public bool LockUsableAreaDisplay { set; get; }

        [JsonProperty("LockUsableAreaTablet")]
        public bool LockUsableAreaTablet { set; get; }

        [JsonProperty("Tools")]
        public Collection<PluginSettingStore>? Tools { set; get; }

        public OpenTabletDriver.Daemon.Library.Settings Migrate(IServiceProvider serviceProvider)
        {
            var pluginFactory = serviceProvider.GetRequiredService<IPluginFactory>();

            var profilesQuery = Profiles?.MigrateAll<Profile, OpenTabletDriver.Daemon.Library.Profiles.Profile>(serviceProvider);
            var tools = Tools?.MigrateAll<PluginSettingStore, PluginSettings>(serviceProvider);

            var profiles = new ProfileCollection(profilesQuery ?? Array.Empty<OpenTabletDriver.Daemon.Library.Profiles.Profile>());
            foreach (var profile in profiles)
            {
                var outputModeType = pluginFactory.GetPluginType(profile.OutputMode.Path);
                if (outputModeType?.IsAssignableTo(typeof(AbsoluteOutputMode)) ?? false)
                {
                    var lockToBounds = LockUsableAreaDisplay || LockUsableAreaTablet;
                    profile.OutputMode[nameof(AbsoluteOutputMode.LockToBounds)].SetValue(lockToBounds);
                }
            }

            return new OpenTabletDriver.Daemon.Library.Settings
            {
                Profiles = profiles,
                Tools = new PluginSettingsCollection(tools ?? Array.Empty<PluginSettings>()),
                Revision = Version.Parse("0.7.0.0")
            };
        }
    }
}
