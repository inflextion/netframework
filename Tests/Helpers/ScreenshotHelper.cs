using Allure.Net.Commons;
using Microsoft.Playwright;

namespace atf.Tests.Helpers
{
    public static class ScreenshotHelper
    {
        public static async Task<string> TakeScreenshotAsync(IPage page, string name, bool fullPage = true)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var safeName = string.Join("_", name.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries)).Trim('_');
            var path = Path.Combine("Screenshots", $"{safeName}-{DateTime.UtcNow:yyyyMMdd-HHmmss}.png");
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            var bytes = await page.ScreenshotAsync(new() { FullPage = fullPage });
            await File.WriteAllBytesAsync(path, bytes);

            AllureApi.AddAttachment(
                name: "Failure_Screenshot",
                type: "image/png",
                content: bytes,
                fileExtension: "png"
            );

            return path;
        }

        public static async Task TakeElementScreenshotAsync(ILocator element, string name)
        {
            var screenshot = await element.ScreenshotAsync();
            AllureApi.AddAttachment(name, "image/png", screenshot);
        }
    }
}

