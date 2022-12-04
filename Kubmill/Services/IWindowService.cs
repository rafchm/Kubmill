using System.Windows;

namespace Kubmill.Services
{
    /// <summary>
    /// Window service interface.
    /// </summary>
    public interface IWindowService
    {
        /// <summary>
        /// Displays window with specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Show<T>() where T : Window;
        /// <summary>
        /// Gets window with specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Get<T>() where T : Window;
    }
}