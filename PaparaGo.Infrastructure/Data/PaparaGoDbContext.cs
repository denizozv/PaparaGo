using Microsoft.EntityFrameworkCore;
using PaparaGo.Domain.Entities;

namespace PaparaGo.Infrastructure.Data
{
    public class PaparaGoDbContext : DbContext
    {
        public PaparaGoDbContext(DbContextOptions<PaparaGoDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ExpenseRequest> ExpenseRequests => Set<ExpenseRequest>();
        public DbSet<PaymentLog> PaymentLogs => Set<PaymentLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enums are integer
            modelBuilder
                .Entity<User>()
                .Property(u => u.Role)
                .HasConversion<int>();

            modelBuilder
                .Entity<ExpenseRequest>()
                .Property(e => e.Status)
                .HasConversion<int>();

            // Optional Fluent Config
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Soft delete 
            modelBuilder.Entity<User>().HasQueryFilter(u => u.IsActive);
            modelBuilder.Entity<Category>().HasQueryFilter(c => c.DeletedAt == null);

        }
    }
}
