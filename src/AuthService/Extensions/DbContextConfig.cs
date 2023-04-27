using AuthService.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace WebApi.Extensions;

/// <summary>
/// 数据上下文配置
/// </summary>
public static class DbContextConfig
{
    public static void AddDbContextConfig(this IServiceCollection Services,IConfiguration Configuration)
    {
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 33));
        var migrationsAssembly = typeof(ApplicationDbContext).Assembly.GetName().Name;
        Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."), serverVersion,
            sql => sql.MigrationsAssembly(migrationsAssembly))
            );

        Services.AddDatabaseDeveloperPageExceptionFilter();
    }
}
