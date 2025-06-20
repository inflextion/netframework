using atf.Core.Models;
using Microsoft.Playwright;
using Serilog;

namespace atf.UI.Pages;

public class UserPage: BasePageILocator
{
    public ILocator SearchBox => Page.GetByPlaceholder("Search by name");
    public ILocator ProductCards => Page.Locator(".product-card");
    
    public ILocator OpenCartButton => Page.GetByRole(AriaRole.Button, new() { Name = "Open Cart" });
    public UserPage(IPage page, PlaywrightSettings settings, ILogger logger) : base(page, settings, logger)
    {
        // Initialize any specific elements or actions for the UserPage here
    }


    
    public async Task AddToCart(string productName)
    {
       await ProductCards
            .Filter(new() { HasText = productName })
            .GetByRole(AriaRole.Button, new() { Name = "Add to Cart" })
            .ClickAsync();
        // second option
        var productLocator =  ProductCards
                                .Filter(new() { HasText = productName })
                                .GetByRole(AriaRole.Button, new() { Name = "Add to Cart" });
        await ClickAsync(productLocator);
    }
    
    public async Task<Dictionary<string, Tuple<string, decimal>>> ReturnProducts()
    {
        var productList = new Dictionary<string, Tuple<string, decimal>>();
        var products = await ProductCards.CountAsync();
        
        for (int i = 0; i < products; i++)
        {
            var productCard = ProductCards.Nth(i);
            var productName = await productCard.GetByRole(AriaRole.Heading).InnerTextAsync();
            var productPriceText = await productCard.GetByText("Price:").InnerTextAsync();
            var productPrice = decimal.Parse(productPriceText.Replace("Price: $", "").Trim());
            var productCategoryText = await productCard.GetByText("Category:").InnerTextAsync();
            var productText = productCategoryText.Replace("Category:", "").Trim();
            productList[productName] = new Tuple<string, decimal>(productText, productPrice);
        }
       return productList; 
    }
}