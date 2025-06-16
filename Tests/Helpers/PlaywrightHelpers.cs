using Microsoft.Playwright;
using atf.Core.Logging;

namespace atf.Tests.Helpers
{
    public static class PlaywrightHelpers
    {
        /// <summary>
        /// Handles popup windows by waiting for them and executing actions
        /// </summary>
        public static async Task<T> HandlePopupAsync<T>(IBrowserContext context, TestLogger logger, Func<Task> triggerAction, Func<IPage, Task<T>> popupAction)
        {
            logger.Information("Setting up popup handler");
            var popupTask = context.WaitForPageAsync();
            await triggerAction();
            var popup = await popupTask;
            logger.Information("Popup opened: {Url}", popup.Url);
            try
            {
                var result = await popupAction(popup);
                return result;
            }
            finally
            {
                await popup.CloseAsync();
                logger.Information("Popup closed");
            }
        }

        /// <summary>
        /// Handles popup windows without return value
        /// </summary>
        public static async Task HandlePopupAsync(IBrowserContext context, TestLogger logger, Func<Task> triggerAction, Func<IPage, Task> popupAction)
        {
            await HandlePopupAsync(context, logger, triggerAction, async popup =>
            {
                await popupAction(popup);
                return true;
            });
        }

        /// <summary>
        /// Handles JavaScript alerts, confirms, and prompts
        /// </summary>
        public static async Task HandleDialogAsync(IPage page, TestLogger logger, Func<Task> triggerAction, bool accept = true, string promptText = null)
        {
            logger.Information("Setting up dialog handler - Accept: {Accept}", accept);
            page.Dialog += async (_, dialog) =>
            {
                logger.Information("Dialog appeared: Type={Type}, Message='{Message}'", dialog.Type, dialog.Message);
                if (accept)
                {
                    if (!string.IsNullOrEmpty(promptText))
                        await dialog.AcceptAsync(promptText);
                    else
                        await dialog.AcceptAsync();
                }
                else
                {
                    await dialog.DismissAsync();
                }
            };
            await triggerAction();
        }
    }
}

