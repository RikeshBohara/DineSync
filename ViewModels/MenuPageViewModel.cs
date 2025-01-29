using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using DineSync.Views.Popups;

namespace DineSync.ViewModels
{
    public partial class MenuPageViewModel : ObservableObject
    {
        #region Fields
        private readonly IMenuCategoryRepository _MenuCategoryRepository;

        private readonly IMenuRepository _MenuRepository;

        [ObservableProperty]
        private ObservableCollection<MenuCategory> _MenuCategories;

        [ObservableProperty]
        private ObservableCollection<Menu> _Menus;

        [ObservableProperty]
        private string _NewMenuCategory;

        [ObservableProperty]
        private MenuCategory _SelectedCategory;

        private Popup _AddMenuCategoryPopup;
        #endregion

        #region Constructor
        public MenuPageViewModel(IMenuCategoryRepository menuCategoryRepository, IMenuRepository menuRepository)
        {
            _MenuCategoryRepository = menuCategoryRepository;
            _MenuRepository = menuRepository;
            MenuCategories = new ObservableCollection<MenuCategory>();
            Menus = new ObservableCollection<Menu>();
            LoadMenuCategory();
            LoadMenu();
        }
        #endregion

        #region Methods
        public async void LoadMenuCategory()
        {
            var categories = await _MenuCategoryRepository.GetMenuCategoriesAsync();
            MenuCategories = new ObservableCollection<MenuCategory>(categories);
        }

        public async void LoadMenu()
        {
            var menus = await _MenuRepository.GetAllMenuAsync();
            Menus = new ObservableCollection<Menu>(menus);
        }

        [RelayCommand]
        public async Task AddMenuCategory()
        {
            if (string.IsNullOrEmpty(NewMenuCategory))
            {
                await Shell.Current.DisplayAlert("Error", "Category Name cannot be empty", "OK");
            }
            else
            {
                await AddNewMenuCategory(NewMenuCategory);
                NewMenuCategory = string.Empty;
                _AddMenuCategoryPopup?.Close();
                LoadMenuCategory(); 
            }
        }

        [RelayCommand]
        public async Task AddNewMenuCategory(string name)
        {
            var newMenuCategory = new MenuCategory
            {
                Name = name
            };
            await _MenuCategoryRepository.AddMenuCategoryAsync(newMenuCategory);
            LoadMenuCategory();
        }

        [RelayCommand]
        public void MenuCategoryPopup(MenuCategory menuCategory)
        {
            var popup = new AddMenuCategoryPopup
            {
                BindingContext = this
            };
            _AddMenuCategoryPopup = popup;
            Shell.Current.ShowPopup(popup);
        }
        #endregion
    }
}
