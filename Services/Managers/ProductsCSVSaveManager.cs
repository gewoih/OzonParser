using OzonParser.Models;
using OzonParser.Services.Base;

namespace OzonParser.Services.Managers
{
    public sealed class ProductsCSVSaveManager : IProductsSaveManager
    {
        public async Task SaveProductsAsync(IDictionary<string, IEnumerable<Product>> productsByCategories, string path)
        {
            foreach (var category in productsByCategories.Keys)
            {
                await File.WriteAllLinesAsync($"{AppDomain.CurrentDomain.BaseDirectory}/{category}.csv", productsByCategories[category].Select(p => p.ToString()));
            }
        }
    }
}
