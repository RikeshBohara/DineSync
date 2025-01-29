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

        public async Task<int> RemoveMenuCategoryAsync(MenuCategory menuCategory)
        {
            return await _Connection.DeleteAsync(menuCategory);
        }
    }
}
