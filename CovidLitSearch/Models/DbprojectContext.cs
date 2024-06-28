using Microsoft.EntityFrameworkCore;

namespace CovidLitSearch.Models;

public class DbprojectContext(DbContextOptions<DbprojectContext> options) : DbContext(options);