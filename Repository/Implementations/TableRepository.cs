using DineSync.Data;
using DineSync.Models;
using DineSync.Repository.Interfaces;
using SQLite;

namespace DineSync.Repository.Implementations
{
    public class TableRepository : ITableRepository
    {
        private readonly SQLiteAsyncConnection _Connection;

        public TableRepository(DbConfig dbConfig)
        {
            _Connection = dbConfig.GetConnection();
        }

        public async Task<Table[]> GetAllTablesAsync()
        {
            return await _Connection.Table<Table>().ToArrayAsync();
        }

        public async Task<int> AddTableAsync(Table table)
        {
            return await _Connection.InsertAsync(table);
        }

        public async Task<int> RemoveTableAsync(Table table)
        {
            return await _Connection.DeleteAsync(table);
        }

        public async Task<int> UpdateTableAsync(Table table)
        {
            return await _Connection.UpdateAsync(table);
        }
    }
}
