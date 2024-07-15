using CaloriesCalculator.Interfaces;
using CaloriesCalculator.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CaloriesCalculator.ViewModel
{
    public partial class CalculateCaloriesViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ProductModel> _products;
        private readonly IDBContext _context;
        public CalculateCaloriesViewModel(IDBContext context)
        {
            _context = context;
            _products = new();
        }

        [RelayCommand]
        private async Task AddProduct()
        {
        }

        [RelayCommand]
        private void RemoveProduct(string id)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);

            if(product != null)
                _products.Remove(product);
        }
    }
}
