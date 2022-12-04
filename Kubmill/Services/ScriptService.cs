using Kubmill.Models.Kubernetes;
using Kubmill.Models.Scripting;
using Kubmill.Repositories;
using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace Kubmill.Services
{
    public class ScriptService : IScriptService
    {
        private readonly IScriptRepository _scriptRepository;

        public ScriptService(IScriptRepository scriptRepository)
        {
            _scriptRepository = scriptRepository;
        }

        public async Task<ScriptResult> RunScript(string scriptFileName, K8Pod pod, Action<ScriptEvent> onEvent)
        {
            var script = _scriptRepository.GetScript(scriptFileName);

            if (script == null)
            {
                return await Task.FromResult(new ScriptResult("Script file not found!"));
            }

            using var ps = PowerShell.Create().AddScript(script.Content);

            AddParameters(ps, pod, script);

            ps.Streams.Debug.DataAdded += (s, e) =>
            {
                onEvent(new ScriptEvent(s as PSDataCollection<DebugRecord>, e));
            };
            ps.Streams.Information.DataAdded += (s, e) =>
            {
                onEvent(new ScriptEvent(s as PSDataCollection<InformationRecord>, e));
            };
            ps.Streams.Progress.DataAdded += (s, e) =>
            {
                onEvent(new ScriptEvent(s as PSDataCollection<ProgressRecord>, e));
            };
            ps.Streams.Warning.DataAdded += (s, e) =>
            {
                onEvent(new ScriptEvent(s as PSDataCollection<WarningRecord>, e));
            };
            ps.Streams.Error.DataAdded += (s, e) =>
            {
                onEvent(new ScriptEvent(s as PSDataCollection<ErrorRecord>, e));
            };

            var res = await ps.InvokeAsync();

            return new ScriptResult
            {
                Errors = ps.Streams.Error,
                Output = res.Select(r => r.ToString())
            };
        }

        private void AddParameters(PowerShell ps, K8Pod pod, ScriptFile script)
        {
            foreach (var par in script.Parameters)
            {
                var paramName = par.Name.TrimStart('$');
                var paramValue = FindParameterValue(paramName, pod);

                if (paramValue != null) ps.AddParameter(paramName, paramValue);
            }
        }

        private static string? FindParameterValue(string paramName, K8Pod pod)
        {
            switch (paramName)
            {
                case "context": return pod.Context;
                case "namespace": return pod.Namespace;
                case "podname": return pod.Name;
            }

            if (paramName.StartsWith("env_"))
            {
                // search in pod's env vars
                return pod.Containers?
                    .SelectMany(p => p.Env ?? new())
                    .FirstOrDefault(e => e.Key == paramName[4..])
                    .Value;
            }

            return null;
        }
    }
}
