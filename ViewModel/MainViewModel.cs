using CaloriesCalculator.Interfaces;
using CaloriesCalculator.Models;
using CaloriesCalculator.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CaloriesCalculator.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ProductModel> _products;
    private readonly IDBContext _context;
    public MainViewModel(IDBContext context)
    {
        _context = context;
        _products = new();
        //_context.ProductUpdate += ProductUpdate;

        //Task.Run(LoadProducts);
    }

    //private async Task LoadProducts()
    //{
    //    foreach (var item in await _context.GetProducts())
    //    {
    //        _products.Add(item);
    //    }
    //}

    //private void ProductUpdate(string actionType, ProductModel product)
    //{
    //    var result = _products.FirstOrDefault(p => p.Id == product.Id);

    //    if (result != null)
    //    {
    //        if (actionType == "Delete")
    //        {
    //            _products.Remove(result);
    //        }
    //        else
    //        {
    //            result.Name = product.Name;
    //            result.Calories = product.Calories;
    //        }
    //    }
    //    else
    //    {
    //        _products.Add(product);
    //    }
    //}

    [RelayCommand]
    private async Task AddProduct()
    {
        await Shell.Current.GoToAsync(nameof(ProductPage));
    }

    [RelayCommand]
    private async Task RemoveProduct(string id)
    {
        await _context.RemoveProduct(id);
    }
    
    [RelayCommand]
    private async Task UpdateProduct(ProductModel data)
    {
        await Shell.Current.GoToAsync(nameof(ProductPage), new Dictionary<string, object>()
        {
            {"Product", data}
        });
    }

    [RelayCommand]
    private async Task Calculate()
    {
        await Shell.Current.GoToAsync(nameof(CalculateCaloriesPage));
    }

}
