using CaloriesCalculator.Interfaces;
using CaloriesCalculator.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System.Reactive.Linq;

namespace CaloriesCalculator.Repositories
{
    public class RealtimeDataBase : IDBContext
    {
        private const string ProductsChildName = "Products";
        private const string CalculateProductsChildName = "CalculateProducts";
        private readonly FirebaseClient _client;

        public event Action<string, ProductModel> ProductUpdate;
        public event Action<string, CalcucaltedTotalData> CalculatedDataUpdate;

        public RealtimeDataBase(string path)
        {
            _client = new FirebaseClient(path);

            _client.Child(ProductsChildName)
                .AsObservable<ProductModel>()
                .Subscribe(data =>
                {
                    if (data.Object != null)
                    {
                        data.Object.Id = data.Key;
                        ProductUpdate?.Invoke(data.EventType.ToString(), data.Object);
                    }
                });

            _client.Child(CalculateProductsChildName)
                .Child("Max")
                .AsObservable<CalcucaltedTotalData>()
                .Subscribe(data =>
                {
                    CalculatedDataUpdate?.Invoke(data.EventType.ToString(), data.Object);
                });
        }

        public async Task AddProduct(ProductModel product)
        {
            await _client.Child(ProductsChildName).PostAsync(product);
        }

        public async Task RemoveProduct(string id)
        {
            await _client.Child(ProductsChildName).Child(id).DeleteAsync();
        }

        public async Task UpdateProduct(ProductModel product)
        {
            await _client.Child(ProductsChildName).Child(product.Id).PutAsync(new { product.Name, product.Calories });
        }

        public async Task<List<ProductModel>> GetProducts()
        {
            var result = await _client.Child(ProductsChildName).OnceAsync<ProductModel>();
            return result.Select(x => new ProductModel() { Id = x.Key, Name = x.Object.Name, Calories = x.Object.Calories }).ToList();
        }

        public async Task SaveCalculatedProducts(CalculatedCaloriesData data)
        {
            var offset = new DateTimeOffset(DateTime.Now);
            var value = offset.ToUnixTimeSeconds();
            data.Id = value;

            await _client.Child(CalculateProductsChildName)
                         .Child("Max")
                         .Child(value.ToString())
                         .PutAsync(data);
        }

        public async Task<List<T>> GetCalculatedTotlaData<T>()
        {
            var data = await _client.Child(CalculateProductsChildName).Child("Max").OnceAsync<T>();
            return data
                .Select(x => x.Object)
                .ToList();
        }

        public async Task<List<ProductInCalculatorModel>> GetProductInCalculatedTotalData(long id)
        {
            var result = await _client.Child(CalculateProductsChildName)
                .Child("Max")
                .Child(id.ToString())
                .OnceSingleAsync<CalculatedCaloriesData>();

            return result?.Products;
        }

    }
}
