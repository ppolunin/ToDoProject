using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetProject.Model;

namespace PetProject.Data
{
    public sealed class MainDB(DbContextOptions<MainDB> opts) : DbContext(opts)
    {
        public DbSet<ToDoRecord> Items { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoRecord>(entity =>
            {
                entity.HasKey(item => item.Id);
                entity.Property(item => item.Content);
                entity.Property(item => item.IsDone);
            });
        }
    }
}
