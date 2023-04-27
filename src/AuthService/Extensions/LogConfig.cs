namespace AuthService.Extensions;

/// <summary>
/// 日志配置
/// </summary>
public static class LogConfig
{
    public static void AddLogConfig(this IServiceCollection Services, IConfiguration Configuration)
    {
        var config = Configuration.GetSection("Seq");
        if (config.GetChildren().Any())
        {
            Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSeq(config);
            });
        }
    }
}
