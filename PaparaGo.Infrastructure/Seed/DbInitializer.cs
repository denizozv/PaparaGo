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

        // First, drop any existing views to avoid conflicts
        var dropViewCommands = new List<string>
        {
            "DROP VIEW IF EXISTS vw_company_weekly_expense_summary;",
            "DROP VIEW IF EXISTS vw_company_monthly_expense_summary;",
            "DROP VIEW IF EXISTS vw_company_daily_expense_summary;",
            "DROP VIEW IF EXISTS vw_personal_weekly_expense_summary;",
            "DROP VIEW IF EXISTS vw_personal_daily_expense_summary;",
            "DROP VIEW IF EXISTS vw_personal_monthly_expense_summary;",
            "DROP VIEW IF EXISTS vw_categorydistribution;",
            "DROP VIEW IF EXISTS vw_topspenders;",
            "DROP VIEW IF EXISTS vw_dailyaverageexpense;"
        };

        foreach (var sql in dropViewCommands)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            await cmd.ExecuteNonQueryAsync();
        }

        var viewCommands = new List<string>
        {
            // Şirket Bazlı Haftalık Rapor
            """
            CREATE VIEW vw_company_weekly_expense_summary AS
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

            // Şirket Bazlı Aylık Rapor
            """
            CREATE VIEW vw_company_monthly_expense_summary AS
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

            // Şirket Bazlı Günlük Rapor
            """
            CREATE VIEW vw_company_daily_expense_summary AS
            SELECT 
                COALESCE(DATE("RequestDate"), CURRENT_DATE) AS "Date",
                COUNT(*) AS "ExpenseCount",
                SUM("Amount") AS "TotalAmount",
                COUNT(CASE WHEN "Status" = 1 THEN 1 END) AS "ApprovedCount",
                COUNT(CASE WHEN "Status" = 2 THEN 1 END) AS "RejectedCount"
            FROM "ExpenseRequests"
            GROUP BY 1
            ORDER BY "Date" DESC;
            """,

              // Personel Bazlı Günlük Rapor
            """
            CREATE VIEW vw_personal_daily_expense_summary AS
            SELECT 
                u."Id" AS "UserId",
                (u."FirstName" || ' ' || u."LastName") AS "UserFullName",
                COALESCE(DATE(e."RequestDate"), CURRENT_DATE) AS "Date",
                COUNT(e."Id") AS "ExpenseCount",
                COALESCE(SUM(e."Amount"), 0) AS "TotalAmount",
                COUNT(CASE WHEN e."Status" = 1 THEN 1 END) AS "ApprovedCount",
                COUNT(CASE WHEN e."Status" = 2 THEN 1 END) AS "RejectedCount"
            FROM 
                "Users" u
            JOIN 
                "ExpenseRequests" e ON u."Id" = e."UserId"
            GROUP BY
                u."Id", u."FirstName", u."LastName", COALESCE(DATE(e."RequestDate"), CURRENT_DATE)
            ORDER BY 
                "Date" DESC, "TotalAmount" DESC;
            """,

            // Personel Bazlı Haftalık Rapor
            """
            CREATE VIEW vw_personal_weekly_expense_summary AS
            SELECT
                u."Id" AS "UserId",
                (u."FirstName" || ' ' || u."LastName") AS "UserFullName",
                COALESCE(DATE_TRUNC('week', e."RequestDate"), DATE_TRUNC('week', CURRENT_DATE)) AS "Week",
                COUNT(e."Id") AS "ExpenseCount",
                COALESCE(SUM(e."Amount"), 0) AS "TotalAmount",
                COUNT(CASE WHEN e."Status" = 1 THEN 1 END) AS "ApprovedCount",
                COUNT(CASE WHEN e."Status" = 2 THEN 1 END) AS "RejectedCount"
            FROM
                "Users" u
            JOIN
                "ExpenseRequests" e ON u."Id" = e."UserId"
            GROUP BY
                u."Id", u."FirstName", u."LastName", COALESCE(DATE_TRUNC('week', e."RequestDate"), DATE_TRUNC('week', CURRENT_DATE))
            ORDER BY
                "Week" DESC, "TotalAmount" DESC;
            """,

            // Personel Bazlı Aylık Rapor
            """
            CREATE VIEW vw_personal_monthly_expense_summary AS
            SELECT 
                e."UserId",
                u."FirstName" || ' ' || u."LastName" AS "UserFullName",
                TO_CHAR(COALESCE(e."RequestDate", CURRENT_DATE), 'YYYY-MM') AS "Month",
                COUNT(*) AS "ExpenseCount",
                COALESCE(SUM(e."Amount"), 0) AS "TotalAmount",
                COUNT(CASE WHEN e."Status" = 1 THEN 1 END) AS "ApprovedCount",
                COUNT(CASE WHEN e."Status" = 2 THEN 1 END) AS "RejectedCount"
            FROM "ExpenseRequests" e
            JOIN "Users" u ON e."UserId" = u."Id"
            GROUP BY 1, 2, TO_CHAR(COALESCE(e."RequestDate", CURRENT_DATE), 'YYYY-MM')
            ORDER BY "Month" DESC;
            """,

            // Kategori Dağılımı
            """
            CREATE VIEW vw_categorydistribution AS
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
            CREATE VIEW vw_topspenders AS
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
            CREATE VIEW vw_dailyaverageexpense AS
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

        // Varsayılan Kullanıcılar
        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new() { FirstName = "Admin", LastName = "User", Email = "admin@paparago.com", PasswordHash = "123456", IBAN = "TR000000000000000000000001", Role = UserRole.Admin },
                new() { FirstName = "Deniz", LastName = "Personel", Email = "deniz@paparago.com", PasswordHash = "123456", IBAN = "TR00000000000000000000033", Role = UserRole.Personel },
                new() { FirstName = "Veli", LastName = "Personel", Email = "veli@paparago.com", PasswordHash = "123456", IBAN = "TR000000000000000000000003", Role = UserRole.Personel },
                new() { FirstName = "Ayşe", LastName = "Personel", Email = "ayse@paparago.com", PasswordHash = "123456", IBAN = "TR000000000000000000000004", Role = UserRole.Personel }
            };
            context.Users.AddRange(users);
        }

        // Varsayılan Kategoriler
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