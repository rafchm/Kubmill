using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kubmill.Models.Kubernetes;
using Kubmill.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;

namespace Kubmill.ViewModels
{
    public partial class DashboardViewModel : PageViewModelBase
    {
        private readonly IKubernetesService _kubService;
        private readonly ISnackbarService _snackbarService;

        [ObservableProperty]
        private string? _context;

        [ObservableProperty]
        private string? _namespace;

        [ObservableProperty]
        private IEnumerable<K8Event>? _events;

        [ObservableProperty]
        private bool _errorsOnly;

        [ObservableProperty]
        private int _deploymentCurrent;

        [ObservableProperty]
        private int _deploymentTotal;

        [ObservableProperty]
        private int _podCurrent;

        [ObservableProperty]
        private int _podTotal;

        [ObservableProperty]
        private int _replicaSetCurrent;

        [ObservableProperty]
        private int _replicaSetTotal;

        public DashboardViewModel(IKubernetesService kubService, ISnackbarService snackbarService)
        {
            _kubService = kubService;
            _snackbarService = snackbarService;
        }

        public override bool OnLoadPage()
        {
            Context = _kubService.SelectedContext;
            Namespace = _kubService.SelectedNamespace;

            DeploymentCurrent = DeploymentTotal =
            PodCurrent = PodTotal =
            ReplicaSetCurrent = ReplicaSetTotal = 0;

            if (RefreshCommand.CanExecute(null))
            {
                RefreshCommand.Execute(null);
            }

            return false;
        }

        [RelayCommand]
        public async Task Refresh()
        {
            try
            {
                IsBusy = true;
                await GetData();
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

        private async Task GetData()
        {
            var ctx = _kubService.SelectedContext;
            var ns = _kubService.SelectedNamespace;

            if (ctx == null || ns == null) return;

            await GetEvents(ctx, ns);
            await GetDeploymentStats(ctx, ns);
            await GetPodStats(ctx, ns);
            await GetReplicaSetStats(ctx, ns);
        }

        private async Task GetEvents(string ctx, string ns)
        {
            var events = await _kubService.GetEvents(ctx, ns, cts?.Token);
            Events = events
                .Where(e => !ErrorsOnly || e.IsError)
                .OrderByDescending(e => e.Timestamp);
        }

        private async Task GetDeploymentStats(string ctx, string ns)
        {
            var deployments = await _kubService.GetDeployments(ctx, ns, cts?.Token);

            DeploymentCurrent = deployments.Count(d => d.Ready == d.Desired);
            DeploymentTotal = deployments.Count();
        }

        private async Task GetPodStats(string ctx, string ns)
        {
            var pods = await _kubService.GetPods(ctx, ns, cts?.Token);

            PodCurrent = pods.Count(p => p.IsReady);
            PodTotal = pods.Count();
        }

        private async Task GetReplicaSetStats(string ctx, string ns)
        {
            var replicas = await _kubService.GetReplicaSets(ctx, ns, cts?.Token);

            ReplicaSetCurrent = replicas.Count(r => (r.Ready ?? 0) == r.Replicas);
            ReplicaSetTotal = replicas.Count();
        }
    }
}
