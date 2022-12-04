using k8s.KubeConfigModels;
using System.Collections.Generic;
using System.Linq;

namespace Kubmill.Models.Kubernetes
{
    public class K8Config
    {
        public IEnumerable<K8Cluster> Clusters { get; }
        public IEnumerable<K8Context> Contexts { get; }

        public K8Config(K8SConfiguration config)
        {
            Clusters = config.Clusters.Select(c => new K8Cluster(c));
            Contexts = config.Contexts.Select(c =>
            {
                var cluster = Clusters.FirstOrDefault(cl => cl.Name == c.ContextDetails.Cluster);
                return new K8Context(c, cluster?.Name ?? "Unknown");
            });
        }
    }
}
