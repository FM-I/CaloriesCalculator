using CommunityToolkit.Mvvm.ComponentModel;

namespace CaloriesCalculator.Models
{
    public partial class SelectedItemProductModel : ProductModel
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedColor))]
        private bool _isSelected;
        public Color SelectedColor { 
            get 
            {
                if(_theme == AppTheme.Dark)
                    return IsSelected? Color.FromHex("#a490e8") : Color.FromHex("#795ae6"); 
                    
                return IsSelected? Colors.AliceBlue: Colors.White; 
            } 
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedColor))]
        private AppTheme _theme;

        public SelectedItemProductModel()
        {
            Theme = Application.Current.RequestedTheme;
            Application.Current.RequestedThemeChanged += ThemeChanged;
        }

        private void ThemeChanged(object? sender, AppThemeChangedEventArgs e)
        {
            Theme = e.RequestedTheme;
        }
    }
}
