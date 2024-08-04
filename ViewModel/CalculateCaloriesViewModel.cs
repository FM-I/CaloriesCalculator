using CaloriesCalculator.Interfaces;
using CaloriesCalculator.Models;
using CaloriesCalculator.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CaloriesCalculator.ViewModel
{
    [QueryProperty("Product", "Product")]
    [QueryProperty("VisivilityCommands", "VisivilityCommands")]
    [QueryProperty("Products", "Products")]
    public partial class CalculateCaloriesViewModel : ObservableObject
    {        
        public ProductInCalculatorModel? Product
        {
            set
            {
                if (value != null)
                    Products.Add(value);
            }
        }

        public double TotalWeight => Math.Round(Products.Sum(p => p.Weight), 2);
        public double TotalCalories => Math.Round(Products.Sum(p => p.TotalCalories), 2);

        [ObservableProperty]
        private bool _visivilityCommands;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalCalories))]
        [NotifyPropertyChangedFor(nameof(TotalWeight))]
        private ObservableCollection<ProductInCalculatorModel> _products;

        private readonly IDBContext _context;

        public CalculateCaloriesViewModel(IDBContext context)
        {
            _context = context;
            Products = new();
            Products.CollectionChanged += Products_CollectionChanged;
        }

        private void Products_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TotalWeight));
            OnPropertyChanged(nameof(TotalCalories));
        }

        [RelayCommand]
        private async Task AddProduct()
        {
            await Shell.Current.GoToAsync(nameof(AddProductPage));
        }

        [RelayCommand]
        private void RemoveProduct(string id)
        {
            var product = Products.FirstOrDefault(x => x.Id == id);

            if (product != null)
            {
                Products.Remove(product);
            }
        }

        [RelayCommand]
        private async Task SaveProducts()
        {
            var data = new CalculatedCaloriesData(Products.ToList());
            
            await _context.SaveCalculatedProducts(data);

            var totalData = new CalcucaltedTotalData()
            {
                Id = data.Id,
                TotalCalories = data.TotalCalories,
                TotalWeight = data.TotalWeight
            };

            var queryParams = new ShellNavigationQueryParameters()
            {
                ["data"] = totalData
            };

            await Shell.Current.GoToAsync("..", queryParams);
        }

    }
}
