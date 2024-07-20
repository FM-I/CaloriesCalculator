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
    private ObservableCollection<CalcucaltedTotalData> _calculatedDatas;
    private readonly IDBContext _context;

    public MainViewModel(IDBContext context)
    {
        _context = context;
        _calculatedDatas = new();

        Task.Run(async () =>
        {
            var data = await _context.GetCalculatedTotlaData<CalcucaltedTotalData>();
            var orderData = data.OrderByDescending(x => x.Id);

            foreach (var item in orderData)
            {
                var value = _calculatedDatas.FirstOrDefault(x => x.Id == item.Id);

                if(value == null)
                    _calculatedDatas.Add(item);
            }
        });

        _context.CalculatedDataUpdate += CalculatedDataUpdate;
    }

    private void CalculatedDataUpdate(string action, CalcucaltedTotalData data)
    {
        var value = _calculatedDatas.FirstOrDefault(x => x.Id == data.Id);

        if(action == "Delete")
        {
            _calculatedDatas.Remove(value);
        }
        else if(value == null)
        {
            _calculatedDatas.Insert(0, data);
        }
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
    private async Task Tap(CalcucaltedTotalData data)
    {
        var products = await _context.GetProductInCalculatedTotalData(data.Id);
        ObservableCollection<ProductInCalculatorModel> productsData = products != null ? [.. products] : new();

        var values = new ShellNavigationQueryParameters()
        {
            ["VisivilityCommands"] = false,
            ["Products"] = productsData
        };

        await Shell.Current.GoToAsync(nameof(CalculateCaloriesPage), values);
    }

}
