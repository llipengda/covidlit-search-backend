using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Models;

public class DbprojectContext(
    DbContextOptions<DbprojectContext> options
) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder
            .UseNpgsql("Name = ConnectionStrings:DBProject")
            .EnableSensitiveDataLogging();
}
