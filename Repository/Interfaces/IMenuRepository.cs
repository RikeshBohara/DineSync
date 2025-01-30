using DineSync.Models;

namespace DineSync.Repository.Interfaces
{
    public interface IMenuRepository
    {
        Task<Menu[]> GetAllMenuAsync();
        Task<int> AddNewMenuAsync(Menu menu);
        Task<int> RemoveMenuAsync(Menu menu);
        Task<MenuCategoryMapping> GetMenuCategoryMappingAsync(int menuId);
        Task AddMenuCategoryMappingAsync(MenuCategoryMapping mapping);
        Task<Menu[]> GetMenusByCategoryAsync(int categoryId);
        Task<Menu> CheckIfMenuExistsAsync(string name, decimal price, string description, int categoryId);
    }
}
