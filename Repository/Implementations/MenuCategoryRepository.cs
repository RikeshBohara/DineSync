using DineSync.Data;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using SQLite;

namespace DineSync.Repository.Implementations
{
    public class MenuCategoryRepository : IMenuCategoryRepository
    {
        #region Fields
        private readonly SQLiteAsyncConnection _Connection;
        #endregion

        #region Constructor
        public MenuCategoryRepository(DbConfig dbConfig)
        {
            _Connection = dbConfig.GetConnection();
        }
        #endregion

        #region Methods
        public async Task<List<MenuCategory>> GetMenuCategoriesAsync()
        {
            return await _Connection.Table<MenuCategory>().ToListAsync();
        }

        public async Task<int> AddMenuCategoryAsync(MenuCategory menuCategory)
        {
            return await _Connection.InsertAsync(menuCategory);
        }

        public async Task<int> RemoveMenuCategoryWithItemsAsync(MenuCategory menuCategory)
        {
            var deleteMenusQuery = @"DELETE FROM Menu 
                                   WHERE Id IN (SELECT MenuId FROM MenuCategoryMapping 
                                                 WHERE MenuCategoryId = ?)";
            var deleteCategoryQuery = @"DELETE FROM MenuCategory 
                                      WHERE Id = ?";
            await _Connection.ExecuteAsync(deleteMenusQuery, menuCategory.Id);
            return await _Connection.ExecuteAsync(deleteCategoryQuery, menuCategory.Id);
        }

        public async Task<MenuCategory> CheckIfCategoryExistsAsync(string name)
        {
            return await _Connection.Table<MenuCategory>().FirstOrDefaultAsync(category => category.Name == name);
        }
        #endregion
    }
}
