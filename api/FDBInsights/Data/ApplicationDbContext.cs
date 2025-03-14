using FDBInsights.Models;
using Microsoft.EntityFrameworkCore;

namespace FDBInsights.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}