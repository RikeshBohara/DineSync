using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DineSync.Repository.Interfaces;
using DineSync.Views.Pages;

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

        private string _Role;

        public string Role
        {
            get => _Role;
            set
            {
                _Role = value;
                OnPropertyChanged();
            }
        }

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

            if (user != null && user.Role== "Owner")
            {
                await Shell.Current.GoToAsync($"/DashboardPage", new Dictionary<string, object> { { "User", user } });
            }
            else if(user!=null && user.Role == "Waiter")
            {
                await Shell.Current.GoToAsync($"/TablePage", new Dictionary<string, object> { { "User", user } });
            }
            else if (user != null && (user.Role == "Kitchen" || user.Role == "Reception"))
            {
                await Shell.Current.GoToAsync($"/OrderPage", new Dictionary<string, object> { { "User", user } });
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Invalid email or password.", "OK");
                return;
            }
            Role = user.Role;
            MessagingCenter.Send(this, "Role", Role);

            Email = string.Empty;
            Password = string.Empty;
        }
    }
}
