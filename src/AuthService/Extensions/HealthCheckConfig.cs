namespace AuthService.Extensions;

/// <summary>
/// 健康检测配置
/// </summary>
public static class HealthCheckConfig
{
    public static void AddHealthCheckConfig(this IServiceCollection Services, IConfiguration Configuration)
    {
        Services.AddHealthChecks()
            .AddSqlServer(Configuration.GetConnectionString("DefaultConnection"), name: "AuthSqlServer");
    }

}
