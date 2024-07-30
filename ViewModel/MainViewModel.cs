using CaloriesCalculator.Interfaces;
using CaloriesCalculator.Models;
using CaloriesCalculator.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using System.Collections.ObjectModel;

namespace CaloriesCalculator.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<GroupCalculatedData> _groupData;

    private readonly IDBContext _context;
    private readonly FirebaseAuthClient _authClient;

    public MainViewModel(IDBContext context, FirebaseAuthClient client)
    {
        _context = context;
        _authClient = client;

        var task = Task.Run(() => _context.GetCalculatedTotlaData<CalcucaltedTotalData>());
        task.Wait();

        var data = task.Result;
        var orderData = data.OrderByDescending(x => x.Id);
        var groups = new List<GroupCalculatedData>();

        foreach (var item in orderData)
        {
            var group = groups.FirstOrDefault(x => x.Date.Year == item.Date.Year
                                                   && x.Date.Month == item.Date.Month
                                                   && x.Date.Day == item.Date.Day);

            if (group == null)
            {
                group = new GroupCalculatedData(item.Date, new());
                groups.Add(group);
            }

            group.Add(item);
        }

        GroupData = new(groups);
        _context.CalculatedDataUpdate += CalculatedDataUpdate;
    }

    private void CalculatedDataUpdate(string action, CalcucaltedTotalData data)
    {
        var group = GroupData.FirstOrDefault(x => x.Date.Year == data.Date.Year
                                                   && x.Date.Month == data.Date.Month
                                                   && x.Date.Day == data.Date.Day);

        if (group == null)
        {
            group = new GroupCalculatedData(data.Date, new());
            GroupData.Insert(0, group);
        }

        var value = group.FirstOrDefault(x => x.Id == data.Id);

        if (action == "Delete" && value != null)
        {
            group.Remove(value);
        }
        else if (value == null)
        {
            group.Insert(0, data);
        }

        if (group.Count == 0)
        {
            GroupData.Remove(group);
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

    [RelayCommand]
    private async Task SignOut()
    {
        bool answer = await Shell.Current.DisplayAlert("Sign Out", "Do you want to leave?", "Yes", "No");

        if (answer)
        {
            _authClient.SignOut();
            await Shell.Current.GoToAsync("//AuthPage");
        }
    }
}
