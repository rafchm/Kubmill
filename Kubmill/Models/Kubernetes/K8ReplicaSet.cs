using k8s.Models;

namespace Kubmill.Models.Kubernetes
{
    public class K8ReplicaSet
    {
        public string Name { get; set; }
        public int Replicas { get; set; }
        public int? Ready { get; set; }

        public K8ReplicaSet(V1ReplicaSet d)
        {
            Name = d.Name();
            Replicas = d.Status.Replicas;
            Ready = d.Status.ReadyReplicas;
        }
    }
}
