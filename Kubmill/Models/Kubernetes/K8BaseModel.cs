using k8s;

namespace Kubmill.Models.Kubernetes
{
    public class K8BaseModel
    {
        public string Data { get; set; }

        public K8BaseModel(object data)
        {
            Data = KubernetesYaml.Serialize(data);
        }
    }
}
