using Consul.AspNetCore;

namespace AuthService.Extension;

/// <summary>
/// Consul网关配置
/// </summary>
public static class ConsulConfig
{
    public static void AddConsulConfig(this IServiceCollection Services, IConfiguration Configuration)
    {
        var consulconfig = Configuration.GetSection("Consul");
        if (consulconfig.GetChildren().Any())
        {
            Services.AddConsul(consulconfig.Bind);
            var serviceconfig = consulconfig.GetSection("Service");
            Services.AddConsulServiceRegistration(serviceconfig.Bind);
        }
    }
}