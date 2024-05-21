using CommunityToolkit.Mvvm.ComponentModel;

namespace Leosac.SharedServices
{
    public class UpdateVersion : ObservableObject
    {
        public UpdateVersion()
        {
            _versionString = "0.0.0";
            _uri = string.Empty;
        }

        private string _versionString;
        private string? _uri;

        public string VersionString
        {
            get => _versionString;
            set => SetProperty(ref _versionString, value);
        }

        public string? Uri
        {
            get => _uri;
            set => SetProperty(ref _uri, value);
        }
    }
}
