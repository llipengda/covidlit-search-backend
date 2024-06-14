using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Models;

public class DbprojectContext(
    DbContextOptions<DbprojectContext> options,
    IConfiguration configuration
) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder
            .UseNpgsql(configuration["ConnectionStrings:DBProject"])
            .EnableSensitiveDataLogging(true);
}
