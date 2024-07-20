﻿using CaloriesCalculator.Interfaces;
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
    private ProductModel _product = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TotalCalories))]
    [NotifyPropertyChangedFor(nameof(ProductSelected))]
    private double? _weight;

    [ObservableProperty]
    private ObservableCollection<ProductModel> _products;

    private readonly IDBContext _context;

    public bool ProductSelected => Product != null && Product.Id != default && Weight > 0;
    public double TotalCalories => Product == null ? 0 : Product.Calories * (double)(Weight == null ? 0 : Weight) / 100;

    public AddProductViewModel(IDBContext context)
    {
        _context = context;
        _products = new();

        Task.Run(async () =>
        {
            foreach (var item in await _context.GetProducts())
            {
                _products.Add(item);
            }

        });

        _context.ProductUpdate += ProductUpdate;
    }

    private void ProductUpdate(string action, ProductModel data)
    {
        var value = _products.FirstOrDefault(x => x.Id == data.Id);

        if(action == "Delete")
        {
            _products.Remove(value);
        }
        else 
        {
            _products.Insert(0, data);
        }
    }

    [RelayCommand]
    private void Tap(ProductModel data)
    {
        if (data != null)
            Product = data;
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
}
