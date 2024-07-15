using CaloriesCalculator.Models;

namespace CaloriesCalculator.Interfaces
{
    public interface IDBContext
    {
        public event Action<string, ProductModel> ProductUpdate;
        Task AddProduct(ProductModel product);
        Task<List<ProductModel>> GetProducts();
        Task RemoveProduct(string id);
        Task UpdateProduct(ProductModel product);
    }
}
