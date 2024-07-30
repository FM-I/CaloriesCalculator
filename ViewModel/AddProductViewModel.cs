using CaloriesCalculator.Interfaces;
using CaloriesCalculator.Models;
using CaloriesCalculator.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CaloriesCalculator.ViewModel;

public partial class AddProductViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalCalories))]
    [NotifyPropertyChangedFor(nameof(ProductSelected))]
    private SelectedItemProductModel? _product;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalCalories))]
    [NotifyPropertyChangedFor(nameof(ProductSelected))]
    private double? _weight;

    [ObservableProperty]
    private ObservableCollection<SelectedItemProductModel> _products;

    [ObservableProperty]
    private string _title;

    private readonly IDBContext _context;

    public bool ProductSelected => Product != null && Product.Id != default && Weight > 0;
    public double TotalCalories => Product == null ? 0 : Product.Calories * (double)(Weight == null ? 0 : Weight) / 100;

    public AddProductViewModel(IDBContext context)
    {
        _context = context;

        Task.Run(async () =>
        {
            var products = await _context.GetProducts();
            Products = new(products.OrderBy(x => x.Name)
                .Select(x => new SelectedItemProductModel() 
                {
                    Id = x.Id,
                    Name = x.Name,
                    Calories = x.Calories
                })
                .ToList());
        });

        _context.ProductUpdate += ProductUpdate;
    }

    private void ProductUpdate(string action, ProductModel data)
    {
        var value = Products.FirstOrDefault(x => x.Id == data.Id);

        if(action == "Delete")
        {
            Products.Remove(value);
        }
        else 
        {

            if(value == null)
            {
                Products.Insert(0, new SelectedItemProductModel()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Calories = data.Calories
                });
            }
            else
            {
                value.Name = data.Name;
                value.Calories = data.Calories;
            }
        }
    }

    [RelayCommand]
    private void Tap(SelectedItemProductModel data)
    {
        if (data != null)
        {
            if(Product != null)
            {
                Product.IsSelected = false;
            }

            Product = data;
            Product.IsSelected = true;
        }
    }

    [RelayCommand]
    private async Task Add()
    {
        var data = new ShellNavigationQueryParameters()
        {
            ["Product"] = new ProductInCalculatorModel(Product) { Weight = (double)Weight }
        };

        await Shell.Current.GoToAsync("..", true, data);
    }

    [RelayCommand]
    private async Task CreateProduct()
    {
        await Shell.Current.GoToAsync(nameof(ProductPage));
    }

    [RelayCommand]
    private async Task UpdateProduct(SelectedItemProductModel data)
    {
        var queryParams = new ShellNavigationQueryParameters()
        {
            ["Product"] = new ProductModel() { Id = data.Id, Name = data.Name, Calories = data.Calories }
        };

        await Shell.Current.GoToAsync(nameof(ProductPage), queryParams);
    }
}