using Microsoft.EntityFrameworkCore;
using PetProject.Model;

namespace PetProject.Data
{
    public sealed class ToDoDB(IConfiguration configuration, DbContextOptions<ToDoDB> opts) : DbContext(opts)
    {
        public DbSet<ToDo> ToDos { get; init; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString(GetType().Name));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDo>(entity =>
            {
                entity.HasKey(item => item.Id);
                entity.Property(item => item.Content);
                entity.Property(item => item.IsDone);
            });
        }
    }
}
