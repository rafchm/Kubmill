using k8s.Models;
using Kubmill.Models.Kubernetes.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Kubmill.Models.Kubernetes
{
    public class K8Pod
    {
        public string? Name { get; }
        public string? Namespace { get; }
        public string? Context { get; }
        public string? RestartState { get; }
        public string? StatusPhase { get; }
        public string? StatusReady { get; }
        public  bool IsReady { get; }
        public string? Age { get; }
        public string? Uid { get; }

        public IEnumerable<K8Container>? Containers { get; set; }

        public K8Pod(string context, V1Pod pod)
        {
            Context = context;
            Namespace = pod.Namespace();
            Name = pod.Metadata.Name;
            Uid = pod.Metadata.Uid;
            Age = pod.GetAge();
            RestartState = pod.GetRestartStatus();
            StatusReady = pod.GetReadyStatus();
            IsReady = pod.GetIsReady();
            StatusPhase = pod.GetStatus();
            Containers = pod.Spec.Containers.Select(c => new K8Container(c));
        }
    }
}
