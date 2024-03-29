using AuthService.Areas.Identity.Services;
using AuthService.Extension;
using AuthService.Seed;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

//DbContext配置
builder.Services.AddDbContextConfig(builder.Configuration);

//健康检查配置
builder.Services.AddHealthCheckConfig(builder.Configuration);

//Log配置
//builder.Services.AddLogConfig(builder.Configuration);

//Consul配置
//builder.Services.AddConsulConfig(builder.Configuration);

//IdentityServer配置
builder.Services.AddIdentityServerConfig(builder.Configuration,builder.Environment);

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.ConfigureApplicationCookie(o => {
    o.ExpireTimeSpan = TimeSpan.FromDays(1);
    o.SlidingExpiration = true;
});
builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromHours(3));
builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages();


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    //app.UseHsts();
}
await app.EnsureSeedData();
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

await app.RunAsync();
