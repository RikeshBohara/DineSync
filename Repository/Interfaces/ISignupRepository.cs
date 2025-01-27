using DineSync.Models;

namespace DineSync.Repository.Interfaces
{
    public interface ISignupRepository
    {
        Task<int> AddUserAsync(User user);
        Task<User> CheckIfEmailExistsAsync(string email);
    }
}
