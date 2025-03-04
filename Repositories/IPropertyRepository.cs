using RealEstateApi.Models;

namespace RealEstateApi.Repositories
{
    public interface IPropertyRepository
    {
        Task<IEnumerable<Property>> GetPropertiesAsync(int categoryId);
        Task<Property?> GetPropertyByIdAsync(int id);
        Task<IEnumerable<Property>> GetTrendingProperties();
        Task<IEnumerable<Property>> SearchPropertiesAsync(string query);
        Task AddPropertyAsync(Property property);
        Task UpdatePropertyAsync(Property property);
        Task DeletePropertyAsync(int id);
        Task SaveChangesAsync();
        Task<bool> CategoryExistsAsync(int categoryId);
        //Task <bool> CategoryExistsAsync(int categoryId);
    }
}
