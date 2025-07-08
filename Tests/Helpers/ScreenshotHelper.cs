using Microsoft.Playwright;

namespace atf.Tests.Helpers
{
    /// <summary>
    /// Provides helper methods for taking screenshots during tests.
    /// </summary>
    public static class ScreenshotHelper
    {
        /// <summary>
        /// Takes a screenshot of the entire page or a specific portion of it.
        /// </summary>
        /// <param name="page">The Playwright page instance to capture.</param>
        /// <param name="name">The name of the screenshot file.</param>
        /// <param name="fullPage">Indicates whether to capture the full page or only the visible portion. Defaults to true.</param>
        /// <returns>The file path of the saved screenshot.</returns>
        public static async Task<string> TakeScreenshotAsync(IPage page, string name, bool fullPage = true)
        {
            try
            {
                // Replace invalid characters in the file name with underscores
                var invalidChars = Path.GetInvalidFileNameChars();
                var safeName = string.Join("_", name.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries)).Trim('_');
                
                // Generate the file path for the screenshot
                var path = Path.Combine("Screenshots", $"{safeName}-{DateTime.UtcNow:yyyyMMdd-HHmmss}.png");
                
                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                
                // Capture the screenshot
                var bytes = await page.ScreenshotAsync(new() { FullPage = fullPage });
                // Save the screenshot to the file system
                await File.WriteAllBytesAsync(path, bytes).ConfigureAwait(false);
                // Attach the screenshot to the Allure report
                AllureHelper.AttachScreenshot(name, bytes);
                // Return the file path of the saved screenshot
                return path;
            }
            catch (Exception ex)
            {
                // Optionally, log the error here using a logger if available
                throw new InvalidOperationException($"Failed to take screenshot '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Takes a screenshot of a specific element on the page.
        /// </summary>
        /// <param name="element">The Playwright locator for the element to capture.</param>
        /// <param name="name">The name of the screenshot file.</param>
        /// <returns>The file path of the saved screenshot.</returns>
        public static async Task<string> TakeElementScreenshotAsync(ILocator element, string name)
        {
            try
            {
                // Replace invalid characters in the file name with underscores
                var invalidChars = Path.GetInvalidFileNameChars();
                var safeName = string.Join("_", name.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries)).Trim('_');
                
                // Generate the file path for the screenshot
                var path = Path.Combine("Screenshots", $"{safeName}-{DateTime.UtcNow:yyyyMMdd-HHmmss}.png");
                
                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                
                // Capture the screenshot of the element
                var screenshot = await element.ScreenshotAsync();
                // Save the screenshot to the file system
                await File.WriteAllBytesAsync(path, screenshot).ConfigureAwait(false);
                // Attach the screenshot to the Allure report
                AllureHelper.AttachScreenshot(name, screenshot);

                // Return the file path of the saved screenshot
                return path;
            }
            catch (Exception ex)
            {
                // Optionally, log the error here using a logger if available
                throw new InvalidOperationException($"Failed to take element screenshot '{name}': {ex.Message}", ex);
            }
        }
    }
}