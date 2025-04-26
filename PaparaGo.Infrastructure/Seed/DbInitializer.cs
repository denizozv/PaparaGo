using PaparaGo.Domain.Entities;
using PaparaGo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PaparaGo.Infrastructure.Seed
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(PaparaGoDbContext context)
        {

            await context.Database.MigrateAsync();

            // Users
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new()
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "admin@paparago.com",
                        PasswordHash = "123456",
                        IBAN = "TR000000000000000000000001",
                        Role = UserRole.Admin
                    },
                    new()
                    {
                        FirstName = "Ali",
                        LastName = "Personel",
                        Email = "ali@paparago.com",
                        PasswordHash = "123456",
                        IBAN = "TR000000000000000000000002",
                        Role = UserRole.Personel
                    },
                                        new()
                    {
                        FirstName = "Ayşe",
                        LastName = "Personel",
                        Email = "ayse@paparago.com",
                        PasswordHash = "123456",
                        IBAN = "TR000000000000000000000003",
                        Role = UserRole.Personel
                    }
                };

                context.Users.AddRange(users);
            }

            // Categories
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new() { Name = "Yemek", Description = "Yemek harcamaları" },
                    new() { Name = "Ulaşım", Description = "Ulaşım harcamaları" },
                    new() { Name = "Konaklama", Description = "Konaklama harcamaları" }
                };

                context.Categories.AddRange(categories);
            }

            await context.SaveChangesAsync();
        }
    }
}
