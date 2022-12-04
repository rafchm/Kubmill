using k8s.Models;
using System.Collections.Generic;
using System.Linq;

namespace Kubmill.Models.Kubernetes
{
    public class K8Container : K8BaseModel
    {
        public string? Name { get; }
        public Dictionary<string,string>? Env { get; }

        public K8Container(V1Container c) : base(c)
        {
            Name = c.Name;
            Env = c.Env?.ToDictionary(e => e.Name, e => e.Value);
        }
    }
}
