using Microsoft.EntityFrameworkCore;
using RealEstateApi.Data;
using RealEstateApi.Models;

namespace RealEstateApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApiDbContext _context;
        public CategoryRepository(ApiDbContext context) { 
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
