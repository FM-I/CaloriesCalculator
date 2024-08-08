using CaloriesCalculator.Interfaces;
using CaloriesCalculator.Models;
using CaloriesCalculator.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using System.Collections.ObjectModel;

namespace CaloriesCalculator.ViewModel;

[QueryProperty("TotalData", "data")]
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<GroupCalculatedData> _groupData;

    public CalcucaltedTotalData TotalData { 
        set
        {
            var group = GroupData.FirstOrDefault(x => x.Date.Year == value.Date.Year
                                                   && x.Date.Month == value.Date.Month
                                                   && x.Date.Day == value.Date.Day);

            if (group == null)
            {
                group = new GroupCalculatedData(value.Date, new());
                GroupData.Insert(0, group);
            }

            if (group.FirstOrDefault(x => x.Id == value.Id) == null)
            {
                group.Insert(0, value);
            }
            
            GroupChanged?.Invoke("InsertOrUpdate");
        } 
    }

    private readonly IDBContext _context;
    private readonly FirebaseAuthClient _authClient;

    public event Action<string> GroupChanged;
    private string _startId = string.Empty;

    public MainViewModel(IDBContext context, FirebaseAuthClient client)
    {
        _context = context;
        _authClient = client;
        GroupData = new();

        var task = Task.Run(UpdateCollection);
        task.Wait();
        _context.CalculatedDataUpdate += CalculatedDataUpdate;
    }

    private void CalculatedDataUpdate(string action, CalcucaltedTotalData data)
    {
        if (action != "Delete")
            return;

        var group = GroupData.FirstOrDefault(x => x.Date.Year == data.Date.Year
                                                   && x.Date.Month == data.Date.Month
                                                   && x.Date.Day == data.Date.Day);

        if (group == null)
            return;

        var value = group.FirstOrDefault(x => x.Id == data.Id);

        if (value != null)
        {
            group.Remove(value);

            if (group.Count == 0)
            {
                GroupData.Remove(group);
            }
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

    [RelayCommand]
    private async Task UpdateCollection()
    {
        var data = await _context.GetCalculatedTotalData<CalcucaltedTotalData>(_startId);
        var orderData = data.OrderByDescending(x => x.Id);
        var groups = new List<GroupCalculatedData>();

        foreach (var item in orderData)
        {
            var group = GroupData.FirstOrDefault(x => x.Date.Year == item.Date.Year
                                                   && x.Date.Month == item.Date.Month
                                                   && x.Date.Day == item.Date.Day);

            if (group == null)
            {
                group = new GroupCalculatedData(item.Date, new());
                GroupData.Add(group);
            }

            if(group.FirstOrDefault(x => x.Id == item.Id) == null)
            {
                group.Add(item);
            }
        }

        if (orderData.Count() != 0)
        {
            _startId = orderData.Last().Id.ToString();
        }
    }

}
