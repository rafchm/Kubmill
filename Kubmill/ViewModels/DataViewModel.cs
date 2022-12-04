using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kubmill.Models.Kubernetes;
using Kubmill.Models.Scripting;
using Kubmill.Repositories;
using Kubmill.Services;
using Kubmill.Views.Windows;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;

namespace Kubmill.ViewModels
{
    public partial class DataViewModel : PageViewModelBase
    {
        private readonly IKubernetesService _kubService;
        private readonly IScriptService _scriptService;
        private readonly IScriptRepository _scriptRepository;
        private readonly ISnackbarService _snackbarService;
        private readonly IWindowService _winService;

        [ObservableProperty]
        private string? _context;

        [ObservableProperty]
        private string? _namespace;

        [ObservableProperty]
        private IEnumerable<K8Pod>? _pods;

        [ObservableProperty]
        private K8Pod? _selectedPod;

        [ObservableProperty]
        private IEnumerable<ScriptFile>? _scripts;

        public DataViewModel(IKubernetesService kubService, IScriptService scriptService,
            IWindowService winService, IScriptRepository scriptRepository, ISnackbarService snackbarService)
            : base()
        {
            _kubService = kubService;
            _scriptService = scriptService;
            _scriptRepository = scriptRepository;
            _snackbarService = snackbarService;
            _winService = winService;
        }

        public override bool OnLoadPage()
        {
            Context = _kubService.SelectedContext;
            Namespace = _kubService.SelectedNamespace;

            if (RefreshCommand.CanExecute(null))
            {
                RefreshCommand.Execute(null);
            }

            return false;
        }

        partial void OnSelectedPodChanged(K8Pod? value)
        {
            Scripts = value != null
                ? _scriptRepository.GetScripts(ScriptContextType.Pod)
                : null;
        }

        private async Task GetPods()
        {
            var ctx = _kubService.SelectedContext;
            var ns = _kubService.SelectedNamespace;

            if (ctx == null || ns == null) return;

            Pods = await _kubService.GetPods(ctx, ns, cts?.Token);
        }

        [RelayCommand]
        public async Task Refresh()
        {
            try
            {
                IsBusy = true;
                await GetPods();
            }
            catch (Exception ex)
            {
                _snackbarService.Show("Error", ex.Message, SymbolRegular.ErrorCircle24);
            }
            finally
            {
                IsBusy = false;
            }            
        }

        [RelayCommand]
        public async Task RunScript(ScriptFile script)
        {
            if (SelectedPod == null) return;

            var win = _winService.Get<ScriptOutputWindow>();

            win.Show();

            var result = await _scriptService.RunScript(script.FileName, SelectedPod, progress => win.RecordEvent(progress));

            win.AddResult(result);
        }
    }
}
