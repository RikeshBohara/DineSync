using DineSync.Models;

namespace DineSync.Repository.Interfaces
{
    public interface IMenuCategoryRepository
    {
        Task<List<MenuCategory>> GetMenuCategoriesAsync();
        Task<int> AddMenuCategoryAsync(MenuCategory menuCategory);
        Task<int> RemoveMenuCategoryWithItemsAsync(MenuCategory menuCategory);
        Task<MenuCategory> CheckIfCategoryExistsAsync(string name);
    }
}
