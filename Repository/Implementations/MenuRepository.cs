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
    }
}
