using CaloriesCalculator.ViewModel;

namespace CaloriesCalculator.Pages;

public partial class SignInPage : ContentPage
{
	public SignInPage(SignInViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}