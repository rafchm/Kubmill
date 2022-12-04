using k8s;
using k8s.Models;
using Kubmill.Configuration;
using Kubmill.Models.Kubernetes;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubmill.Services
{
    /// <inheritdoc/>
    public class KubernetesService : IKubernetesService
    {
        /// <inheritdoc/>
        public string? SelectedContext { get; set; }
        /// <inheritdoc/>
        public string? SelectedNamespace { get; set; }

        private readonly ConcurrentDictionary<string, Kubernetes> _clients = new();
        private readonly IOptions<ConfigOptions> _options;

        public KubernetesService(IOptions<ConfigOptions> options)
        {
            _options = options;
        }

        /// <inheritdoc/>
        public K8Config LoadConfiguration()
        {
            string? kubeconfigPath = _options.Value.KubeConfigPath;

            var configFile = new FileInfo(string.IsNullOrEmpty(kubeconfigPath) 
                ? KubernetesClientConfiguration.KubeConfigDefaultLocation
                : kubeconfigPath);

            var config = KubernetesClientConfiguration.LoadKubeConfig(configFile, false);

            SelectedContext = config.CurrentContext;
            SelectedNamespace = config.Contexts.FirstOrDefault(c => c.Name == config.CurrentContext)?.ContextDetails.Namespace;

            return new K8Config(config);
        }

        private Kubernetes GetClient(string context)
        {
            if (!_clients.ContainsKey(context))
            {
                // Load from the default kubeconfig on the machine.
                var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(currentContext: context);

                config.HttpClientTimeout = TimeSpan.FromSeconds(15);

                // Use the config object to create a client.
                var client = new Kubernetes(config);

                _clients.TryAdd(context, client);
            }

            return _clients[context];
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<K8Namespace>> GetNamespaces(string context, CancellationToken? ct)
        {
            var namespaces = await GetClient(context).ListNamespaceAsync(timeoutSeconds: 10, watch: false, cancellationToken: ct ?? default);

            return namespaces.Items.Select(ns => new K8Namespace(context, ns)).Where(ns => _options.Value.ShowSystemNamespaces || !ns.IsSystem);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<K8Pod>> GetPods(string context, string ns, CancellationToken? ct)
        {
            var client = GetClient(context);

            using var data = await client.CoreV1
                .ListNamespacedPodWithHttpMessagesAsync(ns, timeoutSeconds: 10, watch: false, cancellationToken: ct ?? default);

            return data.Body.Items.Select(pod => new K8Pod(context, pod));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<K8Event>> GetEvents(string context, string ns, CancellationToken? ct)
        {
            var client = GetClient(context);
            var data = await client.CoreV1.ListNamespacedEventAsync(ns, timeoutSeconds: 10, watch: false, cancellationToken: ct ?? default);

            return data.Items.Select(d => new K8Event(d));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<K8Deployment>> GetDeployments(string context, string ns, CancellationToken? ct)
        {
            var client = GetClient(context);
            var data = await client.ListNamespacedDeploymentAsync(ns, timeoutSeconds: 10, watch: false, cancellationToken: ct ?? default);

            return data.Items.Select(d => new K8Deployment(d));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<K8ReplicaSet>> GetReplicaSets(string context, string ns, CancellationToken? ct)
        {
            var client = GetClient(context);
            var data = await client.ListNamespacedReplicaSetAsync(ns, timeoutSeconds: 10, watch: false, cancellationToken: ct ?? default);

            return data.Items.Select(d => new K8ReplicaSet(d));
        }
    }
}
