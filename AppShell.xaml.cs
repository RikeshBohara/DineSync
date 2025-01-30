using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using DineSync.Models;
using DineSync.ViewModels;
using DineSync.Views.Pages;

namespace DineSync
{
    public partial class AppShell : Shell
    {
        private string _Role;
        public string Role
        {
            get => _Role;
            set
            {
                _Role = value;
                OnPropertyChanged();
                UpdateFlyoutItemsVisibility();
            }
        }

        public AppShell()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<LoginPageViewModel, string>(this, "Role", (sender, arg) =>
            {
                Role = arg;
                UpdateFlyoutItemsVisibility();
            });

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
            Routing.RegisterRoute(nameof(EmployeePage), typeof(EmployeePage));
            Routing.RegisterRoute(nameof(EditMenuPage), typeof(EditMenuPage));
            Routing.RegisterRoute(nameof(MenuPage), typeof(MenuPage));
            Routing.RegisterRoute(nameof(OrderPage), typeof(OrderPage));
            Routing.RegisterRoute(nameof(TablePage), typeof(TablePage));
        }

        private void UpdateFlyoutItemsVisibility()
        {
             if (Role == "Owner")
            {
                DashboardPageShellContent.IsVisible = true;
                EmployeePageShellContent.IsVisible = true;
                EditMenuPageShellContent.IsVisible = true;
                TablePageShellContent.IsVisible = true;
                MenuPageShellContent.IsVisible = true;
                OrderPageShellContent.IsVisible = true;
            }
            else if (Role == "Waiter")
            {
                TablePageShellContent.IsVisible = true;
                MenuPageShellContent.IsVisible = true;
                OrderPageShellContent.IsVisible = true;
                DashboardPageShellContent.IsVisible = false;
                EmployeePageShellContent.IsVisible = false;
                EditMenuPageShellContent.IsVisible = false;
            }
            else
            {
                OrderPageShellContent.IsVisible = true;
                TablePageShellContent.IsVisible = false;
                MenuPageShellContent.IsVisible = false;
                DashboardPageShellContent.IsVisible = false;
                EmployeePageShellContent.IsVisible = false;
                EditMenuPageShellContent.IsVisible = false;
            }
        }

        private async void NavigateToLogin(object sender, EventArgs e)
        {
            Role = null;
            await Current.GoToAsync("/LoginPage");
        }
    }
}
