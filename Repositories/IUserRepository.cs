using RealEstateApi.Models;

namespace RealEstateApi.Repositories
{
        public interface IUserRepository
        {
            
            Task<User?> GetUserByEmailAsync(string email);
            Task<bool> UserExistsAsync(string email);
            Task AddUserAsync(User user);
            Task SaveChangesAsync();
        }
    }
