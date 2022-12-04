using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kubmill.Configuration;
using Kubmill.Services;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Kubmill.ViewModels
{
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _appName = App.AppName;

        [ObservableProperty]
        private string _appVersion = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private ThemeType _currentTheme = ThemeType.Unknown;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private bool _showSystemNamespaces = false;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string _configFilePath = "";

        [ObservableProperty]
        private bool _isModified;

        private readonly IConfigService _configurationService;
        private readonly IDialogControl _dialogControl;
        private readonly AppOptions _options;
        private AppOptions? _originalOptions;

        public SettingsViewModel(IConfigService configurationService, IDialogService dialogService)
        {
            _configurationService = configurationService;
            _dialogControl = dialogService.GetDialogControl();
            _options = configurationService.GetAppOptions();
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            _showSystemNamespaces = _options.Config.ShowSystemNamespaces;
            _currentTheme = _options.General.Theme = Theme.GetAppTheme();
            _originalOptions = AppOptions.Clone(_options);

            AppVersion = $"Version: {GetAssemblyVersion()}";

            _isInitialized = true;
        }

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
        }

        private void CheckIsModified()
        {
            IsModified = !_options.IsSame(_originalOptions);
        }

        partial void OnCurrentThemeChanged(ThemeType value)
        {
            _options.General.Theme = value;
            Theme.Apply(value);
            CheckIsModified();
        }

        partial void OnShowSystemNamespacesChanged(bool value)
        {
            _options.Config.ShowSystemNamespaces = value;
            CheckIsModified();
        }

        partial void OnConfigFilePathChanged(string value)
        {
            _options.Config.KubeConfigPath = value;
            CheckIsModified();
        }

        [RelayCommand]
        private void SelectKubeConfig()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Kube config files|config*|Config files|*.config|All Files|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                _configFilePath = dlg.FileName;
            }
        }

        public bool SaveCmdCanExecute()
        {
            return IsModified;
        }

        [RelayCommand(CanExecute = nameof(IsModified))]
        private void OnSave()
        {
            _configurationService.SaveOptions(_options);

            if (_originalOptions?.Config.KubeConfigPath != _options.Config.KubeConfigPath)
            {
                _dialogControl.Show(null, "Updates will take effect after restart.");
            }
        }
    }
}
