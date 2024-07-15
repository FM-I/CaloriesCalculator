using CommunityToolkit.Mvvm.ComponentModel;

namespace CaloriesCalculator.Models
{
    public partial class ProductModel : ObservableObject
    {
        public string Id { get; set; }
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private double _calories;
    }
}
