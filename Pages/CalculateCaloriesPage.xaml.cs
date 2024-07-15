using CaloriesCalculator.ViewModel;

namespace CaloriesCalculator.Pages;

public partial class CalculateCaloriesPage : ContentPage
{
	public CalculateCaloriesPage(CalculateCaloriesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}