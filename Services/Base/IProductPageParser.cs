using OzonParser.Models;

namespace OzonParser.Services.Base
{
    public interface IProductPageParser
    {
        public Task<Product> GetProductAsync(string productUrl);
    }
}
