using OzonParser.Models;

namespace OzonParser.Services.Base
{
    public interface IProductsSaveManager
    {
        public Task SaveProductsAsync(IDictionary<string, IEnumerable<Product>> productsByCategories, string path = "");
    }
}
