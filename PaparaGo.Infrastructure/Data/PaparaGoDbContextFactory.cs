using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PaparaGo.Infrastructure.Data;

public class PaparaGoDbContextFactory : IDesignTimeDbContextFactory<PaparaGoDbContext>
{
    public PaparaGoDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<PaparaGoDbContext>();
        optionsBuilder.UseNpgsql(
            configuration.GetConnectionString("DefaultConnection"),
            x => x.MigrationsAssembly("PaparaGo.WebAPI") // ← 
        );

        return new PaparaGoDbContext(optionsBuilder.Options);
    }
}
