using Kubmill.Models.Scripting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kubmill.Repositories
{
    /// <summary>
    /// Script repository interface with methods to load & access script data.
    /// </summary>
    public interface IScriptRepository
    {
        /// <summary>
        /// Loads all scripts.
        /// </summary>
        /// <returns></returns>
        Task LoadScripts();
        /// <summary>
        /// Gets all loaded scripts.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ScriptFile> GetAllScripts();
        /// <summary>
        /// Gets script for selected file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        ScriptFile? GetScript(string fileName);
        /// <summary>
        /// Gets scripts for selected context type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<ScriptFile> GetScripts(ScriptContextType type);
    }
}