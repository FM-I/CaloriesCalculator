using CaloriesCalculator.ViewModel;
using System.Text.RegularExpressions;

namespace CaloriesCalculator.Pages;

public partial class AddProductPage : ContentPage
{
	public AddProductPage(AddProductViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
		var vm = BindingContext as AddProductViewModel;

		if (string.IsNullOrWhiteSpace(e.NewTextValue))
		{
			vm.Weight = null;
		}
    }
}