namespace Kubmill.Models.Scripting
{
    /// <summary>
    /// Defines script context type, parsed from script comment section.
    /// </summary>
    public enum ScriptContextType
    {
        Unknown,
        Cluster,
        Namespace,
        Pod
    }
}
