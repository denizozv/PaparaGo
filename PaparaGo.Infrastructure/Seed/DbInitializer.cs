using Microsoft.EntityFrameworkCore;
using PaparaGo.Domain.Entities;
using PaparaGo.Infrastructure.Data;

namespace PaparaGo.Infrastructure.Seed;

public static class DbInitializer
{
    public static async Task SeedAsync(PaparaGoDbContext context)
    {
        await context.Database.MigrateAsync();

        var conn = context.Database.GetDbConnection();
        await conn.OpenAsync();

        var viewCommands = new List<string>
        {
            // Şirket Bazlı Haftalık Rapor View
            """
            CREATE OR REPLACE VIEW vw_company_weekly_expense_summary AS
            SELECT 
                DATE_TRUNC('week', "RequestDate") AS "Week",
                COUNT(*) AS "ExpenseCount",
                SUM("Amount") AS "TotalAmount",
                COUNT(CASE WHEN "Status" = 1 THEN 1 END) AS "ApprovedCount",
                COUNT(CASE WHEN "Status" = 2 THEN 1 END) AS "RejectedCount"
            FROM "ExpenseRequests"
            GROUP BY 1
            ORDER BY "Week" DESC;
            """,

            // Şirket Bazlı Aylık Rapor View
            """
            CREATE OR REPLACE VIEW vw_company_monthly_expense_summary AS
            SELECT 
                TO_CHAR("RequestDate", 'YYYY-MM') AS "Month",
                COUNT(*) AS "ExpenseCount",
                SUM("Amount") AS "TotalAmount",
                COUNT(CASE WHEN "Status" = 1 THEN 1 END) AS "ApprovedCount",
                COUNT(CASE WHEN "Status" = 2 THEN 1 END) AS "RejectedCount"
            FROM "ExpenseRequests"
            GROUP BY 1
            ORDER BY "Month" DESC;
            """,

            // Şirket Bazlı Günlük Rapor View
            """
            CREATE OR REPLACE VIEW vw_company_daily_expense_summary AS
            SELECT 
                DATE("RequestDate") AS "Date",
                COUNT(*) AS "ExpenseCount",
                SUM("Amount") AS "TotalAmount",
                COUNT(CASE WHEN "Status" = 1 THEN 1 END) AS "ApprovedCount",
                COUNT(CASE WHEN "Status" = 2 THEN 1 END) AS "RejectedCount"
            FROM "ExpenseRequests"
            GROUP BY 1
            ORDER BY "Date" DESC;
            """,

            // Kategori Dağılımı
            """
            CREATE OR REPLACE VIEW vw_categorydistribution AS
            SELECT 
                c."Name" AS "CategoryName",
                SUM(e."Amount") AS "TotalAmount"
            FROM "ExpenseRequests" e
            JOIN "Categories" c ON e."CategoryId" = c."Id"
            WHERE e."Status" = 1
            GROUP BY c."Name"
            ORDER BY "TotalAmount" DESC;
            """,

            // En Çok Harcayanlar
            """
            CREATE OR REPLACE VIEW vw_topspenders AS
            SELECT 
                u."FirstName" || ' ' || u."LastName" AS "UserFullName",
                SUM(e."Amount") AS "TotalSpent",
                COUNT(*) AS "ExpenseCount"
            FROM "ExpenseRequests" e
            JOIN "Users" u ON e."UserId" = u."Id"
            WHERE e."Status" = 1
            GROUP BY u."FirstName", u."LastName"
            ORDER BY "TotalSpent" DESC;
            """,

            // Günlük Ortalama Harcamalar
            """
            CREATE OR REPLACE VIEW vw_dailyaverageexpense AS
            SELECT 
                DATE("RequestDate") AS "Date",
                AVG(e."Amount") AS "AverageAmount"
            FROM "ExpenseRequests" e
            WHERE e."Status" = 1
            GROUP BY DATE("RequestDate")
            ORDER BY "Date" DESC;
            """
        };

        foreach (var sql in viewCommands)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            await cmd.ExecuteNonQueryAsync();
        }

        await conn.CloseAsync();

        // Varsayılan kullanıcılar
        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new() { FirstName = "Admin", LastName = "User", Email = "admin@paparago.com", PasswordHash = "123456", IBAN = "TR000000000000000000000001", Role = UserRole.Admin },
                new() { FirstName = "Ali", LastName = "Personel", Email = "ali@paparago.com", PasswordHash = "123456", IBAN = "TR000000000000000000000002", Role = UserRole.Personel },
                new() { FirstName = "Veli", LastName = "Personel", Email = "veli@paparago.com", PasswordHash = "123456", IBAN = "TR000000000000000000000003", Role = UserRole.Personel },
                new() { FirstName = "Ayşe", LastName = "Personel", Email = "ayse@paparago.com", PasswordHash = "123456", IBAN = "TR000000000000000000000004", Role = UserRole.Personel }
            };
            context.Users.AddRange(users);
        }

        // Varsayılan kategoriler
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
