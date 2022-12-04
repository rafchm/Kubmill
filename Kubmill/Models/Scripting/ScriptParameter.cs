using System;

namespace Kubmill.Models.Scripting
{
    /// <summary>
    /// Represents script parameter that is parsed from script file.
    /// </summary>
    public class ScriptParameter
    {
        public string Name { get; set; }
        public Type ParamType { get; set; }
        public bool IsMandatory { get; set; }

        public ScriptParameter(string name, Type paramType, bool isMandatory)
        {
            Name = name;
            ParamType = paramType;
            IsMandatory = isMandatory;
        }
    }
}
