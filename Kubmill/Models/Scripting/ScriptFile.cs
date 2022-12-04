using System.Collections.Generic;

namespace Kubmill.Models.Scripting
{
    /// <summary>
    /// Represents script file metadata and content.
    /// </summary>
    public class ScriptFile
    {
        public string Name { get; set; }
        public string FileName { get; }
        public string? Description { get; set; }
        public ScriptContextType ContextType { get; set; }
        public IEnumerable<ScriptParameter> Parameters { get; set; }
        public IList<string> Errors { get; set; }
        public bool IsDebugMode { get; set; }

        public string? Content { get; set; }

        public ScriptFile(string name)
        {
            Name = FileName = name;
            Parameters = new List<ScriptParameter>();
            Errors = new List<string>();
        }
    }
}
