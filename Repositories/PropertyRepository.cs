using Microsoft.EntityFrameworkCore;
using RealEstateApi.Data;
using RealEstateApi.Models;

namespace RealEstateApi.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly ApiDbContext _context;
        public PropertyRepository(ApiDbContext context)
        {
            _context = context;
        }



        public async Task<Models.Property?> GetPropertyByIdAsync(int id)
        {
            return await _context.Properties.FindAsync(id);
        }

   
        

        public async Task<IEnumerable<Models.Property>> SearchPropertiesAsync(string query)
        {
            return await _context.Properties
                .Where(p => p.Name.Contains(query) || p.Address.Contains(query))
                .ToListAsync();
        }

        public async Task AddPropertyAsync(Models.Property property)
        {
            await _context.Properties.AddAsync(property);
            await _context.SaveChangesAsync(); // Ensure changes are saved
        }

        public async Task UpdatePropertyAsync(Models.Property property)
        {
            _context.Properties.Update(property);
            await _context.SaveChangesAsync(); // Ensure changes are saved
        }

        public async Task DeletePropertyAsync(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property != null)
            {
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync(); // Ensure changes are saved
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesAsync(int categoryId)
        {
            return await _context.Properties.Where(p => p.CategoryId == categoryId).ToListAsync();
        }


        public async Task<IEnumerable<Property>> GetTrendingProperties()
        {
            return await _context.Properties.Where(p => p.IsTrending == true).ToListAsync();
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }
    }
}
