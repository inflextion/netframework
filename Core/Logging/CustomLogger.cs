using atf.Core.Config;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using Xunit.Abstractions;

namespace atf.Core.Logging
{
    /// <summary>
    /// XUnit-compatible logger that can write to both test output and regular sinks.
    /// </summary>
    public static class CustomLogger
    {
        private static ILogger? _baseLogger;
        private static ILogger? _currentLogger;

        static CustomLogger()
        {
            InitializeBaseLogger();
            _currentLogger = _baseLogger;
        }

        public static ILogger ForContext(string propertyName, object value)
        {
            return _currentLogger?.ForContext(propertyName, value)
                ?? throw new InvalidOperationException("Logger is not initialized.");
        }

        private static void InitializeBaseLogger()
        {
            try
            {
                var configuration = ConfigManager.Configuration;

                _baseLogger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .Enrich.WithProperty("Application", "AutomationFramework")
                    .CreateLogger();
            }
            catch (Exception ex)
            {
                // Fallback logger if configuration fails
                _baseLogger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .WriteTo.File("logs/test-fallback-.txt", rollingInterval: RollingInterval.Day)
                    .MinimumLevel.Information()
                    .CreateLogger();

                _baseLogger.Warning("Failed to load logger configuration, using fallback: {Exception}", ex.Message);
            }
        }

        /// <summary>
        /// Configures the logger to also write to XUnit test output.
        /// Call this in your test constructor or setup method.
        /// </summary>
        /// <param name="testOutputHelper">The XUnit test output helper</param>
        public static void ConfigureForTest(ITestOutputHelper testOutputHelper, string testName = null)
        {
            if (testOutputHelper == null) return;
            var configuration = ConfigManager.Configuration;

            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.TestOutput(testOutputHelper, outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] (({TestContext}) ({TestName}) ({Application}) {Message:lj}{NewLine}{Exception}")
                .Enrich.WithProperty("Application", "AutomationFramework")
                .Enrich.WithProperty("TestName", testName)
                .Enrich.WithProperty("TestContext", "XUnit");

            // Add TestName if provided
            if (!string.IsNullOrEmpty(testName))
            {
                loggerConfig = loggerConfig.Enrich.WithProperty("TestName", testName);
            }

            _currentLogger = loggerConfig.CreateLogger();
        }

        /// <summary>
        /// Resets the logger to base configuration (call after tests complete).
        /// </summary>
        public static void ResetToBaseConfiguration()
        {
            _currentLogger = _baseLogger;
        }

        public static void Information(string messageTemplate, params object[] propertyValues)
            => _currentLogger.Information(messageTemplate, propertyValues);
        public static void Warning(string messageTemplate, params object[] propertyValues)
            => _currentLogger.Warning(messageTemplate, propertyValues);

        public static void Error(string messageTemplate, params object[] propertyValues)
            => _currentLogger.Error(messageTemplate, propertyValues);

        public static void Error(Exception exception, string messageTemplate, params object[] propertyValues)
            => _currentLogger.Error(exception, messageTemplate, propertyValues);

        public static void Debug(string messageTemplate, params object[] propertyValues)
            => _currentLogger.Debug(messageTemplate, propertyValues);

        public static void Verbose(string messageTemplate, params object[] propertyValues)
            => _currentLogger.Verbose(messageTemplate, propertyValues);

        public static void Fatal(string messageTemplate, params object[] propertyValues)
            => _currentLogger.Fatal(messageTemplate, propertyValues);

        public static void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
            => _currentLogger.Fatal(exception, messageTemplate, propertyValues);

        public static void CloseAndFlush()
        {
            Log.CloseAndFlush();
        }
    }
}