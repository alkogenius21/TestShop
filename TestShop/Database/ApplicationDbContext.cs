using Microsoft.EntityFrameworkCore;

namespace TestShop.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Material> Materials { get; set; }
        public DbSet<Models.Seller> Sellers { get; set; }

    }
}
