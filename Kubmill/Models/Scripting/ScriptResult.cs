using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Kubmill.Models.Scripting
{
    /// <summary>
    /// Represents script result.
    /// </summary>
    public class ScriptResult
    {
        public IEnumerable<ErrorRecord>? Errors { get; set; }
        public IEnumerable<string>? Output { get; set; }

        public ScriptResult()
        {
        }

        public ScriptResult(string error)
        {
            Errors = new List<ErrorRecord>
            {
                new ErrorRecord(new Exception(error), "0", ErrorCategory.InvalidArgument, null)
            };
        }
    }
}
