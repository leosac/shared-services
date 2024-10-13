using System.Diagnostics;
using System.Reflection;

namespace Leosac.SharedServices
{
    public abstract class LeosacAppInfo
    {
        public static LeosacAppInfo? Instance { get; set; }

        protected LeosacAppInfo()
        {
            var location = Assembly.GetEntryAssembly()?.Location;
            ApplicationName = "Leosac Application";
            if (!string.IsNullOrEmpty(location))
            {
                try
                {
                    var fvi = FileVersionInfo.GetVersionInfo(location);
                    if (!string.IsNullOrEmpty(fvi.ProductName))
                    {
                        ApplicationName = fvi.ProductName;
                    }
                }
                catch { }
            }
            ApplicationTitle = ApplicationName;
            CheckPlan = true;
            PerUserInstallation = true;
            ApplicationUrl = "https://leosac.com";
        }

        public string ApplicationName { get; protected set; }

        public string ApplicationTitle { get; protected set; }

        public string? ApplicationCode { get; protected set; }

        public string ApplicationUrl { get; protected set; }

        public bool CheckPlan { get; protected set; }

        public bool? PerUserInstallation { get; protected set; }
    }
}
