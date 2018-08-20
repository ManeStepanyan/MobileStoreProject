using AuthenticationServer.Services;
using AuthenticationServer.UsersRepository;
using AuthenticationServer.Validators;
using DatabaseAccess.Repository;
using DatabaseAccess.SpExecuters;
using DatabaseAccessor.Repository;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace AuthenticationServer
{
    public static class ServiceCollectionExtensions
    {
        private static IConfiguration Configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();
        public static IServiceCollection RegisterServices(
            this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddMvc();
            services.AddIdentityServer().AddDeveloperSigningCredential()
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiResources(Config.GetApiResources())
                    .AddInMemoryClients(Config.GetClients())
                    .AddProfileService<ProfileService>();
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddSingleton(new Repo<User>(
            new MapInfo(Configuration["Mappers:Users"]),
            new SpExecuter(Configuration["ConnectionStrings:UsersDB"])));
            return services;
        }
    }
}
