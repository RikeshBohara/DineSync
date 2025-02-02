using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using DineSync.Views.Popups;

namespace DineSync.ViewModels
{
    public partial class MenuPageViewModel : ObservableObject, IQueryAttributable
    {
        #region Fields
        private readonly IMenuCategoryRepository _MenuCategoryRepository;

        private readonly IMenuRepository _MenuRepository;

        private readonly IUserRepository _UserRepository;

        private readonly IOrderItemRepository _OrderItemRepository;

        [ObservableProperty]
        private ObservableCollection<MenuCategory> _MenuCategories;

        [ObservableProperty]
        private ObservableCollection<Menu> _Menus;

        [ObservableProperty]
        private string _NewMenuCategory;

        [ObservableProperty]
        private MenuCategory _SelectedCategory;

        [ObservableProperty]
        private Menu _SelectedMenu;

        [ObservableProperty]
        private string _SelectedImagePath;

        [ObservableProperty]
        private string _Name;

        [ObservableProperty]
        private decimal _Price;

        [ObservableProperty]
        private string _Description;

        [ObservableProperty]
        private bool _IsImageAdded = true;

        [ObservableProperty]
        private MenuCategory _SelectedMenuCategory;

        [ObservableProperty]
        private Table _SelectedTable;

        [ObservableProperty]
        private ObservableCollection<OrderItem> _CurrentOrderItems;

        [ObservableProperty]
        private Order _CurrentOrder;

        [ObservableProperty]
        private string _OrderNumber;

        [ObservableProperty]
        private int _CurrentQuantity = 1;

        private Popup _AddMenuCategoryPopup;
        #endregion

        #region Constructor
        public MenuPageViewModel(IMenuCategoryRepository menuCategoryRepository, IMenuRepository menuRepository, IUserRepository userRepository, IOrderItemRepository orderItemRepository)
        {
            _MenuCategoryRepository = menuCategoryRepository;
            _MenuRepository = menuRepository;
            _UserRepository = userRepository;
            _OrderItemRepository = orderItemRepository;
            MenuCategories = new ObservableCollection<MenuCategory>();
            Menus = new ObservableCollection<Menu>();
            CurrentOrderItems = new ObservableCollection<OrderItem>();
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
            IsImageAdded = true;
            if (string.IsNullOrEmpty(NewMenuCategory))
            {
                await Shell.Current.DisplayAlert("Error", "Category Name cannot be empty", "OK");
            }
            var existingCategory = await _MenuCategoryRepository.CheckIfCategoryExistsAsync(Name);
            if (existingCategory != null)
            {
                await Shell.Current.DisplayAlert("Error", "This Category already exists. Please add a different Category", "OK");
                Name = string.Empty;
                return;
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
        private async Task ShowMoreMenu(MenuCategory menuCategory)
        {
            if (menuCategory == null) return;

            var result = await Shell.Current.DisplayActionSheet(
                $"Category: {menuCategory.Name}",
                "Cancel",
                "Delete");

            if (result == "Delete")
            {
                await RemoveMenuCategory(menuCategory);
            }
        }

        [RelayCommand]
        public async Task RemoveMenuCategory(MenuCategory menuCategory)
        {
            if (menuCategory == null)
                return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Category",
                $"Are you sure you want to delete the category '{menuCategory.Name}' and all its associated menu items?",
                "Yes", "No");

            if (!confirm)
                return;

            await _MenuCategoryRepository.RemoveMenuCategoryWithItemsAsync(menuCategory);

            MenuCategories.Remove(menuCategory);

            LoadMenu();
        }

        [RelayCommand]
        public async Task PickImage()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result != null)
                {
                    var localFilePath = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
                    using var stream = await result.OpenReadAsync();
                    using var fileStream = File.OpenWrite(localFilePath);
                    await stream.CopyToAsync(fileStream);

                    SelectedImagePath = localFilePath;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsImageAdded = false;
            }
        }

        [RelayCommand]
        public async Task SaveMenuItem()
        {
            if (SelectedCategory == null || string.IsNullOrEmpty(Name) || Price <= 0)
            {
                await Shell.Current.DisplayAlert("Error", "Please fill all required fields", "OK");
                return;
            }

            var newMenu = new Menu
            {
                Name = Name,
                Price = Price,
                Description = Description,
                Image = SelectedImagePath
            };

            var existingMenu = await _MenuRepository.CheckIfMenuExistsAsync(Name, Price, Description, SelectedCategory.Id);
            if (existingMenu != null)
            {
                await Shell.Current.DisplayAlert("Error", "This Menu Item already exists. Please add a different item", "OK");
                Name = string.Empty;
                Price = 0;
                Description = string.Empty;
                SelectedImagePath = string.Empty;
                SelectedCategory = null;
                IsImageAdded = true;
                return;
            }

            await _MenuRepository.AddNewMenuAsync(newMenu);

            var mapping = new MenuCategoryMapping
            {
                MenuCategoryId = SelectedCategory.Id,
                MenuId = newMenu.Id
            };

            await _MenuRepository.AddMenuCategoryMappingAsync(mapping);

            Name = string.Empty;
            Price = 0;
            Description = string.Empty;
            SelectedImagePath = string.Empty;
            SelectedCategory = null;
            IsImageAdded = true;
            LoadMenu();
        }

