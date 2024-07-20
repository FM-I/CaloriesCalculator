using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace CaloriesCalculator.Models
{
    public partial class ProductInCalculatorModel : ObservableObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Calories { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalCalories))]
        private double _weight;

        public double TotalCalories => Math.Round(Weight * (Calories / 100), 2);

        public ProductInCalculatorModel(){}

        public ProductInCalculatorModel(ProductModel product)
        {
            Id = product.Id;
            Name = product.Name;
            Calories = product.Calories;
        }
    }
}
