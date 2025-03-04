using RealEstateApi.Models;

namespace RealEstateApi.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
       
    }
}
