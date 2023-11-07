using Dashboard.Core.Models;
using Dashboard.DataAccess.EntityConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess;

public class DashboardDBContext : IdentityDbContext<UserModel>
{
    public DashboardDBContext() : base() { }

    public DashboardDBContext(DbContextOptions<DashboardDBContext> options) : base(options) { }

    public DbSet<TaskModel> Tasks { get; set; }
    public DbSet<UserModel> IdentityUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SetTableProperties(modelBuilder);
    }

    private static void SetTableProperties(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
    }
}
