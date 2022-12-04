using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kubmill.Models.Kubernetes;
using Kubmill.Repositories;
using Kubmill.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wpf.Ui.Mvvm.Contracts;

namespace Kubmill.ViewModels
{
    public partial class HomeViewModel : PageViewModelBase
    {
        private readonly IKubernetesService _kubService;
        private readonly INavigationService _navService;
        private readonly IScriptRepository _scriptRepository;

        [ObservableProperty]
        private List<K8Context>? _contexts;

        public HomeViewModel(IKubernetesService kubService, INavigationService navService,
            IScriptRepository scriptRepository)
            : base()
        {
            _kubService = kubService;
            _navService = navService;
            _scriptRepository = scriptRepository;
        }

        public override bool OnLoadPage()
        {
            var config = _kubService.LoadConfiguration();

            _scriptRepository.LoadScripts();

            Contexts = config.Contexts.ToList();

            return true;
        }
        
        [RelayCommand]
        public async Task SelectNamespace(K8Namespace ns)
        {
            _kubService.SelectedContext = ns.Context;
            _kubService.SelectedNamespace = ns.Name;

            _navService.Navigate(typeof(Views.Pages.DataPage));

            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task SelectNamespaceDefault(K8Context context)
        {
            _kubService.SelectedContext = context.Name;
            _kubService.SelectedNamespace = context.DefaultNamespace;

            _navService.Navigate(typeof(Views.Pages.DataPage));

            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task LoadContextDetails(K8Context context)
        {
            if (context.Namespaces.Any())
            {
                return;
            }

            context.IsLoaded = false;

            try
            {
                context.Errors.Clear();
                context.Namespaces = await _kubService.GetNamespaces(context.Name, cts?.Token);
                context.IsLoaded = true;
            }
            catch (Exception ex)
            {
                context.Errors.Add(ex.Message ?? "Error");
            }
            finally
            {
                context.IsLoaded = true;
            }
        }
    }
}
