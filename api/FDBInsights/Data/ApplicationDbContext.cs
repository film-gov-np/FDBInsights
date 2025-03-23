using FDBInsights.Models;
using Microsoft.EntityFrameworkCore;

namespace FDBInsights.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<Movies> Movie { get; set; }
    public DbSet<Theater> Theater { get; set; }
    public DbSet<TicketTransaction> TicketTransaction { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserRole>().HasKey("RoleID");
        modelBuilder.Entity<Movies>().HasKey("MovieID");
        modelBuilder.Entity<Movies>(entity => { entity.ToTable("movie", "fdb"); });
        modelBuilder.Entity<Movies>().HasQueryFilter(entity => !entity.IsDeleted);
        modelBuilder.Entity<Theater>().HasKey("TheaterID");
        modelBuilder.Entity<Theater>().HasQueryFilter(entity => !entity.IsDeleted);
        modelBuilder.Entity<Theater>(entity => { entity.ToTable("theater", "fdb"); });
    }
}