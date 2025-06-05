using Microsoft.Extensions.Configuration;

namespace atf.Core.Config
{
    /// <summary>
    /// Interface for accessing application configuration settings.
    /// Enables dependency injection and easier testing with mock configurations.
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// Exposes the raw configuration root for advanced scenarios (Serilog, DI, etc).
        /// </summary>
        IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Gets a configuration value by key with type conversion.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to</typeparam>
        /// <param name="key">The configuration key</param>
        /// <returns>The configuration value converted to type T</returns>
        T Get<T>(string key);

        /// <summary>
        /// Gets a configuration section bound to a strongly-typed object.
        /// </summary>
        /// <typeparam name="T">The type to bind the section to</typeparam>
        /// <param name="key">The section key</param>
        /// <returns>The configuration section bound to type T</returns>
        T GetSection<T>(string key) where T : new();
    }
}