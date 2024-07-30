using CaloriesCalculator.ViewModel;

namespace CaloriesCalculator
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _mainViewModel;
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            _mainViewModel = vm;
            _mainViewModel.GroupChanged += GroupChanged;
        }

        private void GroupChanged(string action)
        {
            if(action == "InsertOrUpdate")
            {
                GroupView.ScrollTo(0, 0, ScrollToPosition.Start);
            }
        }
    }

}