        [RelayCommand]
        private void MenuItemTapped(Menu menu)
        {
            var currentOrderItem = CurrentOrderItems.FirstOrDefault(item => item.MenuItemId == menu.Id);

            if (currentOrderItem != null)
            {
                currentOrderItem.Quantity++;
            }
            else
            {
                currentOrderItem = new OrderItem
                {
                    MenuItemId = menu.Id,
                    Name = menu.Name,
                    Price = menu.Price,
                    Quantity = CurrentQuantity,
                    Icon = menu.Image
                };
                CurrentOrderItems.Add(currentOrderItem);
            }
            UpdateOrderTotal();
        }

        [RelayCommand]
        private void IncrementQuantity(OrderItem orderItem)
        {
            if (orderItem != null)
            {
                orderItem.Quantity++;
                UpdateOrderTotal();
            }
        }

        [RelayCommand]
        private void DecrementQuantity(OrderItem orderItem)
        {
            if (orderItem != null && orderItem.Quantity > 1)
            {
                orderItem.Quantity--;
                UpdateOrderTotal();
            }
            else
            {
                CurrentOrderItems.Remove(orderItem);
                UpdateOrderTotal();
            }
        }

        private void UpdateOrderTotal()
        {
            if (CurrentOrder == null)
                CurrentOrder = new Order();

            CurrentOrder.TotalAmount = CurrentOrderItems.Sum(item => item.Price * item.Quantity);
            OnPropertyChanged(nameof(CurrentOrder));
        }

        [RelayCommand]
        private async void CancelOrder()
        {
            CurrentOrderItems.Clear();
            UpdateOrderTotal();
        }

        [RelayCommand]
        private async Task RemoveMenu()
        {
            if (SelectedMenu != null)
            {
                await _MenuRepository.RemoveMenuAsync(SelectedMenu);
                Menus.Remove(SelectedMenu);
                SelectedMenu = null;
                Name = string.Empty;
                Price = 0;
                Description = string.Empty;
                SelectedImagePath = string.Empty;
                SelectedCategory = null;
                IsImageAdded = true;
                LoadMenu();
            }
        }

        [RelayCommand]
        public async Task CategoryTapped(MenuCategory category)
        {
            if (category != null)
            {
                SelectedMenuCategory = category;
                var filteredMenus = await _MenuRepository.GetMenusByCategoryAsync(category.Id);
                Menus = new ObservableCollection<Menu>(filteredMenus);
            }
            else
            {
                LoadMenu();
            }
        }

        [RelayCommand]
        public async Task LoadAllMenu()
        {
            SelectedMenuCategory = null;

            var menus = await _MenuRepository.GetAllMenuAsync();
            Menus = new ObservableCollection<Menu>(menus);
        }

        [RelayCommand]
        public async Task EditMenu(Menu menu)
        {
            if (menu == null)
                return;

            var mapping = await _MenuRepository.GetMenuCategoryMappingAsync(menu.Id);
            if (mapping != null)
            {
                SelectedMenu = menu;
                SelectedImagePath = menu.Image;
                Name = menu.Name;
                Price = menu.Price;
                SelectedCategory = MenuCategories.FirstOrDefault(category => category.Id == mapping.MenuCategoryId);
                Description = menu.Description;
            }
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

        private async Task LoadWaiterNameAsync()
        {
            if (SelectedTable?.AssignedWaiterId > 0)
            {
                var waiter = await _UserRepository.GetUserByIdAsync(SelectedTable.AssignedWaiterId);
                SelectedTable.WaiterName = waiter?.Name ?? "Unassigned";
                OnPropertyChanged(nameof(SelectedTable));
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("SelectedTable", out var table) && table is Table selected)
            {
                SelectedTable = selected;
                if (SelectedTable.AssignedWaiterId > 0)
                {
                    var waiter = _UserRepository.GetUserByIdAsync(SelectedTable.AssignedWaiterId);
                    _ = LoadWaiterNameAsync();
                }
                OnPropertyChanged(nameof(SelectedTable));
            }
        }
        #endregion
    }
}
