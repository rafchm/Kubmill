using Kubmill.Models.Kubernetes;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kubmill.Services
{
    /// <summary>
    /// Kubernetes service interface.
    /// </summary>
    public interface IKubernetesService
    {
        /// <summary>
        /// Selected context.
        /// </summary>
        string? SelectedContext { get; set; }
        /// <summary>
        /// Selected namespace.
        /// </summary>
        string? SelectedNamespace { get; set; }
        /// <summary>
        /// Loads configuration from kube config file.
        /// </summary>
        /// <returns></returns>
        K8Config LoadConfiguration();
        /// <summary>
        /// Gets namespaces for selected context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<K8Namespace>> GetNamespaces(string context, CancellationToken? ct);
        /// <summary>
        /// Gets pods for selected context and namespace.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ns"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<K8Pod>> GetPods(string context, string ns, CancellationToken? ct);
        /// <summary>
        /// Gets events for selected context and namespace.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ns"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<K8Event>> GetEvents(string context, string ns, CancellationToken? ct);
        /// <summary>
        /// Gets deployments for selected context and namespace.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ns"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<K8Deployment>> GetDeployments(string context, string ns, CancellationToken? ct);
        /// <summary>
        /// Gets replica sets for selected context and namespace.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ns"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IEnumerable<K8ReplicaSet>> GetReplicaSets(string context, string ns, CancellationToken? ct);
    }
}