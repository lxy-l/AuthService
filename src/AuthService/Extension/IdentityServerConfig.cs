using System.Security.Cryptography.X509Certificates;
using Data;
using IdentityServer7.EntityFramework;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Extension;

/// <summary>
/// 认证配置
/// </summary>
public static class IdentityServerConfig
{
    public static void AddIdentityServerConfig(this IServiceCollection Services, IConfiguration Configuration,IWebHostEnvironment environment)
    {
        var connectionString = Configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        Services
            .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        // Identity配置：https://learn.microsoft.com/zh-cn/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-7.0
        Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 3;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        });

        //Cookies配置：https://learn.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.authentication.cookies.cookieauthenticationoptions?view=aspnetcore-7.0
        Services.ConfigureApplicationCookie(options =>
        {
            //options.Cookie.Domain = "aiax.eu.org:8443";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            //options.Cookie.Name = "AuthCookies";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.LoginPath = "/Identity/Account/Login";
            // ReturnUrlParameter requires 
            //using Microsoft.AspNetCore.Authentication.Cookies;
            options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            options.SlidingExpiration = true;
        });

        //IdentityServer配置
        var migrationsAssembly = typeof(ApplicationDbContext).Assembly.GetName().Name;

        var builder = Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 33)), sql =>
                        sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 33)), sql =>
                        sql.MigrationsAssembly(migrationsAssembly));
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 3600;
            })
            .AddAspNetIdentity<IdentityUser>();

        if (environment.IsDevelopment())
        {
            builder.AddDeveloperSigningCredential();
        }
        else
        {
            var certificateSettings = Configuration.GetSection("Certificate");
            var certificate = new X509Certificate2(
                certificateSettings.GetValue<string>("Path")?? throw new InvalidOperationException("X509 证书未配置！"),
                certificateSettings.GetValue<string>("Password")
            );
            builder.AddSigningCredential(certificate);
        }

        Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Administrator"));
        });

        builder.Services
            .AddAuthentication()
            .AddMicrosoftAccount(options =>
            {
                options.ClientId = Configuration["Authentication:Microsoft:ClientId"]
                                   ?? throw new InvalidOperationException("Connection string 'ClientId' not found."); ;
                options.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"]
                                       ?? throw new InvalidOperationException("Connection string 'ClientSecret' not found."); ;
            });
    }
}