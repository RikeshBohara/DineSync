using DineSync.Models;

namespace DineSync.Repository.Interfaces
{
    public interface ILoginRepository
    {
        Task<User> LoginAsync(string email, string password);
    }
}
