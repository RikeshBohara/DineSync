using DineSync.Data;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using SQLite;

namespace DineSync.Repository.Implementations
{
    public class MenuRepository : IMenuRepository
    {
        #region Fields
        private readonly SQLiteAsyncConnection _Connection;
        #endregion

        #region Constructor
        public MenuRepository(DbConfig dbConfig)
        {
            _Connection = dbConfig.GetConnection();
        }
        #endregion

        #region Methods
        public async Task<List<Menu>> GetAllMenuAsync()
        {
            return await _Connection.Table<Menu>().ToListAsync();
        }

        public async Task<int> AddNewMenuAsync(Menu menu)
        {
            return await _Connection.InsertAsync(menu);
        }

        public async Task<int> RemoveMenuAsync(Menu menu)
        {
            return await _Connection.DeleteAsync(menu);
        }

        public async Task AddMenuCategoryMappingAsync(MenuCategoryMapping mapping)
        {
            await _Connection.InsertAsync(mapping);
        }

        public async Task<List<Menu>> GetMenusByCategoryAsync(int categoryId)
        {
            var query = @"select menu.* from Menu menu
                        inner join MenuCategoryMapping mapping on menu.Id = mapping.MenuId
                        where mapping.MenuCategoryId = ?";
            var menus = await _Connection.QueryAsync<Menu>(query, categoryId);
            return menus.ToList();
        }

        public async Task<MenuCategoryMapping> GetMenuCategoryMappingAsync(int menuId)
        {
            return await _Connection.Table<MenuCategoryMapping>().FirstOrDefaultAsync(menu => menu.MenuId == menuId);
        }

        public async Task<Menu> CheckIfMenuExistsAsync(string name, decimal price, string description, int categoryId)
        {
            var query = @"select menu.* 
                        from Menu menu
                        inner join MenuCategoryMapping mapping on menu.Id = mapping.MenuId
                        where mapping.MenuCategoryId = ? 
                        and menu.Name = ? 
                        and menu.Price = ? 
                        and menu.Description = ? 
                        limit 1";
            var menus = await _Connection.QueryAsync<Menu>(query, categoryId, name, price, description);
            return menus.FirstOrDefault();
        }
        #endregion
    }
}
