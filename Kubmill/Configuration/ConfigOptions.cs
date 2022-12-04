using Kubmill.Helpers;
using System;

namespace Kubmill.Configuration
{
    public class ConfigOptions : IEquatable<ConfigOptions?>
    {
        public string? KubeConfigPath { get; set; }
        public bool ShowSystemNamespaces { get; set; }

        public static ConfigOptions Clone(ConfigOptions options)
        {
            return new ConfigOptions
            {
                KubeConfigPath = options.KubeConfigPath,
                ShowSystemNamespaces = options.ShowSystemNamespaces
            };
        }

        public bool Equals(ConfigOptions? other)
        {
            return other is not null &&
                PathComparer.Equals(KubeConfigPath, other.KubeConfigPath) &&
                ShowSystemNamespaces == other.ShowSystemNamespaces;
        }
    }
}
