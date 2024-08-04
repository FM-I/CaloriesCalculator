using CaloriesCalculator.Models;

namespace CaloriesCalculator.Interfaces
{
    public interface IDBContext
    {
        public event Action<string, ProductModel> ProductUpdate;
        public event Action<string, CalcucaltedTotalData> CalculatedDataUpdate;

        Task AddProduct(ProductModel product);
        Task<List<ProductModel>> GetProducts();
        Task RemoveProduct(string id);
        Task UpdateProduct(ProductModel product);
        Task SaveCalculatedProducts(CalculatedCaloriesData data);
        Task<List<T>> GetCalculatedTotalData<T>(string startId);
        Task<List<ProductInCalculatorModel>> GetProductInCalculatedTotalData(long id);
    }
}
