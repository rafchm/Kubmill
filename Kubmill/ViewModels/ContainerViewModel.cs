using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Kubmill.ViewModels
{
    public partial class ContainerViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _applicationTitle = App.AppName;

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationItems = new();

        [ObservableProperty]
        private ObservableCollection<INavigationControl> _navigationFooter = new();

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new();

        [ObservableProperty]
        private Visibility _isBusyVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private int _progress = 0;

        [ObservableProperty]
        private bool _isIndeterminate = true;

        public ContainerViewModel(INavigationService navigationService)
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            NavigationItems = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Home",
                    PageTag = "home",
                    Icon = SymbolRegular.Home24,
                    PageType = typeof(Views.Pages.HomePage)
                },
                new NavigationItem()
                {
                    Content = "Workload",
                    PageTag = "workload",
                    Icon = SymbolRegular.Box24,
                    PageType = typeof(Views.Pages.DataPage)
                },
                new NavigationItem()
                {
                    Content = "Dashboard",
                    PageTag = "dashboard",
                    Icon = SymbolRegular.DataHistogram24,
                    PageType = typeof(Views.Pages.DashboardPage)
                },
                new NavigationItem()
                {
                    Content = "Scripts",
                    PageTag = "scripts",
                    Icon = SymbolRegular.DocumentCatchUp24,
                    PageType = typeof(Views.Pages.ScriptsPage)
                }
            };

            NavigationFooter = new ObservableCollection<INavigationControl>
            {
                new NavigationItem()
                {
                    Content = "Settings",
                    PageTag = "settings",
                    Icon = SymbolRegular.Settings24,
                    PageType = typeof(Views.Pages.SettingsPage)
                }
            };

            TrayMenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem
                {
                    Header = "Home",
                    Tag = "tray_home"
                }
            };

            _isInitialized = true;
        }
    }
}
