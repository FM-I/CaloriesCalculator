using CaloriesCalculator.Pages;

namespace CaloriesCalculator
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ProductPage), typeof(ProductPage));
            Routing.RegisterRoute(nameof(CalculateCaloriesPage), typeof(CalculateCaloriesPage));
            Routing.RegisterRoute(nameof(AddProductPage), typeof(AddProductPage));
        }
    }
}
