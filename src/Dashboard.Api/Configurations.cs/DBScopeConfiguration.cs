using Dashboard.Core.Models;
using Dashboard.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dashboard.Api.Configurations;

public static class DBScopeConfiguration
{
    public static void AddDBConfiguration(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDbContext<DashboardDBContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), builder =>
            {
                builder.MigrationsAssembly("Dashboard.Api");
                builder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "MigrationHistory");
            });
        });

        service.AddIdentity<UserModel, IdentityRole>().AddEntityFrameworkStores<DashboardDBContext>();
    }

    public static void UseDBScopeConfiguration(this IApplicationBuilder app)
    {
        var context = app
        .ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope()
        .ServiceProvider
        .GetService<DashboardDBContext>();

        if (context.Database.IsNpgsql())
        {
            context.Database.Migrate();
        }
    }
}