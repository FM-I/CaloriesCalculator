using CaloriesCalculator.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CaloriesCalculator.ViewModel
{
    public partial class AddProductViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalCalories))]
        private ProductModel product = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalCalories))]
        private double _weight;

        public double TotalCalories => product == null ? 0 : product.Calories * Weight / 100;

        [RelayCommand]
        private void Tap(ProductModel data)
        {
            if(data != null)
                Product = data;
        }
        
        [RelayCommand]
        private async Task Add()
        {
            var data = new ShellNavigationQueryParameters()
            {
                ["Product"] = new ProductInCalculatorModel(product) { Weight = Weight }
            };

            await Shell.Current.GoToAsync("..", true, data);
        }
    }
}
