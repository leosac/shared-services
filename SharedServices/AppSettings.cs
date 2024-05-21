using System.Diagnostics;
using System.Reflection;

namespace Leosac.SharedServices
{
    public class AppSettings : PermanentConfig<AppSettings>
    {
        private static readonly object _objlock = new();
        private static AppSettings? _singleton;

        public static AppSettings GetSingletonInstance()
        {
            return GetSingletonInstance(false);
        }

        public static AppSettings GetSingletonInstance(bool forceRecreate)
        {
            lock (_objlock)
            {
                if (_singleton == null || forceRecreate)
                {
                    _singleton = LoadFromFile(false);
                }

                return _singleton!;
            }
        }

        public AppSettings()
        {
            _installationId = Guid.NewGuid().ToString("D");
            _isAutoUpdateEnabled = true;
        }

        private string _installationId;
        public string InstallationId
        {
            get => _installationId;
            set => SetProperty(ref _installationId, value);
        }

        private bool _isAutoUpdateEnabled;
        public bool IsAutoUpdateEnabled
        {
            get => _isAutoUpdateEnabled;
            set => SetProperty(ref _isAutoUpdateEnabled, value);
        }

        public static FileVersionInfo? GetFileVersionInfo()
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                return FileVersionInfo.GetVersionInfo(assembly.Location);
            }

            return null;
        }
    }
}
