using Kubmill.Configuration;
using Kubmill.Repositories;
using Kubmill.Services;
using Kubmill.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace Kubmill
{
    /// <summary>
    /// Application startup
    /// </summary>
    public partial class App
    {
        public const string AppName = "Kubmill";

        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c =>
            {
                c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location));
                c.AddJsonFile(ConfigService.ConfigFileName, true);
            })
            .ConfigureServices((context, services) =>
            {
                // App Host
                services.AddHostedService<ApplicationHostService>();

                // Page resolver service
                services.AddSingleton<IPageService, PageService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window container with navigation
                services.AddScoped<INavigationWindow, Views.Container>();
                services.AddScoped<ViewModels.ContainerViewModel>();

                // Views and ViewModels
                services.AddScoped<Views.Pages.HomePage>();
                services.AddScoped<ViewModels.HomeViewModel>();
                services.AddScoped<Views.Pages.DashboardPage>();
                services.AddScoped<ViewModels.DashboardViewModel>();
                services.AddScoped<Views.Pages.DataPage>();
                services.AddScoped<ViewModels.DataViewModel>();
                services.AddScoped<Views.Pages.ScriptsPage>();
                services.AddScoped<ViewModels.ScriptsViewModel>();
                services.AddScoped<Views.Pages.SettingsPage>();
                services.AddScoped<ViewModels.SettingsViewModel>();
                

                // Windows and ViewModels
                services.AddTransient<ViewModels.OutputDataViewModel>();
                services.AddTransient<ScriptOutputWindow>();

                // Singleton services
                services.AddSingleton<IKubernetesService, KubernetesService>();
                services.AddSingleton<IConfigService, ConfigService>();
                services.AddSingleton<IScriptService, ScriptService>();
                services.AddSingleton<IWindowService, WindowService>();
                services.AddSingleton<IScriptRepository, ScriptRepository>();
                services.AddSingleton<ISnackbarService, SnackbarService>();
                services.AddSingleton<IDialogService, DialogService>();

                // Configuration
                services.Configure<ConfigOptions>(context.Configuration.GetSection(nameof(AppOptions.Config)));
                services.Configure<GeneralOptions>(context.Configuration.GetSection(nameof(AppOptions.General)));

            }).Build();

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, 
                "Exception", MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true;
        }
    }
}