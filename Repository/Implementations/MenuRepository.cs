using DineSync.Data;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using SQLite;

namespace DineSync.Repository.Implementations
{
    public class MenuRepository : IMenuRepository
    {
        private readonly SQLiteAsyncConnection _Connection;

        public MenuRepository(DbConfig dbConfig)
        {
            _Connection = dbConfig.GetConnection();
        }

        public async Task<Menu[]> GetAllMenuAsync()
        {
            return await _Connection.Table<Menu>().ToArrayAsync();
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

        public async Task<Menu[]> GetMenusByCategoryAsync(int categoryId)
        {
            var mappings = await _Connection.Table<MenuCategoryMapping>()
                                            .Where(menu => menu.MenuCategoryId == categoryId)
                                            .ToArrayAsync();

            var menuIds = mappings.Select(menu => menu.MenuId).ToList();

            if (!menuIds.Any())
                return Array.Empty<Menu>();

            var menus = await _Connection.Table<Menu>()
                                         .Where(menu => menuIds.Contains(menu.Id))
                                         .ToArrayAsync();

            return menus.ToArray();
        }

        public async Task<MenuCategoryMapping> GetMenuCategoryMappingAsync(int menuId)
        {
            return await _Connection.Table<MenuCategoryMapping>().FirstOrDefaultAsync(menu => menu.MenuId == menuId);
        }

        public async Task<Menu> CheckIfMenuExistsAsync(string name, decimal price, string description, int categoryId)
        {
            var categoryMappings = await _Connection.Table<MenuCategoryMapping>()
                .Where(m => m.MenuCategoryId == categoryId)
                .ToArrayAsync();

            if (!categoryMappings.Any())
                return null;

            var menuIdsInCategory = categoryMappings.Select(menu => menu.MenuId).ToList();

            return await _Connection.Table<Menu>()
                .Where(menu => menu.Name == name &&
                           menu.Price == price &&
                           menu.Description == description &&
                           menuIdsInCategory.Contains(menu.Id))
                .FirstOrDefaultAsync();
        }
    }
}
