using FDBInsights.Models;
using Microsoft.EntityFrameworkCore;

namespace FDBInsights.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}