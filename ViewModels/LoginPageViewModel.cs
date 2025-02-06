using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DineSync.Repository.Interfaces;

namespace DineSync.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        #region Fields
        private readonly ILoginRepository _LoginRepository;
        #endregion

        #region Properties
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
        #endregion

        #region Constructor
        public LoginPageViewModel(ILoginRepository loginRepository)
        {
            _LoginRepository = loginRepository;
        }
        #endregion

        #region Methods
        [RelayCommand]
        public void TogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
            PasswordVisibilityIcon = IsPasswordVisible
                ? "view.png"
                : "hide.png";
        }

        [RelayCommand]
        public async Task NavigateToSignup()
        {
            await Shell.Current.GoToAsync("SignupPage");
        }

        [RelayCommand]
        public async Task LoginAsync()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Shell.Current.DisplayAlert("Login Failed", "Please Enter both email and password.", "OK");
                return;
            }

            var user = await _LoginRepository.LoginAsync(Email, Password);

            if (user != null && user.Role== "Owner")
            {
                await Shell.Current.GoToAsync($"/PaymentPage", new Dictionary<string, object> { { "User", user } });
            }
            else if(user != null && user.Role == "Waiter")
            {
                await Shell.Current.GoToAsync($"/TablePage", new Dictionary<string, object> { { "User", user } });
            }
            else if (user != null && (user.Role == "Kitchen"))
            {
                await Shell.Current.GoToAsync($"/OrderPage", new Dictionary<string, object> { { "User", user } });
            }
            else
            {
                await Shell.Current.DisplayAlert("Login Failed", "Invalid email or password.", "OK");
                return;
            }
            Role = user.Role;
            MessagingCenter.Send(this, "Role", Role);

            Email = string.Empty;
            Password = string.Empty;
        }
        #endregion
    }
}
