using Microsoft.EntityFrameworkCore;
using RealEstateApi.Models;

namespace RealEstateApi.Data
{
    public class ApiDbContext: DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
        public DbSet<Models.Property> Properties { get; set; }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.User> Users { get; set; }
    }
}
