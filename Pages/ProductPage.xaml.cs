using CaloriesCalculator.ViewModel;

namespace CaloriesCalculator.Pages;

public partial class ProductPage : ContentPage
{
	public ProductPage(ProductViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}