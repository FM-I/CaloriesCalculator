using CaloriesCalculator.Interfaces;
using CaloriesCalculator.Models;
using CaloriesCalculator.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CaloriesCalculator.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private ObservableCollection<CalcucalteData> _calculateDatas;
    private readonly IDBContext _context;

    public MainViewModel(IDBContext context)
    {
        _context = context;
        _calculateDatas = new();
    }

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
        var values = new ShellNavigationQueryParameters()
        {
            ["VisivilityCommands"] = true
        };

        await Shell.Current.GoToAsync(nameof(CalculateCaloriesPage), values);
    }

    [RelayCommand]
    private async Task Tap(CalcucalteData data)
    {
        var values = new ShellNavigationQueryParameters()
        {
            ["VisivilityCommands"] = false
        };

        await Shell.Current.GoToAsync(nameof(CalculateCaloriesPage), values);
    }

}
