using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Leosac.SharedServices
{
    public class AutoUpdate : ObservableObject
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        public AutoUpdate()
        {
            _hasUpdate = false;
        }

        private bool _hasUpdate;
        private UpdateVersion? _updateVersion;

        public Task<bool> CheckUpdate()
        {
            return CheckUpdate(LeosacAppInfo.Instance?.ApplicationCode);
        }

        public async Task<bool> CheckUpdate(string? applicationCode)
        {
            log.Info("Checking for software update...");

            try
            {
                var client = new HttpClient();
                if (string.IsNullOrEmpty(applicationCode))
                {
                    throw new MaintenanceException("Application Code is required to check for updates.");
                }
                using var response = await client.GetAsync(string.Format("https://download.leosac.com/{0}/latestversion", applicationCode));
                using var content = response.Content;
                var json = await content.ReadAsStringAsync();
                UpdateVersion = JsonConvert.DeserializeObject<UpdateVersion>(json);

                if (UpdateVersion != null)
                {
                    var fvi = AppSettings.GetFileVersionInfo();

                    if (fvi != null && !string.IsNullOrEmpty(fvi.ProductVersion) && !string.IsNullOrEmpty(UpdateVersion.VersionString))
                    {
                        var versionString = fvi.ProductVersion;
                        var hashpos = versionString.IndexOf("+");
                        if (hashpos > 0)
                        {
                            versionString = versionString[..hashpos];
                        }
                        var currentVersion = new Version(versionString);
                        var newVersion = new Version(UpdateVersion.VersionString);

                        if (newVersion > currentVersion)
                        {
                            log.Info("New update available!");
                            HasUpdate = true;
                        }
                        else
                        {
                            log.Info("There is no update available.");
                            HasUpdate = false;
                        }
                    }
                    else
                    {
                        log.Error("Cannot retrieve software versions.");
                    }
                }
                else
                {
                    log.Error("Cannot unserialize the software update.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Software update check failed.", ex);
                HasUpdate = false;
            }

            return HasUpdate;
        }

        public bool HasUpdate
        {
            get => _hasUpdate;
            set => SetProperty(ref _hasUpdate, value);
        }

        public UpdateVersion? UpdateVersion
        {
            get => _updateVersion;
            set => SetProperty(ref _updateVersion, value);
        }

        public void DownloadUpdate()
        {
            if (!string.IsNullOrEmpty(UpdateVersion?.Uri))
            {
                var ps = new ProcessStartInfo(UpdateVersion.Uri)
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(ps);
            }
        }
    }
}
