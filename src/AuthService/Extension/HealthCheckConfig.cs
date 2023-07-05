namespace AuthService.Extension;

/// <summary>
/// 健康检测配置
/// </summary>
public static class HealthCheckConfig
{
    public static void AddHealthCheckConfig(this IServiceCollection Services, IConfiguration Configuration)
    {
        string AuthSqlServer = Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."); 
        Services.AddHealthChecks().AddMySql(AuthSqlServer, name: nameof(AuthSqlServer));
    }

}
