using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Models;

public partial class DbprojectContext : DbContext
{
    public DbprojectContext() { }

    public DbprojectContext(DbContextOptions<DbprojectContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql("Name=ConnectionStrings:DBProject");
}
