using k8s.Models;

namespace Kubmill.Models.Kubernetes
{
    public class K8Deployment
    {
        public string Namespace { get; }
        public string Name { get; }
        public int? Ready { get; }
        public int? Desired { get; }
        public int? Unavailable { get; }

        public K8Deployment(V1Deployment d)
        {
            Namespace = d.Namespace();
            Name = d.Name();
            Ready = d.Status.ReadyReplicas;
            Desired = d.Status.Replicas;
            Unavailable = d.Status.UnavailableReplicas;
        }
    }
}
