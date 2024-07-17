using CaloriesCalculator.ViewModel;

namespace CaloriesCalculator.Pages;

public partial class AddProductPage : ContentPage
{
	public AddProductPage(AddProductViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}