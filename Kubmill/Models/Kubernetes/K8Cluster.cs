using CommunityToolkit.Mvvm.ComponentModel;
using k8s.KubeConfigModels;

namespace Kubmill.Models.Kubernetes
{
    public partial class K8Cluster : ObservableObject
    {
        public string? Name { get; }
        public string? Server { get; }

        public K8Cluster(Cluster cluster)
        {
            Name = cluster.Name;
            Server = cluster.ClusterEndpoint.Server;
        }
    }
}
