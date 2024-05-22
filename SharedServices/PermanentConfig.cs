using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Leosac.SharedServices
{
    public abstract class PermanentConfig<T> : ObservableObject where T : PermanentConfig<T>, new()
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        static readonly JsonSerializer _serializer;

        static PermanentConfig()
        {
            _serializer = new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                Formatting = Formatting.Indented
            };
        }

        public static string? ConfigDirectory { get; set; }

        public static string GetDefaultFileName()
        {
            return string.Format("{0}.json", typeof(T).Name);
        }

        [JsonIgnore]
        public bool IsUserConfiguration { get; protected set; }

        [JsonIgnore]
        public bool FallbackToUserConfiguration { get; protected set; }

        public virtual void SaveToFile()
        {
            var paths = new List<string>
            {
                GetConfigFilePath(true, IsUserConfiguration)
            };
            if (!IsUserConfiguration && FallbackToUserConfiguration)
            {
                paths.Add(GetConfigFilePath(true, true));
            }
            SaveToFile([.. paths]);
        }

        public bool SaveToFile(params string[] filePaths)
        {
            bool saved = false;
            for (int i = 0; i < filePaths.Length && !saved; ++i)
            {
                try
                {
                    log.Info(string.Format("Saving configuration to file {0}...", filePaths[0]));
                    using var file = File.CreateText(filePaths[i]);
                    using var writer = new JsonTextWriter(file);
                    _serializer.Serialize(writer, this);
                    log.Info("Configuration saved.");
                    saved = true;
                }
                catch(Exception ex)
                {
                    log.Error(string.Format("Cannot save to file {0}.", filePaths[i]), ex);
                }
            }
            return saved;
        }

        public static T? LoadFromFile(bool isUserConfiguration, bool fallbackToUserConfiguration = false)
        {
            var paths = new List<string>();
            if (!isUserConfiguration && fallbackToUserConfiguration)
            {
                paths.Add(GetConfigFilePath(GetDefaultFileName(), true));
            }
            paths.Add(GetConfigFilePath(GetDefaultFileName(), isUserConfiguration));
            return LoadFromFile([..paths]);
        }

        public static T? LoadFromFile(params string[] filePaths)
        {
            foreach (var filePath in filePaths)
            {
                log.Info(string.Format("Loading configuration from file {0}...", filePath));
                if (File.Exists(filePath))
                {
                    using var file = File.OpenText(filePath);
                    using var reader = new JsonTextReader(file);
                    var config = _serializer.Deserialize<T>(reader);
                    log.Info("Configuration loaded from file.");
                    return config;
                }
            }

            log.Info("No file found, falling back to default instance.");
            return new T();
        }

        public string GetConfigFilePath()
        {
            return GetConfigFilePath(false);
        }

        public string GetConfigFilePath(bool createFolders)
        {
            return GetConfigFilePath(GetDefaultFileName(), createFolders, IsUserConfiguration);
        }

        public string GetConfigFilePath(bool createFolders, bool isUserConfiguration)
        {
            return GetConfigFilePath(GetDefaultFileName(), createFolders, isUserConfiguration);
        }

        public static string GetConfigFilePath(string fileName, bool isUserConfiguration)
        {
            return GetConfigFilePath(fileName, false, isUserConfiguration);
        }

        public static string GetConfigFilePath(string fileName, bool createFolders, bool isUserConfiguration)
        {
            string path;
            if (!string.IsNullOrEmpty(ConfigDirectory))
            {
                path = ConfigDirectory;
            }
            else
            {
                var perUserInstallation = LeosacAppInfo.Instance?.PerUserInstallation ?? IsPerUserRunningApplication();
                var appData = (perUserInstallation || isUserConfiguration) ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) : Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                path = Path.Combine(appData, "Leosac");
                if (!Directory.Exists(path) && createFolders)
                {
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, LeosacAppInfo.Instance?.ApplicationName ?? "Test");
            }
            if (!Directory.Exists(path) && createFolders)
            {
                Directory.CreateDirectory(path);
            }

            return Path.Combine(path, fileName);
        }

        public static bool IsPerUserRunningApplication()
        {
            var exe = Assembly.GetEntryAssembly()?.Location;
            return !string.IsNullOrEmpty(exe)
                ? exe.StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), StringComparison.InvariantCultureIgnoreCase)
                : false;
        }
    }
}
