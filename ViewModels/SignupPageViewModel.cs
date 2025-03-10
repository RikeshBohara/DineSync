﻿using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DineSync.Models;
using DineSync.Repository.Interfaces;

namespace DineSync.ViewModels
{
    public partial class SignupPageViewModel: ObservableObject
    {
        #region Fields
        private readonly ISignupRepository _SignupRepository;
        #endregion

        #region Properties
        [ObservableProperty]
        private bool _IsPasswordVisible = true;

        [ObservableProperty]
        private string _PasswordVisibilityIcon = "view.png";

        [ObservableProperty]
        private bool _IsPassword2Visible = true;

        [ObservableProperty]
        private string _Password2VisibilityIcon = "view.png";

        [ObservableProperty]
        private string _Name;

        [ObservableProperty]
        private string _Email;

        [ObservableProperty]
        private string _Password;

        [ObservableProperty]
        private string _ConfirmPassword;
        #endregion

        #region Constructor
        public SignupPageViewModel(ISignupRepository signupRepository)
        {
            _SignupRepository = signupRepository;
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
        public void TogglePassword2Visibility()
        {
            IsPassword2Visible = !IsPassword2Visible;
            Password2VisibilityIcon = IsPassword2Visible
                ? "view.png"
                : "hide.png";
        }

        [RelayCommand]
        public async Task NavigateToLogin()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task SignupAsync()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
            {
                await Shell.Current.DisplayAlert("Signup Failed", "Enter all fields", "OK");
                return;
            }

            if (!IsValidDineSyncEmail(Email))
            {
                await Shell.Current.DisplayAlert("Signup Failed", "Please use a valid @dinesync.com email address.", "OK");
                return;
            }

            if(Password.Length < 6)
            {
                await Shell.Current.DisplayAlert("Signup Failed", "Passwords must be at least 6 characters long", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await Shell.Current.DisplayAlert("Signup Failed", "Passwords don't match", "OK");
                return;
            }

            var existingUser = await _SignupRepository.CheckIfEmailExistsAsync(Email);
            if (existingUser != null)
            {
                await Shell.Current.DisplayAlert("Duplicate User", "This email is already registered. Please use a different email", "OK");
                return;
            }

            await _SignupRepository.AddUserAsync(new User
            {
                Name = Name,
                Email = Email,
                Password = Password
            });

            await Shell.Current.DisplayAlert("Success", "User registered successfully!", "OK");

            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;

            await NavigateToLogin();
        }
        public bool IsValidDineSyncEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@dinesync\.com$");
        }
        #endregion
    }
}
