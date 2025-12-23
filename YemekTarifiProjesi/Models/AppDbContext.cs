using Microsoft.EntityFrameworkCore;

namespace YemekTarifiProjesi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tariflerin kaydedileceği tablo
        public DbSet<Recipe> Recipes { get; set; }
    }
}