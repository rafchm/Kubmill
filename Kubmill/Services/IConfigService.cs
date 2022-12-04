using Kubmill.Configuration;

namespace Kubmill.Services
{
    /// <summary>
    /// Configuratio service interface.
    /// </summary>
    public interface IConfigService
    {
        /// <summary>
        /// Gets all application options.
        /// </summary>
        /// <returns></returns>
        AppOptions GetAppOptions();
        /// <summary>
        /// Saves all application options
        /// </summary>
        /// <param name="options"></param>
        void SaveOptions(AppOptions options);
    }
}