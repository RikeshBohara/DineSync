using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DineSync.ViewModels
{
    public partial class AppShellViewModel : ObservableObject
    {
        [RelayCommand]
        public async Task NavigateToLogin()
        {
            await Shell.Current.GoToAsync("LoginPage");
        }
    }
}
