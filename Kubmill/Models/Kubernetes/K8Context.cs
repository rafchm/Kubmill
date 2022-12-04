using CommunityToolkit.Mvvm.ComponentModel;
using k8s.KubeConfigModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kubmill.Models.Kubernetes
{
    public partial class K8Context : ObservableObject
    {
        public string Name { get; }
        public string UserName { get; }
        public string DefaultNamespace { get; }
        public string ClusterName { get; }

        [ObservableProperty]
        private bool _isLoaded;

        [ObservableProperty]
        private IEnumerable<K8Namespace> _namespaces = new List<K8Namespace>();

        [ObservableProperty]
        private ObservableCollection<string> _errors = new();

        public K8Context(Context context, string clusterName)
        {
            Name = context.Name;
            DefaultNamespace = context.ContextDetails.Namespace;
            UserName = context.ContextDetails.User;
            ClusterName = clusterName;
        }
    }
}
