using AuthService.Configuration;
using AuthService.Data;

using IdentityServer7.EntityFramework.Storage.DbContexts;
using IdentityServer7.EntityFramework.Storage.Mappers;

using Microsoft.EntityFrameworkCore;

namespace AuthService.Seed
{
    public static class SeedData
    {
        public static async Task EnsureSeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();

            await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();

            var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            await context.Database.MigrateAsync();

            await CreateClients();

            await CreateScopes();

            await CreateApiResources();

            await CreateIdentityResources();

            await context.SaveChangesAsync();

            async Task CreateClients()
            {
                var clients = await context.Clients.ToDictionaryAsync(x => x.ClientId);

                foreach (var item in Config.Clients)
                {
                    if (!clients.ContainsKey(item.ClientId))
                    {
                        await context.Clients.AddAsync(item.ToEntity());
                    }

                }
            }

            async Task CreateScopes()
            {
                var apiScopes = await context.ApiScopes.ToDictionaryAsync(x => x.Name);

                foreach (var item in Config.ApiScopes)
                {
                    if (!apiScopes.ContainsKey(item.Name))
                    {
                        await context.ApiScopes.AddAsync(item.ToEntity());
                    }

                }
            }

            async Task CreateApiResources()
            {
                var apiResources = await context.ApiResources.ToDictionaryAsync(x => x.Name);

                foreach (var item in Config.ApiResources)
                {
                    if (!apiResources.ContainsKey(item.Name))
                    {
                        await context.ApiResources.AddAsync(item.ToEntity());
                    }
                }
            }

            async Task CreateIdentityResources()
            {
                var identityResources = await context.IdentityResources.ToDictionaryAsync(x => x.Name);

                foreach (var item in Config.IdentityResources)
                {
                    if (!identityResources.ContainsKey(item.Name))
                    {
                        await context.IdentityResources.AddAsync(item.ToEntity());
                    }
                }
            }
        }
    }
}
