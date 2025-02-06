using DineSync.Models;

namespace DineSync.Repository.Interfaces
{
    public interface IMenuRepository
    {
        Task<List<Menu>> GetAllMenuAsync();
        Task<int> AddNewMenuAsync(Menu menu);
        Task<int> RemoveMenuAsync(Menu menu);
        Task<MenuCategoryMapping> GetMenuCategoryMappingAsync(int menuId);
        Task AddMenuCategoryMappingAsync(MenuCategoryMapping mapping);
        Task<List<Menu>> GetMenusByCategoryAsync(int categoryId);
        Task<Menu> CheckIfMenuExistsAsync(string name, decimal price, string description, int categoryId);
        Task<List<Menu>> GetSearchedMenuAsync(string menu);
    }
}
