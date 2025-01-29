using DineSync.Models;

namespace DineSync.Repository.Interfaces
{
    public interface IMenuRepository
    {
        Task<Menu[]> GetAllMenuAsync();
        Task<int> AddNewMenuAsync(Menu menu);
        Task<int> RemoveMenuAsync(Menu menu);
    }
}
