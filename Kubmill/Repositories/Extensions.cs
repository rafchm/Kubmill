using System.Linq;
using System.Management.Automation.Language;

namespace Kubmill.Repositories
{
    /// <summary>
    /// Automation extensions.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Finding if script parameter is mandatory or not.
        /// </summary>
        /// <param name="parameter">script parameter</param>
        /// <returns><c>true</c> if is mandatory, <c>false</c> otherwise</returns>
        internal static bool IsMandatory(this ParameterAst parameter)
        {
            return parameter.Attributes.Any(pa => pa.Find(pa => pa.Extent.ToString() == "Mandatory=$true", false) != null);
        }
    }
}
