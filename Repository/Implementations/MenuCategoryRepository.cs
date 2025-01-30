using DineSync.Data;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using SQLite;

namespace DineSync.Repository.Implementations
{
    public class MenuCategoryRepository : IMenuCategoryRepository
    {
        private readonly SQLiteAsyncConnection _Connection;

        public MenuCategoryRepository(DbConfig dbConfig)
        {
            _Connection = dbConfig.GetConnection();
        }

        public async Task<MenuCategory[]> GetMenuCategoriesAsync()
        {
            return await _Connection.Table<MenuCategory>().ToArrayAsync();
        }

        public async Task<int> AddMenuCategoryAsync(MenuCategory menuCategory)
        {
            return await _Connection.InsertAsync(menuCategory);
        }

        public async Task<int> RemoveMenuCategoryWithItemsAsync(MenuCategory menuCategory)
        {
            var mappings = await _Connection.Table<MenuCategoryMapping>()
                                           .Where(mapping => mapping.MenuCategoryId == menuCategory.Id)
                                           .ToArrayAsync();

            var menuIds = mappings.Select(mapping => mapping.MenuId).ToArray();

            if (menuIds.Any())
            {
                await _Connection.Table<Menu>().DeleteAsync(menu => menuIds.Contains(menu.Id));
            }

            return await _Connection.DeleteAsync(menuCategory);
        }

        public async Task<MenuCategory> CheckIfCategoryExistsAsync(string name)
        {
            return await _Connection.Table<MenuCategory>().FirstOrDefaultAsync(category => category.Name == name);
        }
    }
}
