using Kubmill.Models.Kubernetes;
using Kubmill.Models.Scripting;
using System;
using System.Threading.Tasks;

namespace Kubmill.Services
{
    /// <summary>
    /// Script service interface.
    /// </summary>
    public interface IScriptService
    {
        /// <summary>
        /// Runs selected script with selected pod context.
        /// </summary>
        /// <param name="scriptFileName"></param>
        /// <param name="pod"></param>
        /// <param name="onEvent"></param>
        /// <returns></returns>
        Task<ScriptResult> RunScript(string scriptFileName, K8Pod pod, Action<ScriptEvent> onEvent);
    }
}