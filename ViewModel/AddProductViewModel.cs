using CaloriesCalculator.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CaloriesCalculator.ViewModel
{
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

        public bool ProductSelected => Product != null && Product.Id != default && Weight > 0;
        public double TotalCalories => Product == null ? 0 : Product.Calories * (double)(Weight == null ? 0 : Weight) / 100;

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
                ["Product"] = new ProductInCalculatorModel(Product) { Weight = (double)Weight }
            };

            await Shell.Current.GoToAsync("..", true, data);
        }
    }
}
