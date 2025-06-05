using atf.Core.Models;
using atf.UI.Helpers;
using Microsoft.Playwright;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atf.UI.Pages
{
    internal class WebElementsPage : BasePage
    {
        public WebElementsPage(IPage page, PlaywrightSettings settings, ILogger logger) : base(page, settings, logger) { }

        public async Task FillInTextInputBox(string text)
        {
            Logger.Debug("Filling '{Selector}' with '{Text}'", WebElementsSelectors.TextInput, text);
            try
            {
                await FillAsync(WebElementsSelectors.TextInput, text);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to fill selector '{Selector}' with '{Text}'", WebElementsSelectors.TextInput, text);
                throw;
            }
        }
    }
}