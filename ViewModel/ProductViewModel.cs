using CaloriesCalculator.Interfaces;
using CaloriesCalculator.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CaloriesCalculator.ViewModel;

[QueryProperty("Product", "Product")]
public partial class ProductViewModel : ObservableObject
{
    public ProductModel Product { 
        set
        {
            Name = value.Name;
            Calories = value.Calories;
            _id = value.Id;
        }
    }
    
    private string _id;
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private double _calories;

    private readonly IDBContext _context;

    public ProductViewModel(IDBContext context)
    {
        _context = context;
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Name))
            return;

        if (Calories <= 0)
            return;

        if (string.IsNullOrWhiteSpace(_id))
        {
            await _context.AddProduct(new() { Name = Name, Calories = Calories});
            Name = string.Empty;
            Calories = 0;
        }
        else
        {
            await _context.UpdateProduct(new() { Id = _id, Name = Name, Calories = Calories });
        }
    }
}
