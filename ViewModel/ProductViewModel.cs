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

        var data = new ProductModel() { Id = _id, Name = Name, Calories = Calories };

        if (string.IsNullOrWhiteSpace(_id))
        {
            await _context.AddProduct(data);
            await Shell.Current.DisplayAlert("Success", "Proudct add!", "Ok");
            Name = string.Empty;
            Calories = 0;
        }
        else
        {
            await _context.UpdateProduct(data);
            await Shell.Current.DisplayAlert("Success", "Proudct update!", "Ok");
        }
    }
}
