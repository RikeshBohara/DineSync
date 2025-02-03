using DineSync.Models;

namespace DineSync.Repository.Interfaces
{
    public interface ITableRepository
    {
        Task<Table[]> GetAllTablesAsync();
        Task<int> AddTableAsync(Table table);
        Task<int> RemoveTableAsync(Table table);
        Task<int> UpdateTableAsync(Table table);
        Task<Table> GetTableByIdAsync(int id);
    }
}
