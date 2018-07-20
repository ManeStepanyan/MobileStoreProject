using System.IO;
using CatalogAPI.Models;
using DatabaseAccess.Repository;
using DatabaseAccess.SpExecuters;
using DatabaseAccessor.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // public IConfiguration Configuration { get; }
        private IConfiguration Configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddRazorViewEngine()
                    .AddAuthorization()
                    .AddJsonFormatters();
            services.AddAuthentication("Bearer")
                   .AddIdentityServerAuthentication(options =>
                   {
                       options.Authority = "http://localhost:5000";
                       options.RequireHttpsMetadata = false;
                       options.ApiName = "CatalogAPI";
                   });
            services.AddAuthorization(options => options.AddPolicy("Seller", policy => policy.RequireClaim("role", "2")));
            services.AddSingleton(new Repo<SellerProduct>(
               new MapInfo(this.Configuration["Mappers:Catalog"]),
               new SpExecuter(this.Configuration["ConnectionStrings:CatalogDB"])));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseStaticFiles();
            app.UseAuthentication();
        }
    }
}
