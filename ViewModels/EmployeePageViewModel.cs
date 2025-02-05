using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DineSync.Models;
using DineSync.Repository.Interfaces;

namespace DineSync.ViewModels
{
    public partial class EmployeePageViewModel : ObservableObject
    {
        #region Fields
        private readonly IUserRepository _UserRepository;
        #endregion

        #region Properties
        [ObservableProperty]
        private ObservableCollection<User> _Users;

        [ObservableProperty]
        private User _SelectedUser;

        [ObservableProperty]
        private bool _IsPopupVisible;

        [ObservableProperty]
        private string _Name;

        [ObservableProperty]
        private string _Email;

        [ObservableProperty]
        private string _Password;

        [ObservableProperty]
        private string _Role;

        [ObservableProperty]
        private string _SelectedUserRole;

        [ObservableProperty]
        private string _Title;

        public List<string> Roles { get; } = new List<string>
        {
            "Waiter",
            "Kitchen",
            "Owner"
        };
        #endregion

        #region Constructor
        public EmployeePageViewModel(IUserRepository userRepository)
        {
            _UserRepository = userRepository;
            Users = new ObservableCollection<User>();
            LoadUsers();
        }
        #endregion

        #region Methods
        [RelayCommand]
        public async void LoadUsers()
        {
            var users = await _UserRepository.GetAllUsersAsync();
            Users = new ObservableCollection<User>(users);
            Title = "All Employee";
        }

        [RelayCommand]
        private void AddUser()
        {
            SelectedUser = null;
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Role = string.Empty;
            IsPopupVisible = true;
        }

        [RelayCommand]
        private void EditUser(User user)
        {
            SelectedUser = user;
            Name = user.Name;
            Email = user.Email;
            Password = string.Empty;
            Role = user.Role;
            IsPopupVisible = true;
        }

        [RelayCommand]
        private async Task RemoveUser(User user)
        {
            var confirm = await Shell.Current.DisplayAlert(
                "Confirm Delete",
                $"Are you sure you want to delete {user.Name}?",
                "Yes",
                "No"
            );

            if (confirm)
            {
                await _UserRepository.RemoveUserAsync(user);
                Users.Remove(user);
            }
        }

        [RelayCommand]
        private async Task SaveUser()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Role))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill all fields", "OK");
                return;
            }

            if (SelectedUser == null)
            {
                var newUser = new User
                {
                    Name = Name,
                    Email = Email,
                    Password = Password,
                    Role = Role
                };
                await _UserRepository.AddUserAsync(newUser);
                Users.Add(newUser);
            }
            else
            {
                SelectedUser.Name = Name;
                SelectedUser.Email = Email;
                SelectedUser.Role = Role;
                await _UserRepository.UpdateUserAsync(SelectedUser);
            }

            IsPopupVisible = false;
            LoadUsers();
        }

        [RelayCommand]
        private void CancelEmployee()
        {
            SelectedUser = null;
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Role = string.Empty;
            IsPopupVisible = true;
        }

        [RelayCommand]
        private async Task RoleTapped(string role)
        {
            if (role != null)
            {
                SelectedUserRole = role;
                var filteredRoles = await _UserRepository.GetUsersByRoleAsync(role);
                Users = new ObservableCollection<User>(filteredRoles);
                Title = role;
            }
            else
            {
                LoadUsers();
            }
        }
        #endregion
    }
}
