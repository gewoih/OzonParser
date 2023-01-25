using HtmlAgilityPack;
using OzonParser.Extensions;
using OzonParser.Models;
using OzonParser.Services.Base;

namespace OzonParser.Services.Parsing
{
    public sealed class OzonProductPageParser : IProductPageParser
    {
        private readonly IWebHtmlParser _webHtmlParser;

        public OzonProductPageParser(IWebHtmlParser webHtmlParser)
        {
            _webHtmlParser = webHtmlParser;
        }

        public async Task<Product> GetProductAsync(string productUrl)
        {
            var productPageHtml = await _webHtmlParser.GetHtmlAsync(productUrl, "//*[@data-widget='webProductHeading']//h1");
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(productPageHtml);

            var product = new Product();

            product.Title = htmlDocument.DocumentNode.SelectSingleNode("//*[@data-widget='webProductHeading']//h1").InnerText;
            product.Id = htmlDocument.DocumentNode.SelectSingleNode("//*[@data-widget='webDetailSKU']").InnerText.ParseToUlong();
            product.Url = productUrl;

            var feedbacksNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@data-widget='webReviewProductScore']//div//div//div[2]//a//div//div");
            if (feedbacksNode is not null)
                product.FeebacksCount = htmlDocument.DocumentNode.SelectSingleNode("//*[@data-widget='webReviewProductScore']//div//div//div[2]//a//div//div").InnerText.ParseToInt();
            else
                product.FeebacksCount = 0;

            var brandNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@data-widget='webBrand']//div//a");
            if (brandNode is not null)
                product.Brand = brandNode.InnerText;
            else
                product.Brand = string.Empty;

            var salePriceNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@data-widget='webPrice']//div//div//div//span[1]//span");
            var basePriceNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@data-widget='webPrice']//div//div//div//span[2]");
            if (basePriceNode is not null)
                product.Price = basePriceNode.InnerText.ParseToDecimal();
            else if (salePriceNode is not null)
                product.Price = salePriceNode.InnerText.ParseToDecimal();

            return product;
        }
    }
}
