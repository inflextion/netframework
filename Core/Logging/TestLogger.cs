using atf.Core.Config;
using Serilog;
using Xunit.Abstractions;

namespace atf.Core.Logging
{
    /// <summary>
    /// Simplified test logger that always writes to both console and file
    /// </summary>
    public class TestLogger : IDisposable
    {
        private readonly ILogger _logger;
        private bool _disposed = false;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _testClassName;

        /// <summary>
        /// Creates a TestLogger that writes to both console and class-level file
        /// </summary>
        public TestLogger(ITestOutputHelper testOutputHelper, string testClassName, string browserType = null)
        {
            _testOutputHelper = testOutputHelper;
            _testClassName = testClassName;
            
            var outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{TestClass}] [{Browser}] [{TestMethod}] [{Step}] {Message:lj}{NewLine}{Exception}";
            
            // Create logs directory structure
            var logDirectory = Path.Combine("Logs", testClassName);
            Directory.CreateDirectory(logDirectory);
            
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var logFileName = Path.Combine(logDirectory, $"{testClassName}_{timestamp}.log");
            
            _logger = new LoggerConfiguration()
                .ReadFrom.Configuration(ConfigManager.Configuration)
                .WriteTo.TestOutput(testOutputHelper, outputTemplate: outputTemplate)
                .WriteTo.File(logFileName, 
                    outputTemplate: outputTemplate,
                    shared: true,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 50 * 1024 * 1024) // 50MB limit
                .Enrich.WithProperty("Application", "AutomationFramework")
                .Enrich.WithProperty("TestClass", testClassName)
                .Enrich.WithProperty("TestContext", "XUnit")
                .Enrich.WithProperty("Browser", browserType ?? "N/A")
                .Enrich.WithProperty("TestMethod", "ClassSetup")
                .Enrich.WithProperty("Step", "Initial")
                .MinimumLevel.Debug()
                .CreateLogger();
        }

        /// <summary>
        /// Private constructor for contextual loggers
        /// </summary>
        private TestLogger(ITestOutputHelper testOutputHelper, string testClassName, ILogger contextLogger)
        {
            _testOutputHelper = testOutputHelper;
            _testClassName = testClassName;
            _logger = contextLogger;
        }

        /// <summary>
        /// Creates context for a specific test method with visual separation
        /// </summary>
        public TestLogger ForTestMethod(string testMethodName)
        {
            // Add visual separation before starting new test
            _logger.Information("");
            _logger.Information("═══════════════════════════════════════════════════════════════");
            _logger.Information("🧪 STARTING TEST: {TestMethod}", testMethodName);
            _logger.Information("🕒 Test Started At: {StartTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            _logger.Information("═══════════════════════════════════════════════════════════════");
            _logger.Information("");

            var contextLogger = _logger.ForContext("TestMethod", testMethodName);
            return new TestLogger(_testOutputHelper, _testClassName, contextLogger);
        }

        /// <summary>
        /// Creates context for a specific test step
        /// </summary>
        public TestLogger ForStep(string stepName)
        {
            var contextLogger = _logger.ForContext("Step", stepName);
            return new TestLogger(_testOutputHelper, _testClassName, contextLogger);
        }

        /// <summary>
        /// Creates context with additional properties
        /// </summary>
        public TestLogger ForContext(string propertyName, object value)
        {
            var contextLogger = _logger.ForContext(propertyName, value);
            return new TestLogger(_testOutputHelper, _testClassName, contextLogger);
        }

        /// <summary>
        /// Updates browser context
        /// </summary>
        public TestLogger ForBrowser(string browserType)
        {
            return ForContext("Browser", browserType);
        }

        /// <summary>
        /// Marks the end of a test method with visual separation
        /// </summary>
        public void EndTestMethod(string testMethodName, bool passed = true)
        {
            _logger.Information("");
            _logger.Information("───────────────────────────────────────────────────────────────");
            _logger.Information("🏁 TEST COMPLETED: {TestMethod}", testMethodName);
            _logger.Information("📊 Test Result: {Result}", passed ? "✅ PASSED" : "❌ FAILED");
            _logger.Information("🕒 Test Ended At: {EndTime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            _logger.Information("───────────────────────────────────────────────────────────────");
            _logger.Information("");
            _logger.Information("");
        }

        // Logging methods
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

        public void Fatal(string messageTemplate, params object[] propertyValues)
            => _logger.Fatal(messageTemplate, propertyValues);

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