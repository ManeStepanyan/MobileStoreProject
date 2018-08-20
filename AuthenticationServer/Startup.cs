using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using AuthenticationServer.Services;
using AuthenticationServer.Validators;
using DatabaseAccess.SpExecuters;
using DatabaseAccess.Repository;
using DatabaseAccessor.Repository;
using Microsoft.Extensions.Configuration;
using System.IO;
using AuthenticationServer.UsersRepository;

namespace AuthenticationServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            ServiceCollectionExtensions.RegisterServices(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseMvc();
        }
    }
}