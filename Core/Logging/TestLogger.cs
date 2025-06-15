using atf.Core.Config;
using Serilog;
using Xunit.Abstractions;

namespace atf.Core.Logging
{
    /// <summary>
    /// Thread-safe instance-based logger for test cases with file and console output support.
    /// Provides consistent logging configuration across all test types.
    /// </summary>
    public class TestLogger : IDisposable
    {
        private readonly ILogger _logger;
        private bool _disposed = false;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _browserType;

        /// <summary>
        /// Creates a new TestLogger instance with test output and optional file logging.
        /// </summary>
        /// <param name="testOutputHelper">XUnit test output helper</param>
        /// <param name="testName">Name of the test for context</param>
        /// <param name="writeToFile">Whether to write logs to file (default: true)</param>
        /// <param name="browserType">Browser type for UI tests (optional)</param>
        public TestLogger(ITestOutputHelper testOutputHelper, string testName, bool writeToFile = true, string browserType = null)
        {
            _testOutputHelper = testOutputHelper;
            _browserType = browserType ?? "N/A";
            
            var outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{TestName}] [{Browser}] {Message:lj}{NewLine}{Exception}";
            
            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(ConfigManager.Configuration)
                .WriteTo.TestOutput(testOutputHelper, outputTemplate: outputTemplate)
                .Enrich.WithProperty("Application", "AutomationFramework")
                .Enrich.WithProperty("TestName", testName)
                .Enrich.WithProperty("TestContext", "XUnit")
                .Enrich.WithProperty("Browser", _browserType)
                .MinimumLevel.Debug();

            // Add file logging if requested
            if (writeToFile)
            {
                var logFileName = $"Logs/{testName}.log";
                loggerConfig = loggerConfig.WriteTo.File(logFileName, outputTemplate: outputTemplate);
            }

            _logger = loggerConfig.CreateLogger();
        }

        /// <summary>
        /// Creates a contextual logger with additional properties.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="value">Property value</param>
        /// <returns>Logger with additional context</returns>
        public ILogger ForContext(string propertyName, object value)
        {
            return _logger.ForContext(propertyName, value);
        }

        /// <summary>
        /// Gets the underlying Serilog logger instance.
        /// </summary>
        public ILogger Logger => _logger;

        /// <summary>
        /// Creates a contextual logger for a specific test method.
        /// </summary>
        /// <param name="testMethodName">Name of the test method</param>
        /// <param name="writeToFile">Whether to write logs to file for this test method</param>
        /// <returns>TestLogger configured for the specific test method</returns>
        public TestLogger ForTestMethod(string testMethodName, bool writeToFile = true)
        {
            var methodTestName = $"{testMethodName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
            return new TestLogger(_testOutputHelper, methodTestName, writeToFile, _browserType);
        }

        // Convenience methods for common logging levels
        public void Information(string messageTemplate, params object[] propertyValues)
            => _logger.Information(messageTemplate, propertyValues);

        public void Warning(string messageTemplate, params object[] propertyValues)
            => _logger.Warning(messageTemplate, propertyValues);

        public void Error(string messageTemplate, params object[] propertyValues)
            => _logger.Error(messageTemplate, propertyValues);

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
            => _logger.Error(exception, messageTemplate, propertyValues);

        public void Debug(string messageTemplate, params object[] propertyValues)
            => _logger.Debug(messageTemplate, propertyValues);

        public void Verbose(string messageTemplate, params object[] propertyValues)
            => _logger.Verbose(messageTemplate, propertyValues);

        public void Fatal(string messageTemplate, params object[] propertyValues)
            => _logger.Fatal(messageTemplate, propertyValues);

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
            => _logger.Fatal(exception, messageTemplate, propertyValues);

        public void Dispose()
        {
            if (!_disposed)
            {
                (_logger as IDisposable)?.Dispose();
                _disposed = true;
            }
        }
    }
}