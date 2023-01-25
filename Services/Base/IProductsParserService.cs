using OzonParser.Models;

namespace OzonParser.Services.Base
{
    public interface IProductsParserService
    {
        public Task<IEnumerable<Product>> GetProductsAsync(string keyPhrase);
    }
}
