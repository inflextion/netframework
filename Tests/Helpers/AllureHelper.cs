using Allure.Net.Commons;
using System.Text;
using atf.Core.Logging;
using Newtonsoft.Json.Linq;

namespace atf.Tests.Helpers
{
    /// <summary>
    /// Provides helper methods for attaching information and screenshots to Allure reports.
    /// </summary>
    public static class AllureHelper
    {
        /// <summary>
        /// Attaches a string as an attachment to the Allure report.
        /// </summary>
        /// <param name="name">The name of the attachment.</param>
        /// <param name="content">The content to attach.</param>
        /// <param name="mimeType">The MIME type of the attachment (default is "text/plain").</param>
        /// <param name="ext">The file extension of the attachment (default is ".txt").</param>
        public static void AttachString(string name, string content, string mimeType = "text/plain", string ext = ".txt")
        {
            AllureApi.Step($"Attach: {name}", () =>
            {
                AllureApi.AddAttachment(name, mimeType, Encoding.UTF8.GetBytes(content), ext);
            });
        }

        /// <summary>
        /// Serializes a T class and attaches it as a JSON body to the Allure report.
        /// </summary>
        /// <param name="name">The name of the attachment.</param>
        /// <param name="request">The T object to serialize and attach.</param>
        public static void AttachString<T>(string name, T request) where T: class
        {
            var jsonContent = JsonHelper.Serialize(request, indented: true);
            AllureApi.Step($"Attach: {name}", () =>
            {
                AllureApi.AddAttachment("Request Body", "application/json", Encoding.UTF8.GetBytes(jsonContent));
            });
        }

        /// <summary>
        /// Attaches a screenshot (PNG format) to the Allure report.
        /// </summary>
        /// <param name="name">The name of the screenshot attachment.</param>
        /// <param name="screenshot">The screenshot data as a byte array.</param>
        public static void AttachScreenshot(string name, byte[] screenshot)
        {
            AllureApi.Step($"Attach screenshot: {name}", () =>
            {
                AllureApi.AddAttachment(name, "image/png", screenshot, ".png");
            });
        }

        /// <summary>
        /// Writes Allure environment properties from a configuration file to the Allure results directory.
        /// Logs output using the provided TestLogger.
        /// </summary>
        /// <param name="logger">The TestLogger instance for logging output.</param>
        public static void WriteAllureEnvironmentProperties(TestLogger logger)
        {
            var configPath = "allureConfig.json";
            var outputPath = Path.Combine("allure-results", "environment.properties");

            string configText;
            try
            {
                configText = File.ReadAllText(configPath);
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to read config file '{configPath}': {ex}");
                return; // Can't proceed if the config file isn't readable
            }

            if (!JsonHelper.IsValidJson(configText))
            {
                logger.Error($"Config file '{configPath}' contains invalid JSON.");
                return;
            }

            JObject configJson;
            try
            {
                configJson = JsonHelper.ParseObject(configText);
            }
            catch (Exception ex)
            {
                logger.Error($"Config file '{configPath}' could not be parsed as JSON object: {ex}");
                return;
            }

            if (configJson["environment"] is JObject env)
            {
                Directory.CreateDirectory("allure-results");
                using (var sw = new StreamWriter(outputPath))
                {
                    foreach (var prop in env.Properties())
                    {
                        sw.WriteLine($"{prop.Name}={prop.Value}");
                    }
                }
                logger.Information($"Allure environment properties written to '{outputPath}'.");
            }
            else
            {
                logger.Warning($"No 'environment' object found in '{configPath}'.");
            }
        }
    }
}
