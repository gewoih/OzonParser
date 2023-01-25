using HtmlAgilityPack;
using OzonParser.Models;
using OzonParser.Services.Base;

namespace OzonParser.Services.Parsing
{
	public sealed class OzonProductsParserService : IProductsParserService
	{
		private const string _defaultUrl = "https://www.ozon.ru/search/?from_global=true&text=";
		private readonly ILogger<IProductsParserService> _logger;
		private readonly IWebHtmlParser _webHtmlParser;
		private readonly IProductPageParser _productPageParser;

		public OzonProductsParserService(ILogger<IProductsParserService> logger, IWebHtmlParser webHtmlParser, IProductPageParser productPageParser)
		{
			_logger = logger;
			_webHtmlParser = webHtmlParser;
			_productPageParser = productPageParser;
		}

		public async Task<IEnumerable<Product>> GetProductsAsync(string keyPhrase)
		{
			_logger.LogInformation($"Parsing products by key phrase '{keyPhrase}'.");

			ValidateKeyPhrase(keyPhrase);

			var requestUrl = CreateRequestUrl(keyPhrase);
			var htmlContent = await _webHtmlParser.GetHtmlAsync(requestUrl);

			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(htmlContent);

			var divs = htmlDocument.DocumentNode.SelectNodes("//div//a[@href]");
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
			productLinks.Remove("https://www.ozon.ru/product/elektronnyy-podarochnyy-sertifikat-million-podarkov-3000-ozon-135382627/?perehod=header&sh=0IcNY3FAgQ");

			var products = new List<Product>();
			var productsLoadingTasks = new List<Task>(productLinks.Count);
			foreach (var productLink in productLinks)
			{
				productsLoadingTasks.Add(Task.Run(async () =>
				{
					_logger.LogInformation($"Parsing started for Ozon product...");
					var product = await _productPageParser.GetProductAsync(productLink);
					products.Add(product);
					_logger.LogInformation($"Product successfully parsed from Ozon ('{product}')");
				}));

				if (productsLoadingTasks.Count >= 8)
				{
					Task.WaitAll(productsLoadingTasks.ToArray());
					productsLoadingTasks.Clear();
				}
			}

            Task.WaitAll(productsLoadingTasks.ToArray());
            return products;
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
