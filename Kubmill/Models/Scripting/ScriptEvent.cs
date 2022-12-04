using System.Management.Automation;

namespace Kubmill.Models.Scripting
{
    /// <summary>
    /// Represents script event triggered from running script.
    /// </summary>
    public class ScriptEvent
    {
        public ScriptEventType EventType { get; set; }
        public string Message { get; set; } = "";
        public int Progress { get; set; }

        public ScriptEvent(PSDataCollection<ProgressRecord>? r, DataAddedEventArgs e)
        {
            EventType = ScriptEventType.Progress;

            if (r != null)
            {
                var p = r[e.Index];

                Message = $"{p.Activity}.{p.StatusDescription}: {p.PercentComplete}%";
                Progress = p.PercentComplete;
            }
        }

        public ScriptEvent(PSDataCollection<DebugRecord>? r, DataAddedEventArgs e)
        {
            EventType = ScriptEventType.Debug;

            if (r != null)
            {
                Message = r[e.Index]?.Message ?? "";
            }
        }

        public ScriptEvent(PSDataCollection<InformationRecord>? r, DataAddedEventArgs e)
        {
            EventType = ScriptEventType.Information;

            if (r != null)
            {
                Message = r[e.Index]?.MessageData?.ToString() ?? "";
            }
        }

        public ScriptEvent(PSDataCollection<WarningRecord>? r, DataAddedEventArgs e)
        {
            EventType = ScriptEventType.Warning;

            if (r != null)
            {
                Message = r[e.Index]?.Message ?? "";
            }
        }

        public ScriptEvent(PSDataCollection<ErrorRecord>? r, DataAddedEventArgs e)
        {
            EventType = ScriptEventType.Error;

            if (r != null)
            {
                var err = r[e.Index];

                Message = err?.ErrorDetails?.Message ?? err?.Exception?.Message ?? "Error";
            }
        }
    }
}
