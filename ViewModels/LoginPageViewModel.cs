using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DineSync.Repository.Interfaces;

namespace DineSync.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        private readonly ILoginRepository _LoginRepository;

        [ObservableProperty]
        private string _Email;

        [ObservableProperty]
        private string _Password;

        [ObservableProperty]
        private bool _IsPasswordVisible = true;

        [ObservableProperty]
        private string _PasswordVisibilityIcon = "view.png";

        public LoginPageViewModel(ILoginRepository loginRepository)
        {
            _LoginRepository = loginRepository;
        }

        [RelayCommand]
        private void TogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
            PasswordVisibilityIcon = IsPasswordVisible
                ? "view.png"
                : "hide.png";
        }

        [RelayCommand]
        private async Task NavigateToSignup()
        {
            await Shell.Current.GoToAsync("SignupPage");
        }

        [RelayCommand]
        public async Task LoginAsync()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Shell.Current.DisplayAlert("Error", "Please Enter both email and password.", "OK");
                return;
            }

            var user = await _LoginRepository.LoginAsync(Email, Password);

            if (user != null)
            {
                await Shell.Current.GoToAsync("///HomePage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Invalid email or password.", "OK");
                return;
            }

            Email = string.Empty;
            Password = string.Empty;
        }
    }
}
