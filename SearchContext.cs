using Microsoft.EntityFrameworkCore;
using AspNetCoreSearchApp.Models;

namespace AspNetCoreSearchApp.Data
{
    public class SearchContext : DbContext
    {
        public SearchContext(DbContextOptions<SearchContext> options) : base(options)
        {
        }

        public DbSet<JsonDoc> JsonDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Optionally define constraints or relationships
            modelBuilder.Entity<JsonDoc>()
                .Property(j => j.JsonData)
                .HasColumnType("TEXT"); // SQLite stores JSON as TEXT or CLOB
        }
    }
}
