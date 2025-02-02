using DineSync.Models;

namespace DineSync.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User[]> GetAllUsersAsync();
        Task<int> AddUserAsync(User user);
        Task<int> RemoveUserAsync(User user);
        Task<int> UpdateUserAsync(User user);
        Task<User> GetUserByIdAsync(int id);
    }
}
