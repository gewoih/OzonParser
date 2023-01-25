using HtmlAgilityPack;
using OzonParser.Extensions;
using OzonParser.Models;
using OzonParser.Services.Base;

namespace OzonParser.Services.Parsing
{
	public sealed class OzonParserService : IProductsParserService
	{
		private const string _defaultUrl = "https://www.ozon.ru/search/?from_global=true&text=";
		private readonly ILogger<IProductsParserService> _logger;
		private readonly IWebHtmlParser _webHtmlParser;

		public OzonParserService(ILogger<IProductsParserService> logger, IWebHtmlParser webHtmlParser)
		{
			_logger = logger;
			_webHtmlParser = webHtmlParser;
		}

		public async Task<IEnumerable<Product>> GetProductsAsync(string keyPhrase)
		{
			_logger.LogInformation($"Parsing products by key phrase '{keyPhrase}'.");

			ValidateKeyPhrase(keyPhrase);

			var requestUrl = CreateRequestUrl(keyPhrase);
			var htmlContent = await _webHtmlParser.GetHtmlAsync(requestUrl);

			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(htmlContent);

			var node = htmlDocument.DocumentNode.SelectSingleNode("//*[@data-widget='searchResultsV2']");
			if (node == null)
			    throw new NodeNotFoundException("Main node with Ozon products not found.");

			var divs = node.SelectNodes("//div//a[@href]");
			var productLinks = new List<string>();
			foreach (var div in divs)
			{
				var link = div.GetAttributeValue("href", "");
				if (!link.StartsWith("/product"))
					continue;

				if (!link.Contains("advert"))
					productLinks.Add($"https://ozon.ru{link}");

            }
			productLinks = productLinks.Distinct().ToList();

			var products = new List<Product>();
			foreach (var productLink in productLinks)
			{
                products.Add(await GetProductFromUrl(productLink));
			}

			return products;
		}

		private async Task<Product> GetProductFromUrl(string productUrl)
		{
            var productPageHtml = await _webHtmlParser.GetHtmlAsync(productUrl);
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
			else
				product.Price = salePriceNode.InnerText.ParseToDecimal();

            return product;
		}

		private void ValidateKeyPhrase(string keyPhrase)
		{
			if (string.IsNullOrEmpty(keyPhrase))
			{
				_logger.LogError("Key phrase is null or empty. Parsing of products cannot be performed.");
				throw new ArgumentNullException("Key phrase cannot be null or empty.");
			}
		}

		private string CreateRequestUrl(string keyPhrase)
		{
			var keyPhraseWords = keyPhrase.Split(' ');
			var concattedKeyPhraseWords = string.Join("+", keyPhraseWords);

			return _defaultUrl + concattedKeyPhraseWords;
		}
	}
}
