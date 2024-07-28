using CommunityToolkit.Mvvm.ComponentModel;

namespace CaloriesCalculator.Models
{
    public partial class SelectedItemProductModel : ProductModel
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedColor))]
        private bool _isSelected;
        public Color SelectedColor => IsSelected ? Colors.AliceBlue : Colors.White;
    }
}
