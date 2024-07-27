using CaloriesCalculator.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;

namespace CaloriesCalculator.ViewModel
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;

        public SignInViewModel(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
            
            if (_authClient.User != null)
            {
                //if (_authClient.User?.Credential.Created.AddSeconds(_authClient.User.Credential.ExpiresIn) < DateTime.Now)
                //{
                //    _authClient.SignOut();
                //}
                //else
                //{
                Task.Run(async () => await Shell.Current.GoToAsync($"//MainPage"));
                //}
            }
        }

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [RelayCommand]
        private async Task SignIn()
        {
            try
            {
                var result = await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);

                await Shell.Current.GoToAsync($"//MainPage");

            }
            catch (FirebaseAuthHttpException e)
            {
                await Shell.Current.DisplayAlert("Error", "Login or password incorrect!", "OK");
            }
            catch (Exception e) 
            {
                await Shell.Current.DisplayAlert("Error", e.Message, "OK");
            }

        }

        [RelayCommand]
        private async Task NavigateToSignUp()
        {
            await Shell.Current.GoToAsync(nameof(SignUpPage));
        }

    }
}
