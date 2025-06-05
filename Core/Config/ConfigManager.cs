using Microsoft.Extensions.Configuration;

namespace atf.Core.Config
{
    /// <summary>
    /// Provides access to application configuration settings.
    /// </summary>
    public static class ConfigManager
    {
        private static readonly IConfigurationRoot _configuration;

        static ConfigManager() => _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

        /// <summary>
        /// Exposes the raw configuration root for advanced scenarios (Serilog, DI, etc).
        /// </summary>
        public static IConfigurationRoot Configuration => _configuration;
        public static T Get<T>(string key) => _configuration.GetValue<T>(key);

        public static T GetSection<T>(string key) where T : new() => _configuration.GetSection(key).Get<T>();

        /// <summary>
        /// Gets the default instance for dependency injection scenarios.
        /// </summary>
        public static IConfigManager Instance => new ConfigManagerImplementation();
    }

    /// <summary>
    /// Implementation of IConfigManager that delegates to the static ConfigManager.
    /// </summary>
    internal class ConfigManagerImplementation : IConfigManager
    {
        public IConfigurationRoot Configuration => ConfigManager.Configuration;

        public T Get<T>(string key) => ConfigManager.Get<T>(key);

        public T GetSection<T>(string key) where T : new() => ConfigManager.GetSection<T>(key);
    }
}

