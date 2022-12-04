using k8s.Models;
using System.Collections.Generic;

namespace Kubmill.Models.Kubernetes
{
    public partial class K8Namespace
    {
        public string? Context { get; }
        public string? Name { get; }
        public ICollection<K8Pod>? Pods { get; }

        public bool IsSystem => Name?.StartsWith("kube-") ?? false;

        public K8Namespace(string context, V1Namespace ns)
        {
            Context = context;
            Name = ns.Metadata.Name;
        }
    }
}
