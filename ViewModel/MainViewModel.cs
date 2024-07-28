﻿using CaloriesCalculator.Interfaces;
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
        _groupData = new();

        var task = Task.Run(() => _context.GetCalculatedTotlaData<CalcucaltedTotalData>());
        task.Wait();

        var data = task.Result;
        var orderData = data.OrderByDescending(x => x.Id);

        foreach (var item in orderData)
        {
            var group = _groupData.FirstOrDefault(x => x.Date == new DateOnly(item.Date.Year, item.Date.Month, item.Date.Day));

            if (group == null)
            {
                group = new GroupCalculatedData(item.Date, new() { item });
                _groupData.Add(group);
            }
            else
            {
                group.Add(item);
            }
        }

        _context.CalculatedDataUpdate += CalculatedDataUpdate;
    }

    private void CalculatedDataUpdate(string action, CalcucaltedTotalData data)
    {
        var group = _groupData.FirstOrDefault(x => x.Date == new DateOnly(data.Date.Year, data.Date.Month, data.Date.Day));

        if(group != null)
        {
            var value = group.FirstOrDefault(x => x.Id == data.Id);

            if (action == "Delete")
            {
                group.Remove(value);
            }
            else if (value == null)
            {
                group.Insert(0, data);
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
}
