using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using OpenTabletDriver.Desktop;
using OpenTabletDriver.Desktop.Profiles;
using OpenTabletDriver.Desktop.Reflection;
using static System.Console;

namespace OpenTabletDriver.Console
{
    partial class Program
    {
        static SHA256 sha256 = SHA256.Create();

        static async Task<Settings> GetSettings()
        {
            return await Driver.Instance.GetSettings();
        }

        static async Task ApplySettings(Settings settings)
        {
            await Driver.Instance.SetSettings(settings);
        }

        static async Task ModifySettings(Action<Settings> func)
        {
            var settings = await GetSettings();
            func.Invoke(settings);
            await ApplySettings(settings);
        }

        static async Task ModifyProfile(string profileName, Action<Profile> func)
        {
            await ModifySettings(async s =>
            {
                var profile = await GetProfile(profileName, s);
                if (profile != null)
                {
                    func.Invoke(profile);
                }
                else
                {
                    throw new ArgumentException("No profile exists for the target tablet.");
                }
            });
        }

        static async Task<Profile> GetProfile(string profileName, Settings settings = null)
        {
            const StringComparison comparer = StringComparison.InvariantCultureIgnoreCase;
            settings ??= await GetSettings();

            var profile = settings.Profiles.FirstOrDefault(p => p.Tablet.Equals(profileName, comparer));
            if (profile == null)
            {
                var tablets = await Driver.Instance.GetTablets();
                var tablet = tablets.FirstOrDefault(t => t.Properties.Name.Equals(profileName, comparer));
                if (tablet != null)
                    profile = Profile.GetDefaults(tablet);
            }

            return profile;
        }

        static async Task ListTypes<T>(Func<Type, bool> predicate = null)
        {
            var types = AppInfo.PluginManager.GetChildTypes<T>();
            if (types.Count == 0)
            {
                await Out.WriteAsync("No types found\n");
                return;
            }

            foreach (var type in types)
            {
                if (predicate?.Invoke(type) ?? true)
                {
                    var name = AppInfo.PluginManager.GetFriendlyName(type.FullName);
                    var output = string.IsNullOrWhiteSpace(name) ? type.FullName : $"{type.FullName} [{name}]";
                    await Out.WriteLineAsync(output);
                }
            }
        }

        static string GetSHA256(string path)
        {
            var data = File.ReadAllBytes(path);
            var hash = sha256.ComputeHash(data);
            return string.Join(null, hash.Select(b => b.ToString("X")));
        }

        static void AppendPluginStoreSettingsCollectionByPaths<T>(PluginSettingStoreCollection pssc, params string[] paths) where T : class
        {
            foreach (var path in paths)
            {
                var existing = pssc.FirstOrDefault(x => x.Path == path);
                if (existing == null)
                {
                    var obj = PluginSettingStore.FromPath(path).Construct<T>();
                    pssc.Add(new PluginSettingStore(obj));
                }
                else
                {
                    existing.Enable = true;
                }
            }
        }

        static void DisableAllInPluginStoreSettingsCollectionByPaths(PluginSettingStoreCollection pssc, params string[] paths)
        {
            foreach (string path in paths)
            {
                var plugins = pssc.Where(x => x.Path == path).ToArray();

                if (plugins.Length == 0)
                    Out.WriteLineAsync("No plugins found matching path");

                foreach (var plugin in plugins)
                    plugin.Enable = false;
            }
        }
    }
}
